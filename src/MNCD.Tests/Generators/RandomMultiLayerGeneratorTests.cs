using System;
using System.Linq;
using MNCD.Generators;
using Xunit;

namespace MNCD.Tests.Generators
{
    public class RandomMultiLayerGeneratorTests
    {
        private static Random R = new Random();

        [Fact]
        public void GenerateSingleLayer()
        {
            var generator = new RandomMultiLayerGenerator();
            foreach(var n in Enumerable.Range(2, 10))
            {
                var network = generator.GenerateSingleLayer(n, 0.55);

                Assert.Equal(n, network.Actors.Count());
            }
        }

        [Fact]
        public void GenerateMultiLayer()
        {
            var generator = new RandomMultiLayerGenerator();
            foreach(var l in Enumerable.Range(2, 10))
            {
                foreach(var n in Enumerable.Range(2, 10))
                {
                    var network = generator.Generate(n, l, 0.55);

                    Assert.Equal(n, network.Actors.Count());
                    Assert.Equal(l, network.Layers.Count());
                }
            }
        }
    }
}