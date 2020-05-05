using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNCD.CommunityDetection.SingleLayer
{
    /// <summary>
    /// Implements Label Propagation Algorithm (LPA) based on the
    /// synchronous model.
    ///
    /// Community Detection via Semi–Synchronous Label Propagation Algorithms
    /// Gennaro Cordasco and Luisa Gargano
    /// https://arxiv.org/pdf/1103.4550.pdf
    /// </summary>
    public class LabelPropagation
    {
        private static readonly Random RANDOM = new Random();

        /// <summary>
        /// Computes communities in network based on Label Propagation algorithm.
        /// </summary>
        /// <param name="network">Network in which to compute communities.</param>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        /// <returns>List of communities.</returns>
        public List<Community> GetCommunities(Network network, int maxIterations = 10_000)
        {
            network = network ?? throw new ArgumentNullException(nameof(network));

            if (maxIterations < 1)
            {
                throw new ArgumentException("MaxIterations must be greater than 0.");
            }

            if (network.LayerCount > 1)
            {
                throw new ArgumentException("A single-layer network is required.");
            }

            var labels = InitLabels(network);
            var neighbours = network.FirstLayer.GetNeighboursDict();

            for (int i = 0; i < maxIterations; i++)
            {
                var newLabels = new Dictionary<Actor, int>();
                var hasChanged = false;

                foreach (var a in network.Actors)
                {
                    if (neighbours.ContainsKey(a))
                    {
                        newLabels[a] = MostCommonLabel(a, neighbours[a], labels);
                    }
                    else
                    {
                        // Assign previous label
                        newLabels[a] = labels[a];
                    }

                    if (newLabels[a] != labels[a])
                    {
                        hasChanged = true;
                    }
                }

                if (!hasChanged)
                {
                    break;
                }

                labels = newLabels;
            }

            return LabelsToCommunities(labels);
        }

        private int MostCommonLabel(Actor actor, List<Actor> neighbours, Dictionary<Actor, int> labels)
        {
            var neighbourLabels = new Dictionary<int, int>();
            var maxCount = 0;

            foreach (var n in neighbours)
            {
                var label = labels[n];

                if (neighbourLabels.ContainsKey(label))
                {
                    neighbourLabels[label]++;
                }
                else
                {
                    neighbourLabels[label] = 1;
                }

                if (neighbourLabels[label] > maxCount)
                {
                    maxCount = neighbourLabels[label];
                }
            }

            var maximal = neighbourLabels.Where(nl => nl.Value == maxCount).ToList();

            if (maximal.Count > 1)
            {
                var original = labels[actor];

                // If original label is present in maximal, keep it.
                if (maximal.Any(m => m.Key == original))
                {
                    return original;
                }

                // If there are multiple labels with same count, take one randomly
                return maximal[RANDOM.Next(maximal.Count)].Key;
            }
            else
            {
                return maximal.First().Key;
            }
        }

        private List<Community> LabelsToCommunities(Dictionary<Actor, int> labels)
        {
            return labels
                .GroupBy(l => l.Value)
                .Select(l => new Community(l.Select(a => a.Key)))
                .ToList();
        }

        private Dictionary<Actor, int> InitLabels(Network network)
        {
            var i = 0;
            return network.Actors.ToDictionary(n => n, n => i++);
        }
    }
}