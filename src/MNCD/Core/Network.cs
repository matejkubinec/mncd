using System.Collections.Generic;
using System.Linq;

namespace MNCD.Core
{
    public class Network
    {
        public IList<Layer> Layers = new List<Layer>();
        public IList<Actor> Actors = new List<Actor>();

        public int[,,] ToMatrix()
        {
            var matrix = new int[Layers.Count, Actors.Count, Actors.Count];

            for (var l = 0; l < Layers.Count; l++)
            {
                for (var a1 = 0; a1 < Actors.Count; a1++)
                {
                    for (var a2 = 0; a2 < Actors.Count; a2++)
                    {
                        matrix[l, a1, a2] = 0;
                    }
                }
            }

            for (var l = 0; l < Layers.Count; l++)
            {
                for (var a1 = 0; a1 < Actors.Count; a1++)
                {
                    for (var a2 = 0; a2 < Actors.Count; a2++)
                    {
                        if (Layers[l].IsDirected)
                        {
                            if (Layers[l].Edges.Any(e => e.From == Actors[a1] && e.To == Actors[a2]))
                            {
                                matrix[l, a1, a2]++;
                            }
                        }
                        else
                        {
                            if (Layers[l].Edges.Any(e => (e.From == Actors[a1] && e.To == Actors[a2]) || (e.From == Actors[a2] && e.To == Actors[a1])))
                            {
                                matrix[l, a1, a2]++;
                                matrix[l, a2, a1]++;
                            }
                        }
                    }
                }
            }

            return matrix;
        }
    }
}