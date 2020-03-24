using System.Collections.Generic;
using System.Linq;
using MNCD.Components;
using MNCD.Core;
using MNCD.Flattening;
using MNCD.Neighbourhood;

namespace MNCD.CommunityDetection.MultiLayer
{
    /// <summary>
    /// This class implements community detection based on
    /// CLECC (cross-layer edge clustering coefficient) measure.
    ///
    /// An Introduction to Community Detection in Multi-layered Social Network
    /// https://arxiv.org/ftp/arxiv/papers/1209/1209.6050.pdf
    /// Piotr Bródka, Tomasz Filipowski, Przemysław Kazienko.
    /// </summary>
    public class CLECCCommunityDetection
    {
        /// <summary>
        /// Community detection based on CLECC (cross-layer edge clustering coefficient) measure.
        /// </summary>
        /// <param name="network">
        /// Network on which the community detection will be applied.
        /// </param>
        /// <param name="alpha">
        /// Minimum number of layers on which neighbouring node must be a neighbour with node x.
        /// </param>
        /// <param name="k">
        /// Number of Communities
        /// </param>
        /// <returns>List of communities.</returns>
        public List<Community> Apply(Network network, int alpha, int k)
        {
            var n = CopyNetwork(network);
            var flattened = new BasicFlattening().Flatten(n, true);
            var components = ConnectedComponents(flattened);

            while (k > components.Count())
            {
                var clecc = new Dictionary<Edge, double>();
                foreach (var edge in flattened.FirstLayer.Edges)
                {
                    clecc[edge] = CLECC.GetCLECC(n, edge, alpha);
                }

                var edgesToRemove = GetMinimumCLECCEdges(clecc);
                foreach (var edge in edgesToRemove)
                {
                    flattened.FirstLayer.Edges.Remove(edge);

                    foreach (var layer in n.Layers)
                    {
                        layer.Edges.RemoveAll(e => (e.Pair == edge.Pair) || (e.Reverse().Pair == edge.Pair));
                    }
                }

                components = ConnectedComponents(flattened);
            }

            return ComponentsToCommunities(components);
        }

        private List<Edge> GetMinimumCLECCEdges(Dictionary<Edge, double> clecc)
        {
            var min = clecc.Values.Min();
            return clecc.Where(c => c.Value == min).Select(c => c.Key).ToList();
        }

        private List<Community> ComponentsToCommunities(IEnumerable<List<Actor>> components)
        {
            return components.Select(c => new Community(c)).ToList();
        }

        private IEnumerable<List<Actor>> ConnectedComponents(Network network)
        {
            return Connected.GetConnectedComponents(network.FirstLayer, network.Actors);
        }

        private Network CopyNetwork(Network n)
        {
            var layers = n.Layers.Select(l => new Layer
            {
                Name = l.Name,
                Edges = l.Edges.Select(e => e.Copy()).ToList(),
            }).ToList();
            var interLayer = n.InterLayerEdges.Select(e => e.Copy()).ToList();
            return new Network
            {
                Actors = n.Actors,
                Layers = layers,
                InterLayerEdges = interLayer,
            };
        }
    }
}