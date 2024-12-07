namespace ChainLead.Test
{
    using ChainLead.Contracts.Syntax;

    using NUnit.Framework.Internal;
    using System;
    
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Cases.ChainLeadSyntaxFixtureCases;
    using static ChainLead.Test.Cases.Common;
    using static ChainLead.Test.Dummy.ConditionIndex.Common;
    using static ChainLead.Test.Dummy.HandlerIndex.Common;

    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    public class ChainLeadSyntaxTest<T>
    {
        Dummy.Container<T> _dummyOf;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.GetRandom<T>();
            _dummyOf = new(_token, [A, B, C, A+B, A+B+C], [X, Y, X&Y, X|Y]);

            ChainLeadSyntax.Configure(
                _dummyOf.HandlerMath,
                _dummyOf.ConditionMath.Object);
        }


        [Test]
        public void Zero_Test()
        {
            _dummyOf.HandlerMath.SetZero.Returns(A);

            Assert.That(Handler<T>.Zero,
                Is.SameAs(_dummyOf.Handler(A)));
        }

        [Test]
        public void IsZero_Test()
        {
            _dummyOf.HandlerMath.SetZero.Returns(A);

            Assert.That(_dummyOf.Handler(A).IsZero(), Is.True);
            Assert.That(_dummyOf.Handler(B).IsZero(), Is.False);
        }

        [Test]
        public void MakeHandler__Test()
        {
            bool funcCalled = false;
            Action<T> func = _ => funcCalled = true;

            _dummyOf.HandlerMath.SetMakeHandler.Returns(A);

            MakeHandler(func).Execute(_token);

            Assert.That(funcCalled);
        }

        [Test]
        public void AsHandler__Test()
        {
            bool funcCalled = false;
            Action<T> func = _ => funcCalled = true;

            _dummyOf.HandlerMath.SetMakeHandler.Returns(A);

            func.AsHandler().Execute(_token);

            Assert.That(funcCalled);
        }

        [Test]
        public void A_Then_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstThenSecond(A, B).Returns(A+B);
            
            _dummyOf.Handler(A)
                .Then(_dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstThenSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstThenSecond(A, B).Returns(A+B);

            FirstThenSecond(
                   _dummyOf.Handler(A),
                   _dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XThen_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstThenSecond(A, B).Returns(A+B);

            XThen(_dummyOf.Handler(B))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void JoinFirstWithSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A, B).Returns(A+B);

            JoinFirstWithSecond(
                    _dummyOf.Handler(A),
                    _dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Join_A_With_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A, B).Returns(A+B);

            Join(_dummyOf.Handler(A))
                .With(_dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void JoinXWith_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A, B).Returns(A+B);

            JoinXWith(_dummyOf.Handler(B))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void MergeFirstWithSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A, B).Returns(A+B);

            MergeFirstWithSecond(
                    _dummyOf.Handler(A),
                    _dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Merge_A_With_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A, B).Returns(A+B);

            Merge(_dummyOf.Handler(A))
                .With(_dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void MergeXWith_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A, B).Returns(A+B);

            MergeXWith(_dummyOf.Handler(B))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void PackFirstInSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetPackFirstInSecond(A, B).Returns(A+B);

            PackFirstInSecond(
                    _dummyOf.Handler(A),
                    _dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Pack_A_In_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetPackFirstInSecond(A, B).Returns(A+B);

            Pack(_dummyOf.Handler(A))
                .In(_dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void PackXIn_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetPackFirstInSecond(A, B).Returns(A+B);

            PackXIn(_dummyOf.Handler(B))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void InjectFirstIntoSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A, B).Returns(A+B);

            InjectFirstIntoSecond(
                    _dummyOf.Handler(A),
                    _dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Inject_A_Into_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A, B).Returns(A+B);

            Inject(_dummyOf.Handler(A))
                .Into(_dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void InjectXInto_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A, B).Returns(A+B);

            InjectXInto(_dummyOf.Handler(B))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstCoverSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstCoverSecond(A, B).Returns(A+B);

            FirstCoverSecond(
                    _dummyOf.Handler(A),
                    _dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Use_A_ToCover_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstCoverSecond(A, B).Returns(A+B);

            Use(_dummyOf.Handler(A))
                .ToCover(_dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XCover_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstCoverSecond(A, B).Returns(A+B);

            XCover(_dummyOf.Handler(B))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void FirstWrapSecond_AB_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstWrapSecond(A, B).Returns(A+B);

            FirstWrapSecond(
                    _dummyOf.Handler(A),
                    _dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void Use_A_ToWrap_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstWrapSecond(A, B).Returns(A+B);

            Use(_dummyOf.Handler(A))
                .ToWrap(_dummyOf.Handler(B))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void XWrap_B_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);
            _dummyOf.Handlers[A+B].AddDelegationTo(A, B);
            _dummyOf.HandlerMath.SetFirstWrapSecond(A, B).Returns(A+B);

            XWrap(_dummyOf.Handler(B))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B }));
        }

        [Test]
        public void A_Then_B_Then_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);
            
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstThenSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstThenSecond(A+B, C).Returns(A+B+C);

            _dummyOf.Handler(A)
                .Then(_dummyOf.Handler(B))
                .Then(_dummyOf.Handler(C))    
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstThenSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);
            
            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstThenSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstThenSecond(A+B, C).Returns(A+B+C);

            FirstThenSecond(
                    FirstThenSecond(
                        _dummyOf.Handler(A),
                        _dummyOf.Handler(B)),
                    _dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XThen_B_Then_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstThenSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstThenSecond(A+B, C).Returns(A+B+C);

            XThen(_dummyOf.Handler(B))
                .Then(_dummyOf.Handler(C))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void JoinFirstWithSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A+B, C).Returns(A+B+C);

            JoinFirstWithSecond(
                    JoinFirstWithSecond(
                        _dummyOf.Handler(A),
                        _dummyOf.Handler(B)),
                    _dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Join_A_With_B_ThenWith_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handlers[A+B+C].AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A+B, C).Returns(A+B+C);

            Join(_dummyOf.Handler(A))
                .With(_dummyOf.Handler(B))
                .ThenWith(_dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void JoinXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetJoinFirstWithSecond(A+B, C).Returns(A+B+C);

            JoinXWith(_dummyOf.Handler(B))
                .ThenWith(_dummyOf.Handler(C))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void MergeFirstWithSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A+B, C).Returns(A+B+C);

            MergeFirstWithSecond(
                    MergeFirstWithSecond(
                        _dummyOf.Handler(A),
                        _dummyOf.Handler(B)),
                    _dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Merge_A_With_B_ThenWith_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A+B, C).Returns(A+B+C);

            Merge(_dummyOf.Handler(A))
                .With(_dummyOf.Handler(B))
                .ThenWith(_dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void MergeXWith_B_ThenWith_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetMergeFirstWithSecond(A+B, C).Returns(A+B+C);

            MergeXWith(_dummyOf.Handler(B))
                .ThenWith(_dummyOf.Handler(C))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void PackFirstInSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetPackFirstInSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetPackFirstInSecond(A+B, C).Returns(A+B+C);

            PackFirstInSecond(
                    PackFirstInSecond(
                        _dummyOf.Handler(A),
                        _dummyOf.Handler(B)),
                    _dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Pack_A_In_B_ThenIn_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetPackFirstInSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetPackFirstInSecond(A+B, C).Returns(A+B+C);

            Pack(_dummyOf.Handler(A))
                .In(_dummyOf.Handler(B))
                .ThenIn(_dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void PackXIn_B_ThenIn_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetPackFirstInSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetPackFirstInSecond(A+B, C).Returns(A+B+C);

            PackXIn(_dummyOf.Handler(B))
                .ThenIn(_dummyOf.Handler(C))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void InjectFirstIntoSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A+B, C).Returns(A+B+C);

            InjectFirstIntoSecond(
                    InjectFirstIntoSecond(
                        _dummyOf.Handler(A),
                        _dummyOf.Handler(B)),
                    _dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Inject_A_Into_B_ThenInto_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A+B, C).Returns(A+B+C);

            Inject(_dummyOf.Handler(A))
                .Into(_dummyOf.Handler(B))
                .ThenInto(_dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void InjectXInto_B_ThenInto_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetInjectFirstIntoSecond(A+B, C).Returns(A+B+C);

            InjectXInto(_dummyOf.Handler(B))
                .ThenInto(_dummyOf.Handler(C))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstCoverSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstCoverSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstCoverSecond(A+B, C).Returns(A+B+C);

            FirstCoverSecond(
                    FirstCoverSecond(
                        _dummyOf.Handler(A),
                        _dummyOf.Handler(B)),
                    _dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Use_A_ToCover_B_ThenCover_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstCoverSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstCoverSecond(A+B, C).Returns(A+B+C);

            Use(_dummyOf.Handler(A))
                .ToCover(_dummyOf.Handler(B))
                .ThenCover(_dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XCover_B_ThenCover_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstCoverSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstCoverSecond(A+B, C).Returns(A+B+C);

            XCover(_dummyOf.Handler(B))
                .ThenCover(_dummyOf.Handler(C))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void FirstWrapSecond_ABC_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstWrapSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstWrapSecond(A+B, C).Returns(A+B+C);

            FirstWrapSecond(
                    FirstWrapSecond(
                        _dummyOf.Handler(A),
                        _dummyOf.Handler(B)),
                    _dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void Use_A_ToWrap_B_ThenWrap_C_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstWrapSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstWrapSecond(A+B, C).Returns(A+B+C);

            Use(_dummyOf.Handler(A))
                .ToWrap(_dummyOf.Handler(B))
                .ThenWrap(_dummyOf.Handler(C))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void XWrap_B_ThenWrap_C_WhereXIs_A_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            _dummyOf.Handler(A+B).AddDelegationTo(A, B);
            _dummyOf.Handler(A+B+C).AddDelegationTo(A+B, C);

            _dummyOf.HandlerMath.SetFirstWrapSecond(A, B).Returns(A+B);
            _dummyOf.HandlerMath.SetFirstWrapSecond(A+B, C).Returns(A+B+C);

            XWrap(_dummyOf.Handler(B))
                .ThenWrap(_dummyOf.Handler(C))
                .WhereXIs(_dummyOf.Handler(A))
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(new[] { A, B, C }));
        }

        [Test]
        public void When_Test()
        {
            _dummyOf.HandlerMath.SetConditional(A, X).Returns(B);

            Assert.That(_dummyOf.Handler(A).When(_dummyOf.Conditions[X]), 
                Is.SameAs(_dummyOf.Handler(B)));
        }

        [Test]
        public void WithConditionThat_Test()
        {
            _dummyOf.HandlerMath.SetConditional(A, X).Returns(B);

            var handler = WithConditionThat
                (_dummyOf.Conditions[X])
                (_dummyOf.Handler(A));

            Assert.That(handler,
                Is.SameAs(_dummyOf.Handler(B)));
        }

        [Test]
        public void MakeCondition__Test()
        {
            bool funcCalled = false;
            Func<T, bool> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.Setup__MakeCondition(X);

            Assert.That(MakeCondition(func).Check(_token));
            Assert.That(funcCalled);
        }

        [Test]
        public void AsCondition__Test()
        {
            bool funcCalled = false;
            Func<T, bool> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.Setup__MakeCondition(X);

            Assert.That(func.AsCondition().Check(_token));
            Assert.That(funcCalled);
        }

        [Test]
        public void AsCondition2__Test()
        {
            bool funcCalled = false;
            Predicate<T> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.Setup__MakeCondition(X);

        
            Assert.That(func.AsCondition().Check(_token));
            Assert.That(funcCalled);
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
            _dummyOf.ConditionMath.Setup__Or(X, Y, returns: X|Y);

            var aOrB = _dummyOf.Conditions[X].Or(
                       _dummyOf.Conditions[Y]);

            Assert.That(aOrB,
                Is.SameAs(_dummyOf.Conditions[X|Y]));
        }

        [Test]
        public void And_Test()
        {
            _dummyOf.ConditionMath.Setup__And(X, Y, returns: X&Y);

            var aAndB = _dummyOf.Conditions[X].And(
                        _dummyOf.Conditions[Y]);

            Assert.That(aAndB,
                Is.SameAs(_dummyOf.Conditions[X&Y]));
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
            _dummyOf.HandlerMath.SetAtomize(A).Returns(B);

            Assert.That(_dummyOf.Handler(A).Atomize(),
                Is.SameAs(_dummyOf.Handler(B)));
        }

        [Test]
        public void ReverseFunctionTest()
        {
            static string Log(string a, string b) => a + b;

            var result = Reverse<string>(Log)("World!!!", "Hello ");

            Assert.That(result, 
                Is.EqualTo("Hello World!!!"));
        }
    }
}
