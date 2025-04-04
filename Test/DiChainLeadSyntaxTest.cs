namespace Nuisho.ChainLead.Test
{
    using Nuisho.ChainLead.Contracts;
    using Nuisho.ChainLead.Contracts.Syntax;
    using Nuisho.ChainLead.Contracts.Syntax.DI;
    using Microsoft.Extensions.DependencyInjection;
    
    using static Nuisho.ChainLead.Test.Dummy.HandlerIndex;
    using static Nuisho.ChainLead.Test.Dummy.ConditionIndex;

    [TestFixture]
    public class DiChainLeadSyntaxTest
    {
        public record TypeDoesNotMatter;
        public TypeDoesNotMatter _objectDoesNotMatter = new();

        DummyServiceCollection _dummyOfServiceCollection;
        DummyServiceProvider _dummyOfServiceProvider;
        Dummy.Container<TypeDoesNotMatter> _dummyOf;

        [SetUp]
        public void Setup()
        {
            _dummyOfServiceCollection = [];
            _dummyOfServiceProvider = new();

            _dummyOf = new(_objectDoesNotMatter);
            _dummyOf.Handlers.Generate(A);
            _dummyOf.Conditions.Generate(X);

            _dummyOfServiceProvider.AddSetup<IHandlerMath>(_dummyOf.HandlerMath);
            _dummyOfServiceProvider.AddSetup<IConditionMath>(_dummyOf.ConditionMath);
        }

        [Test]
        public void ConfigureChainLeadSyntax__Adds_NotNull_ServiceDescriptor()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();

            Assert.That(_dummyOfServiceCollection, 
               Has.Count.EqualTo(1));
        }

        [Test]
        public void ConfigureChainLeadSyntax__Adds_CallToken()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            Assert.That(_dummyOfServiceCollection.Last().ServiceType,
                Is.EqualTo(typeof(Extension.CallToken)));
        }

        [Test]
        public void ConfigureChainLeadSyntax__Adds_CallToken_AsSingleton()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            Assert.That(_dummyOfServiceCollection.Last().Lifetime,
                Is.EqualTo(ServiceLifetime.Singleton));
        }

        [Test]
        public void ConfigureChainLeadSyntax__Adds_CallToken_WithImplementationFactory()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            Assert.That(_dummyOfServiceCollection.Last().ImplementationFactory,
                Is.Not.Null);
        }

        [Test]
        public void ConfigureChainLeadSyntax__Adds_CallToken_AsImplementationByFactory()
        {
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();

            var implementation = _dummyOfServiceCollection.Last()
                .ImplementationFactory!(_dummyOfServiceProvider);

            Assert.That(implementation,
                Is.TypeOf<Extension.CallToken>());
        }

        [Test]
        public void ConfigureChainLeadSyntax__Configures_HandlerMath_ByInstanceFromProvider()
        {
            _dummyOf.HandlerMath.Zero_Returns(A);
            _dummyOfServiceCollection.ConfigureChainLeadSyntax();
            _dummyOfServiceCollection.Last().ImplementationFactory!(_dummyOfServiceProvider);

            Assert.That(ChainLeadSyntax.Handler<TypeDoesNotMatter>.Zero,
                Is.SameAs(_dummyOf.Handler(A)));
        }

        [Test]
        public void ConfigureChainLeadSyntax__Configures_ConditionMath_ByInstanceFromProvider()
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
