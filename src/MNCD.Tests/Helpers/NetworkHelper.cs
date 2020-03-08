using System.Collections.Generic;
using MNCD.Core;

namespace MNCD.Tests.Helpers
{
    public static class NetworkHelper
    {
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