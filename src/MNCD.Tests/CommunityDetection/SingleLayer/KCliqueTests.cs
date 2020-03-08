using System.Collections.Generic;
using System.Linq;
using MNCD.CommunityDetection.SingleLayer;
using MNCD.Core;
using MNCD.Generators;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.CommunityDetection.SingleLayer
{
    public class KCliqueTests
    {
        [Fact]
        public void AssignMembership_DistinctCliques()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1");
            var cliques = new List<List<Actor>>()
            {
                new List<Actor>
                {
                    actors[0]
                },
                new List<Actor>
                {
                    actors[1]
                }
            };

            var membership = new KClique().AssignMembership(cliques);

            for (var i = 0; i < 2; i++)
            {
                var actor = actors[i];
                var clique = cliques[i];

                var assigned = membership[actor];

                Assert.NotEmpty(assigned);
                Assert.Collection(assigned,
                    item => Assert.Equal(clique, item));
            }
        }

        [Fact]
        public void AssignMembership_NondistinctCliques()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1");
            var cliques = new List<List<Actor>>()
            {
                new List<Actor>
                {
                    actors[0],
                    actors[1]
                },
                new List<Actor>
                {
                    actors[1],
                    actors[0]
                }
            };

            var membership = new KClique().AssignMembership(cliques);

            for (var i = 0; i < 2; i++)
            {
                var actor = actors[i];
                var assigned = membership[actor];

                Assert.NotEmpty(assigned);
                Assert.Collection(assigned,
                    item => Assert.Equal(cliques[0], item),
                    item => Assert.Equal(cliques[1], item));
            }
        }

        [Fact]
        public void GetAdjacentCliques_DistinctCliques()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1");
            var cliques = new List<List<Actor>>()
            {
                new List<Actor>
                {
                    actors[0],
                },
                new List<Actor>
                {
                    actors[1]
                }
            };

            var kclique = new KClique();
            var membership = kclique.AssignMembership(cliques);

            foreach (var clique in cliques)
            {
                var adjacent = kclique.GetAdjacentCliques(clique, membership);
                Assert.Empty(adjacent);
            }
        }

        [Fact]
        public void GetAdjacentCliques_NondistinctCliques()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1");
            var cliques = new List<List<Actor>>()
            {
                new List<Actor>
                {
                    actors[0],
                    actors[1]
                },
                new List<Actor>
                {
                    actors[1],
                    actors[0]
                }
            };

            var kclique = new KClique();
            var membership = kclique.AssignMembership(cliques);

            foreach (var clique in cliques)
            {
                var adjacent = kclique.GetAdjacentCliques(clique, membership);
                var other = cliques.First(c => !c.Equals(clique));

                Assert.NotEmpty(adjacent);
                Assert.Collection(adjacent, item => Assert.Equal(other, item));
            }
        }

        [Fact]
        public void GetCliqueToActor()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1");
            var cliques = new List<List<Actor>>()
            {
                new List<Actor>
                {
                    actors[0],
                    actors[1]
                },
                new List<Actor>
                {
                    actors[1],
                    actors[0]
                }
            };
            var cliquesActors = new KClique().GetCliqueToActor(cliques);

            foreach (var clique in cliques)
            {
                Assert.True(cliquesActors.ContainsKey(clique));
                Assert.NotNull(cliquesActors[clique]);
            }
        }

        [Fact]
        public void GetPercolationNetwork()
        {
            var kclique = new KClique();
            var actors = ActorHelper.ActorsFrom("a0", "a1");
            var cliques = new List<List<Actor>>()
            {
                new List<Actor>
                {
                    actors[0],
                    actors[1]
                },
                new List<Actor>
                {
                    actors[1],
                    actors[0]
                }
            };
            var cliqueToActor = kclique.GetCliqueToActor(cliques);
            var membership = kclique.AssignMembership(cliques);
            var network = kclique.GetPercolationNetwork(cliques, membership, cliqueToActor, 2);

            Assert.NotNull(network);
            Assert.NotEmpty(network.Layers);
            Assert.NotEmpty(network.Actors);

            var cliqueActors = cliques.Select(c => cliqueToActor[c]).ToList();
            Assert.Equal(network.Actors, cliqueActors);

            var c = new Edge(cliqueActors[0], cliqueActors[1]);
            Assert.Collection(network.Layers[0].Edges,
                e => Assert.True(
                    e.Pair == (e.From, e.To) ||
                    e.Pair == (e.To, e.From)
                )
            );
        }

        [Fact]
        public void GetKCommunities()
        {
            var generator = new CompleteGraphGenerator();
            var kclique = new KClique();
            var network = generator.Generate(4);
            var communties = kclique.GetKCommunities(network, 3);

            Assert.NotEmpty(communties);
            Assert.Collection(communties,
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(network.Actors[0], a),
                    a => Assert.Equal(network.Actors[1], a),
                    a => Assert.Equal(network.Actors[2], a),
                    a => Assert.Equal(network.Actors[3], a)
                )
            );
        }

        [Fact]
        public void GetKCommunities_One()
        {
            //    O               5
            //    | \           / |
            //    |  2 -- 3 -- 4  |
            //    | /           \ |
            //    1               6
            var ac = ActorHelper.Get(7);
            var ne = new Network();
            var ed = new List<Edge>
            {
                new Edge(ac[0], ac[1]),
                new Edge(ac[0], ac[2]),
                new Edge(ac[1], ac[2]),
                new Edge(ac[2], ac[3]),
                new Edge(ac[3], ac[4]),
                new Edge(ac[4], ac[5]),
                new Edge(ac[4], ac[5]),
                new Edge(ac[5], ac[6]),
            };
            ne.Actors = ac;
            ne.Layers.Add(new Layer() { Edges = ed });

            var kclique = new KClique();
            var communties = kclique.GetKCommunities(ne, 3);

            Assert.NotEmpty(communties);
            Assert.Collection(communties,
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(ne.Actors[0], a),
                    a => Assert.Equal(ne.Actors[1], a),
                    a => Assert.Equal(ne.Actors[2], a)
                )
            );
        }

        [Fact]
        public void GetKCommunities_Two()
        {
            //    O      3
            //    | \  / |
            //    |  2   |
            //    | /  \ |
            //    1      4

            var ac = ActorHelper.ActorsFrom("a0", "a1", "a2", "a3", "a4");
            var ne = new Network();
            var ed = new List<Edge>
            {
                new Edge(ac[0], ac[1]),
                new Edge(ac[0], ac[2]),
                new Edge(ac[1], ac[2]),
                new Edge(ac[2], ac[3]),
                new Edge(ac[2], ac[4]),
                new Edge(ac[3], ac[4])
            };
            ne.Actors = ac;
            ne.Layers.Add(new Layer() { Edges = ed });

            var kclique = new KClique();
            var communties = kclique.GetKCommunities(ne, 3);

            Assert.NotEmpty(communties);
            Assert.Collection(communties.OrderBy(c => c.Actors.First().Name),
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(ac[0], a),
                    a => Assert.Equal(ac[1], a),
                    a => Assert.Equal(ac[2], a)
                ),
                c => Assert.Collection(c.Actors.OrderBy(a => a.Name),
                    a => Assert.Equal(ac[2], a),
                    a => Assert.Equal(ac[3], a),
                    a => Assert.Equal(ac[4], a)
                )
            );
        }

        [Fact]
        public void GetActorToClique_Test()
        {
            var kclique = new KClique();
            var ak = ActorHelper.Get(4);
            var av = ActorHelper.Get(2);
            var cl1 = ak.GetRange(0, 2);
            var cl2 = ak.GetRange(2, 2);

            var input = new Dictionary<List<Actor>, Actor>
            {
                { cl1,  av[0] },
                { cl2,  av[1] },
            };
            var output = kclique.GetActorToClique(input);

            Assert.NotEmpty(output);
            Assert.Equal(cl1, output[av[0]]);
            Assert.Equal(cl2, output[av[1]]);
        }
    }
}