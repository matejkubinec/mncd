using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MNCD.Readers;
using Xunit;

namespace MNCD.Tests.Readers
{
    public class EdgeListReaderTests
    {
        private readonly EdgeListReader reader = new EdgeListReader();

        [Fact]
        public void TestLoadFloretine()
        {
            var florentine = File.ReadAllText("SampleData/florentine.edgelist");
            var network = reader.FromString(florentine);

            var actorsNamesExpected = new List<string>
            {
                "Acciaiuoli",
                "Albizzi",
                "Barbadori",
                "Bischeri",
                "Castellani",
                "Ginori",
                "Guadagni",
                "Lamberteschi",
                "Medici",
                "Pazzi",
                "Peruzzi",
                "Ridolfi",
                "Salviati",
                "Strozzi",
                "Tornabuoni"
            };

            actorsNamesExpected.Sort();
            var actorsNamesActual = network.Actors.Select(a => a.Name).ToList();
            actorsNamesActual.Sort();

            Assert.Equal(2, network.Layers.Count);
            Assert.Empty(network.InterLayerEdges);

            Assert.Equal(15, network.Actors.Count);
            Assert.Equal(actorsNamesExpected, actorsNamesActual);

            Assert.Equal("marriage", network.Layers[0].Name);
            Assert.Equal(20, network.Layers[0].Edges.Count);

            Assert.Equal("business", network.Layers[1].Name);
            Assert.Equal(15, network.Layers[1].Edges.Count);
        }

        [Fact]
        public void InterLayerEdge()
        {
            var interlayer = File.ReadAllText("SampleData/interlayer.edgelist");
            var network = reader.FromString(interlayer);

            Assert.Equal(2, network.Layers.Count);
            Assert.NotEmpty(network.InterLayerEdges);
            Assert.Collection(network.InterLayerEdges,
                edge =>
                {
                    Assert.Equal(network.Actors[0], edge.From);
                    Assert.Equal(network.Actors[1], edge.To);

                    Assert.Equal(network.Layers[0], edge.LayerFrom);
                    Assert.Equal(network.Layers[1], edge.LayerTo);
                }
            );
        }

        [Fact]
        public void InterLayerEdgeWithMetadata()
        {
            var interlayer = File.ReadAllText("SampleData/interlayer-metadata.edgelist");
            var network = reader.FromString(interlayer);

            Assert.Equal(2, network.Layers.Count);
            Assert.NotEmpty(network.InterLayerEdges);
            Assert.Collection(network.InterLayerEdges,
                edge =>
                {
                    Assert.Equal(network.Actors[0], edge.From);
                    Assert.Equal(network.Actors[1], edge.To);

                    Assert.Equal(network.Layers[0], edge.LayerFrom);
                    Assert.Equal(network.Layers[1], edge.LayerTo);
                }
            );
            Assert.Collection(network.Actors,
                actor => Assert.Equal("a0", actor.Name),
                actor => Assert.Equal("a1", actor.Name)
            );
            Assert.Collection(network.Layers,
                layer => Assert.Equal("l0", layer.Name),
                layer => Assert.Equal("l1", layer.Name)
            );
        }

        [Fact]
        public void InvalidEdgeList()
        {
            var networkString = File.ReadAllText("SampleData/invalid-edgelist.edgelist");
            Assert.Throws<ArgumentException>(() => reader.FromString(networkString));
        }

        [Fact]
        public void InvalidEdgeListWeight()
        {
            var networkString = File.ReadAllText("SampleData/invalid-weight.edgelist");
            Assert.Throws<ArgumentException>(() => reader.FromString(networkString));
        }
    }
}