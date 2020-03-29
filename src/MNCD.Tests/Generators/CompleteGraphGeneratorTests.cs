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

            Assert.Collection(network.Actors,
                a => Assert.Equal(network.Actors[0], a)
            );
            Assert.Collection(network.Actors, a => Assert.NotNull(a));
            Assert.Collection(network.Layers, l => Assert.Empty(l.Edges));
        }

        [Fact]
        public void Generate2()
        {
            var network = _generator.Generate(2);
            var actors = network.Actors;

            Assert.Collection(actors,
                a => Assert.NotNull(a),
                a => Assert.NotNull(a)
            );
            Assert.Collection(network.Layers, l => Assert.NotEmpty(l.Edges));
            Assert.Collection(network.FirstLayer.Edges,
                e => Assert.True(
                    e.Pair == (actors[0], actors[1]) ||
                    e.Pair == (actors[1], actors[0])
                )
            );
        }

        [Fact]
        public void Generate3()
        {
            var network = _generator.Generate(3);
            var a = network.Actors;

            Assert.Collection(network.Actors,
                a => Assert.NotNull(a),
                a => Assert.NotNull(a),
                a => Assert.NotNull(a)
            );
            Assert.Collection(network.Layers,
                l => Assert.NotEmpty(l.Edges)
            );
            Assert.Collection(network.FirstLayer.Edges,
                e => Assert.True(e.Pair == (a[0], a[1])),
                e => Assert.True(e.Pair == (a[0], a[2])),
                e => Assert.True(e.Pair == (a[1], a[2]))
            );
        }

        [Fact]
        public void Generate1MultiLayer1()
        {
            var network = _generator.GenerateMultiLayer(1, 1);

            Assert.Collection(network.Actors,
                a => Assert.Equal(network.Actors[0], a)
            );
            Assert.Collection(network.Layers,
                l => Assert.Equal(network.Layers[0], l)
            );
            Assert.Empty(network.Layers[0].Edges);
        }

        [Fact]
        public void Generate1MultiLayer2()
        {
            var network = _generator.GenerateMultiLayer(1, 2);
            var a = network.Actors;
            var l = network.Layers;

            Assert.Collection(network.Actors,
                a => Assert.NotNull(a)
            );
            Assert.Collection(network.Layers,
                l => Assert.Empty(l.Edges),
                l => Assert.Empty(l.Edges)
            );
            Assert.Collection(network.InterLayerEdges,
                e => Assert.True(
                    e.From == a[0] &&
                    e.To == a[0] &&
                    e.LayerFrom == l[0] &&
                    e.LayerTo == l[1] &&
                    e.Weight == 1
                )
            );
        }

        [Fact]
        public void Generate2MultiLayer1()
        {
            var network = _generator.GenerateMultiLayer(2, 1);
            var a = network.Actors;

            Assert.Collection(network.Actors,
                a => Assert.NotNull(a),
                a => Assert.NotNull(a)
            );
            Assert.Collection(network.Layers,
                l => Assert.Collection(l.Edges,
                    e => Assert.True(e.Pair == (a[0], a[1]))
                )
            );
        }

        [Fact]
        public void Generate2MultiLayer2()
        {
            var network = _generator.GenerateMultiLayer(2, 2);
            var a = network.Actors;

            Assert.Collection(network.Actors,
                a => Assert.NotNull(a),
                a => Assert.NotNull(a)
            );
            Assert.Collection(network.Layers,
                l => Assert.Collection(l.Edges,
                    e => Assert.True(e.Pair == (a[0], a[1]))
                ),
                l => Assert.Collection(l.Edges,
                    e => Assert.True(e.Pair == (a[0], a[1]))
                )
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