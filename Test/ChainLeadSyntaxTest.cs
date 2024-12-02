namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using Moq;

    using ChainLead.Contracts.Syntax;
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    [TestFixture]
    public class ChainLeadSyntaxTest
    {
        const int Arg = 7568;

        Mock<IHandlerMath> _handlerMath;
        Mock<IConditionMath> _conditionMath;

        string _handlersExecutionResult;

        Mock<IHandler<int>> _handler;
        Mock<IHandler<int>> _handlerA;
        Mock<IHandler<int>> _handlerB;
        Mock<IHandler<int>> _handlerC;
        Mock<IHandler<int>> _handlerAB;
        Mock<IHandler<int>> _handlerABC;

        Mock<ICondition<int>> _condition;
        Mock<ICondition<int>> _conditionA;
        Mock<ICondition<int>> _conditionB;

        [SetUp]
        public void Setup()
        {
            _handlerMath = new Mock<IHandlerMath>();
            _conditionMath = new Mock<IConditionMath>();

            _handler = new Mock<IHandler<int>>() { Name = nameof(_handler) };
            _handlerA = new Mock<IHandler<int>>() { Name = nameof(_handlerA) };
            _handlerB = new Mock<IHandler<int>>() { Name = nameof(_handlerB) };
            _handlerC = new Mock<IHandler<int>>() { Name = nameof(_handlerC) };
            _handlerAB = new Mock<IHandler<int>>() { Name = nameof(_handlerAB) };
            _handlerABC = new Mock<IHandler<int>>() { Name = nameof(_handlerABC) };

            _condition = new Mock<ICondition<int>>();
            _conditionA = new Mock<ICondition<int>>();
            _conditionB = new Mock<ICondition<int>>();

            _handlersExecutionResult = string.Empty;

            _handlerMath
                .Setup(o => o.InjectFirstIntoSecond(It.IsAny<IHandler<int>>(), It.IsAny<IHandler<int>>()))
                .Callback((IHandler<int> x, IHandler<int> y) =>
                {

                });

            _handlerA
                .Setup(o => o.Execute(Arg))
                .Callback(() => _handlersExecutionResult += "A");

            _handlerB
                .Setup(o => o.Execute(Arg))
                .Callback(() => _handlersExecutionResult += "B");

            _handlerC
                .Setup(o => o.Execute(Arg))
                .Callback(() => _handlersExecutionResult += "C");

            _handlerAB
                .Setup(o => o.Execute(Arg))
                .Callback(() =>
                {
                    _handlerA.Object.Execute(Arg);
                    _handlerB.Object.Execute(Arg);
                });

            _handlerABC
                .Setup(o => o.Execute(Arg))
                .Callback(() =>
                {
                    _handlerAB.Object.Execute(Arg);
                    _handlerC.Object.Execute(Arg);
                });

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
        public void AtomizeReturnsMathAtomizeExecutionResult()
        {
            _handlerMath
                .Setup(o => o.Atomize(_handlerA.Object))
                .Returns(_handlerB.Object);

            var product = _handlerA.Object.Atomize();

            Assert.That(product, Is.SameAs(_handlerB.Object));
        }

        [Test]
        public void A_Then_B_ReturnsEquivalentOf_FirstThenSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstThenSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = _handlerA.Object.Then(_handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult, 
                Is.EqualTo("AB"));
        }

        [Test]
        public void FirstThenSecond_AB_ReturnsEquivalentOf_FirstThenSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstThenSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = FirstThenSecond(_handlerA.Object, _handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void XThen_B_WhereXIs_A_ReturnsEquivalentOf_FirstThenSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstThenSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = XThen(_handlerB.Object).WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void JoinFirstWithSecond_AB_ReturnsEquivalentOf_JoinFirstWithSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.JoinFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = JoinFirstWithSecond(_handlerA.Object, _handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void Join_A_With_B_ReturnsEquivalentOf_JoinFirstWithSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.JoinFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = Join(_handlerA.Object).With(_handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void JoinXWith_B_WhereXIs_A_ReturnsEquivalentOf_JoinFirstWithSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.JoinFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = JoinXWith(_handlerB.Object).WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void MergeFirstWithSecond_AB_ReturnsEquivalentOf_MergeFirstWithSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.MergeFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = MergeFirstWithSecond(_handlerA.Object, _handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void Merge_A_With_B_ReturnsEquivalentOf_MergeFirstWithSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.MergeFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = Merge(_handlerA.Object).With(_handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void MergeXWith_B_WhereXIs_A_ReturnsEquivalentOf_MergeFirstWithSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.MergeFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = MergeXWith(_handlerB.Object).WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void PackFirstInSecond_AB_ReturnsEquivalentOf_PackFirstInSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.PackFirstInSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = PackFirstInSecond(_handlerA.Object, _handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void Pack_A_In_B_ReturnsEquivalentOf_PackFirstInSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.PackFirstInSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = Pack(_handlerA.Object).In(_handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void PackXIn_B_WhereXIs_A_ReturnsEquivalentOf_PackFirstInSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.PackFirstInSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = PackXIn(_handlerB.Object).WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void InjectFirstIntoSecond_AB_ReturnsEquivalentOf_InjectFirstIntoSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.InjectFirstIntoSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = InjectFirstIntoSecond(_handlerA.Object, _handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void Inject_A_Into_B_ReturnsEquivalentOf_InjectFirstIntoSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.InjectFirstIntoSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = Inject(_handlerA.Object).Into(_handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void InjectXInto_B_WhereXIs_A_ReturnsEquivalentOf_InjectFirstIntoSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.InjectFirstIntoSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = InjectXInto(_handlerB.Object).WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void FirstCoverSecond_AB_ReturnsEquivalentOf_FirstCoverSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstCoverSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = FirstCoverSecond(_handlerA.Object, _handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void Use_A_ToCover_B_ReturnsEquivalentOf_FirstCoverSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstCoverSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = Use(_handlerA.Object).ToCover(_handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void XCover_B_WhereXIs_A_ReturnsEquivalentOf_FirstCoverSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstCoverSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = XCover(_handlerB.Object).WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void FirstWrapSecond_AB_ReturnsEquivalentOf_FirstWrapSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstWrapSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = FirstWrapSecond(_handlerA.Object, _handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void Use_A_ToWrap_B_ReturnsEquivalentOf_FirstWrapSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstWrapSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = Use(_handlerA.Object).ToWrap(_handlerB.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        [Test]
        public void XWrap_B_WhereXIs_A_ReturnsEquivalentOf_FirstWrapSecond_AB_FromMath()
        {
            _handlerMath
                .Setup(o => o.FirstWrapSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            var handler = XWrap(_handlerB.Object).WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("AB"));
        }

        void SetupMathFirstThenSecond_ABC()
        {
            _handlerMath
                .Setup(o => o.FirstThenSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            _handlerMath
                .Setup(o => o.FirstThenSecond(_handlerAB.Object, _handlerC.Object))
                .Returns(_handlerABC.Object);
        }

        [Test]
        public void A_Then_B_Then_C_ReturnsEquivalentOf_FirstThenSecond_ABC_FromMath()
        {
            SetupMathFirstThenSecond_ABC();

            var handler = _handlerA.Object
                .Then(_handlerB.Object)
                .Then(_handlerC.Object);    

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void FirstThenSecond_ABC_ReturnsEquivalentOf_FirstThenSecond_ABC_FromMath()
        {
            SetupMathFirstThenSecond_ABC();

            var handler =
                FirstThenSecond(
                    FirstThenSecond(
                        _handlerA.Object,
                        _handlerB.Object),
                    _handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void XThen_B_Then_C_WhereXIs_A_ReturnsEquivalentOf_FirstThenSecond_ABC_FromMath()
        {
            SetupMathFirstThenSecond_ABC();

            var handler = XThen(_handlerB.Object)
                .Then(_handlerC.Object)
                .WhereXIs(_handlerA.Object);
            
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        void SetupMathJoinFirstWithSecond_ABC()
        {
            _handlerMath
                .Setup(o => o.JoinFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            _handlerMath
                .Setup(o => o.JoinFirstWithSecond(_handlerAB.Object, _handlerC.Object))
                .Returns(_handlerABC.Object);
        }

        [Test]
        public void JoinFirstWithSecond_ABC_ReturnsEquivalentOf_JoinFirstWithSecond_ABC_FromMath()
        {
            SetupMathJoinFirstWithSecond_ABC();

            var handler = 
                JoinFirstWithSecond(
                    JoinFirstWithSecond(
                        _handlerA.Object, 
                        _handlerB.Object),
                    _handlerC.Object);
            
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void Join_A_With_B_ThenWith_C_ReturnsEquivalentOf_JoinFirstWithSecond_ABC_FromMath()
        {
            SetupMathJoinFirstWithSecond_ABC();

            var handler = Join(_handlerA.Object).With(_handlerB.Object)
                .ThenWith(_handlerC.Object);
            
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void JoinXWith_B_ThenWith_C_WhereXIs_A_ReturnsEquivalentOf_JoinFirstWithSecond_ABC_FromMath()
        {
            SetupMathJoinFirstWithSecond_ABC();

            var handler = JoinXWith(_handlerB.Object)
                .ThenWith(_handlerC.Object)
                .WhereXIs(_handlerA.Object);
            
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        void SetupMergeFirstWithSecond_ABC()
        {
            _handlerMath
                .Setup(o => o.MergeFirstWithSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            _handlerMath
                .Setup(o => o.MergeFirstWithSecond(_handlerAB.Object, _handlerC.Object))
                .Returns(_handlerABC.Object);
        }

        [Test]
        public void MergeFirstWithSecond_ABC_ReturnsEquivalentOf_MergeFirstWithSecond_ABC_FromMath()
        {
            SetupMergeFirstWithSecond_ABC();

            var handler = 
                MergeFirstWithSecond(
                    MergeFirstWithSecond(
                        _handlerA.Object, 
                        _handlerB.Object),
                    _handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void Merge_A_With_B_ThenWith_C_ReturnsEquivalentOf_MergeFirstWithSecond_ABC_FromMath()
        {
            SetupMergeFirstWithSecond_ABC();

            var handler = Merge(_handlerA.Object).With(_handlerB.Object)
                .ThenWith(_handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void MergeXWith_B_ThenWith_C_WhereXIs_A_ReturnsEquivalentOf_MergeFirstWithSecond_ABC_FromMath()
        {
            SetupMergeFirstWithSecond_ABC();

            var handler = MergeXWith(_handlerB.Object)
                .ThenWith(_handlerC.Object)
                .WhereXIs(_handlerA.Object);
            
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        void SetupMathPackFirstInSecond_ABC()
        {
            _handlerMath
                .Setup(o => o.PackFirstInSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            _handlerMath
                .Setup(o => o.PackFirstInSecond(_handlerAB.Object, _handlerC.Object))
                .Returns(_handlerABC.Object);
        }

        [Test]
        public void PackFirstInSecond_ABC_ReturnsEquivalentOf_PackFirstInSecond_ABC_FromMath()
        {
            SetupMathPackFirstInSecond_ABC();

            var handler = 
                PackFirstInSecond(
                    PackFirstInSecond(
                        _handlerA.Object,
                        _handlerB.Object),
                    _handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void Pack_A_In_B_ThenIn_C_ReturnsEquivalentOf_PackFirstInSecond_ABC_FromMath()
        {
            SetupMathPackFirstInSecond_ABC();

            var handler = Pack(_handlerA.Object).In(_handlerB.Object)
                .ThenIn(_handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void PackXIn_B_ThenIn_C_WhereXIs_A_ReturnsEquivalentOf_PackFirstInSecond_ABC_FromMath()
        {
            SetupMathPackFirstInSecond_ABC();

            var handler = PackXIn(_handlerB.Object)
                .ThenIn(_handlerC.Object)
                .WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        void SetupMathInjectFirstIntoSecond()
        {
            _handlerMath
                .Setup(o => o.InjectFirstIntoSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            _handlerMath
                .Setup(o => o.InjectFirstIntoSecond(_handlerAB.Object, _handlerC.Object))
                .Returns(_handlerABC.Object);
        }

        [Test]
        public void InjectFirstIntoSecond_ABC_ReturnsEquivalentOf_InjectFirstIntoSecond_ABC_FromMath()
        {
            SetupMathInjectFirstIntoSecond();

            var handler =
                InjectFirstIntoSecond(
                    InjectFirstIntoSecond(
                        _handlerA.Object,
                        _handlerB.Object),
                    _handlerC.Object);
            
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void Inject_A_Into_B_ThenInto_C_ReturnsEquivalentOf_InjectFirstIntoSecond_ABC_FromMath()
        {
            SetupMathInjectFirstIntoSecond();

            var handler = Inject(_handlerA.Object).Into(_handlerB.Object)
                .ThenInto(_handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void InjectXInto_B_ThenInto_C_WhereXIs_A_ReturnsEquivalentOf_InjectFirstIntoSecond_ABC_FromMath()
        {
            SetupMathInjectFirstIntoSecond();

            var handler = InjectXInto(_handlerB.Object)
                .ThenInto(_handlerC.Object)
                .WhereXIs(_handlerA.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        void SetupMathFirstCoverSecond_ABC()
        {
            _handlerMath
                .Setup(o => o.FirstCoverSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            _handlerMath
                .Setup(o => o.FirstCoverSecond(_handlerAB.Object, _handlerC.Object))
                .Returns(_handlerABC.Object);
        }

        [Test]
        public void FirstCoverSecond_ABC_ReturnsEquivalentOf_FirstCoverSecond_ABC_FromMath()
        {
            SetupMathFirstCoverSecond_ABC();

            var handler =
                FirstCoverSecond(
                    FirstCoverSecond(
                        _handlerA.Object,
                        _handlerB.Object),
                    _handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void Use_A_ToCover_B_ThenCover_C_ReturnsEquivalentOf_FirstCoverSecond_ABC_FromMath()
        {
            SetupMathFirstCoverSecond_ABC();

            var handler = Use(_handlerA.Object).ToCover(_handlerB.Object)
                .ThenCover(_handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void XCover_B_ThenCover_C_WhereXIs_A_ReturnsEquivalentOf_FirstCoverSecond_ABC_FromMath()
        {
            SetupMathFirstCoverSecond_ABC();

            var handler = XCover(_handlerB.Object)
                .ThenCover(_handlerC.Object)
                .WhereXIs(_handlerA.Object);
            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        void SetupMathFirstWrapSecond_ABC()
        {
            _handlerMath
                .Setup(o => o.FirstWrapSecond(_handlerA.Object, _handlerB.Object))
                .Returns(_handlerAB.Object);

            _handlerMath
                .Setup(o => o.FirstWrapSecond(_handlerAB.Object, _handlerC.Object))
                .Returns(_handlerABC.Object);
        }

        [Test]
        public void FirstWrapSecond_ABC_ReturnsEquivalentOf_FirstWrapSecond_ABC_FromMath()
        {
            SetupMathFirstWrapSecond_ABC();

            var handler = 
                FirstWrapSecond(
                    FirstWrapSecond(
                        _handlerA.Object,
                        _handlerB.Object),
                    _handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void Use_A_ToWrap_B_ThenWrap_C_ReturnsEquivalentOf_FirstWrapSecond_ABC_FromMath()
        {
            SetupMathFirstWrapSecond_ABC();

            var handler = Use(_handlerA.Object).ToWrap(_handlerB.Object)
                .ThenWrap(_handlerC.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
        }

        [Test]
        public void XWrap_B_ThenWrap_C_WhereXIs_A_ReturnsEquivalentOf_FirstWrapSecond_ABC_FromMath()
        {
            SetupMathFirstWrapSecond_ABC();

            var handler = XWrap(_handlerB.Object)
                .ThenWrap(_handlerC.Object)
                .WhereXIs(_handlerA.Object);

            handler.Execute(Arg);

            Assert.That(_handlersExecutionResult,
                Is.EqualTo("ABC"));
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
        public void MakeConditionReturnsMathMakeConditionExecutionResult()
        {
            var seed = new Func<int, bool>(_ => true);

            _conditionMath
                .Setup(o => o.MakeCondition(seed))
                .Returns(_condition.Object);

            var product = MakeCondition(seed);

            Assert.That(product, Is.SameAs(_condition.Object));
        }

        [Test]
        public void AsConditionReturnsMathMakeCondition2ExecutionResult()
        {
            var seed = new Func<int, bool>(_ => true);

            _conditionMath
                .Setup(o => o.MakeCondition(seed))
                .Returns(_condition.Object);

            var product = seed.AsCondition();

            Assert.That(product, Is.SameAs(_condition.Object));
        }


        [Test]
        public void OrReturnsMathOrExecutionResult()
        {
            _conditionMath
                .Setup(o => o.Or(_conditionA.Object, _conditionB.Object))
                .Returns(_condition.Object);

            var product = _conditionA.Object.Or(_conditionB.Object);

            Assert.That(product, Is.SameAs(_condition.Object));
        }

        [Test]
        public void NotReturnsMathNotExecutionResult()
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

        [Test]
        public void IsPredictableTrueExecutesMathIsIsPredictableTrue(
            [Values(false, true)] bool expectedResult)
        {
            _conditionMath
                .Setup(o => o.IsPredictableTrue(_condition.Object))
                .Returns(expectedResult);

            var result = _condition.Object.IsPredictableTrue();

            _conditionMath.Verify(o => o.IsPredictableTrue(_condition.Object), Times.Once);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsPredictableFalseExecutesMathIsIsPredictableFalse(
            [Values(false, true)] bool expectedResult)
        {
            _conditionMath
                .Setup(o => o.IsPredictableFalse(_condition.Object))
                .Returns(expectedResult);

            var result = _condition.Object.IsPredictableFalse();

            _conditionMath.Verify(o => o.IsPredictableFalse(_condition.Object), Times.Once);
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
