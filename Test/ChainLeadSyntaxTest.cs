namespace ChainLead.Test
{
    using ChainLead.Contracts.Syntax;
    using ChainLead.Test.Help;
    using System;

    using static ChainLead.Test.Help.Constants;
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    [TestFixture]
    public class ChainLeadSyntaxTest
    {
        private static readonly HandlerIndex
            AB = HandlerIndex.Make("AB"),
            ABC = HandlerIndex.Make("ABC");

        private static readonly ConditionIndex
            XY = ConditionIndex.Make("XY");

        ChainLeadMocks _mockOf;

        [SetUp]
        public void Setup()
        {
            _mockOf = new ChainLeadMocks([A, B, C, AB, ABC], [X, Y, XY]);

            ChainLeadSyntax.Configure(
                _mockOf.HandlerMath.Object,
                _mockOf.ConditionMath.Object);
        }


        [Test]
        public void Zero_Test()
        {
            _mockOf.HandlerMath.Setup__Zero(returns: A);

            Assert.That(Handler<int>.Zero,
                Is.SameAs(_mockOf.Handlers[A].Object));
        }

        [Test]
        public void IsZero_Test(
            [Values(false, true)] bool expectedResult)
        {
            _mockOf.HandlerMath.Setup__IsZero(A, returns: expectedResult);

            Assert.That(_mockOf.Handlers[A].Object.IsZero(), 
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void MakeHandler__Test()
        {
            bool funcCalled = false;
            Action<int> func = _ => funcCalled = true;

            _mockOf.HandlerMath.Setup__MakeHandler(A);

            MakeHandler(func).Execute(Arg);

            Assert.That(funcCalled);
        }

        [Test]
        public void AsHandler__Test()
        {
            bool funcCalled = false;
            Action<int> func = _ => funcCalled = true;

            _mockOf.HandlerMath.Setup__MakeHandler(A);

            func.AsHandler().Execute(Arg);

            Assert.That(funcCalled);
        }

        [Test]
        public void A_Then_B_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            _mockOf.Handlers[A].Object
                .Then(_mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstThenSecond_AB_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            FirstThenSecond(
                   _mockOf.Handlers[A].Object,
                   _mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XThen_B_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            XThen(_mockOf.Handlers[B].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void JoinFirstWithSecond_AB_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            JoinFirstWithSecond(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Join_A_With_B_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            Join(_mockOf.Handlers[A].Object)
                .With(_mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void JoinXWith_B_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            JoinXWith(_mockOf.Handlers[B].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void MergeFirstWithSecond_AB_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            MergeFirstWithSecond(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Merge_A_With_B_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            Merge(_mockOf.Handlers[A].Object)
                .With(_mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void MergeXWith_B_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            MergeXWith(_mockOf.Handlers[B].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void PackFirstInSecond_AB_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            PackFirstInSecond(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Pack_A_In_B_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            Pack(_mockOf.Handlers[A].Object)
                .In(_mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void PackXIn_B_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            PackXIn(_mockOf.Handlers[B].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void InjectFirstIntoSecond_AB_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            InjectFirstIntoSecond(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Inject_A_Into_B_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            Inject(_mockOf.Handlers[A].Object)
                .Into(_mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void InjectXInto_B_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            InjectXInto(_mockOf.Handlers[B].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstCoverSecond_AB_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            FirstCoverSecond(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Use_A_ToCover_B_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            Use(_mockOf.Handlers[A].Object)
                .ToCover(_mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XCover_B_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            XCover(_mockOf.Handlers[B].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstWrapSecond_AB_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            FirstWrapSecond(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Use_A_ToWrap_B_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            Use(_mockOf.Handlers[A].Object)
                .ToWrap(_mockOf.Handlers[B].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XWrap_B_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            XWrap(_mockOf.Handlers[B].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void A_Then_B_Then_C_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);
            
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            _mockOf.Handlers[A].Object
                .Then(_mockOf.Handlers[B].Object)
                .Then(_mockOf.Handlers[C].Object)    
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstThenSecond_ABC_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);
            
            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            FirstThenSecond(
                    FirstThenSecond(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XThen_B_Then_C_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            XThen(_mockOf.Handlers[B].Object)
                .Then(_mockOf.Handlers[C].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void JoinFirstWithSecond_ABC_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            JoinFirstWithSecond(
                    JoinFirstWithSecond(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Join_A_With_B_ThenWith_C_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            Join(_mockOf.Handlers[A].Object)
                .With(_mockOf.Handlers[B].Object)
                .ThenWith(_mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void JoinXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            JoinXWith(_mockOf.Handlers[B].Object)
                .ThenWith(_mockOf.Handlers[C].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void MergeFirstWithSecond_ABC_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            MergeFirstWithSecond(
                    MergeFirstWithSecond(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Merge_A_With_B_ThenWith_C_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            Merge(_mockOf.Handlers[A].Object)
                .With(_mockOf.Handlers[B].Object)
                .ThenWith(_mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void MergeXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            MergeXWith(_mockOf.Handlers[B].Object)
                .ThenWith(_mockOf.Handlers[C].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void PackFirstInSecond_ABC_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            PackFirstInSecond(
                    PackFirstInSecond(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Pack_A_In_B_ThenIn_C_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            Pack(_mockOf.Handlers[A].Object)
                .In(_mockOf.Handlers[B].Object)
                .ThenIn(_mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void PackXIn_B_ThenIn_C_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            PackXIn(_mockOf.Handlers[B].Object)
                .ThenIn(_mockOf.Handlers[C].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void InjectFirstIntoSecond_ABC_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            InjectFirstIntoSecond(
                    InjectFirstIntoSecond(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Inject_A_Into_B_ThenInto_C_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            Inject(_mockOf.Handlers[A].Object)
                .Into(_mockOf.Handlers[B].Object)
                .ThenInto(_mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void InjectXInto_B_ThenInto_C_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            InjectXInto(_mockOf.Handlers[B].Object)
                .ThenInto(_mockOf.Handlers[C].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstCoverSecond_ABC_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            FirstCoverSecond(
                    FirstCoverSecond(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Use_A_ToCover_B_ThenCover_C_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            Use(_mockOf.Handlers[A].Object)
                .ToCover(_mockOf.Handlers[B].Object)
                .ThenCover(_mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XCover_B_ThenCover_C_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            XCover(_mockOf.Handlers[B].Object)
                .ThenCover(_mockOf.Handlers[C].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstWrapSecond_ABC_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            FirstWrapSecond(
                    FirstWrapSecond(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Use_A_ToWrap_B_ThenWrap_C_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            Use(_mockOf.Handlers[A].Object)
                .ToWrap(_mockOf.Handlers[B].Object)
                .ThenWrap(_mockOf.Handlers[C].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XWrap_B_ThenWrap_C_WhereXIs_A_Test()
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            _mockOf.Handlers[AB].AddDelegationTo(A, B);
            _mockOf.Handlers[ABC].AddDelegationTo(AB, C);

            _mockOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _mockOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            XWrap(_mockOf.Handlers[B].Object)
                .ThenWrap(_mockOf.Handlers[C].Object)
                .WhereXIs(_mockOf.Handlers[A].Object)
                .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void When_Test()
        {
            _mockOf.HandlerMath.Setup__Conditional(A, X, returns: B);

            var handler = _mockOf.Handlers[A].Object
                .When(_mockOf.Conditions[X].Object);

            Assert.That(handler, 
                Is.SameAs(_mockOf.Handlers[B].Object));
        }

        [Test]
        public void WithConditionThat_Test()
        {
            _mockOf.HandlerMath.Setup__Conditional(A, X, returns: B);

            var handler = WithConditionThat
                (_mockOf.Conditions[X].Object)
                (_mockOf.Handlers[A].Object);

            Assert.That(handler,
                Is.SameAs(_mockOf.Handlers[B].Object));
        }

        [Test]
        public void MakeCondition__Test()
        {
            bool funcCalled = false;
            Func<int, bool> func = _ => funcCalled = true;

            _mockOf.ConditionMath.Setup__MakeCondition(X);

            Assert.Multiple(() =>
            {
                Assert.That(MakeCondition(func).Check(Arg));
                Assert.That(funcCalled);
            });
        }

        [Test]
        public void AsCondition__Test()
        {
            bool funcCalled = false;
            Func<int, bool> func = _ => funcCalled = true;

            _mockOf.ConditionMath.Setup__MakeCondition(X);

            Assert.Multiple(() =>
            {
                Assert.That(func.AsCondition().Check(Arg));
                Assert.That(funcCalled);
            });
        }

        [Test]
        public void AsCondition2__Test()
        {
            bool funcCalled = false;
            Predicate<int> func = _ => funcCalled = true;

            _mockOf.ConditionMath.Setup__MakeCondition(X);

            Assert.Multiple(() =>
            {
                Assert.That(func.AsCondition().Check(Arg));
                Assert.That(funcCalled);
            });
        }

        [Test]
        public void True_Test()
        {
            _mockOf.ConditionMath.Setup__True(returns: X);

            Assert.That(Condition<int>.True,
                Is.SameAs(_mockOf.Conditions[X].Object));
        }

        [Test]
        public void False_Test()
        {
            _mockOf.ConditionMath.Setup__False(returns: X);

            Assert.That(Condition<int>.False,
                Is.SameAs(_mockOf.Conditions[X].Object));
        }

        [Test]
        public void IsPredictableTrue_Test(
            [Values(false, true)] bool expectedResult)
        {
            _mockOf.ConditionMath.Setup__IsPredictableTrue(X, returns: expectedResult);

            Assert.That(_mockOf.Conditions[X].Object.IsPredictableTrue(), 
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsPredictableFalse_Test(
            [Values(false, true)] bool expectedResult)
        {
            _mockOf.ConditionMath.Setup__IsPredictableFalse(X, returns: expectedResult);

            Assert.That(_mockOf.Conditions[X].Object.IsPredictableFalse(),
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void Or_Test()
        {
            _mockOf.ConditionMath.Setup__Or(X, Y, returns: XY);

            var aOrB = _mockOf.Conditions[X].Object
                .Or(_mockOf.Conditions[Y].Object);

            Assert.That(aOrB,
                Is.SameAs(_mockOf.Conditions[XY].Object));
        }

        [Test]
        public void And_Test()
        {
            _mockOf.ConditionMath.Setup__And(X, Y, returns: XY);

            var aAndB = _mockOf.Conditions[X].Object
                .And(_mockOf.Conditions[Y].Object);

            Assert.That(aAndB,
                Is.SameAs(_mockOf.Conditions[XY].Object));
        }

        [Test]
        public void Not_Test()
        {
            _mockOf.ConditionMath.Setup__Not(X, returns: Y);

            Assert.That(Not(_mockOf.Conditions[X].Object),
                Is.SameAs(_mockOf.Conditions[Y].Object));
        }

        [Test]
        public void AtomizeTest()
        {
            _mockOf.HandlerMath.Setup__Atomize(A, returns: B);

            Assert.That(_mockOf.Handlers[A].Object.Atomize(),
                Is.SameAs(_mockOf.Handlers[B].Object));
        }

        [Test]
        public void ReverseFunctionTest()
        {
            string Log(string a, string b) => a + b;

            var result = Reverse<string>(Log)("World!!!", "Hello ");

            Assert.That(result, Is.EqualTo("Hello World!!!"));
        }
    }
}
