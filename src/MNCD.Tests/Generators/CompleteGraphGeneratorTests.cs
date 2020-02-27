using MNCD.Generators;
using Xunit;

namespace MNCD.Tests.Generators
{
    public class CompleteGraphGeneratorTests
    {
        private CompleteGraphGenerator _generator = new CompleteGraphGenerator();

        [Fact]
        public void Generate1()
        {
            var network = _generator.Generate(1);

            Assert.Equal(1, network.Actors.Count);
            Assert.Equal(1, network.Layers.Count);
            Assert.Empty(network.Layers[0].Edges);
        }

        [Fact]
        public void Generate2()
        {
            var network = _generator.Generate(2);

            Assert.Equal(2, network.Actors.Count);
            Assert.Equal(1, network.Layers.Count);
            Assert.NotEmpty(network.Layers[0].Edges);
            Assert.Equal(1, network.Layers[0].Edges.Count);

            var f = network.Actors[0];
            var t = network.Actors[1];
            var e = network.Layers[0].Edges[0];

            Assert.True((e.From == f && e.To == t) || (e.From == t && e.To == f));
        }

        [Fact]
        public void Generate3()
        {
            var network = _generator.Generate(3);

            Assert.Equal(3, network.Actors.Count);
            Assert.Equal(1, network.Layers.Count);
            Assert.NotEmpty(network.Layers[0].Edges);
            Assert.Equal(3, network.Layers[0].Edges.Count);

            var f = network.Actors[0];
            var t = network.Actors[1];
            var e = network.Layers[0].Edges[0];

            Assert.True((e.From == f && e.To == t) || (e.From == t && e.To == f));

            f = network.Actors[0];
            t = network.Actors[2];
            e = network.Layers[0].Edges[1];

            Assert.True((e.From == f && e.To == t) || (e.From == t && e.To == f));

            f = network.Actors[1];
            t = network.Actors[2];
            e = network.Layers[0].Edges[2];

            Assert.True((e.From == f && e.To == t) || (e.From == t && e.To == f));
        }
    }
}