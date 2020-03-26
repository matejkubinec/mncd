using MNCD.CommunityDetection.SingleLayer;
using MNCD.CommunityDetection.MultiLayer;
using MNCD.Tests.Helpers;
using Xunit;
using MNCD.Generators;
using System;
using System.Linq;

namespace MNCD.Tests.CommunityDetection.MultiLayer
{
    public class AbacusTests
    {
        [Fact]
        public void TwoLayerTriangles()
        {
            var N = NetworkHelper.TwoLayerTriangles();
            var A = N.Actors;
            var abacus = new ABACUS();
            var communities = abacus.Apply(N, n => new Louvain().Apply(n), 2);

            Assert.Collection(communities,
                com => Assert.Collection(com.Actors,
                    a => Assert.Equal(A[0], a),
                    a => Assert.Equal(A[1], a),
                    a => Assert.Equal(A[2], a)
                ),
                com => Assert.Collection(com.Actors,
                    a => Assert.Equal(A[3], a),
                    a => Assert.Equal(A[4], a),
                    a => Assert.Equal(A[5], a)
                )
            );
        }

        [Fact]
        public void TestRandom()
        {
            var random = new Random();
            var generator = new RandomMultiLayerGenerator();
            for (var i = 8; i < 15; i++)
            {
                var p = random.Next(6, 10) / 10.0;
                var l = random.Next(2, 10);
                var n = generator.Generate(i, l, p);
                var a = n.Actors;
                var abacus = new ABACUS();
                var communities = abacus.Apply(n, n => new Louvain().Apply(n), 2);
                var actors = communities.SelectMany(c => c.Actors).Distinct();

                Assert.Empty(a.Except(actors));
                communities.ForEach(c => Assert.NotEmpty(c.Actors));
            }
        }
    }
}