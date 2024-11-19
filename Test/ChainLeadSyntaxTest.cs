namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using Moq;

    using ChainLead.Contracts.Syntax;
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    [TestFixture]
    public class ChainLeadSyntaxTest
    {
        Mock<IHandlerMath> _handlerMath;
        Mock<IConditionMath> _conditionMath;

        Mock<IHandler<int>> _handler;
        Mock<IHandler<int>> _handlerA;
        Mock<IHandler<int>> _handlerB;
        Mock<IHandler<int>> _mathFirstThenSecond_AB;
        Mock<IHandler<int>> _mathJoinFirstWithSecond_AB;
        Mock<IHandler<int>> _mathMergeFirstWithSecond_AB;
        Mock<IHandler<int>> _mathPutFirstInSecond_AB;
        Mock<IHandler<int>> _mathInjectFirstIntoSecond_AB;
        Mock<IHandler<int>> _mathFirstCoverSecond_AB;
        Mock<IHandler<int>> _mathFirstWrapSecond_AB;

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
            _mathFirstThenSecond_AB = new Mock<IHandler<int>>();
            _mathJoinFirstWithSecond_AB = new Mock<IHandler<int>>();
            _mathMergeFirstWithSecond_AB = new Mock<IHandler<int>>();
            _mathPutFirstInSecond_AB = new Mock<IHandler<int>>();
            _mathInjectFirstIntoSecond_AB = new Mock<IHandler<int>>();
            _mathFirstCoverSecond_AB = new Mock<IHandler<int>>();
            _mathFirstWrapSecond_AB = new Mock<IHandler<int>>();

            _condition = new Mock<ICondition<int>>();
            _conditionA = new Mock<ICondition<int>>();
            _conditionB = new Mock<ICondition<int>>();

            _handlerMath
                .Setup(o => o.FirstThenSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_mathFirstThenSecond_AB.Object);

            _handlerMath
                .Setup(o => o.JoinFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_mathJoinFirstWithSecond_AB.Object);

            _handlerMath
                .Setup(o => o.MergeFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_mathMergeFirstWithSecond_AB.Object);

            _handlerMath
                .Setup(o => o.PutFirstInSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_mathPutFirstInSecond_AB.Object);

            _handlerMath
                .Setup(o => o.InjectFirstIntoSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_mathInjectFirstIntoSecond_AB.Object);

            _handlerMath
                .Setup(o => o.FirstCoverSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_mathFirstCoverSecond_AB.Object);

            _handlerMath
                .Setup(o => o.FirstWrapSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_mathFirstWrapSecond_AB.Object);
            
            ConfigureChainLeadSyntax
                .WithHandlerMath(_handlerMath.Object)
                .AndWithConditionMath(_conditionMath.Object);
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
        public void A_Then_B_Returns_FirstThenSecond_AB_FromMath()
        {
            var product = _handlerA.Object.Then(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathFirstThenSecond_AB.Object));
        }

        [Test]
        public void FirstThenSecond_AB_Returns_FirstThenSecond_AB_FromMath()
        {
            var product = FirstThenSecond(_handlerA.Object, _handlerB.Object);
            Assert.That(product, Is.SameAs(_mathFirstThenSecond_AB.Object));
        }

        [Test]
        public void XThen_B_WhereXIs_A_Returns_FirstThenSecond_AB_FromMath()
        {
            var product = XThen(_handlerB.Object).WhereXIs(_handlerA.Object);
            Assert.That(product, Is.SameAs(_mathFirstThenSecond_AB.Object));
        }

        [Test]
        public void JoinFirstWithSecond_AB_Returns_JoinFirstWithSecond_AB_FromMath()
        {
            var product = JoinFirstWithSecond(_handlerA.Object, _handlerB.Object);
            Assert.That(product, Is.SameAs(_mathJoinFirstWithSecond_AB.Object));
        }

        [Test]
        public void Join_A_With_B_Returns_JoinFirstWithSecond_AB_FromMath()
        {
            var product = Join(_handlerA.Object).With(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathJoinFirstWithSecond_AB.Object));
        }

        [Test]
        public void JoinXWith_B_WhereXIs_A_JoinFirstWithSecond_AB_FromMath()
        {
            var product = JoinXWith(_handlerB.Object).WhereXIs(_handlerA.Object);
            Assert.That(product, Is.SameAs(_mathJoinFirstWithSecond_AB.Object));
        }

        [Test]
        public void MergeFirstWithSecond_AB_Returns_MergeFirstWithSecond_AB_FromMath()
        {
            var product = MergeFirstWithSecond(_handlerA.Object, _handlerB.Object);
            Assert.That(product, Is.SameAs(_mathMergeFirstWithSecond_AB.Object));
        }

        [Test]
        public void Merge_A_With_B_Returns_MergeFirstWithSecond_AB_FromMath()
        {
            var product = Merge(_handlerA.Object).With(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathMergeFirstWithSecond_AB.Object));
        }

        [Test]
        public void MergeXWith_B_WhereXIs_A_MergeFirstWithSecond_AB_FromMath()
        {
            var product = JoinXWith(_handlerB.Object).WhereXIs(_handlerA.Object);
            Assert.That(product, Is.SameAs(_mathMergeFirstWithSecond_AB.Object));
        }

        [Test]
        public void PutFirstInSecond_AB_Returns_PutFirstInSecond_AB_FromMath()
        {
            var product = PutFirstInSecond(_handlerA.Object, _handlerB.Object);
            Assert.That(product, Is.SameAs(_mathPutFirstInSecond_AB.Object));
        }

        [Test]
        public void Put_A_In_B_Returns_PutFirstInSecond_AB_FromMath()
        {
            var product = Put(_handlerA.Object).In(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathPutFirstInSecond_AB.Object));
        }

        [Test]
        public void PutXIn_B_WhereXIs_A_Returns_PutFirstInSecond_AB_FromMath()
        {
            var product = PutXIn(_handlerB.Object).WhereXIs(_handlerA.Object);
            Assert.That(product, Is.SameAs(_mathPutFirstInSecond_AB.Object));
        }

        [Test]
        public void InjectFirstIntoSecond_AB_Returns_InjectFirstIntoSecond_AB_FromMath()
        {
            var product = InjectFirstIntoSecond(_handlerA.Object, _handlerB.Object);
            Assert.That(product, Is.SameAs(_mathInjectFirstIntoSecond_AB.Object));
        }

        [Test]
        public void Inject_A_Into_B_InjectFirstIntoSecond_AB_FromMath()
        {
            var product = Inject(_handlerA.Object).Into(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathInjectFirstIntoSecond_AB.Object));
        }

        [Test]
        public void InjectXInto_B_WhereXIs_A_Returns_InjectFirstIntoSecond_AB_FromMath()
        {
            var product = InjectXInto(_handlerB.Object).WhereXIs(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathInjectFirstIntoSecond_AB.Object));
        }

        [Test]
        public void FirstCoverSecond_AB_Returns_FirstCoverSecond_AB_FromMath()
        {
            var product = FirstCoverSecond(_handlerA.Object, _handlerB.Object);
            Assert.That(product, Is.SameAs(_mathFirstCoverSecond_AB.Object));
        }

        [Test]
        public void Use_A_ToCover_B_Returns_FirstCoverSecond_AB_FromMath()
        {
            var product = Use(_handlerA.Object).ToCover(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathFirstCoverSecond_AB.Object));
        }

        [Test]
        public void XWrap_B_WhereCIs_A_Returns_FirstCoverSecond_AB_FromMath()
        {
            var product = XWrap(_handlerB.Object).WhereXIs(_handlerA.Object);
            Assert.That(product, Is.SameAs(_mathFirstCoverSecond_AB.Object));
        }

        [Test]
        public void FirstWrapSecond_AB_Returns_FirstWrapSecond_AB_FromMath()
        {
            var product = FirstWrapSecond(_handlerA.Object, _handlerB.Object);
            Assert.That(product, Is.SameAs(_mathFirstWrapSecond_AB.Object));
        }

        [Test]
        public void Use_A_ToCWrap_B_Returns_FirstWrapSecond_AB_FromMath()
        {
            var product = Use(_handlerA.Object).ToWrap(_handlerB.Object);
            Assert.That(product, Is.SameAs(_mathFirstWrapSecond_AB.Object));
        }

        [Test]
        public void XWrap_B_WhereCIs_A_Returns_FirstWrapSecond_AB_FromMath()
        {
            var product = XWrap(_handlerB.Object).WhereXIs(_handlerA.Object);
            Assert.That(product, Is.SameAs(_mathFirstWrapSecond_AB.Object));
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
