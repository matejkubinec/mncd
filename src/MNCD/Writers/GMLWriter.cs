using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNCD.Core;

namespace MNCD.Writers
{
    /// <summary>
    /// GML Format based on https://web.archive.org/web/20190303094704/http://www.fim.uni-passau.de:80/fileadmin/files/lehrstuhl/brandenburg/projekte/gml/gml-technical-report.pdf.
    /// </summary>
    public class GMLWriter
    {
        /// <summary>
        /// Converts network to the GML format.
        /// </summary>
        /// <param name="network">Network to be converted.</param>
        /// <param name="communities">Optional list of communities.</param>
        /// <returns>Network in GML format.</returns>
        public string ToGML(Network network, List<Community> communities = null)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            if (communities is null)
            {
                communities = new List<Community>();
            }

            var sb = new StringBuilder();
            AppendHeader(sb);
            AppendNodes(sb, network, communities);
            AppendEdges(sb, network);
            AppendFooter(sb);
            return sb.ToString();
        }

        private void AppendHeader(StringBuilder sb)
        {
            sb.AppendLine("graph [");
            sb.AppendLine("  id 0");
            sb.AppendLine("  directed 0");
        }

        private void AppendFooter(StringBuilder sb)
        {
            sb.AppendLine("]");
        }

        private void AppendNodes(StringBuilder sb, Network network, List<Community> communities)
        {
            var communityIdCounter = 0;
            var communityToId = communities.ToDictionary(c => c, c => communityIdCounter++);
            var actorToCommunity = network.Actors
                    .ToDictionary(a => a, a => communities.FirstOrDefault(c => c.Actors.Contains(a)));
            foreach (var actor in network.Actors)
            {
                var community = actorToCommunity[actor];

                sb.AppendLine($"  node [");
                sb.AppendLine($"    id {actor.Id}");
                sb.AppendLine($"    label \"{actor.Name}\"");

                if (community != null)
                {
                    var communityId = communityToId[community];
                    sb.AppendLine($"    community {communityId}");
                }

                sb.AppendLine($"  ]");
            }
        }

        private void AppendEdges(StringBuilder sb, Network network)
        {
            int layerIdCounter = 0;
            var layerToId = network.Layers.ToDictionary(l => l, l => layerIdCounter++);

            foreach (var layer in network.Layers)
            {
                var layerId = layerToId[layer];

                foreach (var edge in layer.Edges)
                {
                    sb.AppendLine($"  edge [");
                    sb.AppendLine($"    source {edge.From.Id}");
                    sb.AppendLine($"    target {edge.To.Id}");
                    sb.AppendLine($"    layer {layerId}");
                    sb.AppendLine($"  ]");
                }
            }

            foreach (var edge in network.InterLayerEdges)
            {
                var sourceId = layerToId[edge.LayerFrom];
                var targetId = layerToId[edge.LayerTo];
                var layerId = $"{sourceId}.{targetId}";

                sb.AppendLine($"  edge [");
                sb.AppendLine($"    source {edge.From.Id}");
                sb.AppendLine($"    target {edge.To.Id}");
                sb.AppendLine($"    layer {layerId}");
                sb.AppendLine($"  ]");
            }
        }
    }
}