using MNCD.CommunityDetection.SingleLayer;
using MNCD.Core;
using MNCD.Generators;
using MNCD.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MNCD.Tests.CommunityDetection.SingleLayer
{
    public class LabelPropagationTests
    {
        private readonly LabelPropagation _labelPropagation = new LabelPropagation();

        [Fact]
        public void NetworkNull()
        {
            Assert.Throws<ArgumentNullException>(() => _labelPropagation.GetCommunities(null));
        }

        [Fact]
        public void MaxIterationsLessThanZero()
        {
            var network = new Network();
            Assert.Throws<ArgumentException>(() => _labelPropagation.GetCommunities(network, 0));
            Assert.Throws<ArgumentException>(() => _labelPropagation.GetCommunities(network, -1));
        }

        [Fact]
        public void TwoActorsWithoutEdge()
        {
            var actor1 = new Actor();
            var actor2 = new Actor();
            var actors = new List<Actor> { actor1, actor2 };
            var layer = new Layer();
            var network = new Network(layer, actors);

            var communities = _labelPropagation.GetCommunities(network);

            Assert.Collection(communities,
                c1 => Assert.Collection(c1.Actors, a => Assert.Equal(actor1, a)),
                c2 => Assert.Collection(c2.Actors, a => Assert.Equal(actor2, a)));
        }

        [Fact]
        public void TwoActorsWithEdge()
        {
            var actor1 = new Actor();
            var actor2 = new Actor();
            var actors = new List<Actor> { actor1, actor2 };
            var edge1 = new Edge(actor1, actor2);
            var edges = new List<Edge> { edge1 };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);

            var communities = _labelPropagation.GetCommunities(network);

            Assert.Collection(communities,
                c1 => Assert.Collection(c1.Actors, a => Assert.Equal(actor1, a)),
                c2 => Assert.Collection(c2.Actors, a => Assert.Equal(actor2, a)));
        }

        [Fact]
        public void TwoCommunitiesTestNetwork()
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
            var communities = _labelPropagation.GetCommunities(network);

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
        public void TestRandom()
        {
            var generator = new RandomMultiLayerGenerator();
            var random = new Random();
            var network = generator.GenerateSingleLayer(5, 0.65);
            var communities = _labelPropagation.GetCommunities(network);

            var count = communities.SelectMany(c => c.Actors).Count();
            Assert.Equal(5, count);
        }
    }
}
