using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;
using MNCD.Evaluation.SingleLayer;
using MNCD.Generators;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.Evaluation.SingleLayer
{
    public class PerformanceTests
    {
        private readonly CompleteGraphGenerator generator = new CompleteGraphGenerator();

        [Fact]
        public void OneActor()
        {
            var network = generator.Generate(1);
            var communities = new List<Community>
            {
                new Community(network.Actors[0])
            };
            var performance = Performance.Get(network, communities);

            Assert.Equal(1.0, performance);
        }

        [Fact]
        public void TwoActorsTwoCommunities()
        {
            var network = generator.Generate(2);
            var communities = new List<Community>
            {
                new Community(network.Actors[0]),
                new Community(network.Actors[1])
            };
            var performance = Performance.Get(network, communities);

            Assert.Equal(0.0, performance);
        }

        [Fact]
        public void TwoActorsOneCommunity()
        {
            var network = generator.Generate(2);
            var communities = new List<Community>
            {
                new Community(network.Actors),
            };
            var performance = Performance.Get(network, communities);

            Assert.Equal(1.0, performance);
        }

        [Fact]
        public void FourActorsTwoCommunities()
        {
            var network = generator.Generate(4);
            var communities = new List<Community>
            {
                new Community(network.Actors.GetRange(0, 3)),
                new Community(network.Actors.Last())
            };
            var performance = Performance.Get(network, communities);

            Assert.Equal(0.5, performance);
        }

        [Fact]
        public void FourActorsTwoCommunitiesEqualSplit()
        {
            var network = generator.Generate(4);
            var communities = new List<Community>
            {
                new Community(network.Actors.GetRange(0, 2)),
                new Community(network.Actors.GetRange(2, 2))
            };
            var performance = Performance.Get(network, communities);

            Assert.Equal(1.0 / 3.0, performance);
        }

        [Fact]
        public void ThreeToTwo()
        {
            var actors = ActorHelper.Get(3);
            var edges = new List<Edge>
            {
                new Edge(actors[0], actors[1]),
                new Edge(actors[1], actors[2])
            };
            var communities = new List<Community>
            {
                new Community(actors[0]),
                new Community(actors[1], actors[2])
            };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);
            var performance = Performance.Get(network, communities);

            Assert.Equal(2.0 / 3.0, performance);
        }

        [Fact]
        public void PerformanceBetweenOneAndZero()
        {
            var r = new Random();
            var g = new RandomMultiLayerGenerator();
            for(var n = 2; n < 50; n++)
            {
                var network = g.GenerateSingleLayer(n, 0.65);
                var communityCount = r.Next(2, n);
                var communities = Enumerable
                    .Range(0, communityCount)
                    .Select(c => new Community())
                    .ToList();
                foreach(var actor in network.Actors)
                {
                    var c = r.Next(0, communityCount);
                    communities[c].Actors.Add(actor);
                }
                var performance = Performance.Get(network, communities);

                Assert.True(performance >= 0.0, "Performance was less than zero.");
                Assert.True(performance <= 1.0, "Performance was greater than one.");
            }
        }
    }
}