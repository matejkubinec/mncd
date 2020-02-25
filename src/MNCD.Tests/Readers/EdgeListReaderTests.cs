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
            Assert.Equal(1, network.InterLayerEdges.Count);
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