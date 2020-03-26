using MNCD.Core;
using MNCD.Tests.Helpers;
using MNCD.Writers;
using System.Collections.Generic;
using Xunit;

namespace MNCD.Tests.Writers
{
    public class ActorCommunityListWriterTests
    {
        private readonly ActorCommunityListWriter writer = new ActorCommunityListWriter();

        [Fact]
        public void CommunitiesEmpty()
        {
            var actors = ActorHelper.Get(2);
            var communities = new List<Community>();
            var output = writer.ToString(actors, communities);

            Assert.Equal("", output);
        }

        [Fact]
        public void CommunitiesEmptyWithMetadata()
        {
            var actors = ActorHelper.Get(2);
            var communities = new List<Community>();
            var output = writer.ToString(actors, communities, true);
            var lines = output.Split('\n');
            Assert.Collection(lines,
                line => Assert.Equal("# Actors", line),
                line => Assert.Equal("0 a0", line),
                line => Assert.Equal("1 a1", line),
                line => Assert.Equal("", line)
            );
        }

        [Fact]
        public void CommunitiesDistinct()
        {
            var actors = ActorHelper.Get(2);
            var communities = new List<Community>
            {
                new Community(actors[0]),
                new Community(actors[1]),
            };
            var output = writer.ToString(actors, communities);
            var lines = output.Split('\n');
            Assert.Collection(lines,
                line => Assert.Equal("0 0", line),
                line => Assert.Equal("1 1", line),
                line => Assert.Equal("", line)
            );
        }

        [Fact]
        public void CommunitiesDistinctWithMetadata()
        {
            var actors = ActorHelper.Get(2);
            var communities = new List<Community>
            {
                new Community(actors[0]),
                new Community(actors[1]),
            };
            var output = writer.ToString(actors, communities, true);
            var lines = output.Split('\n');
            Assert.Collection(lines,
                line => Assert.Equal("0 0", line),
                line => Assert.Equal("1 1", line),
                line => Assert.Equal("# Actors", line),
                line => Assert.Equal("0 a0", line),
                line => Assert.Equal("1 a1", line),
                line => Assert.Equal("# Communities", line),
                line => Assert.Equal("0 c0", line),
                line => Assert.Equal("1 c1", line),
                line => Assert.Equal("", line)
            );
        }
    }
}