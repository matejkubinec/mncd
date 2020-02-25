using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Evaluation
{
    public static class Homogenity
    {
        public static double Compute(Community community, Network network)
        {
            var d = network.Layers.Count;
            var edgeLayerCounts = new List<double>();

            foreach (var layer in network.Layers)
            {
                var edgesInLayer = 0;

                foreach (var edge in layer.Edges)
                {
                    if (community.Actors.Contains(edge.From) &&
                        community.Actors.Contains(edge.To))
                    {
                        edgesInLayer++;
                    }
                }

                edgeLayerCounts.Add(edgesInLayer);
            }

            var sigmaC = GetSigmaC(edgeLayerCounts, d);
            var sigmaCMax = GetSigmaCMax(edgeLayerCounts);

            return 1 - sigmaC / sigmaCMax;
        }

        private static double GetSigmaC(List<double> pcds, int d)
        {
            var avgC = pcds.Average();
            var res = 0.0;
            foreach (var pcd in pcds)
            {
                res += Math.Pow(pcd - avgC, 2);
            }
            return Math.Sqrt(res);
        }

        private static double GetSigmaCMax(List<double> pcds)
        {
            return Math.Sqrt(Math.Pow(pcds.Max() - 1, 2) / 2.0);
        }
    }
}