using System;
using System.Linq;
using MNCD.Readers;
using Xunit;

namespace MNCD.Tests.Readers
{
    public class ActorCommunityListReaderTests
    {
        private readonly ActorCommunityListReader reader = new ActorCommunityListReader();

        [Fact]
        public void InvalidString()
        {
            var input = "a0 a1 a1";
            Assert.Throws<ArgumentException>(() => reader.FromString(input));

            input = "\n";
            Assert.Throws<ArgumentException>(() => reader.FromString(input));

            input = "";
            Assert.Throws<ArgumentException>(() => reader.FromString(input));
        }

        [Fact]
        public void StringToCommunities()
        {
            var input = "a0 c0\na1 c1";
            var output = reader.FromString(input);

            Assert.NotEmpty(output);
            Assert.Collection(
                output,
                item => Assert.Equal("a0", item.Actors.First().Name),
                item => Assert.Equal("a1", item.Actors.First().Name));
        }
    }
}