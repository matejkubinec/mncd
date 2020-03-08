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
        public int ActorCount => Actors.Count;

        public Network()
        {
        }

        public Network(Layer layer)
        {
            Layers.Add(layer);
        }

        public Network(List<Layer> layers, List<Actor> actors)
        {
            Layers = layers;
            Actors = actors;
        }

        public Network(Layer layer, List<Actor> actors) : this(layer)
        {
            Actors = actors;
        }
    }
}