using System.Collections.Generic;
using MNCD.Core;
using MNCD.Tests.Helpers;
using Xunit;
using MNCD.Extensions;
using MNCD.Generators;

namespace MNCD.Tests.Core
{
    public class NetworkTests
    {
        [Fact]
        public void LayerDegreesDictTests()
        {
            var actors = ActorHelper.Get(4);
            var edges = new List<Edge>
            {
                new Edge(actors[0], actors[1]),
                new Edge(actors[0], actors[2]),
                new Edge(actors[0], actors[3])
            };
            var layer = new Layer(edges);
            var network = new Network(layer, actors);
            var degreesDict = network.LayerDegreesDict(0);

            Assert.Collection(degreesDict.Keys,
                a => Assert.Equal(actors[0], a),
                a => Assert.Equal(actors[1], a),
                a => Assert.Equal(actors[2], a),
                a => Assert.Equal(actors[3], a)
            );
            Assert.Collection(degreesDict.Values,
                i => Assert.Equal(3, i),
                i => Assert.Equal(1, i),
                i => Assert.Equal(1, i),
                i => Assert.Equal(1, i)
            );
        }
 
        [Fact]
        public void LayerToAdjencyMatrixTests()
        {
            var network = new CompleteGraphGenerator().Generate(3);
            var adj = network.LayerToAdjencyMatrix(0);

            Assert.Equal(adj, new double[,]
                {
                    { 0, 1, 1 },
                    { 1, 0, 1 },                    
                    { 1, 1, 0 }
                }
            );
        }
    }
}