using System.Collections.Generic;
using System.Linq;

namespace MNCD.Core
{
    /// <summary>
    /// Class representing a multi-layer network.
    /// </summary>
    public class Network
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        public Network()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="layer">Layer to be included in the network.</param>
        public Network(Layer layer)
        {
            Layers.Add(layer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="layers">Layers to be included in the network.</param>
        /// <param name="actors">List of layers to be included in the network.</param>
        public Network(List<Layer> layers, List<Actor> actors)
        {
            Layers = layers;
            Actors = actors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="layer">Layer to be included in the network.</param>
        /// <param name="actors">List of layers to be included in the network.</param>
        public Network(Layer layer, List<Actor> actors)
            : this(layer)
        {
            Actors = actors;
        }

        /// <summary>
        /// Gets or sets a list of layers.
        /// </summary>
        public List<Layer> Layers { get; set; } = new List<Layer>();

        /// <summary>
        /// Gets or sets a list of actors in the network.
        /// </summary>
        public List<Actor> Actors { get; set; } = new List<Actor>();

        /// <summary>
        /// Gets or sets a list of interlayer edges in the network.
        /// </summary>
        public List<InterLayerEdge> InterLayerEdges { get; set; } = new List<InterLayerEdge>();

        /// <summary>
        /// Gets the first layer of the network.
        /// </summary>
        public Layer FirstLayer => Layers.FirstOrDefault();

        /// <summary>
        /// Gets the number of layers.
        /// </summary>
        public int LayerCount => Layers.Count;

        /// <summary>
        /// Gets the number of actors.
        /// </summary>
        public int ActorCount => Actors.Count;
    }
}