using MNCD.Core;
using MNCD.Flattening;
using MNCD.Tests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace MNCD.Tests.Flattening
{
    public class LocalSimplificationTests
    {
        private readonly LocalSimplification localSimplification = new LocalSimplification();

        [Fact]
        public void ValidateInputArguments()
        {
            Assert.Throws<ArgumentException>(() => localSimplification.BasedOnLayerRelevance(new Network(new Layer()), new double[] { }, 1.0));
            Assert.Throws<ArgumentException>(() => localSimplification.BasedOnLayerRelevance(new Network(new Layer()), new double[] { 1.0, 2.0 }, 1.0));
        }

        [Fact]
        public void TwoLayers()
        {
            var actors = ActorHelper.Get(2);
            var edges = new List<Edge> { new Edge(actors[0], actors[1]) };
            var layer1 = new Layer(edges);
            var layer2 = new Layer(edges);
            var network = new Network();
            network.Actors = actors;
            network.Layers.Add(layer1);
            network.Layers.Add(layer2);
            var threshold = 0.5;
            var relevances = new double[] { 0.51, 0.49 };
            var flattened = localSimplification.BasedOnLayerRelevance(network, relevances, threshold);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.Layers);
            Assert.Equal(actors, flattened.Actors);
            Assert.Collection(flattened.FirstLayer.Edges,
                e => 
                {
                    Assert.Equal(actors[0], e.From);
                    Assert.Equal(actors[1], e.To);
                    Assert.Equal(1, e.Weight);
                }
            );
        }

        [Fact]
        public void TwoLayerInterLayer()
        {
            var actors = ActorHelper.Get(2);
            var edges = new List<Edge> { new Edge(actors[0], actors[1]) };
            var layer1 = new Layer(edges);
            var layer2 = new Layer(edges);
            var interLayerEdges = new List<InterLayerEdge>
            {
                new InterLayerEdge
                {
                    From = actors[0],
                    To = actors[1],
                    LayerFrom = layer1,
                    LayerTo = layer2
                }
            };
            var network = new Network();
            network.Actors = actors;
            network.InterLayerEdges = interLayerEdges;
            network.Layers.Add(layer1);
            network.Layers.Add(layer2);
            var threshold = 0.5;
            var relevances = new double[] { 0.51, 0.49 };
            var flattened = localSimplification.BasedOnLayerRelevance(network, relevances, threshold);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.Layers);
            Assert.Equal(actors, flattened.Actors);
            Assert.Collection(flattened.FirstLayer.Edges,
                e => 
                {
                    Assert.Equal(actors[0], e.From);
                    Assert.Equal(actors[1], e.To);
                    Assert.Equal(2, e.Weight);
                }
            );
        }

        [Fact]
        public void TwoLayersWeighted()
        {
            var actors = ActorHelper.Get(2);
            var edges = new List<Edge> { new Edge(actors[0], actors[1]) };
            var layer1 = new Layer(edges);
            var layer2 = new Layer(edges);
            var network = new Network();
            network.Actors = actors;
            network.Layers.Add(layer1);
            network.Layers.Add(layer2);
            var threshold = 0.5;
            var relevances = new double[] { 0.51, 0.49 };
            var flattened = localSimplification.BasedOnLayerRelevance(network, relevances, threshold, true);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.Layers);
            Assert.Equal(actors, flattened.Actors);
            Assert.Collection(flattened.FirstLayer.Edges,
                e => 
                {
                    Assert.Equal(actors[0], e.From);
                    Assert.Equal(actors[1], e.To);
                    Assert.Equal(0.51, e.Weight);
                }
            );
        }

        [Fact]
        public void TwoLayerInterLayerWeighted()
        {
            var actors = ActorHelper.Get(2);
            var edges = new List<Edge> { new Edge(actors[0], actors[1]) };
            var layer1 = new Layer(edges);
            var layer2 = new Layer(edges);
            var interLayerEdges = new List<InterLayerEdge>
            {
                new InterLayerEdge
                {
                    From = actors[0],
                    To = actors[1],
                    LayerFrom = layer1,
                    LayerTo = layer2
                }
            };
            var network = new Network();
            network.Actors = actors;
            network.InterLayerEdges = interLayerEdges;
            network.Layers.Add(layer1);
            network.Layers.Add(layer2);
            var threshold = 0.5;
            var relevances = new double[] { 0.51, 0.49 };
            var flattened = localSimplification.BasedOnLayerRelevance(network, relevances, threshold, true);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.Layers);
            Assert.Equal(actors, flattened.Actors);
            Assert.Collection(flattened.FirstLayer.Edges,
                e => 
                {
                    Assert.Equal(actors[0], e.From);
                    Assert.Equal(actors[1], e.To);
                    Assert.Equal(1.01, e.Weight);
                }
            );
        }
    }
}
