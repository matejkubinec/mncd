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
            var network = TestNetwork;
            var communities = fluidC.Compute(network, 2);
            var totalActors = communities.Sum(c => c.Actors.Count);

            var c3 = communities.First(c => c.Actors.Count == 3);
            var c4 = communities.First(c => c.Actors.Count == 4);

            Assert.True(c3.Actors.Any(a => a.Name == "2"));
            Assert.True(c3.Actors.Any(a => a.Name == "3"));
            Assert.True(c3.Actors.Any(a => a.Name == "6"));

            Assert.True(c4.Actors.Any(a => a.Name == "0"));
            Assert.True(c4.Actors.Any(a => a.Name == "1"));
            Assert.True(c4.Actors.Any(a => a.Name == "4"));
            Assert.True(c4.Actors.Any(a => a.Name == "5"));
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