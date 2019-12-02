using MNCD.Core;
using System.Collections.Generic;

namespace MNCD.Tests
{
    public static class TestHelper
    {
        public readonly static List<Actor> Actors3 = new List<Actor> { A1, A2, A3 };

        public readonly static Actor A1 = new Actor("A1");
        public readonly static Actor A2 = new Actor("A2");
        public readonly static Actor A3 = new Actor("A3");

        public static string FloretineString = @"
#TYPE multiplex

#LAYERS
marriage,UNDIRECTED
business,UNDIRECTED

#ACTOR ATTRIBUTES
priorates, NUMERIC
totalties, NUMERIC
wealth, NUMERIC

#ACTORS
Acciaiuoli,53,2,10
Albizzi,65,3,36
Barbadori,0,14,55
Bischeri,12,9,44
Castellani,22,18,20
Ginori,0,9,32
Guadagni,21,14,8
Lamberteschi,0,14,42
Medici,53,54,103
Pazzi,0,7,48
Peruzzi,42,32,49
Pucci,0,1,3
Ridolfi,38,4,27
Salviati,35,5,10
Strozzi,74,29,146
Tornabuoni,0,7,48

#EDGES
Ridolfi,Tornabuoni,marriage
Ridolfi,Strozzi,marriage
Peruzzi,Strozzi,marriage
Pazzi,Salviati,marriage
Medici,Tornabuoni,marriage
Medici,Tornabuoni,business
Medici,Salviati,business
Medici,Salviati,marriage
Medici,Ridolfi,marriage
Medici,Pazzi,business
Lamberteschi,Peruzzi,business
Guadagni,Tornabuoni,marriage
Guadagni,Lamberteschi,marriage
Guadagni,Lamberteschi,business
Ginori,Medici,business
Castellani,Strozzi,marriage
Castellani,Peruzzi,marriage
Castellani,Peruzzi,business
Castellani,Lamberteschi,business
Bischeri,Strozzi,marriage
Bischeri,Peruzzi,marriage
Bischeri,Peruzzi,business
Bischeri,Lamberteschi,business
Bischeri,Guadagni,business
Bischeri,Guadagni,marriage
Barbadori,Peruzzi,business
Barbadori,Medici,marriage
Barbadori,Medici,business
Barbadori,Ginori,business
Barbadori,Castellani,business
Barbadori,Castellani,marriage
Albizzi,Medici,marriage
Albizzi,Guadagni,marriage
Albizzi,Ginori,marriage
Acciaiuoli,Medici,marriage
        ".Trim();

        public readonly static Network TwoLayerUndirected = new Network
        {
            Actors = Actors3,
            Layers = new List<Layer>
            {
                new Layer
                {
                    Edges = new List<Edge>
                    {
                        new Edge(A1, A2),
                        new Edge(A1, A3)
                    },
                    IsDirected = false
                },
                new Layer
                {
                    Edges = new List<Edge>
                    {
                        new Edge(A3, A1)
                    },
                    IsDirected = false
                },
            }
        };

        public readonly static Network TwoLayerDirected = new Network
        {
            Actors = Actors3,
            Layers = new List<Layer>
            {
                new Layer
                {
                    Edges = new List<Edge>
                    {
                        new Edge(A1, A2),
                        new Edge(A1, A3)
                    },
                    IsDirected = true
                },
                new Layer
                {
                    Edges = new List<Edge>
                    {
                        new Edge(A3, A1)
                    },
                    IsDirected = true
                },
            }
        };

        public readonly static Network TwoLayerCombination = new Network
        {
            Actors = Actors3,
            Layers = new List<Layer>
            {
                new Layer
                {
                    Edges = new List<Edge>
                    {
                        new Edge(A1, A2),
                        new Edge(A1, A3)
                    },
                    IsDirected = false
                },
                new Layer
                {
                    Edges = new List<Edge>
                    {
                        new Edge(A3, A1)
                    },
                    IsDirected = true
                },
            }
        };
    }
}
