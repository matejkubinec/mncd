using MNCD.Core;
using MNCD.Flattening;
using Xunit;

namespace MNCD.Tests
{
    public class MergeFlatteningTests
    {
        [Fact]
        public void TwoLayer()
        {
            var network = TestHelper.TwoLayerUndirected;
            var flattened = new MergeFlattening().Merge(network, true);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.Layers);
            Assert.NotEmpty(flattened.FirstLayer.Edges);
            Assert.Equal(TestHelper.Actors3, flattened.Actors);
            Assert.Collection(flattened.FirstLayer.Edges,
                e =>
                {
                    Assert.Equal(TestHelper.A1, e.From);
                    Assert.Equal(TestHelper.A2, e.To);
                    Assert.Equal(1.0, e.Weight);
                },
                e =>
                {
                    Assert.Equal(TestHelper.A1, e.From);
                    Assert.Equal(TestHelper.A3, e.To);
                    Assert.Equal(2.0, e.Weight);
                }
            );
        }

        [Fact]
        public void TwoLayerInterLayerEdge()
        {
            var network = TestHelper.TwoLayerUndirected;
            network.InterLayerEdges.Add(new InterLayerEdge
            {
                LayerFrom = network.Layers[0],
                LayerTo = network.Layers[1],
                From = network.Actors[0],
                To = network.Actors[1],
                Weight = 1
            });
            var flattened = new MergeFlattening().Merge(network, true);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.Layers);
            Assert.NotEmpty(flattened.FirstLayer.Edges);
            Assert.Equal(TestHelper.Actors3, flattened.Actors);
            Assert.Collection(flattened.FirstLayer.Edges,
                e =>
                {
                    Assert.Equal(TestHelper.A1, e.From);
                    Assert.Equal(TestHelper.A2, e.To);
                    Assert.Equal(2.0, e.Weight);
                },
                e =>
                {
                    Assert.Equal(TestHelper.A1, e.From);
                    Assert.Equal(TestHelper.A3, e.To);
                    Assert.Equal(2.0, e.Weight);
                }
            );
        }
    }
}
