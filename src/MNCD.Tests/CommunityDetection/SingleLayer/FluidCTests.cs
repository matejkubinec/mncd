using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.CommunityDetection.SingleLayer;
using MNCD.Core;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.CommunityDetection.SingleLayer
{
    public class FluidCTests
    {
        private readonly FluidC fluidC = new FluidC();

        [Fact]
        public void TestArguments()
        {
            var actors = ActorHelper.Get(10);
            var network = new Network();
            network.Actors = actors;
            network.Layers.Add(new Layer());

            // k <= 0
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, 0, 100));
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, -1, 100));

            // k > actors.Count            
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, 11, 100));

            // maxIterations >= 1
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, 5, 0));
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, 5, 1));

            // layerCount != 1
            network.Layers.Add(new Layer());
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, 5, 10));

            network.Layers = new List<Layer>();
            Assert.Throws<ArgumentException>(() => fluidC.Compute(network, 5, 10));
        }

        [Fact]
        public void TestSingleActor()
        {
            var actor = new Actor("a0");
            var network = new Network();
            network.Actors.Add(actor);
            network.Layers.Add(new Layer());

            var expected = new List<Community> { new Community(actor) };
            var actual = fluidC.Compute(network, 1).ToList();

            Assert.Equal(actual.Count, expected.Count);
            Assert.Equal(actual[0].Actors.Count, expected[0].Actors.Count);
            Assert.Equal(actual[0].Actors[0], expected[0].Actors[0]);
        }

        [Fact]
        public void TestTwoActors()
        {
            var actors = ActorHelper.Get(2);
            var network = new Network();
            network.Actors = actors;
            network.Layers.Add(new Layer
            {
                Edges = new List<Edge>
                {
                    new Edge(actors[0], actors[1])
                }
            });

            var expected = new List<Community>
            {
                new Community(actors[0]),
                new Community(actors[1]),
            };
            var actual = fluidC.Compute(network, 2)
                .OrderBy(c => c.Actors.First().Name)
                .ToList();

            Assert.Equal(actual.Count, expected.Count);

            Assert.Equal(actual[0].Actors.Count, expected[0].Actors.Count);
            Assert.Equal(actual[0].Actors[0], expected[0].Actors[0]);

            Assert.Equal(actual[1].Actors.Count, expected[1].Actors.Count);
            Assert.Equal(actual[1].Actors[0], expected[1].Actors[0]);
        }

        [Fact]
        public void TestTwoCliqueCommunities()
        {
            //    O           4
            //    | \       / |
            //    |  2  -- 3  | 
            //    | /       \ |
            //    1           5

            var actors = ActorHelper.Get(7);
            var network = new Network();
            var ed = new List<Edge>
            {
                new Edge(actors[0], actors[1]),
                new Edge(actors[0], actors[2]),
                new Edge(actors[1], actors[2]),
                new Edge(actors[2], actors[3]),
                new Edge(actors[3], actors[4]),
                new Edge(actors[3], actors[5]),
                new Edge(actors[4], actors[5])
            };
            network.Actors = actors;
            network.Layers.Add(new Layer() { Edges = ed });

            var expected = new List<Community>
            {
                new Community(actors.GetRange(0, 3)),
                new Community(actors.GetRange(3, 3)),
            };
            var actual = fluidC.Compute(network, 2)
                .OrderBy(c => c.Actors.First().Name)
                .ToList();

            actual[0] = new Community(actual[0].Actors.OrderBy(a => a.Name));
            actual[1] = new Community(actual[1].Actors.OrderBy(a => a.Name));


            Assert.Equal(expected.Count, actual.Count);

            Assert.Equal(expected[0].Actors.Count, actual[0].Actors.Count);
            Assert.Equal(expected[0].Actors[0], actual[0].Actors[0]);
            Assert.Equal(expected[0].Actors[1], actual[0].Actors[1]);
            Assert.Equal(expected[0].Actors[2], actual[0].Actors[2]);

            Assert.Equal(expected[1].Actors.Count, actual[1].Actors.Count);
            Assert.Equal(expected[1].Actors[0], actual[1].Actors[0]);
            Assert.Equal(expected[1].Actors[1], actual[1].Actors[1]);
            Assert.Equal(expected[1].Actors[2], actual[1].Actors[2]);
        }
    }
}