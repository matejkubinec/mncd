using System.Collections.Generic;
using MNCD.Core;

namespace MNCD.Tests.Helpers
{
    public static class NetworkHelper
    {
        public static Network TwoLayerTriangles()
        {
            // L1             L2
            // 0               4
            // | \   L1--L2  / |
            // |  2 ------- 3  |
            // | /           \ |
            // 1               5
            var actors = ActorHelper.Get(6);
            var edges = new List<Edge>
            {
                new Edge(actors[0], actors[1]),
                new Edge(actors[0], actors[2]),
                new Edge(actors[1], actors[2]),
                new Edge(actors[2], actors[3]),
                new Edge(actors[3], actors[4]),
                new Edge(actors[3], actors[5]),
                new Edge(actors[4], actors[5])
            };
            var layerOne = new Layer(edges);
            var layerTwo = new Layer(edges);
            var layers = new List<Layer> { layerOne, layerTwo };
            return new Network(layers, actors);
        }

        public static Network InitBarabasi()
        {
            var A = ActorHelper.Get(9);
            var E = new List<Edge>
            {
                new Edge(A[0], A[1]),
                new Edge(A[0], A[2]),
                new Edge(A[0], A[3]),
                new Edge(A[1], A[2]),
                new Edge(A[1], A[4]),
                new Edge(A[2], A[3]),
                new Edge(A[3], A[4]),
                new Edge(A[4], A[5]),
                new Edge(A[5], A[6]),
                new Edge(A[5], A[7]),
                new Edge(A[5], A[8]),
                new Edge(A[6], A[8]),
                new Edge(A[7], A[8])
            };
            var L = new Layer(E);
            var N = new Network(L, A);
            return N;
        }
    }
}