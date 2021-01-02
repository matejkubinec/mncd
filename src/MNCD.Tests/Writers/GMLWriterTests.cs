using MNCD.Core;
using MNCD.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MNCD.Tests.Writers
{
    public class GMLWriterTests
    {
        private readonly GMLWriter _writer = new GMLWriter();

        [Fact]
        public void NetworkIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => _writer.ToGML(null));
        }

        [Fact]
        public void ConvertsEmptyNetwork()
        {
            var network = new Network();
            var gml = _writer.ToGML(network);

            var expected = new StringBuilder()
                .AppendLine("graph [")
                .AppendLine("  id 0")
                .AppendLine("  directed 0")
                .AppendLine("]")
                .ToString();

            Assert.Equal(gml, expected);
        }

        [Fact]
        public void ConvertsNetworkWithOneActor()
        {
            var network = new Network
            {
                Actors = new List<Actor>
                {
                    new Actor(1, "Actor One")
                }
            };
            var gml = _writer.ToGML(network);

            var expected = new StringBuilder()
                .AppendLine("graph [")
                .AppendLine("  id 0")
                .AppendLine("  directed 0")
                .AppendLine("  node [")
                .AppendLine("    id 1")
                .AppendLine("    label \"Actor One\"")
                .AppendLine("  ]")
                .AppendLine("]")
                .ToString();

            Assert.Equal(gml, expected);
        }

        [Fact]
        public void ConvertsNetworkWithOneActorAndOneEdge()
        {
            var actors = new List<Actor>
            {
                new Actor(1, "Actor One")
            };
            var network = new Network
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "Layer One",
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[0])
                        }
                    }
                }
            };
            var gml = _writer.ToGML(network);

            var expected = new StringBuilder()
                .AppendLine("graph [")
                .AppendLine("  id 0")
                .AppendLine("  directed 0")
                .AppendLine("  node [")
                .AppendLine("    id 1")
                .AppendLine("    label \"Actor One\"")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 1")
                .AppendLine("    target 1")
                .AppendLine("    layer 1.0")
                .AppendLine("  ]")
                .AppendLine("]")
                .ToString();

            Assert.Equal(gml, expected);
        }

        [Fact]
        public void ConvertsNetworkWithMultipleLayers()
        {
            var actors = new List<Actor>
            {
                new Actor(1, "Actor One"),
                new Actor(2, "Actor Two"),
            };
            var network = new Network
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "Layer One",
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[0])
                        }
                    },
                    new Layer
                    {
                        Name = "Layer Two",
                        Edges = new List<Edge>
                        {
                            new Edge(actors[1], actors[1])
                        }
                    }
                }
            };
            var gml = _writer.ToGML(network);

            var expected = new StringBuilder()
                .AppendLine("graph [")
                .AppendLine("  id 0")
                .AppendLine("  directed 0")
                .AppendLine("  node [")
                .AppendLine("    id 1")
                .AppendLine("    label \"Actor One\"")
                .AppendLine("  ]")
                .AppendLine("  node [")
                .AppendLine("    id 2")
                .AppendLine("    label \"Actor Two\"")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 1")
                .AppendLine("    target 1")
                .AppendLine("    layer 1.0")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 2")
                .AppendLine("    target 2")
                .AppendLine("    layer 2.0")
                .AppendLine("  ]")
                .AppendLine("]")
                .ToString();

            Assert.Equal(gml, expected);
        }

        [Fact]
        public void ConvertsNetworkWithInterlayerEdges()
        {
            var actors = new List<Actor>
            {
                new Actor(1, "Actor One"),
                new Actor(2, "Actor Two"),
            };
            var layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "Layer One",
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[0])
                        }
                    },
                    new Layer
                    {
                        Name = "Layer Two",
                        Edges = new List<Edge>
                        {
                            new Edge(actors[1], actors[1])
                        }
                    }
                };
            var network = new Network
            {
                Actors = actors,
                Layers = layers,
                InterLayerEdges = new List<InterLayerEdge>
                {
                    new InterLayerEdge
                    {
                        From = actors[0],
                        LayerFrom = layers[0],
                        To = actors[1],
                        LayerTo = layers[1],
                    }
                }
            };
            var gml = _writer.ToGML(network);

            var expected = new StringBuilder()
                .AppendLine("graph [")
                .AppendLine("  id 0")
                .AppendLine("  directed 0")
                .AppendLine("  node [")
                .AppendLine("    id 1")
                .AppendLine("    label \"Actor One\"")
                .AppendLine("  ]")
                .AppendLine("  node [")
                .AppendLine("    id 2")
                .AppendLine("    label \"Actor Two\"")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 1")
                .AppendLine("    target 1")
                .AppendLine("    layer 1.0")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 2")
                .AppendLine("    target 2")
                .AppendLine("    layer 2.0")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 1")
                .AppendLine("    target 2")
                .AppendLine("    layer 1.2")
                .AppendLine("  ]")
                .AppendLine("]")
                .ToString();

            Assert.Equal(gml, expected);
        }

        [Fact]
        public void ConvertsNetworkWithCommunities()
        {
            var actors = new List<Actor>
            {
                new Actor(1, "Actor One"),
                new Actor(2, "Actor Two"),
            };
            var communities = new List<Community>
            {
                new Community(actors.Where(a => a.Id == 1).ToList()),
                new Community(actors.Where(a => a.Id == 2).ToList())
            };
            var layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "Layer One",
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[0])
                        }
                    },
                    new Layer
                    {
                        Name = "Layer Two",
                        Edges = new List<Edge>
                        {
                            new Edge(actors[1], actors[1])
                        }
                    }
                };
            var network = new Network
            {
                Actors = actors,
                Layers = layers,
                InterLayerEdges = new List<InterLayerEdge>
                {
                    new InterLayerEdge
                    {
                        From = actors[0],
                        LayerFrom = layers[0],
                        To = actors[1],
                        LayerTo = layers[1],
                    }
                }
            };
            var gml = _writer.ToGML(network, communities);

            var expected = new StringBuilder()
                .AppendLine("graph [")
                .AppendLine("  id 0")
                .AppendLine("  directed 0")
                .AppendLine("  node [")
                .AppendLine("    id 1")
                .AppendLine("    label \"Actor One\"")
                .AppendLine("    community 1")
                .AppendLine("  ]")
                .AppendLine("  node [")
                .AppendLine("    id 2")
                .AppendLine("    label \"Actor Two\"")
                .AppendLine("    community 2")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 1")
                .AppendLine("    target 1")
                .AppendLine("    layer 1.0")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 2")
                .AppendLine("    target 2")
                .AppendLine("    layer 2.0")
                .AppendLine("  ]")
                .AppendLine("  edge [")
                .AppendLine("    source 1")
                .AppendLine("    target 2")
                .AppendLine("    layer 1.2")
                .AppendLine("  ]")
                .AppendLine("]")
                .ToString();

            Assert.Equal(gml, expected);
        }
    }
}
