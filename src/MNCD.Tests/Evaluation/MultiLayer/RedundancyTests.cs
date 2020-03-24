using MNCD.Core;
using MNCD.Evaluation.MultiLayer;
using MNCD.Tests.Helpers;
using System.Collections.Generic;
using Xunit;

namespace MNCD.Tests.Evaluation.MultiLayer
{
    public class RedundancyTests
    {
        [Fact]
        public void RedundancyEqualsZero()
        {
            var a = ActorHelper.Get(2);
            var e = new List<Edge>
            {
                new Edge(a[0], a[1])
            };
            var l0 = new Layer(e);
            var l1 = new Layer();
            var l = new List<Layer> { l0, l1 };
            var n = new Network(l, a);
            var c = new Community(a);

            var r = Redundancy.Compute(c, n);

            Assert.Equal(0.0, r);
        }

        [Fact]
        public void RedundancyEqualsOne()
        {
            var a = ActorHelper.Get(2);
            var e0 = new List<Edge>
            {
                new Edge(a[0], a[1])
            };
            var e1 = new List<Edge>
            {
                new Edge(a[1], a[0])
            };
            var l0 = new Layer(e0);
            var l1 = new Layer(e1);
            var l = new List<Layer> { l0, l1 };
            var n = new Network(l, a);
            var c = new Community(a);

            var r = Redundancy.Compute(c, n);

            Assert.Equal(1.0, r);
        }
    }
}
