using MNCD.CommunityDetection.SingleLayer;
using MNCD.CommunityDetection.MultiLayer;
using MNCD.Tests.Helpers;
using Xunit;

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
    }
}