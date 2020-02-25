using System.Collections.Generic;
using MNCD.Core;
using MNCD.Evaluation;
using Xunit;

namespace MNCD.Tests.Evaluation
{
    public class ExclusivityTests
    {
        [Fact]
        public void ExclusivityEqualsOne()
        {
            var actors = new List<Actor>
            {
                new Actor("a1"),
                new Actor("a2"),
                new Actor("a3"),
                new Actor("a4")
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
                            new Edge(actors[0], actors[1])
                        }
                    },
                    new Layer
                    {
                        Edges = new List<Edge>
                        {
                            new Edge(actors[2], actors[3])
                        }
                    }
                }
            };
            var community = new Community(actors);

            Assert.Equal(1, Exclusivity.Compute(community, network));
        }

        [Fact]
        public void ExclusivityEqualsZero()
        {
            var actors = new List<Actor>
            {
                new Actor("a1"),
                new Actor("a2")
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
                            new Edge(actors[0], actors[1])
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

            Assert.Equal(0, Exclusivity.Compute(community, network));
        }
    }
}