using MNCD.Flattening;
using System.Linq;
using Xunit;

namespace MNCD.Tests
{
    public class MergeFlatteningTests
    {
        [Fact]
        public void TwoLayerUndirected()
        {
            var network = TestHelper.TwoLayerUndirected;
            var flattened = new MergeFlattening().Merge(network);

            Assert.NotNull(flattened);
            Assert.NotEmpty(flattened.Layers);
            Assert.NotEmpty(flattened.FirstLayer.Edges);
            Assert.Equal(1, flattened.Layers.Count);
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
        public void TwoLayerDirected()
        {
            var network = TestHelper.TwoLayerDirected;
            var flattened = new MergeFlattening().Merge(network);

            Assert.Equal(1, flattened.Layers.Count);
            Assert.True(flattened.Layers[0].IsDirected);
            Assert.Equal(TestHelper.Actors3, flattened.Actors);
            Assert.Equal(3, flattened.Layers[0].Edges.Count);
            Assert.Equal(1, flattened.Layers[0].Edges.First(e => e.From == TestHelper.A1 && e.To == TestHelper.A2).Weight);
            Assert.Equal(1, flattened.Layers[0].Edges.First(e => e.From == TestHelper.A1 && e.To == TestHelper.A3).Weight);
            Assert.Equal(1, flattened.Layers[0].Edges.First(e => e.From == TestHelper.A3 && e.To == TestHelper.A1).Weight);
        }

        [Fact]
        public void TwoLayerCombination()
        {
            var network = TestHelper.TwoLayerCombination;
            var flattened = new MergeFlattening().Merge(network);

            Assert.Equal(1, flattened.Layers.Count);
            Assert.True(flattened.Layers[0].IsDirected);
            Assert.Equal(TestHelper.Actors3, flattened.Actors);
            Assert.Equal(4, flattened.Layers[0].Edges.Count);
            Assert.Equal(1, flattened.Layers[0].Edges.First(e => e.From == TestHelper.A2 && e.To == TestHelper.A1).Weight);
            Assert.Equal(1, flattened.Layers[0].Edges.First(e => e.From == TestHelper.A1 && e.To == TestHelper.A2).Weight);
            Assert.Equal(1, flattened.Layers[0].Edges.First(e => e.From == TestHelper.A1 && e.To == TestHelper.A3).Weight);
            Assert.Equal(2, flattened.Layers[0].Edges.First(e => e.From == TestHelper.A3 && e.To == TestHelper.A1).Weight);
        }
    }
}
