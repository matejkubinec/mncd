using MNCD.Core;
using MNCD.Neighbourhood;
using MNCD.Tests.Helpers;
using System.Collections.Generic;
using Xunit;

namespace MNCD.Tests.Measures
{
    public class BetweennessTests
    {
        [Fact]
        public void Betweenness_Test1()
        {
            var actor1 = new Actor("A1");
            var actor2 = new Actor("A2");
            var actors = new List<Actor> { actor1, actor2 };
            var edge1 = new Edge(actor1, actor2);
            var edges = new List<Edge> { edge1 };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);

            var betweeness = Betweenness.Get(network);

            Assert.Equal(1, betweeness[actor1]);
            Assert.Equal(1, betweeness[actor2]);
        }

        [Fact]
        public void Betweenness_Test2()
        {
            var actor1 = new Actor("A1");
            var actor2 = new Actor("A2");
            var actor3 = new Actor("A3");
            var actor4 = new Actor("A4");
            var actors = new List<Actor> { actor1, actor2, actor3, actor4 };
            var edge1 = new Edge(actor1, actor2);
            var edge2 = new Edge(actor3, actor4);
            var edges = new List<Edge> { edge1, edge2 };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);

            var betweeness = Betweenness.Get(network);

            Assert.Equal(1, betweeness[actor1]);
            Assert.Equal(1, betweeness[actor2]);
            Assert.Equal(1, betweeness[actor3]);
            Assert.Equal(1, betweeness[actor4]);
        }

        [Fact]
        public void Betweenness_Test3()
        {
            var actor1 = new Actor("A1");
            var actor2 = new Actor("A2");
            var actor3 = new Actor("A3");
            var actor4 = new Actor("A4");
            var actors = new List<Actor> { actor1, actor2, actor3, actor4 };
            var edge1 = new Edge(actor1, actor2);
            var edge2 = new Edge(actor2, actor3);
            var edge3 = new Edge(actor3, actor4);
            var edge4 = new Edge(actor4, actor1);
            var edges = new List<Edge> { edge1, edge2, edge3, edge4 };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);

            var betweeness = Betweenness.Get(network);

            Assert.Equal(1, betweeness[actor1]);
            Assert.Equal(1, betweeness[actor2]);
            Assert.Equal(1, betweeness[actor3]);
            Assert.Equal(1, betweeness[actor4]);
        }
    }
}
