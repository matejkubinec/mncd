using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Evaluation;
using MNCD.Core;
using MNCD.Tests.Helpers;
using Xunit;
using MNCD.Evaluation.SingleLayer;

namespace MNCD.Tests.Evaluation.SingleLayer
{
    public class ModularityTests
    {
        [Fact]
        public void Zero()
        {
            for (var i = 0; i < 10; i++)
            {
                var network = new MNCD.Generators.CompleteGraphGenerator().Generate(2 + i);
                var communities = new List<Community> { new Community(network.Actors) };
                var modularity = Modularity.Compute(network, communities);

                Assert.Equal(0.0, modularity);
            }
        }

        [Fact]
        public void OptimalParition()
        {
            var N = NetworkHelper.InitBarabasi();
            var A = N.Actors;
            var C = new List<Community>
            {
                new Community(A[0], A[1], A[2], A[3], A[4]),
                new Community(A[5], A[6], A[7], A[8])
            };
            var M = Modularity.Compute(N, C);

            Assert.Equal(0.41, Math.Round(M, 2));
        }

        [Fact]
        public void SuboptimalParition()
        {
            var N = NetworkHelper.InitBarabasi();
            var A = N.Actors;
            var C = new List<Community>
            {
                new Community(A[0], A[1], A[2]),
                new Community(A[3], A[4], A[5], A[6], A[7], A[8])
            };
            var M = Modularity.Compute(N, C);

            Assert.Equal(0.22, Math.Round(M, 2));
        }

        [Fact]
        public void SingleCommunity()
        {
            var N = NetworkHelper.InitBarabasi();
            var A = N.Actors;
            var C = new List<Community>
            {
                new Community(A),
            };
            var M = Modularity.Compute(N, C);

            Assert.Equal(0.0, M);
        }

        [Fact]
        public void NegativeModularity()
        {
            var N = NetworkHelper.InitBarabasi();
            var A = N.Actors;
            var C = A.Select(a => new Community(a)).ToList();
            var M = Modularity.Compute(N, C);

            Assert.Equal(-0.12, Math.Round(M, 2));
        }
    }
}