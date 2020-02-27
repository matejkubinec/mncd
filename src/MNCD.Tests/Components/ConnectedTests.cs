using System.Collections.Generic;
using System.Linq;
using MNCD.Components;
using MNCD.Core;
using MNCD.Tests.Helpers;
using Xunit;

namespace MNCD.Tests.Components
{
    public class ConnectedTests
    {
        [Fact]
        public void GetComponent_OneComponent()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1", "a2", "a3");
            var layer = new Layer();
            layer.Edges.Add(new Edge(actors[0], actors[1]));
            layer.Edges.Add(new Edge(actors[0], actors[3]));
            layer.Edges.Add(new Edge(actors[1], actors[2]));

            foreach (var actor in actors)
            {
                var component = Connected
                    .GetComponent(layer, actor)
                    .OrderBy(a => a.Name);

                Assert.Collection(component,
                    item => Assert.Equal(item, actors[0]),
                    item => Assert.Equal(item, actors[1]),
                    item => Assert.Equal(item, actors[2]),
                    item => Assert.Equal(item, actors[3])
                );
            }
        }

        [Fact]
        public void GetComponent_TwoComponents()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1", "a2", "a3");
            var layer = new Layer();
            layer.Edges.Add(new Edge(actors[0], actors[1]));
            layer.Edges.Add(new Edge(actors[2], actors[3]));

            foreach (var actor in actors.GetRange(0, 2))
            {
                var component = Connected
                    .GetComponent(layer, actor)
                    .OrderBy(a => a.Name);

                Assert.Collection(component,
                    item => Assert.Equal(item, actors[0]),
                    item => Assert.Equal(item, actors[1])
                );
            }

            foreach (var actor in actors.GetRange(2, 2))
            {
                var component = Connected
                    .GetComponent(layer, actor)
                    .OrderBy(a => a.Name);

                Assert.Collection(component,
                    item => Assert.Equal(item, actors[2]),
                    item => Assert.Equal(item, actors[3])
                );
            }
        }

        [Fact]
        public void GetComponent_NoComponents()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1", "a2", "a3");
            var layer = new Layer();

            foreach (var actor in actors)
            {
                var component = Connected
                    .GetComponent(layer, actor)
                    .OrderBy(a => a.Name);

                Assert.Collection(component, item => Assert.Equal(item, actor));
            }
        }

        [Fact]
        public void GetConnectedComponents_OneComponent()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1", "a2", "a3");
            var layer = new Layer();
            layer.Edges.Add(new Edge(actors[0], actors[1]));
            layer.Edges.Add(new Edge(actors[0], actors[3]));
            layer.Edges.Add(new Edge(actors[1], actors[2]));

            var components = layer.GetConnectedComponents(actors);

            Assert.NotEmpty(components);
            Assert.Collection(components, component =>
                Assert.Collection(component.OrderBy(c => c.Name),
                    item => Assert.Equal(item, actors[0]),
                    item => Assert.Equal(item, actors[1]),
                    item => Assert.Equal(item, actors[2]),
                    item => Assert.Equal(item, actors[3])
                ));
        }

        [Fact]
        public void GetConnectedComponents_TwoComponents()
        {
            var actors = ActorHelper.ActorsFrom("a0", "a1", "a2", "a3");
            var layer = new Layer();
            layer.Edges.Add(new Edge(actors[0], actors[1]));
            layer.Edges.Add(new Edge(actors[2], actors[3]));

            var components = layer.GetConnectedComponents(actors);

            Assert.NotEmpty(components);
            Assert.Collection(components,
                component => Assert.Collection(component.OrderBy(c => c.Name),
                    item => Assert.Equal(item, actors[0]),
                    item => Assert.Equal(item, actors[1])
                ),
                component => Assert.Collection(component.OrderBy(c => c.Name),
                    item => Assert.Equal(item, actors[2]),
                    item => Assert.Equal(item, actors[3])
                ));
        }

        [Fact]
        public void GetConnectedComponents_NoComponents()
        {
            var layer = new Layer();
            var components = layer.GetConnectedComponents();

            Assert.Empty(components);
        }
    }
}