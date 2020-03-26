using MNCD.Core;

namespace MNCD.Evaluation.MultiLayer
{
    /// <summary>
    /// Implements complementarity measure.
    /// Finding Redundant and Complementary Communities in Multidimensional Networks
    /// http://www.michelecoscia.com/wp-content/uploads/2012/08/cosciacikm11.pdf
    /// Michele Berlingerio, Michele Coscia, Fosca Giannotti.
    /// </summary>
    public static class Complementarity
    {
        /// <summary>
        /// Conjuction of variety, exclusivity and homogenity.
        /// </summary>
        /// <param name="community">Community for which complementarity will be computed.</param>
        /// <param name="network">Network in which community resides.</param>
        /// <returns>Complementary value for community.</returns>
        public static double Compute(Community community, Network network)
        {
            return
                Variety.Compute(community, network) *
                Exclusivity.Compute(community, network) *
                Homogenity.Compute(community, network);
        }
    }
}