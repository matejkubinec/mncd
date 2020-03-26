using MNCD.Core;
using MNCD.Neighbourhood;
using MNCD.Tests.Helpers;
using System.Collections.Generic;
using Xunit;

namespace MNCD.Tests.Measures
{
    public class CLECCTests
    {
        [Fact]
        public void CLECCEqualsZero()
        {
            // 0          4
            // | \      / |
            // |  2 -- 3  |
            // | /      \ |
            // 1          5
            var actors = ActorHelper.Get(6);
            var edge = new Edge(actors[2], actors[3]);
            var edges = new List<Edge>
            {
                new Edge(actors[0], actors[1]),
                new Edge(actors[0], actors[2]),
                new Edge(actors[1], actors[2]),
                edge,
                new Edge(actors[3], actors[4]),
                new Edge(actors[3], actors[5]),
                new Edge(actors[4], actors[5])
            };
            var layers = new List<Layer>
            {
                new Layer(edges),
                new Layer(edges),
            };
            var network = new Network(layers, actors);
            var clecc = CLECC.GetCLECC(network, edge, 2);

            Assert.Equal(0.0, clecc);
        }

        [Fact]
        public void CLECCEqualsOne()
        {
            // 0          4
            // | \      / |
            // |  2 -- 3  |
            // | /      \ |
            // 1          5
            var actors = ActorHelper.Get(6);
            var edges = new List<Edge>
            {
                new Edge(actors[0], actors[1]),
                new Edge(actors[0], actors[2]),
                new Edge(actors[1], actors[2]),
                new Edge(actors[2], actors[3]),
                new Edge(actors[3], actors[4]),
                new Edge(actors[3], actors[5]),
                new Edge(actors[4], actors[5])
            };
            var edge = edges[0];
            var layers = new List<Layer>
            {
                new Layer(edges),
                new Layer(edges),
            };
            var network = new Network(layers, actors);
            var clecc = CLECC.GetCLECC(network, edge, 2);

            Assert.Equal(1.0, clecc);
        }

        [Fact]
        public void CLECCEqualsHalf()
        {
            // 0          4
            // | \      / |
            // |  2 -- 3  |
            // | /      \ |
            // 1          5
            var actors = ActorHelper.Get(6);
            var edges = new List<Edge>
            {
                new Edge(actors[0], actors[1]),
                new Edge(actors[0], actors[2]),
                new Edge(actors[1], actors[2]),
                new Edge(actors[2], actors[3]),
                new Edge(actors[3], actors[4]),
                new Edge(actors[3], actors[5]),
                new Edge(actors[4], actors[5])
            };
            var edge = edges[1];
            var layers = new List<Layer>
            {
                new Layer(edges),
                new Layer(edges),
            };
            var network = new Network(layers, actors);
            var clecc = CLECC.GetCLECC(network, edge, 2);

            Assert.Equal(0.5, clecc);
        }
    }
}
