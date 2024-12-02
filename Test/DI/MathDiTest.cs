namespace ChainLead.Test.DI
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using ChainLead.Implementation.DI;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;

    [TestFixture]
    public class MathDiTest
    {
        Mock<IServiceCollection> _serviceCollection;
        ServiceDescriptor? _descriptor;

        [SetUp]
        public void Setup()
        {
            _descriptor = null;

            _serviceCollection = new Mock<IServiceCollection>();
            
            _serviceCollection
                .Setup(o => o.Add(It.IsAny<ServiceDescriptor>()))
                .Callback((ServiceDescriptor descriptor) => _descriptor = descriptor);
        }

        [Test]
        public void AddConditionMathAddsNewTokenDescriptor()
        {
            _serviceCollection.Object.AddConditionMath();
            Assert.That(_descriptor, Is.Not.Null);
        }

        [Test]
        public void AddConditionMathAddsImplementationFor_IConditionMath()
        {
            _serviceCollection.Object.AddConditionMath();
            Assert.That(_descriptor!.ServiceType, 
                Is.EqualTo(typeof(IConditionMath)));
        }


        [Test]
        public void AddConditionMathAdds_ConditionMath_AsImplementation()
        {
            _serviceCollection.Object.AddConditionMath();
            Assert.That(_descriptor!.ImplementationType,
                Is.EqualTo(typeof(ConditionMath)));
        }

        [Test]
        public void AddConditionMathAddsItAsSingleton()
        {
            _serviceCollection.Object.AddConditionMath();
            Assert.That(_descriptor!.Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }

        [Test]
        public void AddHandlerMathAddsNewTokenDescriptor()
        {
            _serviceCollection.Object.AddHandlerMath();
            Assert.That(_descriptor, Is.Not.Null);
        }

        [Test]
        public void AddHandlerMathAddsImplementationFor_IHandlerMath()
        {
            _serviceCollection.Object.AddHandlerMath();
            Assert.That(_descriptor!.ServiceType,
                Is.EqualTo(typeof(IHandlerMath)));
        }

        [Test]
        public void AddHandlerMathAdds_HandlerMath_AsImplementation()
        {
            _serviceCollection.Object.AddHandlerMath();
            Assert.That(_descriptor!.ImplementationType,
                Is.EqualTo(typeof(HandlerMath)));
        }

        [Test]
        public void AddHandlerMathAddsItAsSingleton()
        {
            _serviceCollection.Object.AddHandlerMath();
            Assert.That(_descriptor!.Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }
    }
}
