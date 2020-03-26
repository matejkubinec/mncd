using MNCD.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MNCD.Readers
{
    // https://rdrr.io/cran/multinet/man/IO.html
    public class MpxReader
    {
        public async Task<Network> FromStream(Stream stream)
        {
            var content = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                content = await reader.ReadToEndAsync();
            }

            return FromString(content);
        }

        public async Task<Network> FromPath(string path)
        {
            var content = await File.ReadAllTextAsync(path);
            return FromString(content);
        }

        public Network FromString(string content)
        {
            var network = new Network();
            var lines = Regex.Split(content, "\r\n|\r|\n");
            var type = string.Empty;
            var layers = new List<string>();
            var actors = new List<string>();
            var nodes = new List<string>();
            var edges = new List<string>();

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("--"))
                {
                    continue;
                }

                if (lines[i].StartsWith("#TYPE"))
                {
                    type = lines[i].Substring(5);
                }

                if (lines[i].StartsWith("#LAYERS"))
                {
                    while (lines[++i] != string.Empty)
                    {
                        layers.Add(lines[i]);

                        if (i + 1 == lines.Length)
                        {
                            break;
                        }
                    }
                }

                if (lines[i].StartsWith("#ACTORS"))
                {
                    while (lines[++i] != string.Empty)
                    {
                        actors.Add(lines[i]);

                        if (i + 1 == lines.Length)
                        {
                            break;
                        }
                    }
                }

                if (lines[i].StartsWith("#EDGES"))
                {
                    while (lines[++i] != string.Empty)
                    {
                        edges.Add(lines[i]);

                        if (i + 1 == lines.Length)
                        {
                            break;
                        }
                    }
                }
            }

            foreach (var layer in layers)
            {
                var info = layer.Split(",");
                network.Layers.Add(new Layer
                {
                    Name = info[0],
                });
            }

            foreach (var actor in actors)
            {
                var actorInfo = actor.Split(",");
                var parsedActor = new Actor(actorInfo[0]);
                network.Actors.Add(parsedActor);
            }

            foreach (var edgeInput in edges)
            {
                var edgeInfo = edgeInput.Split(",");
                var fromActor = network.Actors.First(a => a.Name == edgeInfo[0]);
                var toActor = network.Actors.First(a => a.Name == edgeInfo[1]);
                var edge = new Edge(fromActor, toActor);
                network.Layers.First(l => l.Name == edgeInfo[2]).Edges.Add(edge);
            }

            return network;
        }
    }
}