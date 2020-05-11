using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Clustering
{
    public class KMedoids<T>
    {
        private static Random Random = new Random();

        private Dictionary<(T, T), double> Similarities = new Dictionary<(T, T), double>();
        private List<T> Items = new List<T>();

        public KMedoids(List<T> items, Dictionary<(T, T), double> similarities)
        {
            Items = items;
            Similarities = similarities;
        }

        public List<List<T>> Clusterize(int k)
        {
            var medoids = InitializeMedoids(k);
            var clusters = InitializeClusters(medoids);
            var hasChanged = true;

            while (hasChanged)
            {
                foreach (var item in Items)
                {
                    var nearest = medoids.First();

                    foreach (var medoid in medoids)
                    {
                        if (Similarities[(item, medoid)] > Similarities[(item, nearest)])
                        {
                            nearest = medoid;
                        }
                    }

                    var newMedoids = new List<T>();
                    foreach (var c in clusters)
                    {
                        newMedoids.Add(NewMedoid(c.Key, c.Value));
                    }

                    if (newMedoids.All(m => medoids.Contains(m)))
                    {
                        hasChanged = false;
                    }
                    else
                    {
                        medoids = newMedoids;
                        clusters = InitializeClusters(medoids);
                    }
                }
            }

            return clusters.Select(c => c.Value).ToList();
        }

        private double AverageSimilarity(T medoid, List<T> clusterItems)
        {
            var avg = 0.0;

            foreach (var item in clusterItems)
            {
                avg += Similarities[(item, medoid)];
            }

            return avg / (double)clusterItems.Count;
        }

        private T NewMedoid(T medoid, List<T> clusterItems)
        {
            var best = medoid;
            var bestSimilarity = AverageSimilarity(best, clusterItems);

            foreach (var item in clusterItems)
            {
                var avgSimilarity = AverageSimilarity(item, clusterItems);

                if (avgSimilarity > bestSimilarity)
                {
                    best = item;
                    bestSimilarity = avgSimilarity;
                }
            }

            return best;
        }

        private Dictionary<T, List<T>> InitializeClusters(List<T> medoids)
        {
            var clusters = medoids.ToDictionary(m => m, m => new List<T>());

            foreach (var item in Items)
            {
                var nearest = medoids.First();

                foreach (var medoid in medoids)
                {
                    if (Similarities[(item, medoid)] > Similarities[(item, nearest)])
                    {
                        nearest = medoid;
                    }
                }

                clusters[nearest].Add(item);
            }

            return clusters;
        }

        private List<T> InitializeMedoids(int k)
        {
            var medoids = new List<T>();

            while (medoids.Count < k)
            {
                var medoid = Items[Random.Next(Items.Count)];

                if (!medoids.Contains(medoid))
                {
                    medoids.Add(medoid);
                }
            }

            return medoids;
        }
    }
}