namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using Moq;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    [TestFixture]
    public class ChainLeadSyntaxTest
    {
        Mock<IHandlerMath> _handlerMath;
        Mock<IConditionMath> _conditionMath;

        Mock<IHandler<int>> _handler;
        Mock<IHandler<int>> _handlerA;
        Mock<IHandler<int>> _handlerB;
        Mock<IHandler<int>> _joinedAB;
        Mock<IHandler<int>> _mergedAB;
        Mock<IHandler<int>> _deepMergedAB;
        Mock<IHandler<int>> _injectedAB;
        Mock<IHandler<int>> _deepInjectedAB;
        Mock<IHandler<int>> _wrappedAB;
        Mock<IHandler<int>> _deepWrappedAB;

        Mock<ICondition<int>> _condition;
        Mock<ICondition<int>> _conditionA;
        Mock<ICondition<int>> _conditionB;

        [SetUp]
        public void Setup()
        {
            _handlerMath = new Mock<IHandlerMath>();
            _conditionMath = new Mock<IConditionMath>();

            _handler = new Mock<IHandler<int>>();
            _handlerA = new Mock<IHandler<int>>();
            _handlerB = new Mock<IHandler<int>>();
            _joinedAB = new Mock<IHandler<int>>();
            _mergedAB = new Mock<IHandler<int>>();
            _deepMergedAB = new Mock<IHandler<int>>();
            _injectedAB = new Mock<IHandler<int>>();
            _deepInjectedAB = new Mock<IHandler<int>>();
            _wrappedAB = new Mock<IHandler<int>>();
            _deepWrappedAB = new Mock<IHandler<int>>();

            _condition = new Mock<ICondition<int>>();
            _conditionA = new Mock<ICondition<int>>();
            _conditionB = new Mock<ICondition<int>>();

            _handlerMath
                .Setup(o => o.Join(_handlerA.Object, _handlerB.Object))
                .Returns(_joinedAB.Object);

            _handlerMath
                .Setup(o => o.Merge(_handlerA.Object, _handlerB.Object))
                .Returns(_mergedAB.Object);

            _handlerMath
                .Setup(o => o.DeepMerge(_handlerA.Object, _handlerB.Object))
                .Returns(_deepMergedAB.Object);

            _handlerMath
                .Setup(o => o.Inject(_handlerA.Object, _handlerB.Object))
                .Returns(_injectedAB.Object);

            _handlerMath
                .Setup(o => o.DeepInject(_handlerA.Object, _handlerB.Object))
                .Returns(_deepInjectedAB.Object);

            _handlerMath
                .Setup(o => o.Wrap(_handlerA.Object, _handlerB.Object))
                .Returns(_wrappedAB.Object);

            _handlerMath
                .Setup(o => o.DeepWrap(_handlerA.Object, _handlerB.Object))
                .Returns(_deepWrappedAB.Object);

            ConfigureMath
                .ForHandlers(_handlerMath.Object)
                .AndForConditions(_conditionMath.Object);
        }

        [Test]
        public void MakeHandlerReturnsMathMakeExecutionResult()
        {
            var seed = new Action<int>(_ => { });

            _handlerMath
                .Setup(o => o.MakeHandler(seed))
                .Returns(_handler.Object);
            
            var product = MakeHandler(seed);

            Assert.That(product, Is.SameAs(_handler.Object));
        }

        [Test]
        public void ThenReturnsMathJoinExecutionResult()
        {
            var product = _handlerA.Object.Then(_handlerB.Object);
            
            Assert.That(product, Is.SameAs(_joinedAB.Object));
        }

        [Test]
        public void JoinReturnsMathJoinExecutionResult()
        {
            var product = Join(_handlerA.Object, _handlerB.Object);

            Assert.That(product, Is.SameAs(_joinedAB.Object));
        }

        [Test]
        public void JoinXToReturnsMathJoinExecutionResult()
        {
            var product = Join(_handlerA.Object).To(_handlerB.Object);

            Assert.That(product, Is.SameAs(_joinedAB.Object));
        }

        [Test]
        public void JoinItToReturnsMathJoinExecutionResult()
        {
            var joinItToB = JoinItTo(_handlerB.Object);
            var product = joinItToB(_handlerA.Object);

            Assert.That(product, Is.SameAs(_joinedAB.Object));
        }

        [Test]
        public void MergeReturnsMathJoinExecutionResult()
        {
            var product = Merge(_handlerA.Object, _handlerB.Object);

            Assert.That(product, Is.SameAs(_mergedAB.Object));
        }

        [Test]
        public void MergeXWithReturnsMathJoinExecutionResult()
        {
            var product = Merge(_handlerA.Object).With(_handlerB.Object);

            Assert.That(product, Is.SameAs(_mergedAB.Object));
        }

        [Test]
        public void MergeItWithReturnsMathJoinExecutionResult()
        {
            var mergeItWithB = MergeItWith(_handlerB.Object);
            var product = mergeItWithB(_handlerA.Object);

            Assert.That(product, Is.SameAs(_mergedAB.Object));
        }

        [Test]
        public void DeepMergeReturnsMathJoinExecutionResult()
        {
            var product = DeepMerge(_handlerA.Object, _handlerB.Object);

            Assert.That(product, Is.SameAs(_deepMergedAB.Object));
        }

        [Test]
        public void DeepMergeXWithReturnsMathJoinExecutionResult()
        {
            var product = DeepMerge(_handlerA.Object).With(_handlerB.Object);

            Assert.That(product, Is.SameAs(_deepMergedAB.Object));
        }

        [Test]
        public void DeepMergeItWithReturnsMathJoinExecutionResult()
        {
            var deepMergeItWithB = DeepMergeItWith(_handlerB.Object);
            var product = deepMergeItWithB(_handlerA.Object);

            Assert.That(product, Is.SameAs(_deepMergedAB.Object));
        }

        [Test]
        public void InjectReturnsMathJoinExecutionResult()
        {
            var product = Inject(_handlerA.Object, _handlerB.Object);

            Assert.That(product, Is.SameAs(_injectedAB.Object));
        }

        [Test]
        public void InjectXIntoReturnsMathJoinExecutionResult()
        {
            var product = Inject(_handlerA.Object).Into(_handlerB.Object);

            Assert.That(product, Is.SameAs(_injectedAB.Object));
        }

        [Test]
        public void InjectItIntoReturnsMathJoinExecutionResult()
        {
            var injectItIntoB = InjectItInto(_handlerB.Object);
            var product = injectItIntoB(_handlerA.Object);

            Assert.That(product, Is.SameAs(_injectedAB.Object));
        }

        [Test]
        public void DeepInjectReturnsMathJoinExecutionResult()
        {
            var product = DeepInject(_handlerA.Object, _handlerB.Object);

            Assert.That(product, Is.SameAs(_deepInjectedAB.Object));
        }

        [Test]
        public void DeepInjectXIntoReturnsMathJoinExecutionResult()
        {
            var product = DeepInject(_handlerA.Object).Into(_handlerB.Object);

            Assert.That(product, Is.SameAs(_deepInjectedAB.Object));
        }

        [Test]
        public void DeepInjectItIntoReturnsMathJoinExecutionResult()
        {
            var deepInjectItIntoB = DeepInjectItInto(_handlerB.Object);
            var product = deepInjectItIntoB(_handlerA.Object);

            Assert.That(product, Is.SameAs(_deepInjectedAB.Object));
        }

        [Test]
        public void WrapReturnsMathJoinExecutionResult()
        {
            var product = Wrap(_handlerA.Object, _handlerB.Object);

            Assert.That(product, Is.SameAs(_wrappedAB.Object));
        }

        [Test]
        public void WrapXUpReturnsMathJoinExecutionResult()
        {
            var product = Wrap(_handlerA.Object).Up(_handlerB.Object);

            Assert.That(product, Is.SameAs(_wrappedAB.Object));
        }

        [Test]
        public void WrapItUpReturnsMathJoinExecutionResult()
        {
            var wrapItUp = WrapItUp(_handlerB.Object);
            var product = wrapItUp(_handlerA.Object);

            Assert.That(product, Is.SameAs(_wrappedAB.Object));
        }


        [Test]
        public void DeepWrapReturnsMathJoinExecutionResult()
        {
            var product = DeepWrap(_handlerA.Object, _handlerB.Object);

            Assert.That(product, Is.SameAs(_deepWrappedAB.Object));
        }

        [Test]
        public void DeepWrapXUpReturnsMathJoinExecutionResult()
        {
            var product = DeepWrap(_handlerA.Object).Up(_handlerB.Object);

            Assert.That(product, Is.SameAs(_deepWrappedAB.Object));
        }

        [Test]
        public void DeepWrapItUpReturnsMathJoinExecutionResult()
        {
            var deepWrapItUp = DeepWrapItUp(_handlerB.Object);
            var product = deepWrapItUp(_handlerA.Object);

            Assert.That(product, Is.SameAs(_deepWrappedAB.Object));
        }

        [Test]
        public void WhenReturnsMathConditionalExecutionResult()
        {
            _handlerMath
                .Setup(o => o.Conditional(_handlerA.Object, _condition.Object))
                .Returns(_handlerB.Object);

            var product = _handlerA.Object.When(_condition.Object);

            Assert.That(product, Is.SameAs(_handlerB.Object));
        }

        [Test]
        public void ZeroIsMathZero()
        {
            _handlerMath
                .Setup(o => o.Zero<int>())
                .Returns(_handler.Object);

            Assert.That(
                Handler<int>.Zero,
                Is.SameAs(_handler.Object));
        }

        [Test]
        public void IsZeroExecutesMathIsZero(
            [Values(false, true)] bool expectedResult)
        {
            _handlerMath
                .Setup(o => o.IsZero(_handler.Object))
                .Returns(expectedResult);

            var result = _handler.Object.IsZero();

            _handlerMath.Verify(o => o.IsZero(_handler.Object), Times.Once);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MakeReturnsMathMakePredicateExcutionResult()
        {
            var seed = new Func<int, bool>(_ => true);

            _conditionMath
                .Setup(o => o.MakeCondition(seed))
                .Returns(_condition.Object);

            var product = MakeCondition(seed);

            Assert.That(product, Is.SameAs(_condition.Object));
        }

        [Test]
        public void AsPredicateReturnsMathMakePredicat2ExcutionResult()
        {
            var seed = new Func<int, bool>(_ => true);

            _conditionMath
                .Setup(o => o.MakeCondition(seed))
                .Returns(_condition.Object);

            var product = seed.AsCondition();

            Assert.That(product, Is.SameAs(_condition.Object));
        }


        [Test]
        public void OrReturnsMathOrExcutionResult()
        {
            _conditionMath
                .Setup(o => o.Or(_conditionA.Object, _conditionB.Object))
                .Returns(_condition.Object);

            var product = _conditionA.Object.Or(_conditionB.Object);

            Assert.That(product, Is.SameAs(_condition.Object));
        }

        [Test]
        public void NotReturnsMathNotExcutionResult()
        {
            _conditionMath
                .Setup(o => o.Not(_conditionA.Object))
                .Returns(_conditionB.Object);

            var product = Not(_conditionA.Object);

            Assert.That(product, Is.SameAs(_conditionB.Object));
        }

        [Test]
        public void TrueReturnsMathTrue()
        {
            _conditionMath
                .Setup(o => o.True<int>())
                .Returns(_condition.Object);

            Assert.That(
                Condition<int>.True,
                Is.SameAs(_condition.Object));
        }

        [Test]
        public void FalseReturnsMathFalse()
        {
            _conditionMath
                .Setup(o => o.False<int>())
                .Returns(_condition.Object);

            Assert.That(
                Condition<int>.False,
                Is.SameAs(_condition.Object));
        }
    }
}
