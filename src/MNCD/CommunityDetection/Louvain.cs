using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.CommunityDetection
{
    // https://en.wikipedia.org/wiki/Louvain_modularity
    // https://perso.uclouvain.be/vincent.blondel/publications/08BG.pdf
    public class Louvain
    {
        private readonly int MAX_PASSES = 100;

        // TODO: handle directed networks
        public List<Dictionary<Actor, List<Actor>>> Compute(Network inputNetwork)
        {
            var network = inputNetwork;
            var passes = 0;
            var previousCount = -1;
            var hierarchies = new List<Dictionary<Actor, List<Actor>>>
            {
                network.Actors.ToDictionary(a => a, a => new List<Actor> { a })
            };

            while (passes < MAX_PASSES)
            {
                var communities = PhaseOne(network);
                network = PhaseTwo(communities, network.Layers.First().Edges);

                if (previousCount == network.Actors.Count())
                {
                    break;
                }

                previousCount = network.Actors.Count();

                hierarchies.Add(BuildHierarchyLevel(network, communities));

                passes++;
            }

            return hierarchies;
        }


        private Dictionary<Actor, List<Actor>> BuildHierarchyLevel(Network network, IEnumerable<Community> communities)
        {
            var communityActorsToActors = new Dictionary<Actor, List<Actor>>();
            for (var i = 0; i < network.Actors.Count(); i++)
            {
                communityActorsToActors[network.Actors[i]] = communities.ElementAt(i).Actors;
            }
            return communityActorsToActors;
        }

        private IEnumerable<Community> PhaseOne(Network network)
        {
            var edges = network.Layers.First().Edges;
            var m = edges.Sum(e => e.Weight);
            var actorToNeighbours = network.Actors.ToDictionary(a => a, a => GetNeighbours(a, edges));
            var communities = network.Actors.Select(a => new Community(a)).ToList();

            var moved = true;
            var passes = 0;
            while (moved && passes < MAX_PASSES)
            {
                moved = false;
                foreach (var actor in network.Actors)
                {
                    var maxGain = 0.0;
                    var newCommunity = communities.First();

                    foreach (var neighbour in actorToNeighbours[actor])
                    {
                        var neighbourCommunity = communities.First(c => c.Actors.Contains(neighbour));
                        var gain = ModularityGain2(actor, neighbourCommunity, m, edges);

                        if (gain > maxGain)
                        {
                            maxGain = gain;
                            newCommunity = neighbourCommunity;
                        }
                    }

                    if (maxGain <= 0)
                    {
                        continue;
                    }

                    moved = true;
                    communities.First(c => c.Actors.Contains(actor)).Actors.Remove(actor);
                    newCommunity.Actors.Add(actor);
                }
                passes++;
            }

            return communities.Where(c => c.Actors.Count != 0).ToList();
        }

        private Network PhaseTwo(IEnumerable<Community> communities, IEnumerable<Edge> edges)
        {
            var i = 0;
            var communityToActor = communities.ToDictionary(c => c, c => new Actor() { Name = (i++).ToString() });
            var actors = communityToActor.Values.ToList();
            var newEdges = new List<Edge>();

            foreach (var community in communities)
            {
                var from = communityToActor[community];

                foreach (var actor in community.Actors)
                {
                    foreach (var edge in edges.Where(e => e.From == actor || e.To == actor))
                    {
                        var toCommunity = communities.First(c => c.Actors.Contains(edge.From == actor ? edge.To : edge.From));
                        var to = communityToActor[toCommunity];
                        var newEdge = newEdges.FirstOrDefault(e => (e.From == from && e.To == to) || (e.From == to && e.To == from));

                        if (newEdge != null)
                        {
                            newEdge.Weight += edge.Weight / 2;
                        }
                        else
                        {
                            newEdges.Add(new Edge(from, to) { Weight = edge.Weight / 2 });
                        }
                    }
                }
            }

            return new Network
            {
                Actors = actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Edges = newEdges
                    }
                }
            };
        }

        private IEnumerable<Actor> GetNeighbours(Actor actor, IEnumerable<Edge> edges)
        {
            return edges.Where(e => e.From == actor || e.To == actor).Select(e => e.From == actor ? e.To : e.From);
        }

        private double ModularityGain2(Actor i, Community c, double m, IEnumerable<Edge> edges)
        {
            // sum of weights inside the community
            var sumIn = edges.Where(e => c.Actors.Contains(e.From) && c.Actors.Contains(e.To)).Sum(e => e.Weight);
            // sum of weights of the links incident to nodes in c
            var sumTot = edges.Where(e => c.Actors.Contains(e.From) || c.Actors.Contains(e.To)).Sum(e => e.Weight);
            // sum of weights of the links from node i
            var ki = edges.Where(e => e.From == i || e.To == i).Sum(e => e.Weight);
            // sum of weights of the links from the node i to nodes in C
            var kiIn = edges.Where(e => (e.From == i || e.To == i) && (c.Actors.Contains(e.From) || c.Actors.Contains(e.To))).Sum(e => e.Weight);

            return
                (((sumIn + 2 * kiIn) / (2 * m)) - Math.Pow((sumTot + ki) / (2 * m), 2))
                -
                ((sumIn / (2 * m)) - Math.Pow(sumTot / (2 * m), 2) - Math.Pow(ki / (2 * m), 2));
        }
    }
}
