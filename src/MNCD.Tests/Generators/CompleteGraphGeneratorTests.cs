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

        [Fact]
        public void Generate1MultiLayer1()
        {
            var network = _generator.GenerateMultiLayer(1, 1);

            Assert.Equal(1, network.Actors.Count);
            Assert.Equal(1, network.Layers.Count);
            Assert.Empty(network.Layers[0].Edges);
        }

        [Fact]
        public void Generate1MultiLayer2()
        {
            var network = _generator.GenerateMultiLayer(1, 2);

            Assert.Equal(1, network.Actors.Count);
            Assert.Equal(2, network.LayerCount);
            Assert.Empty(network.Layers[0].Edges);
            Assert.Empty(network.Layers[1].Edges);
            Assert.NotEmpty(network.InterLayerEdges);
        }

        [Fact]
        public void Generate2MultiLayer1()
        {
            var network = _generator.GenerateMultiLayer(2, 1);

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
        public void Generate2MultiLayer2()
        {
            var network = _generator.GenerateMultiLayer(2, 2);

            Assert.Equal(2, network.Actors.Count);
            Assert.Equal(2, network.Layers.Count);

            Assert.NotEmpty(network.Layers[0].Edges);
            Assert.Collection(network.Layers[0].Edges,
                edge =>
                {
                    var f = network.Actors[0];
                    var t = network.Actors[1];
                    Assert.True((f, t) == edge.Pair || (t, f) == edge.Pair);
                }
            );

            Assert.NotEmpty(network.Layers[1].Edges);
            Assert.Equal(1, network.Layers[1].Edges.Count);
            Assert.Collection(network.Layers[0].Edges,
                edge =>
                {
                    var f = network.Actors[0];
                    var t = network.Actors[1];
                    Assert.True((f, t) == edge.Pair || (t, f) == edge.Pair);
                }
            );

            Assert.Collection(network.InterLayerEdges,
                e =>
                {
                    var f = network.Actors[0];
                    var lf = network.Layers[0];
                    var t = network.Actors[1];
                    var lt = network.Layers[1];
                    var p = e.InterLayerPair;

                    Assert.True(p == (f, lf, t, lt));
                },
                e =>
                {
                    var f = network.Actors[0];
                    var lf = network.Layers[0];
                    var t = network.Actors[0];
                    var lt = network.Layers[1];
                    var p = e.InterLayerPair;

                    Assert.True(p == (f, lf, t, lt));
                },
                e =>
                {
                    var f = network.Actors[1];
                    var lf = network.Layers[0];
                    var t = network.Actors[1];
                    var lt = network.Layers[1];
                    var p = e.InterLayerPair;

                    Assert.True(p == (f, lf, t, lt));
                }
            );
        }
    }
}