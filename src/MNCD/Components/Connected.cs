using MNCD.Core;
using MNCD.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Components
{
    /// <summary>
    /// Implements algorithm for computing connected components.
    /// </summary>
    public static class Connected
    {
        /// <summary>
        /// Gets connected components in supplied layer.
        /// </summary>
        /// <param name="layer">Layer.</param>
        /// <param name="actors">Network actors.</param>
        /// <returns>List of connected components.</returns>
        public static IEnumerable<List<Actor>> GetConnectedComponents(this Layer layer, IEnumerable<Actor> actors = null)
        {
            var seen = new HashSet<Actor>();
            foreach (var actor in actors ?? layer.GetLayerActors())
            {
                if (!seen.Contains(actor))
                {
                    var component = GetComponent(layer, actor)
                        .Distinct()
                        .ToList();

                    yield return component;

                    foreach (var actorInComponent in component)
                    {
                        seen.Add(actorInComponent);
                    }
                }
            }

            yield break;
        }

        /// <summary>
        /// Get connected component.
        /// </summary>
        /// <param name="layer">Layer in which to compute component.</param>
        /// <param name="start">Start actor.</param>
        /// <returns>Connected component.</returns>
        internal static IEnumerable<Actor> GetComponent(Layer layer, Actor start)
        {
            var neighboursDict = layer.GetNeighboursDict();
            var seen = new HashSet<Actor>();
            var nextLevel = new HashSet<Actor> { start };

            while (nextLevel.Any())
            {
                var thisLevel = nextLevel;
                nextLevel = new HashSet<Actor>();

                foreach (var actor in thisLevel)
                {
                    if (!seen.Contains(actor))
                    {
                        seen.Add(actor);

                        yield return actor;

                        if (!neighboursDict.ContainsKey(actor))
                        {
                            continue;
                        }

                        foreach (var neighbour in neighboursDict[actor])
                        {
                            nextLevel.Add(neighbour);
                        }
                    }
                }
            }

            yield break;
        }
    }
}