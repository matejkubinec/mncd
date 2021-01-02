using System.Collections.Generic;
using MNCD.Core;
using MNCD.Evaluation;
using MNCD.Evaluation.MultiLayer;
using Xunit;

namespace MNCD.Tests.Evaluation.MultiLayer
{
    public class HomogenityTests
    {
        [Fact]
        public void HomogenityEqualsOne()
        {
            var actors = new List<Actor>
            {
                new Actor(1, "a1"),
                new Actor(2, "a2"),
                new Actor(3, "a3"),
                new Actor(4, "a4"),
                new Actor(5, "a5"),
                new Actor(6, "a6")
            };
            var network = new Network
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
                    },
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
            var community = new Community(actors.GetRange(0, 4));
            var homogenity = Homogenity.Compute(community, network);

            Assert.Equal(1, homogenity);
        }

        [Fact]
        public void HomogenityEqualsMinusOne()
        {
            var actors = new List<Actor>
            {
                new Actor(1, "a1"),
                new Actor(2, "a2"),
                new Actor(3, "a3"),
                new Actor(4, "a4"),
                new Actor(5, "a5"),
                new Actor(6, "a6")
            };
            var network = new Network
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
                    },
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                        }
                    }
                }
            };
            var community = new Community(actors.GetRange(0, 4));
            var homogenity = Homogenity.Compute(community, network);

            Assert.Equal(-1, homogenity);
        }

        [Fact]
        public void HomogenityEqualsZero()
        {
            var actors = new List<Actor>
            {
                new Actor(1, "a1"),
                new Actor(2, "a2"),
                new Actor(3, "a3"),
                new Actor(4, "a4")
            };
            var network = new Network
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[1]),
                            new Edge(actors[2], actors[3])
                        }
                    },
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[0], actors[1])
                        }
                    }
                }
            };
            var community = new Community(actors);
            var homogenity = Homogenity.Compute(community, network);

            Assert.Equal(0, homogenity);
        }
    }
}