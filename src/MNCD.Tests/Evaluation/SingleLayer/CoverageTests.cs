using System.Collections.Generic;
using System.Linq;
using MNCD.Core;
using MNCD.Evaluation.SingleLayer;
using MNCD.Generators;
using Xunit;

namespace MNCD.Tests.Evaluation.SingleLayer
{
    public class CoverageTests
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
            var coverage = Coverage.Get(network, communities);

            Assert.Equal(1.0, coverage);
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
            var coverage = Coverage.Get(network, communities);

            Assert.Equal(0.0, coverage);
        }

        [Fact]
        public void TwoActorsOneCommunity()
        {
            var network = generator.Generate(2);
            var communities = new List<Community>
            {
                new Community(network.Actors),
            };
            var coverage = Coverage.Get(network, communities);

            Assert.Equal(1.0, coverage);
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
            var coverage = Coverage.Get(network, communities);

            Assert.Equal(0.5, coverage);
        }
    }
}