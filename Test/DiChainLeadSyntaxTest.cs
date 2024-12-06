namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Contracts.Syntax.DI;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;

    [TestFixture]
    public class DiChainLeadSyntaxTest
    {
        Mock<IServiceCollection> _serviceCollection;
        Mock<IServiceProvider> _serviceProvider;
        Mock<IConditionMath> _conditionMath;
        Mock<IHandlerMath> _handlerMath;

        ServiceDescriptor? _callTokenDescriptor;

        [SetUp]
        public void Setup()
        {
            _callTokenDescriptor = null;

            _serviceCollection = new Mock<IServiceCollection>();
            _serviceProvider = new Mock<IServiceProvider>();
            _conditionMath = new Mock<IConditionMath>();
            _handlerMath = new Mock<IHandlerMath>();

            _serviceProvider
                .Setup(o => o.GetService(typeof(IConditionMath)))
                .Returns(_conditionMath.Object);

            _serviceProvider
                .Setup(o => o.GetService(typeof(IHandlerMath)))
                .Returns(_handlerMath.Object);

            _serviceCollection
                .Setup(o => o.Add(It.Is<ServiceDescriptor>(
                       x => x.ServiceType == typeof(Extension.CallToken))))
                .Callback((ServiceDescriptor descriptor) => _callTokenDescriptor = descriptor);
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsNotNullServiceDescriptor()
        {
            _serviceCollection.Object.ConfigureChainLeadSyntax();
            Assert.That(_callTokenDescriptor, Is.Not.Null);
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsCallToken()
        {
            _serviceCollection.Object.ConfigureChainLeadSyntax();
            Assert.That(_callTokenDescriptor!.ServiceType,
                Is.EqualTo(typeof(Extension.CallToken)));
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsCallTokenAsSingleton()
        {
            _serviceCollection.Object.ConfigureChainLeadSyntax();
            Assert.That(_callTokenDescriptor!.Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsCallTokenAsImplementationByFactory()
        {
            _serviceCollection.Object.ConfigureChainLeadSyntax();

            var implementation = _callTokenDescriptor!
                .ImplementationFactory!(_serviceProvider.Object);

            Assert.That(implementation,
                Is.TypeOf<Extension.CallToken>());
        }

        [Test]
        public void ConfigureChainLeadSyntaxConfiguresHandlerMathByInstanceFromProvider()
        {
            var zero = new Mock<IHandler<int>>();

            _handlerMath
                .Setup(o => o.Zero<int>())
                .Returns(zero.Object);

            _serviceCollection.Object.ConfigureChainLeadSyntax();

            _callTokenDescriptor!.ImplementationFactory!(_serviceProvider.Object);

            Assert.That(ChainLeadSyntax.Handler<int>.Zero,
                Is.SameAs(zero.Object));
        }

        [Test]
        public void ConfigureChainLeadSyntaxConfiguresConditionMathByInstanceFromProvider()
        {
            var @true = new Mock<ICondition<int>>();

            _conditionMath
                .Setup(o => o.True<int>())
                .Returns(@true.Object);

            _serviceCollection.Object.ConfigureChainLeadSyntax();

            _callTokenDescriptor!.ImplementationFactory!(_serviceProvider.Object);

            Assert.That(ChainLeadSyntax.Condition<int>.True,
                Is.SameAs(@true.Object));
        }
    }
}
