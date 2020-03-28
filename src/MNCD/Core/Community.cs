using System.Collections.Generic;

namespace MNCD.Core
{
    /// <summary>
    /// Class that represents a community.
    /// </summary>
    public class Community
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Community"/> class.
        /// </summary>
        public Community()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Community"/> class with supplied actor.
        /// </summary>
        /// <param name="actor">Actor to be included in the community.</param>
        public Community(Actor actor)
        {
            Actors.Add(actor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Community"/> class with supplied actors.
        /// </summary>
        /// <param name="actors">Actor to be included in the community.</param>
        public Community(IEnumerable<Actor> actors)
        {
            Actors.AddRange(actors);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Community"/> class with supplied actors.
        /// </summary>
        /// <param name="actors">Actor to be included in the community.</param>
        public Community(params Actor[] actors)
        {
            Actors.AddRange(actors);
        }

        /// <summary>
        /// Gets a list of actors in the community.
        /// </summary>
        public List<Actor> Actors { get; private set; } = new List<Actor>();

        /// <summary>
        /// Gets the size of the community.
        /// </summary>
        public int Size => Actors.Count;

        /// <summary>
        /// Check if community contains actor.
        /// </summary>
        /// <param name="actor">Actor to be checked.</param>
        /// <returns>True if actor is included in the community.</returns>
        public bool Contains(Actor actor)
        {
            return Actors.Contains(actor);
        }
    }
}