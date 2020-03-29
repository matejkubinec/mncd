using MNCD.Core;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Extensions
{
    /// <summary>
    /// Network extensions.
    /// </summary>
    public static class NetworkExtensions
    {
        /// <summary>
        /// Creates dictionary where keys are actors, and values are degrees of actors.
        /// </summary>
        /// <param name="network">Single-Layer network.</param>
        /// <param name="weight">Include weights of edges in degree.</param>
        /// <returns>Actor to degree dictionary.</returns>
        public static Dictionary<Actor, double> GetActorToDegree(this Network network, bool weight = false)
        {
            var edges = network.FirstLayer.Edges;
            return network.Actors.ToDictionary(a => a, a => edges.Where(e => e.From == a || e.To == a).Sum(e => weight ? e.Weight : 1.0));
        }

        /// <summary>
        /// Converts layer to an adjacency matrix.
        /// </summary>
        /// <param name="network">Network in which the layer is.</param>
        /// <param name="layer">Layer to be converted.</param>
        /// <returns>Adjacency matrix of supplied layer.</returns>
        public static double[,] LayerToAdjacencyMatrix(this Network network, int layer)
        {
            var len = network.Actors.Count;
            var actorToIndex = network.Actors.Select((a, i) => (a, i)).ToDictionary(a => a.a, a => a.i);
            var matrix = new double[len, len];

            for (var i = 0; i < len; i++)
            {
                for (var j = 0; j < len; j++)
                {
                    matrix[i, j] = 0.0;
                }
            }

            foreach (var edge in network.Layers[layer].Edges)
            {
                var f = actorToIndex[edge.From];
                var t = actorToIndex[edge.To];
                var w = edge.Weight;

                matrix[f, t] = w;
                matrix[t, f] = w;
            }

            return matrix;
        }

        /// <summary>
        /// Gets actor to degree dictionary for supplied layer.
        /// </summary>
        /// <param name="network">Network.</param>
        /// <param name="layer">Layer in which the degrees should be computed.</param>
        /// <returns>Actor to degree dictionary.</returns>
        public static Dictionary<Actor, int> LayerDegreesDict(this Network network, int layer)
        {
            var deg = network.Actors.ToDictionary(a => a, a => 0);
            foreach (var edge in network.Layers[layer].Edges)
            {
                deg[edge.From]++;
                deg[edge.To]++;
            }

            return deg;
        }

        /// <summary>
        /// Creates dicionary where keys are layers, and value is their index.
        /// </summary>
        /// <param name="network">Multi-layered network.</param>
        /// <returns>Layer to index dictionary.</returns>
        public static Dictionary<Layer, int> GetLayerToIndex(this Network network)
        {
            var i = 0;
            return network.Layers.ToDictionary(l => l, l => i++);
        }
    }
}
