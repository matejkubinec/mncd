using MNCD.Core;
using MNCD.Extensions;
using System.Collections.Generic;
using System.Text;

namespace MNCD.Writers
{
    public class ActorCommunityListWriter
    {
        public string ToString(
            List<Actor> actors,
            List<Community> communities = null,
            bool includeMetadata = false)
        {
            var actorToIndex = actors.ToIndexDictionary();
            var communityToIndex = communities.ToIndexDictionary();

            var sb = new StringBuilder();
            foreach (var c in communities)
            {
                foreach (var a in c.Actors)
                {
                    var actor = actorToIndex[a];
                    var community = communityToIndex[c];

                    sb.Append($"{actor} {community}\n");
                }
            }

            if (includeMetadata)
            {
                WriteActors(sb, actors, actorToIndex);
                if (communities != null)
                {
                    WriteCommunities(sb, communities, communityToIndex);
                }
            }

            return sb.ToString();
        }

        private void WriteActors(StringBuilder sb, List<Actor> actors, Dictionary<Actor, int> actorToIndex)
        {
            if (actors.Count > 0)
            {
                sb.Append("# Actors\n");
                foreach (var a in actors)
                {
                    var actor = actorToIndex[a];
                    var name = string.IsNullOrWhiteSpace(a.Name) ? "-" : a.Name;
                    sb.Append($"{actor} {name}\n");
                }
            }
        }

        private void WriteCommunities(StringBuilder sb, List<Community> communities, Dictionary<Community, int> communityToIndex)
        {
            if (communities.Count > 0)
            {
                sb.Append("# Communities\n");
                foreach (var a in communities)
                {
                    var community = communityToIndex[a];
                    var name = "c" + community;
                    sb.Append($"{community} {name}\n");
                }
            }
        }
    }
}