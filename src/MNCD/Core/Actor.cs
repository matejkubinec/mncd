using System.Collections.Generic;

namespace MNCD.Core
{
    public class Actor
    {
        public IList<NumericAttribute> Attributes = new List<NumericAttribute>();
        public string Name { get; set; }

        public Actor()
        {
        }

        public Actor(string name)
        {
            Name = name;
        }
    }
}