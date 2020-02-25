using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNCD.Core;

namespace MNCD.Writers
{
    public class EdgeListWriter
    {
        public string ToString(Network network)
        {
            var actors = network.Actors.ToDictionary(a => a, a => a.Name.Replace(" ", "-"));
            var layers = network.Layers.ToDictionary(l => l, l => l.Name.Replace(" ", "-"));

            var sb = new StringBuilder();

            foreach (var layer in network.Layers)
            {
                var l = layers[layer];
                foreach (var edge in layer.Edges)
                {
                    var a1 = actors[edge.From];
                    var a2 = actors[edge.To];
                    var w = edge.Weight;
                    sb.Append($"{a1} {l} {a2} {l} {w}\n");
                }
            }

            foreach (var interLayerEdge in network.InterLayerEdges)
            {
                var l1 = layers[interLayerEdge.LayerFrom];
                var l2 = layers[interLayerEdge.LayerTo];
                var a1 = actors[interLayerEdge.From];
                var a2 = actors[interLayerEdge.To];
                var w = interLayerEdge.Weight;
                sb.Append($"{a1} {l1} {a2} {l2} {w}\n");
            }

            return sb.ToString();
        }

        public string ToString(List<Community> communities)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < communities.Count; i++)
            {
                var actors = communities[i].Actors;
                for (var j = 0; j < actors.Count; j++)
                {
                    var actor = actors[j];
                    builder.Append($"{actor.Name.Replace(" ", "")} c{i}\n");
                }
            }
            return builder.ToString();
        }
    }
}