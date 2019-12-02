using System.Collections.Generic;

namespace MNCD.Core
{
    public class Layer
    {
        public string Name { get; set; }
        public bool IsDirected { get; set; }
        public IList<Edge> Edges = new List<Edge>();
    }
}