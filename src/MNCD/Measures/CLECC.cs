using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

// TODO: add tests for CLECC
namespace MNCD.Neighbourhood
{
    /// <summary>
    /// This class implements the cross-layer edge clustering coefficient (CLECC)
    /// Based on:
    /// An Introduction to Community Detection in Multi-layered Social Network.
    /// https://arxiv.org/ftp/arxiv/papers/1209/1209.6050.pdf
    /// Piotr Bródka, Tomasz Filipowski, Przemysław Kazienko
    /// </summary>
    public static class CLECC
    {
        /// <summary>
        /// Computes the CLECC measure for supplied edge.
        /// </summary>
        /// <param name="e">
        /// Edge, for which the CLECC measure will be computed.
        /// </param>
        /// <param name="alpha">
        /// Minimum number of layers on which neighbouring node must be a 
        /// neighbour with node x.
        /// </param>
        /// <returns>
        /// Proportion between the common multi-layered neighbours and all
        /// multi-layered neighbours of x and y.
        /// </returns>
        public static double GetCLECC(Network n, Edge e, int alpha)
        {
            var x = MN(n, e.From, alpha);
            var y = MN(n, e.To, alpha);
            var xy = new List<Actor> { e.From, e.To };

            return x.Intersect(y).Count() / (double)x.Union(y).Except(xy).Count();
        }

        private static List<Actor> MN(Network n, Actor x, int alpha)
        {
            return MultiLayerNeighbourhood.GetMN(n, x, alpha);
        }
    }
}