using System;
using System.Collections.Generic;
using MNCD.Core;

namespace MNCD.Readers
{
    public class EdgeListReader : IReader
    {
        public Network FromString(string input)
        {
            var actors = new Dictionary<string, Actor>();
            var layers = new Dictionary<string, Layer>();
            var rows = new List<EdgeListRow>();

            foreach (var line in input.Split('\n'))
            {
                var values = line.Split(" ");

                if (values.Length != 5)
                {
                    throw new ArgumentException("Invalid edgelist.");
                }

                var a1 = values[0];
                var l1 = values[1];
                var a2 = values[2];
                var l2 = values[3];
                var w = values[4];

                if (!double.TryParse(w, out var value))
                {
                    throw new ArgumentException("Invalid weight.");
                }

                var row = new EdgeListRow
                {
                    Actor1 = a1,
                    Layer1 = l1,
                    Actor2 = a2,
                    Layer2 = l2,
                    Weight = w
                };

                if (!actors.ContainsKey(a1))
                {
                    actors.Add(a1, new Actor { Name = a1 });
                }

                if (!actors.ContainsKey(a2))
                {
                    actors.Add(a2, new Actor { Name = a2 });
                }

                if (!layers.ContainsKey(l1))
                {
                    layers.Add(l1, new Layer { Name = l1 });
                }

                if (!layers.ContainsKey(l2))
                {
                    layers.Add(l2, new Layer { Name = l2 });
                }

                rows.Add(row);
            }

            var network = new Network();

            foreach (var actor in actors)
            {
                network.Actors.Add(actor.Value);
            }

            foreach (var layer in layers)
            {
                network.Layers.Add(layer.Value);
            }

            foreach (var row in rows)
            {
                var a1 = actors[row.Actor1];
                var a2 = actors[row.Actor2];
                var l1 = layers[row.Layer1];
                var l2 = layers[row.Layer1];
                var w = double.Parse(row.Weight);

                if (row.Layer1 == row.Layer2)
                {
                    var edge = new Edge
                    {
                        From = a1,
                        To = a2,
                        Weight = w
                    };
                    l1.Edges.Add(edge);
                }
                else
                {
                    var edge = new InterLayerEdge
                    {
                        From = a1,
                        To = a1,
                        LayerFrom = l1,
                        LayerTo = l2,
                        Weight = w
                    };
                    network.InterLayerEdges.Add(edge);
                }
            }

            return network;
        }

        private class EdgeListRow
        {
            public string Actor1 { get; set; }
            public string Layer1 { get; set; }
            public string Actor2 { get; set; }
            public string Layer2 { get; set; }
            public string Weight { get; set; }
        }
    }
}