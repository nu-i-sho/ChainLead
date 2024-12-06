namespace ChainLead.Test
{
    using ChainLead.Contracts.Syntax;
    using System;

    using static ChainLead.Test.Cases.Common;
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Dummy.HandlerIndex.Common;
    using static ChainLead.Test.Dummy.ConditionIndex.Common;
    using NUnit.Framework.Internal;

    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    [TestFixture(typeof(Types.Class))]
    [TestFixture(typeof(Types.Struct))]
    [TestFixture(typeof(Types.ReadonlyStruct))]
    [TestFixture(typeof(Types.Record))]
    [TestFixture(typeof(Types.RecordStruct))]
    [TestFixture(typeof(Types.ReadonlyRecordStruct))]
    public class ChainLeadSyntaxTest<T>
    {
        private static readonly Dummy.HandlerIndex AB = new("AB"), ABC = new("ABC");
        private static readonly Dummy.ConditionIndex XY = new("XY");

        Dummy.Container<T> _dummyOf;
        T _token; 

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.Get<T>(765049);
            _dummyOf = new(_token, [A, B, C, AB, ABC], [X, Y, XY]);

            ChainLeadSyntax.Configure(
                _dummyOf.HandlerMath.Object,
                _dummyOf.ConditionMath.Object);
        }


        [Test]
        public void Zero_Test()
        {
            _dummyOf.HandlerMath.Setup__Zero(returns: A);

            Assert.That(Handler<T>.Zero,
                Is.SameAs(_dummyOf.Handlers[A]));
        }

        [Test]
        public void IsZero_Test(
            [Values(false, true)] bool expectedResult)
        {
            _dummyOf.HandlerMath.Setup__IsZero(A, returns: expectedResult);

            Assert.That(_dummyOf.Handlers[A].IsZero(), 
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void MakeHandler__Test()
        {
            bool funcCalled = false;
            Action<T> func = _ => funcCalled = true;

            _dummyOf.HandlerMath.Setup__MakeHandler(A);

            MakeHandler(func).Execute(_token);

            Assert.That(funcCalled);
        }

        [Test]
        public void AsHandler__Test()
        {
            bool funcCalled = false;
            Action<T> func = _ => funcCalled = true;

            _dummyOf.HandlerMath.Setup__MakeHandler(A);

            func.AsHandler().Execute(_token);

            Assert.That(funcCalled);
        }

        [Test]
        public void A_Then_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            _dummyOf.Handlers[A]
                .Then(_dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstThenSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            FirstThenSecond(
                   _dummyOf.Handlers[A],
                   _dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XThen_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);

            XThen(_dummyOf.Handlers[B])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void JoinFirstWithSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            JoinFirstWithSecond(
                    _dummyOf.Handlers[A],
                    _dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Join_A_With_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            Join(_dummyOf.Handlers[A])
                .With(_dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void JoinXWith_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);

            JoinXWith(_dummyOf.Handlers[B])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void MergeFirstWithSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            MergeFirstWithSecond(
                    _dummyOf.Handlers[A],
                    _dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Merge_A_With_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            Merge(_dummyOf.Handlers[A])
                .With(_dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void MergeXWith_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);

            MergeXWith(_dummyOf.Handlers[B])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void PackFirstInSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            PackFirstInSecond(
                    _dummyOf.Handlers[A],
                    _dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Pack_A_In_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            Pack(_dummyOf.Handlers[A])
                .In(_dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void PackXIn_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);

            PackXIn(_dummyOf.Handlers[B])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void InjectFirstIntoSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            InjectFirstIntoSecond(
                    _dummyOf.Handlers[A],
                    _dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Inject_A_Into_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            Inject(_dummyOf.Handlers[A])
                .Into(_dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void InjectXInto_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);

            InjectXInto(_dummyOf.Handlers[B])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstCoverSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            FirstCoverSecond(
                    _dummyOf.Handlers[A],
                    _dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Use_A_ToCover_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            Use(_dummyOf.Handlers[A])
                .ToCover(_dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XCover_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);

            XCover(_dummyOf.Handlers[B])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstWrapSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            FirstWrapSecond(
                    _dummyOf.Handlers[A],
                    _dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Use_A_ToWrap_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            Use(_dummyOf.Handlers[A])
                .ToWrap(_dummyOf.Handlers[B])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XWrap_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);

            XWrap(_dummyOf.Handlers[B])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void A_Then_B_Then_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);
            
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            _dummyOf.Handlers[A]
                .Then(_dummyOf.Handlers[B])
                .Then(_dummyOf.Handlers[C])    
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstThenSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);
            
            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            FirstThenSecond(
                    FirstThenSecond(
                        _dummyOf.Handlers[A],
                        _dummyOf.Handlers[B]),
                    _dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XThen_B_Then_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstThenSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstThenSecond(AB, C, returns: ABC);

            XThen(_dummyOf.Handlers[B])
                .Then(_dummyOf.Handlers[C])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void JoinFirstWithSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            JoinFirstWithSecond(
                    JoinFirstWithSecond(
                        _dummyOf.Handlers[A],
                        _dummyOf.Handlers[B]),
                    _dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Join_A_With_B_ThenWith_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            Join(_dummyOf.Handlers[A])
                .With(_dummyOf.Handlers[B])
                .ThenWith(_dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void JoinXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__JoinFirstWithSecond(AB, C, returns: ABC);

            JoinXWith(_dummyOf.Handlers[B])
                .ThenWith(_dummyOf.Handlers[C])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void MergeFirstWithSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            MergeFirstWithSecond(
                    MergeFirstWithSecond(
                        _dummyOf.Handlers[A],
                        _dummyOf.Handlers[B]),
                    _dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Merge_A_With_B_ThenWith_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            Merge(_dummyOf.Handlers[A])
                .With(_dummyOf.Handlers[B])
                .ThenWith(_dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void MergeXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__MergeFirstWithSecond(AB, C, returns: ABC);

            MergeXWith(_dummyOf.Handlers[B])
                .ThenWith(_dummyOf.Handlers[C])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void PackFirstInSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            PackFirstInSecond(
                    PackFirstInSecond(
                        _dummyOf.Handlers[A],
                        _dummyOf.Handlers[B]),
                    _dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Pack_A_In_B_ThenIn_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            Pack(_dummyOf.Handlers[A])
                .In(_dummyOf.Handlers[B])
                .ThenIn(_dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void PackXIn_B_ThenIn_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__PackFirstInSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__PackFirstInSecond(AB, C, returns: ABC);

            PackXIn(_dummyOf.Handlers[B])
                .ThenIn(_dummyOf.Handlers[C])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void InjectFirstIntoSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            InjectFirstIntoSecond(
                    InjectFirstIntoSecond(
                        _dummyOf.Handlers[A],
                        _dummyOf.Handlers[B]),
                    _dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Inject_A_Into_B_ThenInto_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            Inject(_dummyOf.Handlers[A])
                .Into(_dummyOf.Handlers[B])
                .ThenInto(_dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void InjectXInto_B_ThenInto_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__InjectFirstIntoSecond(AB, C, returns: ABC);

            InjectXInto(_dummyOf.Handlers[B])
                .ThenInto(_dummyOf.Handlers[C])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstCoverSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            FirstCoverSecond(
                    FirstCoverSecond(
                        _dummyOf.Handlers[A],
                        _dummyOf.Handlers[B]),
                    _dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Use_A_ToCover_B_ThenCover_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            Use(_dummyOf.Handlers[A])
                .ToCover(_dummyOf.Handlers[B])
                .ThenCover(_dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XCover_B_ThenCover_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstCoverSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstCoverSecond(AB, C, returns: ABC);

            XCover(_dummyOf.Handlers[B])
                .ThenCover(_dummyOf.Handlers[C])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstWrapSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            FirstWrapSecond(
                    FirstWrapSecond(
                        _dummyOf.Handlers[A],
                        _dummyOf.Handlers[B]),
                    _dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Use_A_ToWrap_B_ThenWrap_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            Use(_dummyOf.Handlers[A])
                .ToWrap(_dummyOf.Handlers[B])
                .ThenWrap(_dummyOf.Handlers[C])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XWrap_B_ThenWrap_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handlers[AB].AddDelegationTo(A, B);
            _dummyOf.Handlers[ABC].AddDelegationTo(AB, C);

            _dummyOf.HandlerMath.Setup__FirstWrapSecond(A, B, returns: AB);
            _dummyOf.HandlerMath.Setup__FirstWrapSecond(AB, C, returns: ABC);

            XWrap(_dummyOf.Handlers[B])
                .ThenWrap(_dummyOf.Handlers[C])
                .WhereXIs(_dummyOf.Handlers[A])
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void When_Test()
        {
            _dummyOf.HandlerMath.Setup__Conditional(A, X, returns: B);

            Assert.That(_dummyOf.Handlers[A].When(_dummyOf.Conditions[X]), 
                Is.SameAs(_dummyOf.Handlers[B]));
        }

        [Test]
        public void WithConditionThat_Test()
        {
            _dummyOf.HandlerMath.Setup__Conditional(A, X, returns: B);

            var handler = WithConditionThat
                (_dummyOf.Conditions[X])
                (_dummyOf.Handlers[A]);

            Assert.That(handler,
                Is.SameAs(_dummyOf.Handlers[B]));
        }

        [Test]
        public void MakeCondition__Test()
        {
            bool funcCalled = false;
            Func<T, bool> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.Setup__MakeCondition(X);

            Assert.Multiple(() =>
            {
                Assert.That(MakeCondition(func).Check(_token));
                Assert.That(funcCalled);
            });
        }

        [Test]
        public void AsCondition__Test()
        {
            bool funcCalled = false;
            Func<T, bool> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.Setup__MakeCondition(X);

            Assert.Multiple(() =>
            {
                Assert.That(func.AsCondition().Check(_token));
                Assert.That(funcCalled);
            });
        }

        [Test]
        public void AsCondition2__Test()
        {
            bool funcCalled = false;
            Predicate<T> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.Setup__MakeCondition(X);

            Assert.Multiple(() =>
            {
                Assert.That(func.AsCondition().Check(_token));
                Assert.That(funcCalled);
            });
        }

        [Test]
        public void True_Test()
        {
            _dummyOf.ConditionMath.Setup__True(returns: X);

            Assert.That(Condition<T>.True,
                Is.SameAs(_dummyOf.Conditions[X]));
        }

        [Test]
        public void False_Test()
        {
            _dummyOf.ConditionMath.Setup__False(returns: X);

            Assert.That(Condition<T>.False,
                Is.SameAs(_dummyOf.Conditions[X]));
        }

        [Test]
        public void IsPredictableTrue_Test(
            [Values(false, true)] bool expectedResult)
        {
            _dummyOf.ConditionMath.Setup__IsPredictableTrue(X, returns: expectedResult);

            Assert.That(_dummyOf.Conditions[X].IsPredictableTrue(), 
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsPredictableFalse_Test(
            [Values(false, true)] bool expectedResult)
        {
            _dummyOf.ConditionMath.Setup__IsPredictableFalse(X, returns: expectedResult);

            Assert.That(_dummyOf.Conditions[X].IsPredictableFalse(),
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void Or_Test()
        {
            _dummyOf.ConditionMath.Setup__Or(X, Y, returns: XY);

            var aOrB = _dummyOf.Conditions[X].Or(
                       _dummyOf.Conditions[Y]);

            Assert.That(aOrB,
                Is.SameAs(_dummyOf.Conditions[XY]));
        }

        [Test]
        public void And_Test()
        {
            _dummyOf.ConditionMath.Setup__And(X, Y, returns: XY);

            var aAndB = _dummyOf.Conditions[X].And(
                        _dummyOf.Conditions[Y]);

            Assert.That(aAndB,
                Is.SameAs(_dummyOf.Conditions[XY]));
        }

        [Test]
        public void Not_Test()
        {
            _dummyOf.ConditionMath.Setup__Not(X, returns: Y);

            Assert.That(Not(_dummyOf.Conditions[X]),
                Is.SameAs(_dummyOf.Conditions[Y]));
        }

        [Test]
        public void AtomizeTest()
        {
            _dummyOf.HandlerMath.Setup__Atomize(A, returns: B);

            Assert.That(_dummyOf.Handlers[A].Atomize(),
                Is.SameAs(_dummyOf.Handlers[B]));
        }

        [Test]
        public void ReverseFunctionTest()
        {
            static string Log(string a, string b) => a + b;

            var result = Reverse<string>(Log)("World!!!", "Hello ");

            Assert.That(result, Is.EqualTo("Hello World!!!"));
        }
    }
}
