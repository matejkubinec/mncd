using MNCD.CommunityDetection.MultiLayer;
using MNCD.CommunityDetection.SingleLayer;
using MNCD.Core;
using MNCD.Generators;
using MNCD.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MNCD.Tests.CommunityDetection.MultiLayer
{
    public class CLECCCommunityDetectionTests
    {
        [Fact]
        public void ConnectedTriangles()
        {
            // 0          4
            // | \      / |
            // |  2 -- 3  |
            // | /      \ |
            // 1          5
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
            var layer = new Layer(edges);
            var network = new Network(layer, actors);
            var communities = new CLECCCommunityDetection().Apply(network, 1, 2);

            Assert.Collection(communities.OrderBy(c => c.Actors.First().Name),
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(actors[0], a),
                    a => Assert.Equal(actors[1], a),
                    a => Assert.Equal(actors[2], a)
                ),
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(actors[3], a),
                    a => Assert.Equal(actors[4], a),
                    a => Assert.Equal(actors[5], a)
                )
            );
        }

        [Fact]
        public void ConnectedTrianglesMultiLayer()
        {
            // L1            L2
            // 0             4
            // | \  L1-L2  / |
            // |  2 ----- 3  |
            // | /         \ |
            // 1             5
            var A = ActorHelper.Get(6);
            var e0 = new List<Edge>
            {
                new Edge(A[0], A[1]),
                new Edge(A[0], A[2]),
                new Edge(A[1], A[2]),
            };
            var e1 = new List<Edge>
            {
                new Edge(A[3], A[4]),
                new Edge(A[3], A[5]),
                new Edge(A[4], A[5])
            };
            var l0 = new Layer(e0);
            var l1 = new Layer(e1);
            var L = new List<Layer> { l0, l1 };
            var I = new List<InterLayerEdge>
            {
                new InterLayerEdge(A[2], l0, A[3], l1)
            };
            var network = new Network(L, A)
            {
                InterLayerEdges = I
            };
            var communities = new CLECCCommunityDetection().Apply(network, 1, 2);

            Assert.Collection(communities.OrderBy(c => c.Actors.First().Name),
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(A[0], a),
                    a => Assert.Equal(A[1], a),
                    a => Assert.Equal(A[2], a)
                ),
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(A[3], a),
                    a => Assert.Equal(A[4], a),
                    a => Assert.Equal(A[5], a)
                )
            );
        }

        [Fact]
        public void TestRandom()
        {
            var random = new Random();
            var generator = new RandomMultiLayerGenerator();
            for (var n = 2; n < 15; n++)
            {
                var network = generator.GenerateSingleLayer(n, 0.65);
                var communities = new CLECCCommunityDetection().Apply(network, 1, 2);

                var count = communities.SelectMany(c => c.Actors).Count();
                Assert.Equal(n, count);
            }
        }
    }
}
