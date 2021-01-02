using System.Collections.Generic;
using MNCD.Clique;
using MNCD.Core;
using Xunit;

namespace MNCD.Tests.Clique
{
    public class BronKerboschTests
    {
        [Fact]
        public void InitNodesTest()
        {
            var network = new Network()
            {
                Actors = new List<Actor>
                {
                    new Actor(0, "a0"),
                    new Actor(1, "a1"),
                    new Actor(2, "a2")
                }
            };

            var nodes = new BronKerbosch().InitNodes(network);

            Assert.Equal(network.Actors, nodes);
        }

        [Fact]
        public void InitNeighboursTest()
        {
            var actors = new List<Actor>
            {
                new Actor(0, "a0"),
                new Actor(1, "a1"),
                new Actor(2, "a2")
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[1]),
                            new Edge(actors[1], actors[2]),
                        }
                    }
                }
            };

            var neighbours = new BronKerbosch().InitNeighbours(network);

            Assert.Collection(neighbours[actors[0]],
                item => Assert.Equal(item, actors[1]));

            Assert.Collection(neighbours[actors[1]],
                item => Assert.Equal(item, actors[0]),
                item => Assert.Equal(item, actors[2]));

            Assert.Collection(neighbours[actors[2]],
                item => Assert.Equal(item, actors[1]));
        }

        [Fact]
        public void ComputeCompleteTwo()
        {
            var actors = new List<Actor>
            {
                new Actor(0, "a0"),
                new Actor(1, "a1")
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[1])
                        }
                    }
                }
            };

            var cliques = new BronKerbosch().GetMaximalCliques(network);

            Assert.Collection(cliques, item => Assert.Equal(item, actors));
        }

        [Fact]
        public void ComputeCompleteThree()
        {
            var actors = new List<Actor>
            {
                new Actor(0, "a0"),
                new Actor(1, "a1"),
                new Actor(2, "a2"),
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[1]),
                            new Edge(actors[0], actors[2]),
                            new Edge(actors[1], actors[2])
                        }
                    }
                }
            };

            var cliques = new BronKerbosch().GetMaximalCliques(network);

            Assert.Collection(cliques, item => Assert.Equal(item, actors));
        }

        [Fact]
        public void ComputeTwoCompleteThreeConnected()
        {
            var actors = new List<Actor>
            {
                new Actor(0, "a0"),
                new Actor(1, "a1"),
                new Actor(2, "a2"),
                new Actor(3, "a3"),
                new Actor(4, "a4"),
            };
            var network = new Network()
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[1]),
                            new Edge(actors[0], actors[2]),
                            new Edge(actors[1], actors[2]),
                            new Edge(actors[2], actors[3]),
                            new Edge(actors[2], actors[4]),
                            new Edge(actors[3], actors[4]),
                        }
                    }
                }
            };

            var cliques = new BronKerbosch().GetMaximalCliques(network);

            Assert.Collection(cliques,
                item => Assert.Equal(item, actors.GetRange(0, 3)),
                item => Assert.Equal(item, actors.GetRange(2, 3)));
        }
    }
}
