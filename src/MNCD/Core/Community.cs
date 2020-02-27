using System.Collections.Generic;

namespace MNCD.Core
{
    public class Community
    {
        public readonly List<Actor> Actors = new List<Actor>();

        public int Size => Actors.Count;

        public Community()
        {
        }

        public Community(Actor actor)
        {
            Actors.Add(actor);
        }

        public Community(IEnumerable<Actor> actors)
        {
            Actors.AddRange(actors);
        }
    }
}