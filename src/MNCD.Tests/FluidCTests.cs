using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.CommunityDetection;
using MNCD.Core;
using Xunit;

namespace MNCD.Tests
{
    public class FluidCTests
    {
        private FluidC fluidC = new FluidC();

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void CommunityCount(int k)
        {
            var network = TestNetwork;

            var communities = fluidC.Compute(network, k);
            var totalActors = communities.Sum(c => c.Actors.Count);

            Assert.Equal(communities.Count, k);
            Assert.Equal(totalActors, network.Actors.Count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(8)]
        public void InvalidArguments(int k)
        {
            var network = TestNetwork;
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, k));
        }

        [Fact]
        public void Community()
        {
            var actors = new List<Actor>
            {
                new Actor("a0"),
                new Actor("a1"),
                new Actor("a2"),
                new Actor("a3"),
                new Actor("a4"),
                new Actor("a5"),
            };
            var network = new Network
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[2]),
                            new Edge(actors[0], actors[1]),
                            new Edge(actors[1], actors[2]),
                            new Edge(actors[2], actors[3]),
                            new Edge(actors[3], actors[4]),
                            new Edge(actors[3], actors[5]),
                            new Edge(actors[4], actors[5])
                        }
                    }
                }
            };
            var initial = new List<Actor>
            {
                actors[0],
                actors[5]
            };

            var communities = new FluidC().Compute(network, initial).OrderBy(c => c.Actors.Count).ToList();

            Assert.Equal(3, communities[0].Actors.Count);
            Assert.Equal(3, communities[1].Actors.Count);
        }

        private Network TestNetwork
        {
            get
            {
                var a = Enumerable
                    .Range(0, 7)
                    .Select(i => new Actor { Name = i.ToString() })
                    .ToList();

                var e = new List<Edge>
                {
                    new Edge(a[0], a[1]),
                    new Edge(a[0], a[4]),
                    new Edge(a[0], a[5]),
                    new Edge(a[1], a[5]),
                    new Edge(a[1], a[2]),
                    new Edge(a[2], a[3]),
                    new Edge(a[2], a[6]),
                    new Edge(a[3], a[6]),
                    new Edge(a[4], a[5]),
                    new Edge(a[5], a[6]),
                };

                var n = new Network
                {
                    Actors = a,
                    Layers = new List<Layer>
                    {
                        new Layer
                        {
                            Edges = e
                        }
                    }
                };

                return n;
            }
        }
    }
}