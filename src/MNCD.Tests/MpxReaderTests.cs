using MNCD.Readers;
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

            var actorsAttributes = new double[][]
            {
                new double[] { 53, 2, 10 },
                new double[] { 65, 3, 36 },
                new double[] { 0, 14, 55 },
                new double[] { 12, 9, 44 },
                new double[] { 22, 18, 20 },
                new double[] { 0, 9, 32 },
                new double[] { 21, 14, 8 },
                new double[] { 0, 14, 42 },
                new double[] { 53, 54, 103 },
                new double[] { 0, 7, 48 },
                new double[] { 42, 32, 49 },
                new double[] { 0, 1, 3 },
                new double[] { 38, 4, 27 },
                new double[] { 35, 5, 10 },
                new double[] { 74, 29, 146 },
                new double[] { 0, 7, 48  }
            };

            Assert.Equal(16, florentine.Actors.Count);
            Assert.Equal(3, florentine.Actors[0].Attributes.Count);
            for (var i = 0; i < actorsNames.Length; i++)
            {
                Assert.Equal(actorsNames[i], florentine.Actors[i].Name);

                Assert.Equal("priorates", florentine.Actors[i].Attributes[0].Name);
                Assert.Equal(actorsAttributes[i][0], florentine.Actors[i].Attributes[0].Value);

                Assert.Equal("totalties", florentine.Actors[i].Attributes[1].Name);
                Assert.Equal(actorsAttributes[i][1], florentine.Actors[i].Attributes[1].Value);

                Assert.Equal("wealth", florentine.Actors[i].Attributes[2].Name);
                Assert.Equal(actorsAttributes[i][2], florentine.Actors[i].Attributes[2].Value);
            }

            Assert.Equal(2, florentine.Layers.Count);

            Assert.Equal("marriage", florentine.Layers[0].Name);
            Assert.False(florentine.Layers[0].IsDirected);
            Assert.Equal(20, florentine.Layers[0].Edges.Count);

            Assert.Equal("business", florentine.Layers[1].Name);
            Assert.False(florentine.Layers[1].IsDirected);
            Assert.Equal(15, florentine.Layers[1].Edges.Count);
        }
    }
}

