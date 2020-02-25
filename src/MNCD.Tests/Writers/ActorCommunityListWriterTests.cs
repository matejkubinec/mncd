using System.Collections.Generic;
using MNCD.Core;
using MNCD.Writers;
using Xunit;

namespace MNCD.Tests.Writers
{
    public class ActorCommunityListWriterTests
    {
        private readonly ActorCommunityListWriter writer = new ActorCommunityListWriter();

        [Fact]
        public void CommunitiesToString()
        {
            var input = new List<Community>
            {
                new Community
                (
                    new List<Actor>
                    {
                        new Actor("a0")
                    }
                ),
                new Community
                (
                    new List<Actor>
                    {
                        new Actor("a1")
                    }
                )
            };

            var output = writer.ToString(input);

            var lines = output.Split('\n');

            Assert.Equal("a0 c0", lines[0]);
            Assert.Equal("a1 c1", lines[1]);
        }
    }
}