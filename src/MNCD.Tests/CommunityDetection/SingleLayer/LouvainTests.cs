using System.Collections.Generic;
using System.Linq;
using MNCD.CommunityDetection.SingleLayer;
using MNCD.Core;
using MNCD.Generators;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.CommunityDetection.SingleLayer
{
    public class LouvainTests
    {
        [Fact]
        public void Test()
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
            var communities = new Louvain().Apply(network);

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
    }
}