namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Contracts.Syntax.DI;
    using Microsoft.Extensions.DependencyInjection;
    
    using static ChainLead.Test.Dummy.HandlerIndex.Common;
    using static ChainLead.Test.Dummy.ConditionIndex.Common;

    [TestFixture]
    public class DiChainLeadSyntaxTest
    {
        public record TypeDoesNotMatter;
        public TypeDoesNotMatter _objectDoesNotMetter = new();

        DummyServiceCollection _dummyOfServiceCollection;
        DummyServiceProvider _dummyOfServiceProvider;
        Dummy.Container<TypeDoesNotMatter> _dummyOf;

        [SetUp]
        public void Setup()
        {
            _dummyOfServiceCollection = [];
            _dummyOfServiceProvider = new();
            _dummyOf = new(_objectDoesNotMetter);

            _dummyOfServiceProvider.AddSetup<IHandlerMath>(_dummyOf.HandlerMath);
            _dummyOfServiceProvider.AddSetup<IConditionMath>(_dummyOf.ConditionMath);
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsNotNullServiceDescriptor()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();

            Assert.That(_dummyOfServiceCollection, 
               Has.Count.EqualTo(1));
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsCallToken()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            Assert.That(_dummyOfServiceCollection.Last().ServiceType,
                Is.EqualTo(typeof(Extension.CallToken)));
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsCallTokenAsSingleton()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            Assert.That(_dummyOfServiceCollection.Last().Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsCallTokenWithImplementationFactory()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            Assert.That(_dummyOfServiceCollection.Last().ImplementationFactory,
                Is.Not.Null);
        }

        [Test]
        public void ConfigureChainLeadSyntaxAddsCallTokenAsImplementationByFactory()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();

            var implementation = _dummyOfServiceCollection.Last()
                .ImplementationFactory!(_dummyOfServiceProvider);

            Assert.That(implementation,
                Is.TypeOf<Extension.CallToken>());
        }

        [Test]
        public void ConfigureChainLeadSyntaxConfiguresHandlerMathByInstanceFromProvider()
        {
            _dummyOf.HandlerMath.Zero_Returns(A);
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            _dummyOfServiceCollection.Last().ImplementationFactory!(_dummyOfServiceProvider);

            Assert.That(ChainLeadSyntax.Handler<TypeDoesNotMatter>.Zero,
                Is.SameAs(_dummyOf.Handler(A)));
        }

        [Test]
        public void ConfigureChainLeadSyntaxConfiguresConditionMathByInstanceFromProvider()
        {
            _dummyOf.ConditionMath.True_Returns(X);
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            _dummyOfServiceCollection.Last().ImplementationFactory!(_dummyOfServiceProvider);

            Assert.That(ChainLeadSyntax.Condition<TypeDoesNotMatter>.True,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        public class DummyServiceCollection :
            List<ServiceDescriptor>,
            IServiceCollection;

        public class DummyServiceProvider : IServiceProvider
        {
            private readonly Dictionary<Type, object> _setup = [];

            public void AddSetup<TContract>(TContract impl) =>
                _setup.Add(typeof(TContract), impl!); 

            public object? GetService(Type serviceType) =>
                _setup.GetValueOrDefault(serviceType);
        }
    }
}
