namespace Nuisho.ChainLead.Test
{
    using Contracts;
    using Implementation;
    using Implementation.DI;
    using Microsoft.Extensions.DependencyInjection;

    [TestFixture]
    public class DiMathTest
    {
        DummyServiceCollection _dummyOfServiceCollection;

        [SetUp]
        public void Setup()
        {
            _dummyOfServiceCollection = [];
        }

        [Test]
        public void AddConditionMath__Adds_NewTokenDescriptor()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection,
               Has.Count.EqualTo(1));
        }

        [Test]
        public void AddConditionMath__Adds_ImplementationFor_IConditionMath()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection.Last().ServiceType,
                Is.EqualTo(typeof(IConditionMath)));
        }

        [Test]
        public void AddConditionMath__Adds_ConditionMath_AsImplementation()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection.Last().ImplementationType,
                Is.EqualTo(typeof(ConditionMath)));
        }

        [Test]
        public void AddConditionMath__AddsIt_AsSingleton()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection.Last().Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }

        [Test]
        public void AddHandlerMath__Adds_NewTokenDescriptor()
        {
            _dummyOfServiceCollection.AddHandlerMath();
            Assert.That(_dummyOfServiceCollection,
               Has.Count.EqualTo(1));
        }

        [Test]
        public void AddHandlerMath__Adds_ImplementationFor_IHandlerMath()
        {
            _dummyOfServiceCollection.AddHandlerMath();
            Assert.That(_dummyOfServiceCollection.Last().ServiceType,
                Is.EqualTo(typeof(IHandlerMath)));
        }

        [Test]
        public void AddHandlerMath__Adds_HandlerMath_AsImplementation()
        {
            _dummyOfServiceCollection.AddHandlerMath();
            Assert.That(_dummyOfServiceCollection.Last().ImplementationType,
                Is.EqualTo(typeof(HandlerMath)));
        }

        [Test]
        public void AddHandlerMath__AddsIt_AsSingleton()
        {
            _dummyOfServiceCollection.AddHandlerMath();
            Assert.That(_dummyOfServiceCollection.Last().Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }

        public class DummyServiceCollection :
            List<ServiceDescriptor>,
            IServiceCollection;
    }
}
