using System;
using Castle.MicroKernel.Registration;
using Xunit;

namespace WindsorContainer.Tests
{
    public class WindsorContainerTests
    {
        [Fact]
        public void Release_ForTransientComponent_ShouldCallDisposeOnComponent()
        {
            var container = new Castle.Windsor.WindsorContainer();
            container.Register(Component.For<IFoo>().ImplementedBy<Foo>().LifeStyle.Transient);
            var foo = container.Resolve<IFoo>();
            container.Release(foo);
            Assert.Equal(Foo.NumberAfterDispose, foo.Number);
        }

        [Fact]
        public void Release_ForSingletonComponent_ShouldNotCallDisposeOnComponent()
        {
            var container = new Castle.Windsor.WindsorContainer();
            container.Register(Component.For<IFoo>().ImplementedBy<Foo>().LifeStyle.Singleton);
            var foo = container.Resolve<IFoo>();
            container.Release(foo);
            Assert.Equal(Foo.NumberBeforeDispose, foo.Number);
        }

        interface IFoo : IDisposable
        {
            int Number { get; }
        }

        class Foo : IFoo
        {
            public static int NumberBeforeDispose = 42;
            public static int NumberAfterDispose = -1;

            public Foo()
            {
                Number = NumberBeforeDispose;
            }

            public int Number { get; private set; }

            public void Dispose()
            {
                Number = NumberAfterDispose;
            }
        }
    }
}
