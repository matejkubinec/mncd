namespace MNCD.Core
{
    /// <summary>
    /// Class representing an edge.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        public Edge()
        {
            Weight = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class with supplied actors.
        /// </summary>
        /// <param name="from">Actor from.</param>
        /// <param name="to">Actor to.</param>
        public Edge(Actor from, Actor to)
            : this()
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class with supplied actors and weight.
        /// </summary>
        /// <param name="from">Actor from.</param>
        /// <param name="to">Actor to.</param>
        /// <param name="weight">Weight of the edge.</param>
        public Edge(Actor from, Actor to, double weight)
            : this(from, to)
        {
            Weight = weight;
        }

        /// <summary>
        /// Gets or sets the weight of the edge.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets actor where the edge begins.
        /// </summary>
        public Actor From { get; set; }

        /// <summary>
        /// Gets or sets actor where the edge ends.
        /// </summary>
        public Actor To { get; set; }

        /// <summary>
        /// Gets the edge as a actor pair.
        /// </summary>
        public (Actor, Actor) Pair => (From, To);

        /// <summary>
        /// Reverses the direction of the edge.
        /// </summary>
        /// <returns>Returns reversed edge.</returns>
        public Edge Reverse() => new Edge()
        {
            From = To,
            To = From,
            Weight = Weight,
        };

        /// <summary>
        /// Copies the current edge.
        /// </summary>
        /// <returns>New instance of an edge.</returns>
        public Edge Copy() => new Edge()
        {
            From = From,
            To = To,
            Weight = Weight,
        };
    }
}