using System.Collections.Generic;
using System.Text;
using MNCD.Core;
using MNCD.Helpers;

namespace MNCD.Writers
{
    public class EdgeListWriter
    {
        public string ToString(Network network, bool includeMetadata = false)
        {
            var actorToIndex = network.Actors.ToIndexDictionary();
            var layerToIndex = network.Layers.ToIndexDictionary();

            var sb = new StringBuilder();

            foreach (var layer in network.Layers)
            {
                var l = layerToIndex[layer];
                foreach (var edge in layer.Edges)
                {
                    var a1 = actorToIndex[edge.From];
                    var a2 = actorToIndex[edge.To];
                    var w = edge.Weight;
                    sb.Append($"{a1} {l} {a2} {l} {w}\n");
                }
            }

            foreach (var interLayerEdge in network.InterLayerEdges)
            {
                var l1 = layerToIndex[interLayerEdge.LayerFrom];
                var l2 = layerToIndex[interLayerEdge.LayerTo];
                var a1 = actorToIndex[interLayerEdge.From];
                var a2 = actorToIndex[interLayerEdge.To];
                var w = interLayerEdge.Weight;
                sb.Append($"{a1} {l1} {a2} {l2} {w}\n");
            }

            if (includeMetadata)
            {
                WriteActors(sb, network.Actors, actorToIndex);
                WriteLayers(sb, network.Layers, layerToIndex);
            }

            return sb.ToString();
        }

        public string ToString(
            List<Actor> actors,
            List<Community> communities,
            bool includeMetadata = false)
        {
            var actorToIndex = actors.ToIndexDictionary();
            var communityToIndex = communities.ToIndexDictionary();

            var sb = new StringBuilder();
            foreach (var c in communities)
            {
                foreach (var a in c.Actors)
                {
                    var actor = actorToIndex[a];
                    var community = communityToIndex[c];

                    sb.Append($"{actor} {community}\n");
                }
            }

            if (includeMetadata)
            {
                WriteActors(sb, actors, actorToIndex);
                WriteCommunities(sb, communities, communityToIndex);
            }

            return sb.ToString();
        }

        private void WriteActors(StringBuilder sb, List<Actor> actors, Dictionary<Actor, int> actorToIndex)
        {
            if (actors.Count > 0)
            {
                sb.Append("# Actors\n");
                foreach (var a in actors)
                {
                    var actor = actorToIndex[a];
                    var name = string.IsNullOrWhiteSpace(a.Name) ? "-" : a.Name;
                    sb.Append($"{actor} {name}\n");
                }
            }
        }

        private void WriteLayers(StringBuilder sb, List<Layer> layers, Dictionary<Layer, int> layerToIndex)
        {
            if (layers.Count > 0)
            {
                sb.Append("# Layers\n");
                foreach (var a in layers)
                {
                    var layer = layerToIndex[a];
                    var name = string.IsNullOrWhiteSpace(a.Name) ? "-" : a.Name;
                    sb.Append($"{layer} {name}\n");
                }
            }
        }

        private void WriteCommunities(StringBuilder sb, List<Community> communities, Dictionary<Community, int> communityToIndex)
        {
            if (communities.Count > 0)
            {
                sb.Append("# Communities\n");
                foreach (var a in communities)
                {
                    var community = communityToIndex[a];
                    var name = "c" + community;
                    sb.Append($"{community} {name}\n");
                }
            }
        }
    }
}