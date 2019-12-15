using System.Collections.Generic;
using System.Linq;
using MNCD.CommunityDetection;
using MNCD.Core;
using Xunit;

namespace MNCD.Tests
{
    public class LouvainTests
    {
        [Fact]
        public void LouvainNetwork1()
        {
            var network = LouvainTestNetwork1;
            var hierarchy = new Louvain().Compute(network);

            var c1 = new List<Actor>
            {
                network.Actors[0],
                network.Actors[1],
                network.Actors[2]
            };

            var c2 = new List<Actor>
            {
                network.Actors[3],
                network.Actors[4],
                network.Actors[5]
            };

            Assert.Equal(2, hierarchy.Count);
            Assert.Equal(network.Actors.Count(), hierarchy[0].Keys.Count);
            Assert.Equal(c1, hierarchy[1].Values.ElementAt(0));
            Assert.Equal(c2, hierarchy[1].Values.ElementAt(1));
        }

        [Fact]
        public void LouvainNetwork2()
        {
            var network = LouvainTestNetwork2;
            var hierarchy = new Louvain().Compute(network);
            // TODO: finish up
        }

        private Network LouvainTestNetwork1
        {
            get
            {
                var actors = Enumerable.Range(0, 6).Select(i => new Actor() { Name = i.ToString() }).ToList();
                var edges = new List<Edge>
                {
                    new Edge(actors.ElementAt(0), actors.ElementAt(2)) { Weight = 2 },
                    new Edge(actors.ElementAt(1), actors.ElementAt(2)) { Weight = 2 },
                    new Edge(actors.ElementAt(2), actors.ElementAt(3)) { Weight = 1 },
                    new Edge(actors.ElementAt(3), actors.ElementAt(4)) { Weight = 3 },
                    new Edge(actors.ElementAt(3), actors.ElementAt(5)) { Weight = 3 },
                };
                var network = new Network()
                {
                    Actors = actors,
                    Layers = new List<Layer>
                    {
                        new Layer
                        {
                            Edges = edges,
                            IsDirected = false
                        }
                    }
                };
                return network;
            }
        }

        private Network LouvainTestNetwork2
        {
            get
            {
                var actors = Enumerable.Range(0, 9).Select(i => new Actor() { Name = i.ToString() }).ToList();
                var edges = new List<Edge>
                {
                    new Edge(actors.ElementAt(0), actors.ElementAt(2)) { Weight = 2 },
                    new Edge(actors.ElementAt(1), actors.ElementAt(2)) { Weight = 2 },
                    new Edge(actors.ElementAt(2), actors.ElementAt(3)) { Weight = 1 },
                    new Edge(actors.ElementAt(3), actors.ElementAt(4)) { Weight = 3 },
                    new Edge(actors.ElementAt(3), actors.ElementAt(5)) { Weight = 3 },
                    new Edge(actors.ElementAt(2), actors.ElementAt(6)) { Weight = 1 },
                    new Edge(actors.ElementAt(6), actors.ElementAt(7)) { Weight = 4 },
                    new Edge(actors.ElementAt(7), actors.ElementAt(8)) { Weight = 4 },
                };
                var network = new Network()
                {
                    Actors = actors,
                    Layers = new List<Layer>
                    {
                        new Layer
                        {
                            Edges = edges,
                            IsDirected = false
                        }
                    }
                };
                return network;
            }
        }
    }
}