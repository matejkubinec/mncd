namespace MNCD.Core
{
    public class InterLayerEdge : Edge
    {
        public Layer LayerFrom { get; set; }

        public Layer LayerTo { get; set; }

        public (Actor, Layer, Actor, Layer) InterLayerPair => (From, LayerFrom, To, LayerTo);

        public InterLayerEdge()
        {
        }

        public InterLayerEdge(Actor f, Layer lf, Actor t, Layer lt) : base(f, t)
        {
            LayerFrom = lf;
            LayerTo = lt;
        }

        public InterLayerEdge(Actor f, Layer lf, Actor t, Layer lt, double w) : this(f, lf, t, lt)
        {
            Weight = w;
        }
    }
}
