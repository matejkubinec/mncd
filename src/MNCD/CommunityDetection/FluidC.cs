using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.CommunityDetection
{
    // https://arxiv.org/pdf/1703.09307.pdf
    public class FluidC
    {
        public void Compute(Network network, int k)
        {
            if (k < 1 || k > network.Actors.Count)
            {
                throw new ArgumentException("K must be greater than 0 and less or equal then number of actors.");
            }

            var communities = Initialize(network, k);
            var densities = communities.ToDictionary(c => c, c => GetDensity(c));

            // TODO: Implement FluidC
        }

        private void SuperStep(Network network)
        {
            var actors = network.Actors;
        }

        private double GetDensity(Community community)
        {
            return 1.0 / community.Actors.Count();
        }

        private IEnumerable<Community> Initialize(Network network, int k)
        {
            return GetRandomActors(network, k).Select(a => new Community(a));
        }

        private IEnumerable<Actor> GetRandomActors(Network network, int k)
        {
            var random = new Random();
            var actors = new List<Actor>();

            while (actors.Count() != k)
            {
                var index = random.Next(0, network.Actors.Count());
                var actor = network.Actors.ElementAt(index);

                if (actors.Contains(actor))
                {
                    continue;
                }

                actors.Add(actor);
            }

            return actors;
        }
    }
}