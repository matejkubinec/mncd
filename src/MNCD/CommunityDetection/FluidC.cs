using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;
using MNCD.Helpers;

namespace MNCD.CommunityDetection
{
    // https://arxiv.org/pdf/1703.09307.pdf
    public class FluidC
    {
        private static Random random = new Random();

        public IList<Community> Compute(Network network, List<Actor> initialSeed)
        {
            var communities = initialSeed.Select(a => new Community(a)).ToList();
            return GetCommunties(network, communities);
        }

        public IList<Community> Compute(Network network, int k)
        {
            if (k < 1 || k > network.Actors.Count)
            {
                throw new ArgumentException("K must be greater than 0 and less or equal then number of actors.");
            }

            var communities = Initialize(network, k);
            return GetCommunties(network, communities);
        }

        private IList<Community> GetCommunties(Network network, IList<Community> initialCommunities)
        {
            var communities = initialCommunities;
            var change = false;
            do
            {
                change = false;
                var actorToCommunity = GetActorToCommunity(network.Actors, communities);
                var actorToNeighbours = GetActorToNeighbours(network.Actors, network.Layers[0].Edges);
                var communityToDensity = communities.ToDictionary(c => c, c => GetDensity(c));
                var actors = network.Actors;
                actors.Shuffle();

                foreach (var actor in actors)
                {
                    var actorCommunity = actorToCommunity[actor];
                    var newCommunity = UpdateRule(actor, actorCommunity, actorToNeighbours, actorToCommunity, communityToDensity);

                    if (actorCommunity != newCommunity)
                    {
                        change = true;
                        actorToCommunity[actor] = newCommunity;
                        newCommunity.Actors.Add(actor);

                        if (actorCommunity != null)
                        {
                            actorCommunity.Actors.Remove(actor);
                        }

                        communityToDensity = communities.ToDictionary(c => c, c => GetDensity(c));
                    }
                }

                // communities = actorToCommunity.Values.Where(c => c != null).Distinct().ToList();
            }
            while (change);

            return communities;
        }

        private Community UpdateRule(
            Actor actor,
            Community actorCommunity,
            IDictionary<Actor, IList<Actor>> actorToNeighbours,
            IDictionary<Actor, Community> actorToCommunity,
            IDictionary<Community, double> communityToDensity
            )
        {

            var neighbours = actorToNeighbours[actor];
            var max = 0.0;
            var candidates = new List<Community>();

            foreach (var neighbour in neighbours.Append(actor))
            {
                var neighbourCommunity = actorToCommunity[neighbour];

                if (neighbourCommunity == null)
                {
                    continue;
                }

                var density = communityToDensity[neighbourCommunity];
                var value = density * KroneckerDelta(actorCommunity, neighbourCommunity);

                if (value >= max)
                {
                    max = value;
                    candidates.Add(neighbourCommunity);
                }
            }

            if (candidates.Contains(actorCommunity) || candidates.Count == 0)
            {
                return actorCommunity;
            }
            else
            {
                return candidates[random.Next(candidates.Count)];
            }
        }

        private double GetDensity(Community community)
        {
            return 1.0 / community.Actors.Count();
        }

        private IList<Community> Initialize(Network network, int k)
        {
            return GetRandomActors(network, k).Select(a => new Community(a)).ToList();
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

        private IDictionary<Actor, Community> GetActorToCommunity(IList<Actor> actors, IList<Community> communities)
        {
            return actors.ToDictionary(a => a, a => communities.FirstOrDefault(c => c.Actors.Contains(a)));
        }

        private IDictionary<Actor, IList<Actor>> GetActorToNeighbours(IList<Actor> actors, IList<Edge> edges)
        {

            return actors.ToDictionary(a => a, a => (IList<Actor>)edges.Where(e => e.To == a || e.From == a).Select(e => e.From == a ? e.To : e.From).ToList());
        }

        private int KroneckerDelta(Community c1, Community c2)
        {
            return c1 == c2 ? 1 : 0;
        }
    }
}