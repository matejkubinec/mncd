using MNCD.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MNCD.Readers
{
    /// <summary>
    /// Implements reading a subset of MPX format.
    /// See more: https://rdrr.io/cran/multinet/man/IO.html.
    /// </summary>
    public class MpxReader
    {
        /// <summary>
        /// Downloads a mpx from supplied url a converts it into a network.
        /// </summary>
        /// <param name="url">Datasets url.</param>
        /// <returns>Created network.</returns>
        public async Task<Network> FromUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    var client = new HttpClient();

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return FromString(content);
                    }
                    else
                    {
                        throw new ArgumentException("Request was not successful, reason: " + response.ReasonPhrase);
                    }
                }
            }

            throw new ArgumentException("Url is not valid.");
        }

        /// <summary>
        /// Reads a network from a text stream that contains string in MPX format.
        /// </summary>
        /// <param name="stream">String stream.</param>
        /// <returns>Network.</returns>
        public async Task<Network> FromStream(Stream stream)
        {
            var content = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                content = await reader.ReadToEndAsync();
            }

            return FromString(content);
        }

        /// <summary>
        /// Reads network from a mpx file.
        /// </summary>
        /// <param name="path">Path to a mpx file.</param>
        /// <returns>Network.</returns>
        public async Task<Network> FromPath(string path)
        {
            var content = await File.ReadAllTextAsync(path);
            return FromString(content);
        }

        /// <summary>
        /// Reads a network from a string in mpx format.
        /// </summary>
        /// <param name="content">String in mpx format.</param>
        /// <returns>Network.</returns>
        public Network FromString(string content)
        {
            var idCounter = 1;
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
                var parsedActor = new Actor(idCounter++, actorInfo[0]);
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