using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Evaluation;
using MNCD.Core;
using MNCD.Extensions;

namespace MNCD.CommunityDetection.SingleLayer
{
    // https://arxiv.org/abs/0803.0476
    public class Louvain
    {
        public List<Community> Apply(Network inputNetwork)
        {
            // TODO: add checks
            var network = inputNetwork;
            List<Community> best = network.Actors
                .Select(a => new Community())
                .ToList();
            List<Community> communities;
            Dictionary<Actor, Community> actorToCommunity;
            
            while(true)
            {
                (communities, actorToCommunity) = PhaseOne(network);

                network = PhaseTwo(network, communities, actorToCommunity);

                if (network.Actors.Count == 1)
                {
                    break;
                }

                best = communities.Where(c => c.Size > 0).ToList();
            }

            return best;
        }

        internal (List<Community>, Dictionary<Actor, Community>) PhaseOne(Network network)
        {
            var actorToCommunity = network.Actors
                .ToDictionary(a => a,a => new Community(a));
            var communities = actorToCommunity.Values.ToList();
            var actorToNeighbours = network.FirstLayer.GetNeighboursDict();

            // First Phase - Local Optimum
            var change = true;
            var iterations = 0;
            while (change && iterations < 1000)
            {
                iterations++;
                change = false;
                
                foreach(var actor in network.Actors)
                {
                    var ac = actorToCommunity[actor];
                    var mc = Modularity.Compute(network, communities);
                    var maxModularity = mc;
                    var com = ac;

                    foreach(var neighbour in actorToNeighbours[actor])
                    {
                        var nc = actorToCommunity[neighbour];

                        if (ac == nc)
                        {
                            continue;
                        }

                        ac.Actors.Remove(actor);
                        nc.Actors.Add(actor);

                        var comms = communities.Where(c => c.Size > 0).ToList();
                        var nm = Modularity.Compute(network, comms);

                        if (nm > maxModularity)
                        {
                            maxModularity = nm;
                            com = nc;
                        }

                        ac.Actors.Add(actor);
                        nc.Actors.Remove(actor);
                    }

                    if (maxModularity > mc)
                    {
                        change = true;
                        ac.Actors.Remove(actor);
                        com.Actors.Add(actor);
                        actorToCommunity[actor] = com; 
                    }
                }
            }

            return (communities, actorToCommunity);
        }

        internal Network PhaseTwo(
            Network network,
            List<Community> communities,
            Dictionary<Actor, Community> actorToCommunity)
        {
            var i = 0;
            var newCommunityToActor = communities
                .Where(c => c.Size > 0)
                .ToDictionary(c => c, c => new Actor("a" + i++));
            var newEdges = new Dictionary<(Actor f, Actor t), double>();
            
            foreach(var edge in network.FirstLayer.Edges)
            {
                var fc = actorToCommunity[edge.From];
                var tc = actorToCommunity[edge.To];
                var w = edge.Weight;

                var fa = newCommunityToActor[fc];
                var ta = newCommunityToActor[tc];

                if (!newEdges.ContainsKey((fa, ta)))
                {
                    if (!newEdges.ContainsKey((ta, fa)))
                    {
                        newEdges.Add((fa, ta), w);
                    }
                    else
                    {
                        newEdges[(ta, fa)] += w;
                    }
                }
                else
                {
                    newEdges[(fa, ta)] += w;
                }
            }

            var newActors = newCommunityToActor.Values.ToList();
            var newNetwork = new Network(new Layer(), newActors);
            foreach(var newEdge in newEdges)
            {
                var edge = new Edge
                {
                    From = newEdge.Key.f,
                    To = newEdge.Key.t,
                    Weight = newEdge.Value
                };
                newNetwork.FirstLayer.Edges.Add(edge);
            }
            return newNetwork;
        }

        internal double ModularityGain(Network n, Community c, Actor i)
        {
            var sumIn = GetSumIn(n, c);
            var sumTot = GetSumTot(n, c);
            var kIn = GetKIn(n, i, c);
            var kI = GetKI(n, i);            
            var m = GetM(n);

            return
            (((sumIn + kIn) / (2 * m)) - Math.Pow((sumTot + kI) / (2 * m), 2)) -
            ((sumIn / (2 * m)) - Math.Pow(sumTot / (2 * m), 2) - Math.Pow((kI / (2 * m)), 2));
        }

        // Sum of the weights of the links inside C
        internal double GetSumIn(Network n, Community c)
        {
            var res = 0.0;
            foreach(var e in n.FirstLayer.Edges)
            {
                if (c.Actors.Contains(e.From) && c.Actors.Contains(e.To))
                {
                    res += e.Weight;
                }
            }
            return res;
        }

        // Sum of the weights of the links incident to nodes in C
        internal double GetSumTot(Network n, Community c)
        {
            var res = 0.0;
            foreach(var e in n.FirstLayer.Edges)
            {
                if (c.Actors.Contains(e.From))
                {
                    if (!c.Actors.Contains(e.To))
                    {
                        res += e.Weight;
                    }
                }
                else if (c.Actors.Contains(e.To))
                {
                    if (!c.Actors.Contains(e.From))
                    {
                        res += e.Weight;
                    }
                }
            }
            return res;
        }

        // Sum of the weights of the links incident to node i
        internal double GetKI(Network n, Actor i)
        {
            var res = 0.0;
            foreach(var e in n.FirstLayer.Edges)
            {
                if(e.From == i || e.To == i)
                {
                    res += e.Weight;
                }
            }
            return res;
        }

        // Sum of the weights of the links from i to nodes in C
        internal double GetKIn(Network n, Actor i, Community c)
        {
            var res = 0.0;
            foreach(var e in n.FirstLayer.Edges)
            {
                if (e.From == i)
                {
                    if (c.Actors.Contains(e.To))
                    {
                        res += e.Weight;
                    }
                }
                else if (e.To == i)
                {
                    if (c.Actors.Contains(e.From))
                    {
                        res += e.Weight;
                    }
                }
            }
            return res;
        }

        // Sum of the weights of all the links in the network
        internal double GetM(Network network)
        {
            return network.FirstLayer.Edges.Sum(e => e.Weight);
        }
    }
}