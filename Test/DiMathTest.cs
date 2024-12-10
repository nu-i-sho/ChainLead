namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using ChainLead.Implementation.DI;
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
        public void AddConditionMathAddsNewTokenDescriptor()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection,
               Has.Count.EqualTo(1));
        }

        [Test]
        public void AddConditionMathAddsImplementationFor_IConditionMath()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection.Last().ServiceType,
                Is.EqualTo(typeof(IConditionMath)));
        }

        [Test]
        public void AddConditionMathAdds_ConditionMath_AsImplementation()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection.Last().ImplementationType,
                Is.EqualTo(typeof(ConditionMath)));
        }

        [Test]
        public void AddConditionMathAddsItAsSingleton()
        {
            _dummyOfServiceCollection.AddConditionMath();
            Assert.That(_dummyOfServiceCollection.Last().Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }

        [Test]
        public void AddHandlerMathAddsNewTokenDescriptor()
        {
            _dummyOfServiceCollection.AddHandlerMath();
            Assert.That(_dummyOfServiceCollection, 
               Has.Count.EqualTo(1));
        }

        [Test]
        public void AddHandlerMathAddsImplementationFor_IHandlerMath()
        {
            _dummyOfServiceCollection.AddHandlerMath();
            Assert.That(_dummyOfServiceCollection.Last().ServiceType,
                Is.EqualTo(typeof(IHandlerMath)));
        }

        [Test]
        public void AddHandlerMathAdds_HandlerMath_AsImplementation()
        {
            _dummyOfServiceCollection.AddHandlerMath();
            Assert.That(_dummyOfServiceCollection.Last().ImplementationType,
                Is.EqualTo(typeof(HandlerMath)));
        }

        [Test]
        public void AddHandlerMathAddsItAsSingleton()
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
