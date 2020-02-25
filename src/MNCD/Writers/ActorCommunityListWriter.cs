using System.Collections.Generic;
using System.Text;
using MNCD.Core;

namespace MNCD.Writers
{
    public class ActorCommunityListWriter
    {
        public string ToString(List<Community> communities)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < communities.Count; i++)
            {
                var actors = communities[i].Actors;
                for (var j = 0; j < actors.Count; j++)
                {
                    var actor = actors[j];
                    builder.Append($"{actor.Name.Replace(" ", "")} c{i}\n");
                }
            }
            return builder.ToString();
        }
    }
}