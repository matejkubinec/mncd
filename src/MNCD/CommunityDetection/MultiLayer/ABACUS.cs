using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;
using MNCD.Extensions;

namespace MNCD.CommunityDetection.SingleLayer
{
    ///  4.3 - page 13
    /// 
    public class ABACUS
    {
        /// <summary>
        /// Applies ABACUS community detection algorithm on supplied network.
        /// ABACUS: frequent pAttern mining-BAsed Community discovery in mUltidimensional networkS
        /// Michele Berlingerio, Fabio Pinelli, Francesco Calabrese
        /// https://arxiv.org/pdf/1303.2025.pdf
        /// </summary>
        /// <param name="network"></param>
        /// <param name="Func<Network, List<Community>>">
        /// Community detection algorithm
        /// </param>
        /// <param name="treshold"></param>
        /// <returns></returns>
        public List<Community> Apply(
            Network network,
            Func<Network, List<Community>> cd,
            double treshold)
        {
            var membership = network.Actors
                .ToDictionary(a => a, a => new HashSet<(int, int)>());
            var layerNetworks = network.Layers
                .Select(l => LayerToNetwork(l));

            for (var i = 0; i < network.LayerCount; i++)
            {
                var communities = cd(layerNetworks.ElementAt(i));

                for (var j = 0; j < communities.Count; j++)
                {
                    foreach (var actor in communities[j].Actors)
                    {
                        membership[actor].Add((i, j));
                    }
                }
            }
        }

        private void ECLAT(
            Dictionary<Actor, HashSet<(int, int)>> membership,
            double treshold)
        {

        }

        private Network LayerToNetwork(Layer layer)
        {
            return new Network
            {
                Layers = new List<Layer> { layer },
                Actors = layer.GetActors()
            };
        }
    }
}