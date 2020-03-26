using MNCD.Core;
using MNCD.Neighbourhood;
using MNCD.Tests.Helpers;
using System.Collections.Generic;
using Xunit;

namespace MNCD.Tests.Neighbourhood
{
    public class MultiLayerNeighbourhoodTests
    {
        [Fact]
        public void MN_0()
        {
            var network = GetNetwork();
            var actors = network.Actors;
            var x = actors[0];
            var alpha = 2;
            var mn = MultiLayerNeighbourhood.GetMN(network, x, alpha);

            Assert.Collection(mn,
                a => Assert.Equal(actors[1], a),
                a => Assert.Equal(actors[2], a)
            );
        }

        [Fact]
        public void MN_1()
        {
            var network = GetNetwork();
            var actors = network.Actors;
            var x = actors[1];
            var alpha = 2;
            var mn = MultiLayerNeighbourhood.GetMN(network, x, alpha);

            Assert.Collection(mn,
                a => Assert.Equal(actors[0], a),
                a => Assert.Equal(actors[2], a)
            );
        }

        [Fact]
        public void MN_2()
        {
            var network = GetNetwork();
            var actors = network.Actors;
            var x = actors[2];
            var alpha = 2;
            var mn = MultiLayerNeighbourhood.GetMN(network, x, alpha);

            Assert.Collection(mn,
                a => Assert.Equal(actors[0], a),
                a => Assert.Equal(actors[1], a),
                a => Assert.Equal(actors[3], a)
            );
        }

        private Network GetNetwork()
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
            var layers = new List<Layer>
            {
                new Layer(edges),
                new Layer(edges),
            };
            return new Network(layers, actors);
        }
    }
}
