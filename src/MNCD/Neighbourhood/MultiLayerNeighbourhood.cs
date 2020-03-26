using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Neighbourhood
{
    /// <summary>
    /// This class implements the concept of multi-layer neighbourhood based on
    /// An Introduction to Community Detection in Multi-layered Social Network.
    ///
    /// https://arxiv.org/ftp/arxiv/papers/1209/1209.6050.pdf
    /// Piotr Bródka, Tomasz Filipowski, Przemysław Kazienko
    /// </summary>
    public static class MultiLayerNeighbourhood
    {
        /// <summary>
        /// Return neighbourhood of node x, which are nodes which are the
        /// neighbours of the given node x on at least alpha layers.
        /// </summary>
        /// <param name="n">
        /// Multi-layer network.
        /// </param>
        /// <param name="x">
        /// Node (Actor), for which the neighbourhood will be found.
        /// </param>
        /// <param name="alpha">
        /// Minimum number of layers on which neighbouring node must be a 
        /// neighbour with node x.
        /// </param>
        /// <returns>Multilayer neighbourhood of node x.</returns>
        public static List<Actor> GetMN(Network n, Actor x, int alpha)
        {
            var neighbours = new Dictionary<Actor, int>();
            foreach (var l in n.Layers)
            {
                foreach (var e in l.Edges)
                {
                    if (e.From != x && e.To != x)
                    {
                        continue;
                    }

                    var neighbour = e.From == x ? e.To : e.From;


                    if (neighbours.ContainsKey(neighbour))
                    {
                        neighbours[neighbour]++;
                    }
                    else
                    {
                        neighbours[neighbour] = 1;
                    }
                }
            }

            return neighbours
                .Where(pair => pair.Value >= alpha)
                .Select(pair => pair.Key)
                .ToList();
        }
    }
}