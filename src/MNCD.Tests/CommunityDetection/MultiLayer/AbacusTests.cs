using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.CommunityDetection.SingleLayer;
using MNCD.CommunityDetection.MultiLayer;
using MNCD.Core;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.CommunityDetection.MultiLayer
{
    public class AbacusTests
    {
        [Fact]
        public void Test()
        {
            var n = NetworkHelper.TwoLayerTriangles();
            var abacus = new ABACUS();
            abacus.Apply(n, n => new Louvain().Apply(n), 2);
        }
    }
}