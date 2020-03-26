using MNCD.Readers;
using System.Linq;
using Xunit;

namespace MNCD.Tests
{
    public class MpxReaderTests
    {
        [Fact]
        public void FlorentineFromString()
        {
            var florentine = new MpxReader().FromString(TestHelper.FloretineString);

            var actorsNames = new string[]
            {
                "Acciaiuoli",
                "Albizzi",
                "Barbadori",
                "Bischeri",
                "Castellani",
                "Ginori",
                "Guadagni",
                "Lamberteschi",
                "Medici",
                "Pazzi",
                "Peruzzi",
                "Pucci",
                "Ridolfi",
                "Salviati",
                "Strozzi",
                "Tornabuoni"
            };

            Assert.Equal(16, florentine.Actors.Count);
            Assert.Equal(actorsNames, florentine.Actors.Select(a => a.Name));
            Assert.Equal(2, florentine.Layers.Count);
            Assert.Equal("marriage", florentine.Layers[0].Name);
            Assert.Equal(20, florentine.Layers[0].Edges.Count);
            Assert.Equal("business", florentine.Layers[1].Name);
            Assert.Equal(15, florentine.Layers[1].Edges.Count);
        }
    }
}

