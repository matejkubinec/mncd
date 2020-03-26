using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Readers
{
    public class ActorCommunityListReader
    {
        public List<Community> FromString(string input)
        {
            var communityMap = new Dictionary<string, Community>();
            var actorMap = new Dictionary<string, Actor>();

            foreach (var row in input.Split('\n'))
            {
                var values = row.Split(' ');

                if (values.Length != 2)
                {
                    throw new ArgumentException("Invalid community list.");
                }

                var actor = GetActor(values[0], actorMap);
                var community = GetCommunity(values[1], communityMap);

                community.Actors.Add(actor);
            }

            return communityMap.Values.ToList();
        }

        private Actor GetActor(string actorName, Dictionary<string, Actor> actorMap)
        {
            if (!actorMap.ContainsKey(actorName))
            {
                actorMap.Add(actorName, new Actor(actorName));
            }

            return actorMap[actorName];
        }

        private Community GetCommunity(string communityString, Dictionary<string, Community> communityMap)
        {
            if (!communityMap.ContainsKey(communityString))
            {
                communityMap.Add(communityString, new Community());
            }

            return communityMap[communityString];
        }
    }
}