using System.Collections.Generic;
using MNCD.Core;
using MNCD.Flattening;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.Flattening
{
    public class WeightedFlatteningTests
    {
        [Fact]
        public void OneLayer()
        {
            var actors = ActorHelper.Get(2);
            var edges = new List<Edge> { new Edge(actors[0], actors[1]) };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);
            var weights = new Dictionary<(Layer, Layer), double>
            { 
                { (layer, layer), 2.0 }
            };
            var flattened = new WeightedFlattening().Flatten(network, weights);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.FirstLayer.Edges);
            Assert.Collection(
                flattened.FirstLayer.Edges,
                e => Assert.True(
                    e.From == actors[0] &&
                    e.To == actors[1] &&
                    e.Weight == 2.0
                )
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
            var weights = new Dictionary<(Layer, Layer), double>
            { 
                { (layer1, layer1), 1.0 },
                { (layer1, layer2), 2.0 },
                { (layer2, layer2), 3.0 }
            };
            var flattened = new WeightedFlattening().Flatten(network, weights);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.FirstLayer.Edges);
            Assert.Collection(
                flattened.FirstLayer.Edges,
                e => Assert.True(
                    e.From == actors[0] &&
                    e.To == actors[1] &&
                    e.Weight == 6.0
                )
            );
        }
    }
}