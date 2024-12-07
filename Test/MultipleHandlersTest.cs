﻿namespace ChainLead.Test
{
    using ChainLead.Contracts;
    
    using static ChainLead.Test.Cases.Common;
    using static ChainLead.Test.Cases.MultipleHandlersFixtureCases;
    using static ChainLead.Test.Dummy.Index.Common;
    
    
    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    [_IX_][_X_][_XI_][_XII_][_XIII_][_XIV_][_XV_][_XVI_]
    public class MultipleHandlersTest<T>(
        IMultipleHandlersMathFactory mathFactory)
    {
        static readonly string[] Ids = ["AB", "ABC", "ABCD", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"];
        static readonly string[] Jds = ["012", "01234", "01234567890"];

        Dummy.Container<T> _dummyOf;
        IMultipleHandlersMath _math;
        List<Dummy.Index> _callsLog;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.GetRandom<T>();
            _dummyOf = new Dummy.Container<T>(_token, [], []);
            _math = mathFactory.Create(_dummyOf.ConditionMath);
            _callsLog = [];
        }

        [Test]
        public void PackAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds)
        {
            var chain = SetupChain(_math.PackChain, ids, jds);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                ids.Reverse().Select(i => Index(i, jds.Last())),
                ids.SelectMany(i => jds.Reverse().Skip(1)
                       .Select(j => Index(i, j))
                       .Concat([Index(i)])));

            Assert.That(_callsLog, 
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void JoinAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds)
        {
            SetupConditionMathAnd();

            var chain = SetupChain(_math.JoinChain, ids, jds);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                ids.Select(i => Index(i, jds.Last())),
                ids.SelectMany(i => jds.Reverse().Skip(1)
                       .Select(j => Index(i, j))
                       .Concat([Index(i)])));

            Assert.That(_callsLog, 
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void InjectAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds)
        {
            var chain = SetupChain(_math.InjectChain, ids, jds);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                ids.Reverse()
                    .SelectMany(i => jds.Reverse()
                        .Select(j => Index(i, j))),
                ids.Select(Index));

            Assert.That(_callsLog, 
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void CoverAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds) =>
                CoverWrapOrThenAllConditionsTrueTest(_math.CoverChain, ids, jds);

        [Test]
        public void WrapAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds) =>
                CoverWrapOrThenAllConditionsTrueTest(_math.WrapChain, ids, jds);

        [Test]
        public void ThenAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds) =>
                CoverWrapOrThenAllConditionsTrueTest(_math.ThenChain, ids, jds);

        void CoverWrapOrThenAllConditionsTrueTest(
            Func<IEnumerable<IHandler<T>>, IHandler<T>> append,
            string ids,
            string jds)
        {
            var chain = SetupChain(append, ids, jds);

            chain.Execute(_token);

            var expectedCallsLog =
                ids.SelectMany(i => 
                    jds.Reverse().Select(j => Index(i, j))
                       .Concat([Index(i)]));

            Assert.That(_callsLog, 
                Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void MergeAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds)
        {
            SetupConditionMathAnd();

            var chain = SetupChain(_math.MergeChain, ids, jds);
            chain.Execute(_token);

            var expectedCallsLog = Enumerable.Concat(
                ids.SelectMany(i => jds.Reverse()
                       .Select(j => Index(i, j))),
                ids.Select(Index));

            Assert.That(_callsLog, 
                Is.EqualTo(expectedCallsLog));
        }

        IHandler<T> SetupChain(
            Func<IEnumerable<IHandler<T>>, IHandler<T>> makeChain,
            string ids,
            string jds)
        {
            List<IHandler<T>> handlers = new();

            foreach(var i in ids) 
            {
                var handlerIndex = HandlerIndex(i);
                var conditionIndices = jds.Select(j => ConditionIndex(i, j));

                _dummyOf.Handlers.Add(handlerIndex);
                _dummyOf.Conditions.AddRange(conditionIndices);

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
                _dummyOf.Conditions.Add(x & y);

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

        static Dummy.Index Index(char i) => HandlerIndex(i);

        static Dummy.Index Index(char i, char j) => ConditionIndex(i, j);
    }
}
