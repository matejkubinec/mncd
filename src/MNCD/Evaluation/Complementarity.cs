using MNCD.Core;

namespace MNCD.Evaluation
{
    public static class Complementarity
    {
        public static double Compute(Community community, Network network)
        {
            return
                Variety.Compute(community, network) *
                Exclusivity.Compute(community, network) *
                Homogenity.Compute(community, network);
        }
    }
}