using MNCD.Core;

namespace MNCD.Evaluation
{
    // https://dl.acm.org/doi/pdf/10.1145/2063576.2063921
    //  how many different dimensions are detectable among the community
    public static class Variety
    {
        public static double Compute(Community community, Network network)
        {
            var d = network.Layers.Count;
            var dc = 0;

            var actors = community.Actors;
            foreach (var layer in network.Layers)
            {
                foreach (var edge in layer.Edges)
                {
                    if (actors.Contains(edge.From) && actors.Contains(edge.To))
                    {
                        dc++;
                        break;
                    }
                }
            }

            // TODO: handle interlayer edges

            return (dc - 1) / (d - 1);
        }
    }
}