using MNCD.Core;
using MNCD.Writers;
using System.Collections.Generic;
using Xunit;

namespace MNCD.Tests.Writers
{
    public class EdgeListWriterTests
    {
        private readonly EdgeListWriter writer = new EdgeListWriter();

        [Fact]
        public void NetworkWithoutEdges()
        {
            var network = new Network();
            var edgeListString = writer.ToString(network);

            Assert.Equal("", edgeListString);
        }

        [Fact]
        public void NetworkWithoutEdgesWithMetadata()
        {
            var network = new Network();
            var edgeListString = writer.ToString(network, true);

            Assert.Equal("", edgeListString);
        }

        [Fact]
        public void NetworkOneEdge()
        {
            var actors = new List<Actor>
            {
                new Actor() { Name = "a1" },
                new Actor() { Name = "a2" }
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "l1",
                        Edges = new List<Edge>
                        {
                            new Edge { From = actors[0], To = actors[1], Weight = 1 }
                        }
                    }
                }
            };
            var edgeListString = writer.ToString(network);

            Assert.Equal("0 0 1 0 1\n", edgeListString);
        }

        [Fact]
        public void NetworkOneEdgeWithMetadata()
        {
            var actors = new List<Actor>
            {
                new Actor() { Name = "a1" },
                new Actor() { Name = "a2" }
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "l1",
                        Edges = new List<Edge>
                        {
                            new Edge { From = actors[0], To = actors[1], Weight = 1 }
                        }
                    }
                }
            };
            var edgeListString = writer.ToString(network, true);

            var lines = edgeListString.Split('\n');
            Assert.Collection(lines,
                line => Assert.Equal("0 0 1 0 1", line),
                line => Assert.Equal("# Actors", line),
                line => Assert.Equal("0 a1", line),
                line => Assert.Equal("1 a2", line),
                line => Assert.Equal("# Layers", line),
                line => Assert.Equal("0 l1", line),
                line => Assert.Equal("", line)
            );
        }

        [Fact]
        public void NetworkInterLayerEdge()
        {
            var actors = new List<Actor>
            {
                new Actor() { Name = "a1" },
                new Actor() { Name = "a2" }
            };
            var layers = new List<Layer>
            {
                new Layer { Name = "l1" },
                new Layer { Name = "l2" }
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = layers,
                InterLayerEdges = new List<InterLayerEdge>
                {
                    new InterLayerEdge
                    {
                        From = actors[0],
                        To = actors[1],
                        LayerFrom = layers[0],
                        LayerTo = layers[1],
                        Weight = 1
                    }
                }
            };
            var edgeListString = writer.ToString(network);

            Assert.Equal("0 0 1 1 1\n", edgeListString);
        }

        [Fact]
        public void NetworkInterLayerEdgeWithMetadata()
        {
            var actors = new List<Actor>
            {
                new Actor() { Name = "a1" },
                new Actor() { Name = "a2" }
            };
            var layers = new List<Layer>
            {
                new Layer { Name = "l1" },
                new Layer { Name = "l2" }
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = layers,
                InterLayerEdges = new List<InterLayerEdge>
                {
                    new InterLayerEdge
                    {
                        From = actors[0],
                        To = actors[1],
                        LayerFrom = layers[0],
                        LayerTo = layers[1],
                        Weight = 1
                    }
                }
            };
            var edgeListString = writer.ToString(network, true);

            var lines = edgeListString.Split('\n');
            Assert.Collection(lines,
                line => Assert.Equal("0 0 1 1 1", line),
                line => Assert.Equal("# Actors", line),
                line => Assert.Equal("0 a1", line),
                line => Assert.Equal("1 a2", line),
                line => Assert.Equal("# Layers", line),
                line => Assert.Equal("0 l1", line),
                line => Assert.Equal("1 l2", line),
                line => Assert.Equal("", line)
            );
        }
    }
}
