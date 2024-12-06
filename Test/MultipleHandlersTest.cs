namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using Moq;

    using static ChainLead.Test.Cases.MultipleHandlersFixtureCases;
    using static System.Linq.Enumerable;

    
    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    [_IX_][_X_][_XI_][_XII_][_XIII_][_XIV_][_XV_][_XVI_]
    public class MultipleHandlersTest<T>(
        IMultipleHandlersMathFactory mathFactory,
        T token)
    {
        static readonly string[] Ids = ["AB", "ABC", "ABCD", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"];
        static readonly string[] Jds = ["012", "01234", "01234567890"];

        Dummy.Container<T> _dummyOf;
        IMultipleHandlersMath _math;
        List<Dummy.Index> _callsLog;

        [SetUp]
        public void Setup()
        {
            _dummyOf = new Dummy.Container<T>(token, [], []);
            _math = mathFactory.Create(_dummyOf.ConditionMath.Object);
            _callsLog = [];
        }

        [Test]
        public void PackAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds)
        {
            var chain = SetupChain(_math.PackChain, ids, jds);
            chain.Execute(token);

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
            chain.Execute(token);

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
            chain.Execute(token);

            var expectedCallsLog = Enumerable.Concat(
                ids.Reverse().SelectMany(i => jds.Reverse()
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

            chain.Execute(token);

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
            chain.Execute(token);

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
            foreach(var (handlerIndex, conditionIndices) in 
                Enumerable.Zip(
                    ids.Select(HandlerIndex),
                    ids.Select(i => jds.Select(j => ConditionIndex(i, j)))))
            {
                _dummyOf.Handlers.GenerateMore(handlerIndex);
                _dummyOf.Conditions.GenerateMore(conditionIndices);

                handlers.Add(_dummyOf.Conditions[conditionIndices]
                  .Aggregate(_dummyOf.Handlers[handlerIndex].Pure,
                             _math.Conditional));
            }

            _dummyOf.Handlers.AddLoggingInto(_callsLog);
            _dummyOf.Conditions.AddLoggingInto(_callsLog);
            _dummyOf.Conditions.SetResults(true);

            return makeChain(handlers);
        }

        void SetupConditionMathAnd() =>
            _dummyOf.ConditionMath
                .Setup(o => o.And(It.IsAny<ICondition<T>>(), It.IsAny<ICondition<T>>()))
                .Returns((ICondition<T> a, ICondition<T> b) =>
                {
                    var ab = new Mock<ICondition<T>>();
                    ab.Setup(o => o.Check(token))
                      .Returns(() => a.Check(token) && b.Check(token));

                    return ab.Object;
                });

        static Dummy.HandlerIndex HandlerIndex(char i) =>
            new($"h[{i}]");

        static Dummy.ConditionIndex ConditionIndex(char i, char j) =>
            new($"c[{i}][{j}]");

        static Dummy.Index Index(char i) => HandlerIndex(i);

        static Dummy.Index Index(char i, char j) => ConditionIndex(i, j);
    }
}
