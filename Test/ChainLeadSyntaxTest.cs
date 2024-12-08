namespace ChainLead.Test
{
    using ChainLead.Contracts.Syntax;
    using ChainLead.Test.Utils;
    using NUnit.Framework.Internal;
    using System;
    
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Cases.Common;
    using static ChainLead.Test.Dummy.ConditionIndex.Common;
    using static ChainLead.Test.Dummy.HandlerIndex.Common;

    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    public class ChainLeadSyntaxTest<T>
    {
        readonly Dummy.HandlerIndex AB = A + B, ABC = A + B + C;

        Dummy.Container<T> _dummyOf;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.GetRandom<T>();
            _dummyOf = new(_token, [A, B, C, AB, ABC], [X, Y, X&Y, X|Y]);

            ChainLeadSyntax.Configure(
                _dummyOf.HandlerMath,
                _dummyOf.ConditionMath);
        }


        [Test]
        public void Zero_Test()
        {
            _dummyOf.HandlerMath.Zero_Returns(A);

            Assert.That(Handler<T>.Zero,
                Is.SameAs(_dummyOf.Handler(A)));
        }

        [Test]
        public void IsZero_Test()
        {
            _dummyOf.HandlerMath.Zero_Returns(A);

            Assert.That(_dummyOf.Handler(A).IsZero(), Is.True);
            Assert.That(_dummyOf.Handler(B).IsZero(), Is.False);
        }

        [Test]
        public void MakeHandler__Test()
        {
            bool funcCalled = false;
            Action<T> func = _ => funcCalled = true;

            _dummyOf.HandlerMath.MakeHandler_Returns(A);

            MakeHandler(func).Execute(_token);

            Assert.That(funcCalled);
        }

        [Test]
        public void AsHandler__Test()
        {
            bool funcCalled = false;
            Action<T> func = _ => funcCalled = true;

            _dummyOf.HandlerMath.MakeHandler_Returns(A);

            func.AsHandler().Execute(_token);

            Assert.That(funcCalled);
        }

        [Test]
        public void A_Then_B_Test()
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstThenSecond(A, B).Returns(AB);
            
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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstThenSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstThenSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.JoinFirstWithSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.JoinFirstWithSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.JoinFirstWithSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.MergeFirstWithSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.MergeFirstWithSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.MergeFirstWithSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.PackFirstInSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.PackFirstInSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.PackFirstInSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.InjectFirstIntoSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.InjectFirstIntoSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.InjectFirstIntoSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstCoverSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstCoverSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstCoverSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstWrapSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstWrapSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B].LogInto(execution);
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.HandlerMath.FirstWrapSecond(A, B).Returns(AB);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);
            
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstThenSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstThenSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);
            
            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstThenSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstThenSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstThenSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstThenSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.JoinFirstWithSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.JoinFirstWithSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.JoinFirstWithSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.JoinFirstWithSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.JoinFirstWithSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.JoinFirstWithSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.MergeFirstWithSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.MergeFirstWithSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.MergeFirstWithSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.MergeFirstWithSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.MergeFirstWithSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.MergeFirstWithSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.PackFirstInSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.PackFirstInSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.PackFirstInSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.PackFirstInSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.PackFirstInSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.PackFirstInSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.InjectFirstIntoSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.InjectFirstIntoSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.InjectFirstIntoSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.InjectFirstIntoSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.InjectFirstIntoSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.InjectFirstIntoSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstCoverSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstCoverSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstCoverSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstCoverSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstCoverSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstCoverSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstWrapSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstWrapSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstWrapSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstWrapSecond(AB, C).Returns(ABC);

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

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            _dummyOf.Handler(AB).DelegatesTo(A, B);
            _dummyOf.Handler(ABC).DelegatesTo(AB, C);

            _dummyOf.HandlerMath.FirstWrapSecond(A, B).Returns(AB);
            _dummyOf.HandlerMath.FirstWrapSecond(AB, C).Returns(ABC);

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
            _dummyOf.HandlerMath.Conditional(A, X).Returns(B);

            Assert.That(_dummyOf.Handler(A).When(_dummyOf.Condition(X)), 
                Is.SameAs(_dummyOf.Handler(B)));
        }

        [Test]
        public void WithConditionThat_Test()
        {
            _dummyOf.HandlerMath.Conditional(A, X).Returns(B);

            var handler = WithConditionThat
                (_dummyOf.Condition(X))
                (_dummyOf.Handler(A));

            Assert.That(handler,
                Is.SameAs(_dummyOf.Handler(B)));
        }

        [Test]
        public void MakeCondition__Test()
        {
            bool funcCalled = false;
            Func<T, bool> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.MakeCondition_Returns(X);

            Assert.That(MakeCondition(func).Check(_token));
            Assert.That(funcCalled);
        }

        [Test]
        public void AsCondition__Test()
        {
            bool funcCalled = false;
            Func<T, bool> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.MakeCondition_Returns(X);

            Assert.That(func.AsCondition().Check(_token));
            Assert.That(funcCalled);
        }

        [Test]
        public void AsCondition2__Test()
        {
            bool funcCalled = false;
            Predicate<T> func = _ => funcCalled = true;

            _dummyOf.ConditionMath.MakeCondition_Returns(X);

        
            Assert.That(func.AsCondition().Check(_token));
            Assert.That(funcCalled);
        }

        [Test]
        public void True_Test()
        {
            _dummyOf.ConditionMath.True_Returns(X);

            Assert.That(Condition<T>.True,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void False_Test()
        {
            _dummyOf.ConditionMath.False_Returns(X);

            Assert.That(Condition<T>.False,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void IsPredictableTrue_Test()
        {
            _dummyOf.ConditionMath.True_Returns(X);

            Assert.That(_dummyOf.Condition(X).IsPredictableTrue());
            Assert.That(_dummyOf.Condition(Y).IsPredictableTrue(),
                Is.False);
        }

        [Test]
        public void IsPredictableFalse_Test(
            [Values(false, true)] bool expectedResult)
        {
            _dummyOf.ConditionMath.False_Returns(X);

            Assert.That(_dummyOf.Condition(X).IsPredictableFalse());
            Assert.That(_dummyOf.Condition(Y).IsPredictableFalse(),
                Is.False);
        }

        [Test]
        public void Or_Test()
        {
            _dummyOf.ConditionMath.Or(X, Y).Returns(X|Y);

            var aOrB = _dummyOf.Condition(X).Or(
                       _dummyOf.Condition(Y));

            Assert.That(aOrB,
                Is.SameAs(_dummyOf.Condition(X|Y)));
        }

        [Test]
        public void And_Test()
        {
            _dummyOf.ConditionMath.And(X, Y).Returns(X&Y);

            var aAndB = _dummyOf.Condition(X).And(
                        _dummyOf.Condition(Y));

            Assert.That(aAndB,
                Is.SameAs(_dummyOf.Condition(X&Y)));
        }

        [Test]
        public void Not_Test()
        {
            _dummyOf.ConditionMath.Not(X).Returns(Y);

            Assert.That(Not(_dummyOf.Condition(X)),
                Is.SameAs(_dummyOf.Condition(Y)));
        }

        [Test]
        public void AtomizeTest()
        {
            _dummyOf.HandlerMath.Atomize(A).Returns(B);

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
