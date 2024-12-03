namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using System;
    using Moq;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    
    [TestFixture]
    public class ChainLeadSyntaxTest
    {
        const int Arg = 7568;
        const string A = "A", B = "B", C = "C", AB = "AB", ABC = "ABC";
        
        class Mocks
        {
            public HandlerMathMock HandlerMath { get; }

            public ConditionMathMock ConditionMath { get; }

            public Dictionary<string, HandlerMock> Handler { get; }

            public Dictionary<string, Mock<ICondition<int>>> Condition { get; }

            public Mocks()
            {
                Handler = new Dictionary<string, HandlerMock>();
                Condition = new Dictionary<string, Mock<ICondition<int>>>();

                foreach (var i in new[] { A, B, C, AB, ABC })
                {
                    Handler.Add(i, new HandlerMock(Handler) { Name = $"h[{i}]" });
                    Condition.Add(i, new Mock<ICondition<int>> { Name = $"c[{i}]" });
                }
                
                HandlerMath = new HandlerMathMock(Handler, Condition);
                ConditionMath = new ConditionMathMock(Condition);
            }

            public class HandlerMathMock(
                Dictionary<string, HandlerMock> handlers,
                Dictionary<string, Mock<ICondition<int>>> conditions) 
                : Mock<IHandlerMath>
            {
                public void Setup__Zero(string returns) =>
                     Setup(o => o.Zero<int>())
                    .Returns(handlers[returns].Object);

                public void Setup__IsZero(string i, bool returns) =>
                     Setup(o => o.IsZero(handlers[i].Object))
                    .Returns(returns);

                public void Setup__MakeHandler(string i)
                {
                    Action<int>? action = default;

                    Setup(o => o.MakeHandler(It.IsAny<Action<int>>()))
                   .Returns(handlers[i].Object)
                   .Callback((Action<int> f) => action = f);

                    handlers[i]
                        .Setup(o => o.Execute(Arg))
                        .Callback((int x) => action(x));
                }

                public void Setup__FirstThenSecond(string first, string second, string returns) =>
                     Setup(o => o.FirstThenSecond(
                        handlers[first].Object,
                        handlers[second].Object))
                    .Returns(handlers[returns].Object);

                public void Setup__JoinFirstWithSecond(string first, string second, string returns) =>
                     Setup(o => o.JoinFirstWithSecond(
                        handlers[first].Object,
                        handlers[second].Object))
                    .Returns(handlers[returns].Object);

                public void Setup__MergeFirstWithSecond(string first, string second, string returns) =>
                     Setup(o => o.MergeFirstWithSecond(
                        handlers[first].Object,
                        handlers[second].Object))
                    .Returns(handlers[returns].Object);

                public void Setup__PackFirstInSecond(string first, string second, string returns) =>
                     Setup(o => o.PackFirstInSecond(
                        handlers[first].Object,
                        handlers[second].Object))
                    .Returns(handlers[returns].Object);

                public void Setup__InjectFirstIntoSecond(string first, string second, string returns) =>
                     Setup(o => o.InjectFirstIntoSecond(
                        handlers[first].Object,
                        handlers[second].Object))
                    .Returns(handlers[returns].Object);

                public void Setup__FirstCoverSecond(string first, string second, string returns) =>
                     Setup(o => o.FirstCoverSecond(
                        handlers[first].Object,
                        handlers[second].Object))
                    .Returns(handlers[returns].Object);

                public void Setup__FirstWrapSecond(string first, string second, string returns) =>
                    Setup(o => o.FirstWrapSecond(
                        handlers[first].Object,
                        handlers[second].Object))
                   .Returns(handlers[returns].Object);

                public void Setup__Conditional(string handler, string condition, string returns) =>
                     Setup(o => o.Conditional(
                         handlers[handler].Object,
                         conditions[condition].Object))
                    .Returns(handlers[returns].Object);

                public void Setup__Atomize(string i, string returns) =>
                     Setup(o => o.Atomize(handlers[i].Object))
                    .Returns(handlers[returns].Object);
            }

            public class HandlerMock(
                Dictionary<string, HandlerMock> handlers) 
                : Mock<IHandler<int>>
            {
                public void Setup__Execute__ToCall(Action<int> f) =>
                     Setup(o => o.Execute(Arg))
                    .Callback((int x) => f(x));

                public void Setup__Execute__ToLogInto(IList<string> acc) =>
                     Setup(o => o.Execute(Arg))
                    .Callback(() => acc.Add(Name));

                public void Setup__Execute__ToDelegateTo(params string[] indexes) =>
                     Setup(o => o.Execute(Arg))
                    .Callback(() =>
                    {
                        foreach (var i in indexes)
                            handlers[i].Object.Execute(Arg);
                    });
            }

            public class ConditionMathMock(
                Dictionary<string, Mock<ICondition<int>>> conditions) 
                : Mock<IConditionMath>
            {
                public void Setup__MakeCondition(string i)
                {
                    Func<int, bool>? predicate = default;

                    Setup(o => o.MakeCondition(It.IsAny<Func<int, bool>>()))
                   .Returns(conditions[i].Object)
                   .Callback((Func<int, bool> f) => predicate = f);

                    conditions[i]
                        .Setup(o => o.Check(Arg))
                        .Returns((int x) => predicate!(x));
                }

                public void Setup__True(string returns) =>
                     Setup(o => o.True<int>())
                    .Returns(conditions[returns].Object);

                public void Setup__False(string returns) =>
                     Setup(o => o.False<int>())
                    .Returns(conditions[returns].Object);

                public void Setup__IsPredictableTrue(string i, bool returns) =>
                     Setup(o => o.IsPredictableTrue(conditions[i].Object))
                    .Returns(returns);

                public void Setup__IsPredictableFalse(string i, bool returns) =>
                     Setup(o => o.IsPredictableFalse(conditions[i].Object))
                    .Returns(returns);

                public void Setup__Or(string first, string second, string returns) =>
                     Setup(o => o.Or(
                        conditions[first].Object,
                        conditions[second].Object))
                    .Returns(conditions[returns].Object);

                public void Setup__And(string first, string second, string returns) =>
                     Setup(o => o.And(
                        conditions[first].Object,
                        conditions[second].Object))
                    .Returns(conditions[returns].Object);

                public void Setup__Not(string i, string returns) =>
                     Setup(o => o.Not(conditions[i].Object))
                    .Returns(conditions[returns].Object);
            }
        }

        Mocks _mockOf;

        [SetUp]
        public void Setup()
        {
            _mockOf = new Mocks();

            ChainLeadSyntax.Configure(
                _mockOf.HandlerMath.Object,
                _mockOf.ConditionMath.Object);
        }


        [Test]
        public void Zero_Test()
        {
            _mockOf.HandlerMath.Setup__Zero(returns: A);

            Assert.That(Handler<int>.Zero,
                Is.SameAs(_mockOf.Handler[A].Object));
        }

        [Test]
        public void IsZero_Test(
            [Values(false, true)] bool expectedResult)
        {
            _mockOf.HandlerMath.Setup__IsZero(A, returns: expectedResult);

            Assert.That(_mockOf.Handler[A].Object.IsZero(), 
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void MakeHandler__Test()
        {
            bool funcCalled = false;
            Action<int> func = _ => funcCalled = true;

            _mockOf.HandlerMath.Setup__MakeHandler(A);

            MakeHandler(func).Execute(Arg);

            Assert.That(funcCalled, Is.True);
        }

        [Test]
        public void AsHandler__Test()
        {
            bool funcCalled = false;
            Action<int> func = _ => funcCalled = true;

            _mockOf.HandlerMath.Setup__MakeHandler(A);

            func.AsHandler().Execute(Arg);

            Assert.That(funcCalled, Is.True);
        }

        [Test]
        public void A_Then_B_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            _mockOf.Handler[A].Object
                .Then(_mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void FirstThenSecond_AB_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            FirstThenSecond(
                   _mockOf.Handler[A].Object,
                   _mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void XThen_B_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            XThen(_mockOf.Handler[B].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void JoinFirstWithSecond_AB_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            JoinFirstWithSecond(
                    _mockOf.Handler[A].Object,
                    _mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void Join_A_With_B_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            Join(_mockOf.Handler[A].Object)
                .With(_mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void JoinXWith_B_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            JoinXWith(_mockOf.Handler[B].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void MergeFirstWithSecond_AB_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            MergeFirstWithSecond(
                    _mockOf.Handler[A].Object,
                    _mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void Merge_A_With_B_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            Merge(_mockOf.Handler[A].Object)
                .With(_mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void MergeXWith_B_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            MergeXWith(_mockOf.Handler[B].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void PackFirstInSecond_AB_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            PackFirstInSecond(
                    _mockOf.Handler[A].Object,
                    _mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void Pack_A_In_B_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            Pack(_mockOf.Handler[A].Object)
                .In(_mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void PackXIn_B_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            PackXIn(_mockOf.Handler[B].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void InjectFirstIntoSecond_AB_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            InjectFirstIntoSecond(
                    _mockOf.Handler[A].Object,
                    _mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void Inject_A_Into_B_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            Inject(_mockOf.Handler[A].Object)
                .Into(_mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void InjectXInto_B_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            InjectXInto(_mockOf.Handler[B].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void FirstCoverSecond_AB_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            FirstCoverSecond(
                    _mockOf.Handler[A].Object,
                    _mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void Use_A_ToCover_B_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            Use(_mockOf.Handler[A].Object)
                .ToCover(_mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void XCover_B_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            XCover(_mockOf.Handler[B].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void FirstWrapSecond_AB_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            FirstWrapSecond(
                    _mockOf.Handler[A].Object,
                    _mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void Use_A_ToWrap_B_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            Use(_mockOf.Handler[A].Object)
                .ToWrap(_mockOf.Handler[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void XWrap_B_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            XWrap(_mockOf.Handler[B].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]" }));
        }

        [Test]
        public void A_Then_B_Then_C_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            _mockOf.Handler[A].Object
                .Then(_mockOf.Handler[B].Object)
                .Then(_mockOf.Handler[C].Object)    
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void FirstThenSecond_ABC_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            FirstThenSecond(
                    FirstThenSecond(
                        _mockOf.Handler[A].Object,
                        _mockOf.Handler[B].Object),
                    _mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void XThen_B_Then_C_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            XThen(_mockOf.Handler[B].Object)
                .Then(_mockOf.Handler[C].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void JoinFirstWithSecond_ABC_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            JoinFirstWithSecond(
                    JoinFirstWithSecond(
                        _mockOf.Handler[A].Object,
                        _mockOf.Handler[B].Object),
                    _mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void Join_A_With_B_ThenWith_C_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            Join(_mockOf.Handler[A].Object)
                .With(_mockOf.Handler[B].Object)
                .ThenWith(_mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void JoinXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            JoinXWith(_mockOf.Handler[B].Object)
                .ThenWith(_mockOf.Handler[C].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void MergeFirstWithSecond_ABC_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            MergeFirstWithSecond(
                    MergeFirstWithSecond(
                        _mockOf.Handler[A].Object,
                        _mockOf.Handler[B].Object),
                    _mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void Merge_A_With_B_ThenWith_C_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            Merge(_mockOf.Handler[A].Object)
                .With(_mockOf.Handler[B].Object)
                .ThenWith(_mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void MergeXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            MergeXWith(_mockOf.Handler[B].Object)
                .ThenWith(_mockOf.Handler[C].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void PackFirstInSecond_ABC_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            PackFirstInSecond(
                    PackFirstInSecond(
                        _mockOf.Handler[A].Object,
                        _mockOf.Handler[B].Object),
                    _mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void Pack_A_In_B_ThenIn_C_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            Pack(_mockOf.Handler[A].Object)
                .In(_mockOf.Handler[B].Object)
                .ThenIn(_mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void PackXIn_B_ThenIn_C_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            PackXIn(_mockOf.Handler[B].Object)
                .ThenIn(_mockOf.Handler[C].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void InjectFirstIntoSecond_ABC_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            InjectFirstIntoSecond(
                    InjectFirstIntoSecond(
                        _mockOf.Handler[A].Object,
                        _mockOf.Handler[B].Object),
                    _mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void Inject_A_Into_B_ThenInto_C_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            Inject(_mockOf.Handler[A].Object)
                .Into(_mockOf.Handler[B].Object)
                .ThenInto(_mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void InjectXInto_B_ThenInto_C_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            InjectXInto(_mockOf.Handler[B].Object)
                .ThenInto(_mockOf.Handler[C].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void FirstCoverSecond_ABC_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            FirstCoverSecond(
                    FirstCoverSecond(
                        _mockOf.Handler[A].Object,
                        _mockOf.Handler[B].Object),
                    _mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void Use_A_ToCover_B_ThenCover_C_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            Use(_mockOf.Handler[A].Object)
                .ToCover(_mockOf.Handler[B].Object)
                .ThenCover(_mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void XCover_B_ThenCover_C_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            XCover(_mockOf.Handler[B].Object)
                .ThenCover(_mockOf.Handler[C].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void FirstWrapSecond_ABC_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            FirstWrapSecond(
                    FirstWrapSecond(
                        _mockOf.Handler[A].Object,
                        _mockOf.Handler[B].Object),
                    _mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void Use_A_ToWrap_B_ThenWrap_C_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            Use(_mockOf.Handler[A].Object)
                .ToWrap(_mockOf.Handler[B].Object)
                .ThenWrap(_mockOf.Handler[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void XWrap_B_ThenWrap_C_WhereXIs_A_Test()
        {
            List<string> execution = [];

            _mockOf.Handler[A].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[B].Setup__Execute__ToLogInto(execution);
            _mockOf.Handler[C].Setup__Execute__ToLogInto(execution);

            _mockOf.Handler[AB].Setup__Execute__ToDelegateTo(A, B);
            _mockOf.Handler[ABC].Setup__Execute__ToDelegateTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            XWrap(_mockOf.Handler[B].Object)
                .ThenWrap(_mockOf.Handler[C].Object)
                .WhereXIs(_mockOf.Handler[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { "h[A]", "h[B]", "h[C]" }));
        }

        [Test]
        public void When_Test()
        {
            _mockOf.HandlerMath.Setup__Conditional(A, A, returns: B);

            var handler = _mockOf.Handler[A].Object
                .When(_mockOf.Condition[A].Object);

            Assert.That(handler, 
                Is.SameAs(_mockOf.Handler[B].Object));
        }

        [Test]
        public void WithConditionThat_Test()
        {
            _mockOf.HandlerMath.Setup__Conditional(A, A, returns: B);

            var handler = WithConditionThat
                (_mockOf.Condition[A].Object)
                (_mockOf.Handler[A].Object);

            Assert.That(handler,
                Is.SameAs(_mockOf.Handler[B].Object));
        }

        [Test]
        public void MakeCondition__Test()
        {
            bool funcCalled = false;
            Func<int, bool> func = _ => funcCalled = true;

            _mockOf.ConditionMath.Setup__MakeCondition(A);

            Assert.That(MakeCondition(func).Check(Arg), Is.True);
            Assert.That(funcCalled, Is.True);
        }

        [Test]
        public void AsCondition__Test()
        {
            bool funcCalled = false;
            Func<int, bool> func = _ => funcCalled = true;

            _mockOf.ConditionMath.Setup__MakeCondition(A);

            Assert.That(func.AsCondition().Check(Arg), Is.True);
            Assert.That(funcCalled, Is.True);
        }

        [Test]
        public void AsCondition2__Test()
        {
            bool funcCalled = false;
            Predicate<int> func = _ => funcCalled = true;

            _mockOf.ConditionMath.Setup__MakeCondition(A);

            Assert.That(func.AsCondition().Check(Arg), Is.True);
            Assert.That(funcCalled, Is.True);
        }

        [Test]
        public void True_Test()
        {
            _mockOf.ConditionMath.Setup__True(returns: A);

            Assert.That(
                Condition<int>.True,
                Is.SameAs(_mockOf.Condition[A].Object));
        }

        [Test]
        public void False_Test()
        {
            _mockOf.ConditionMath.Setup__False(returns: A);

            Assert.That(
                Condition<int>.False,
                Is.SameAs(_mockOf.Condition[A].Object));
        }

        [Test]
        public void IsPredictableTrue_Test(
            [Values(false, true)] bool expectedResult)
        {
            _mockOf.ConditionMath.Setup__IsPredictableTrue(A, returns: expectedResult);

            Assert.That(_mockOf.Condition[A].Object.IsPredictableTrue(), 
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsPredictableFalse_Test(
            [Values(false, true)] bool expectedResult)
        {
            _mockOf.ConditionMath.Setup__IsPredictableFalse(A, returns: expectedResult);

            Assert.That(_mockOf.Condition[A].Object.IsPredictableFalse(),
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void Or_Test()
        {
            _mockOf.ConditionMath.Setup__Or(A, B, returns: AB);

            var aOrB = _mockOf.Condition[A].Object
                .Or(_mockOf.Condition[B].Object);

            Assert.That(aOrB,
                Is.SameAs(_mockOf.Condition[AB].Object));
        }

        [Test]
        public void And_Test()
        {
            _mockOf.ConditionMath.Setup__And(A, B, returns: AB);

            var aAndB = _mockOf.Condition[A].Object
                .And(_mockOf.Condition[B].Object);

            Assert.That(aAndB,
                Is.SameAs(_mockOf.Condition[AB].Object));
        }

        [Test]
        public void Not_Test()
        {
            _mockOf.ConditionMath.Setup__Not(A, returns: B);

            Assert.That(Not(_mockOf.Condition[A].Object),
                Is.SameAs(_mockOf.Condition[B].Object));
        }

        [Test]
        public void AtomizeTest()
        {
            _mockOf.HandlerMath.Setup__Atomize(A, returns: B);

            Assert.That(_mockOf.Handler[A].Object.Atomize(),
                Is.SameAs(_mockOf.Handler[B].Object));
        }

        [Test]
        public void ReverseFunctionTest()
        {
            string Log(string a, string b) => a + b;

            var result = Reverse<string>(Log)("A", "B");

            Assert.That(result, Is.EqualTo("BA"));
        }
    }
}
