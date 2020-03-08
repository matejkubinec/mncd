using System.Collections.Generic;
using System.Linq;
using MNCD.Core;
using MNCD.Extensions;

namespace MNCD.Components
{
    public static class Connected
    {
        public static IEnumerable<List<Actor>> GetConnectedComponents(this Layer layer, IEnumerable<Actor> actors = null)
        {
            var seen = new HashSet<Actor>();
            foreach (var actor in actors ?? layer.GetActors())
            {
                if (!seen.Contains(actor))
                {
                    var component = GetComponent(layer, actor)
                        .Distinct() // TODO: check if it's the same as hash set
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