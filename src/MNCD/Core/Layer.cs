using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Core
{
    public class Layer
    {
        public string Name { get; set; }
        public List<Edge> Edges = new List<Edge>();

        public Layer()
        {
        }

        public Layer(string name)
        {
            Name = name;
        }

        public Layer(List<Edge> edges)
        {
            Edges = edges;
        }
    }
}