using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;
using MNCD.Extensions;

namespace MNCD.CommunityDetection.SingleLayer
{
    // https://arxiv.org/pdf/1703.09307.pdf
    public class FluidC
    {
        private static Random Random = new Random();

        public IEnumerable<Community> Compute(Network network, int k, int maxIterations = 100)
        {
            if (k <= 0) throw new ArgumentException("K must be greater than zero.");
            if (maxIterations <= 1) throw new ArgumentException("MaxIterations must be greater than 1.");
            if (network.LayerCount != 1) throw new ArgumentException("Network must have only one layer.");
            if (network.Actors.Count < k) throw new ArgumentException("K must be less than number of actors.");

            var maxDensity = 1.0;
            var actors = network.Actors.OrderBy(r => Random.NextDouble());
            var neighbours = network.Layers.First().GetNeighboursDict();
            var communities = actors.Take(k).ToDictionary(a => a, a => new Community(a));
            var density = communities.ToDictionary(c => c.Value, c => maxDensity);

            var iterations = 0;
            var shouldContinue = true;

            while (shouldContinue && iterations < maxIterations)
            {
                shouldContinue = false;
                iterations += 1;

                // Random Order
                actors = network.Actors.OrderBy(r => Random.NextDouble());

                foreach (var actor in actors)
                {
                    var counter = new Dictionary<Community, double>();

                    if (communities.ContainsKey(actor))
                    {
                        counter[communities[actor]] = density[communities[actor]];
                    }

                    if (neighbours.ContainsKey(actor))
                    {
                        foreach (var neighbour in neighbours[actor])
                        {
                            if (communities.ContainsKey(neighbour))
                            {
                                counter[communities[neighbour]] = density[communities[neighbour]];
                            }
                        }
                    }

                    Community newCommunity;
                    if (counter.Keys.Count > 0)
                    {
                        // Check communities with highest density
                        var maximal = counter.Values.Max();
                        var bestCommunities = counter.Where(c => (maximal - c.Value) < 0.0001);

                        // Check if actor is in a best community
                        if (bestCommunities.Any(b => b.Key.Actors.Contains(actor)))
                        {
                            newCommunity = communities[actor];
                        }
                        else
                        {
                            shouldContinue = true;

                            // Randomly choose new community
                            newCommunity = bestCommunities.OrderBy(c => Random.NextDouble()).First().Key;

                            // Update older community
                            if (communities.ContainsKey(actor))
                            {
                                communities[actor].Actors.Remove(actor);
                                density[communities[actor]] = maxDensity / communities[actor].Size;
                            }

                            // Update new community
                            communities[actor] = newCommunity;
                            communities[actor].Actors.Add(actor);
                            density[communities[actor]] = maxDensity / communities[actor].Size;
                        }
                    }
                }
            }

            return communities.Values.Distinct();
        }
    }
}