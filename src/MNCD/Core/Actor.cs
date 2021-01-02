namespace MNCD.Core
{
    /// <summary>
    /// Represent an actor in a multi-layred network.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Actor"/> class.
        /// </summary>
        public Actor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Actor"/> class with supplied name.
        /// </summary>
        /// <param name="id">ID of the actor.</param>
        /// <param name="name">Name of the actor.</param>
        public Actor(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Gets or sets actors name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets actors id.
        /// </summary>
        public int Id { get; set; }
    }
}