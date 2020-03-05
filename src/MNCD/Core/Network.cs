using System.Collections.Generic;
using System.Linq;

namespace MNCD.Core
{
    public class Network
    {
        public List<Layer> Layers = new List<Layer>();
        public List<Actor> Actors = new List<Actor>();
        public List<InterLayerEdge> InterLayerEdges = new List<InterLayerEdge>();
        public Layer FirstLayer => Layers.FirstOrDefault();
        public int LayerCount => Layers.Count;

        public Network()
        {
        }

        public Network(Layer layer)
        {
            Layers.Add(layer);
        }

        public Network(Layer layer, List<Actor> actors) : this(layer)
        {
            Actors = actors;
        }

        public Network(List<Layer> layers, List<Actor> actors)
        {
            Layers = layers;
            Actors = actors;
        }

        // TODO: move to network extensions
        public Dictionary<Layer, int> GetLayerToIndex()
        {
            var i = 0;
            return Layers.ToDictionary(l => l, l => i++);
        }
    }
}