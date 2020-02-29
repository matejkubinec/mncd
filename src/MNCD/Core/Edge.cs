using System.Collections;
using System.Collections.Generic;

namespace MNCD.Core
{
    public class Edge
    {
        public IList<NumericAttribute> Attributes = new List<NumericAttribute>();
        public Actor From { get; set; }
        public Actor To { get; set; }
        public (Actor, Actor) Pair => (From, To);
        public double Weight { get; set; }

        public Edge()
        {
            Weight = 1;
        }

        public Edge(Actor from, Actor to) : this()
        {
            From = from;
            To = to;
        }

        public Edge(Actor from, Actor to, double weight) : this(from, to)
        {
            Weight = weight;
        }
    }
}