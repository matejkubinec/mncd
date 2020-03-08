using System.Collections.Generic;
using MNCD.Core;
using MNCD.Extensions;
using Xunit;

namespace MNCD.Tests.Core
{
    public class LayerTests
    {
        [Fact]
        public void GetActors()
        {
            var actors = new List<Actor>()
            {
                new Actor("a0"),
                new Actor("a1"),
                new Actor("a2"),
                new Actor("a3")
            };
            var layer = new Layer
            {
                Edges = new List<Edge>
                {
                    new Edge(actors[0], actors[1]),
                    new Edge(actors[2], actors[3]),
                }
            };

            var expected = actors;
            var actual = layer.GetActors();


            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNeighboursDict()
        {
            var actors = new List<Actor>()
            {
                new Actor("a0"),
                new Actor("a1"),
                new Actor("a2"),
                new Actor("a3")
            };
            var layer = new Layer
            {
                Edges = new List<Edge>
                {
                    new Edge(actors[0], actors[1]),
                    new Edge(actors[2], actors[3]),
                }
            };

            var dict = layer.GetNeighboursDict();

            Assert.Collection(dict[actors[0]], item => Assert.Equal(item, actors[1]));
            Assert.Collection(dict[actors[1]], item => Assert.Equal(item, actors[0]));

            Assert.Collection(dict[actors[2]], item => Assert.Equal(item, actors[3]));
            Assert.Collection(dict[actors[3]], item => Assert.Equal(item, actors[2]));
        }
    }
}