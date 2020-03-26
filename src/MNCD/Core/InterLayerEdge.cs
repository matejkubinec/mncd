namespace MNCD.Core
{
    /// <summary>
    /// Class representing an interlayer edge.
    /// </summary>
    public class InterLayerEdge : Edge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterLayerEdge"/> class.
        /// </summary>
        public InterLayerEdge()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterLayerEdge"/> class.
        /// </summary>
        /// <param name="f">Actor from.</param>
        /// <param name="lf">Layer from.</param>
        /// <param name="t">Actor to.</param>
        /// <param name="lt">Layer to.</param>
        public InterLayerEdge(Actor f, Layer lf, Actor t, Layer lt)
            : base(f, t)
        {
            LayerFrom = lf;
            LayerTo = lt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterLayerEdge"/> class.
        /// </summary>
        /// <param name="f">Actor from.</param>
        /// <param name="lf">Layer from.</param>
        /// <param name="t">Actor to.</param>
        /// <param name="lt">Layer to.</param>
        /// <param name="w">Weight of an edge.</param>
        public InterLayerEdge(Actor f, Layer lf, Actor t, Layer lt, double w)
            : this(f, lf, t, lt)
        {
            Weight = w;
        }

        /// <summary>
        /// Gets or sets layer where the edge begins.
        /// </summary>
        public Layer LayerFrom { get; set; }

        /// <summary>
        /// Gets or sets layer where the edge ends.
        /// </summary>
        public Layer LayerTo { get; set; }

        /// <summary>
        /// Gets a pair of an interlayer edge.k
        /// </summary>
        public (Actor, Layer, Actor, Layer) InterLayerPair => (From, LayerFrom, To, LayerTo);

        /// <summary>
        /// Coppies current interlayer edge.
        /// </summary>
        /// <returns>New instance of interlayer edge.</returns>
        public new InterLayerEdge Copy() => new InterLayerEdge
        {
            From = From,
            LayerFrom = LayerFrom,
            To = To,
            LayerTo = LayerTo,
            Weight = Weight,
        };
    }
}
