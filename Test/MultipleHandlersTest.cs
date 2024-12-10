namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Test.Utils;

    using static ChainLead.Test.Cases.MultipleHandlers;
    using static ChainLead.Test.Dummy.Common;


    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    [_IX_][_X_][_XI_][_XII_][_XIII_][_XIV_][_XV_][_XVI_]
    public class MultipleHandlersTest<T>(
        IMultipleHandlersMathFactory mathFactory)
    {
        Dummy.Container<T> _dummyOf;
        IMultipleHandlersMath _math;
        List<Dummy.ChainElementIndex> _callsLog;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.GetRandom<T>();
            _dummyOf = new(_token);
            _math = mathFactory.Create(_dummyOf.ConditionMath);
            _callsLog = [];
        }

        [Test]
        public void PackAllConditionsTrueTest(
            [IIndices] string i_s,
            [JIndices] string j_s)
        {
            var chain = SetupChain(_math.PackChain, i_s, j_s);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                i_s.Reverse().Select(i => Index(i, j_s.Last())),
                i_s.SelectMany(i => j_s.Reverse().Skip(1)
                       .Select(j => Index(i, j))
                       .Concat([Index(i)])));

            Assert.That(_callsLog,
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void JoinAllConditionsTrueTest(
            [IIndices] string i_s, 
            [JIndices] string j_s)
        {
            SetupConditionMathAnd();

            var chain = SetupChain(_math.JoinChain, i_s, j_s);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                i_s.Select(i => Index(i, j_s.Last())),
                i_s.SelectMany(i => j_s.Reverse().Skip(1)
                       .Select(j => Index(i, j))
                       .Concat([Index(i)])));

            Assert.That(_callsLog,
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void InjectAllConditionsTrueTest(
            [IIndices] string i_s,
            [JIndices] string j_s)
        {
            var chain = SetupChain(_math.InjectChain, i_s, j_s);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                i_s.Reverse()
                   .SelectMany(i => j_s.Reverse()
                       .Select(j => Index(i, j))),
                i_s.Select(Index));

            Assert.That(_callsLog,
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void CoverAllConditionsTrueTest(
            [IIndices] string i_s,
            [JIndices] string j_s) =>
                CoverWrapOrThenAllConditionsTrueTest(_math.CoverChain, i_s, j_s);

        [Test]
        public void WrapAllConditionsTrueTest(
            [IIndices] string i_s,
            [JIndices] string j_s) =>
                CoverWrapOrThenAllConditionsTrueTest(_math.WrapChain, i_s, j_s);

        [Test]
        public void ThenAllConditionsTrueTest(
            [IIndices] string i_s,
            [JIndices] string j_s) =>
                CoverWrapOrThenAllConditionsTrueTest(_math.ThenChain, i_s, j_s);

        void CoverWrapOrThenAllConditionsTrueTest(
            Func<IEnumerable<IHandler<T>>, IHandler<T>> append,
            string i_s,
            string j_s)
        {
            var chain = SetupChain(append, i_s, j_s);

            chain.Execute(_token);

            var expectedCallsLog =
                i_s.SelectMany(i => 
                    j_s.Reverse().Select(j => Index(i, j))
                       .Concat([Index(i)]));

            Assert.That(_callsLog,
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void MergeAllConditionsTrueTest(
            [IIndices] string i_s,
            [JIndices] string j_s)
        {
            SetupConditionMathAnd();

            var chain = SetupChain(_math.MergeChain, i_s, j_s);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                i_s.SelectMany(i => j_s.Reverse()
                       .Select(j => Index(i, j))),
                i_s.Select(Index));

            Assert.That(_callsLog,
                Is.EqualTo(expectedCallsLog));
        }

        IHandler<T> SetupChain(
            Func<IEnumerable<IHandler<T>>, IHandler<T>> makeChain,
            string i_s,
            string j_s)
        {
            List<IHandler<T>> handlers = [];

            foreach(var i in i_s) 
            {
                var handlerIndex = HandlerIndex(i);
                var conditionIndices = j_s.Select(j => ConditionIndex(i, j));

                _dummyOf.Handlers.Generate(handlerIndex);
                _dummyOf.Conditions.Generate(conditionIndices);

                var handler = _dummyOf.Conditions[conditionIndices]
                     .Aggregate(_dummyOf.Handler(handlerIndex).Pure, _math.Conditional);

                handlers.Add(handler);
            }

            _dummyOf.Handlers.LogInto(_callsLog);
            _dummyOf.Conditions.LogInto(_callsLog);
            _dummyOf.Conditions.Return(true);

            return makeChain(handlers);
        }

        void SetupConditionMathAnd() =>
            _dummyOf.ConditionMath.And(Any, Any).Implements((x, y) =>
            {
                _dummyOf.Conditions.Generate(x & y);

                _dummyOf.Condition(x & y).Returns(true);
                _dummyOf.Condition(x & y).AddCallback(() =>
                {
                    _dummyOf.Condition(x).Check(_token);
                    _dummyOf.Condition(y).Check(_token);
                });

                return x & y;
            });

        static Dummy.HandlerIndex HandlerIndex(char i) =>
            new($"[{i}]");

        static Dummy.ConditionIndex ConditionIndex(char i, char j) =>
            new($"[{i}][{j}]");

        static Dummy.ChainElementIndex Index(char i) => HandlerIndex(i);

        static Dummy.ChainElementIndex Index(char i, char j) => ConditionIndex(i, j);
    }
}
