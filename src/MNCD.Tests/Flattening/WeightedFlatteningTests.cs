using System.Collections.Generic;
using MNCD.Core;
using MNCD.Flattening;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.Flattening
{
    public class WeightedFlatteningTests
    {
        private readonly WeightedFlattening weightedFlattening = new WeightedFlattening();

        [Fact]
        public void OneLayer()
        {
            var actors = ActorHelper.Get(2);
            var edges = new List<Edge> { new Edge(actors[0], actors[1]) };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);
            var weights = new double[,]
            { 
                { 2.0 }
            };
            var flattened = weightedFlattening.Flatten(network, weights);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.FirstLayer.Edges);
            Assert.Collection(
                flattened.FirstLayer.Edges,
                e => 
                {
                    Assert.Equal(actors[0], e.From);
                    Assert.Equal(actors[1], e.To);
                    Assert.Equal(2.0, e.Weight);
                }
            );
        }

        [Fact]
        public void TwoLayersAndInterLayer()
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
                    LayerTo = layer2,
                    Weight = 1.0
                }
            };
            var network = new Network();
            network.Actors = actors;
            network.InterLayerEdges = interLayerEdges;
            network.Layers = new List<Layer> { layer1, layer2 }; 
            var weights = new double[,]
            { 
                { 1.0 , 3.0 },
                { 3.0 , 2.0 },
            };
            var flattened = weightedFlattening.Flatten(network, weights);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.FirstLayer.Edges);
            Assert.Collection(flattened.FirstLayer.Edges,
                e => 
                {
                    Assert.Equal(actors[0], e.From);
                    Assert.Equal(actors[1], e.To);
                    Assert.Equal(6.0, e.Weight);
                }
            );
        }
    }
}