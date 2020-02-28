using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;
using MNCD.Extensions;

namespace MNCD.CommunityDetection.SingleLayer
{
    // https://arxiv.org/pdf/1703.09307.pdf
    // https://github.com/taynaud/python-louvain/blob/master/community/community_louvain.py
    public class Louvain
    {
        // TODO: add tests for bets partition
        public List<Community> GetBestPartition(Network network)
        {

        }

        public void GenerateDendrogram()
        {
            // TODO: check if there are any nodes

            
        }

        // TODO: add tests for modularity
        internal double Modularity(Network network, List<Community> communities)
        {
            var actorToCommunity = network.GetActorToCommunity(communities);
            var actorToNeighbours = network.GetActorToNeighbours();
            var actorToDegree = network.GetActorToDegree(true);
            var edgeToWeight = network.GetEdgeToWeight(0);
            
            var deg = communities.ToDictionary(c => c, c => 0.0);
            var inc = communities.ToDictionary(c => c, c => 0.0);
            foreach(var actor in network.Actors)
            {
                var community = actorToCommunity[actor];
                deg[community] += actorToDegree[actor];

                foreach(var neighbour in actorToNeighbours[actor])
                {
                    var weight = edgeToWeight[(actor, neighbour)];

                    if (actorToCommunity[neighbour] == community)
                    {
                        if (actor == neighbour)
                        {
                            inc[community] += weight;
                        }
                        else
                        {
                            inc[community] += weight / 2.0;
                        }
                    }
                }
            }

            var res = 0.0;
            var layer = network.Layers.First();
            var edgeCount = layer.IsDirected ? layer.Edges.Count : layer.Edges.Count * 2;
            foreach(var com in communities)
            {
                res += Math.Pow((inc[com] / edgeCount) - (deg[com] / (2 * edgeCount)), 2);
            }
            return res;
        }
    }
}