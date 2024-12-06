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

        Mock<IConditionMath> _conditionMath;
        IMultipleHandlersMath _math;
        List<string> _callsLog;

        [SetUp]
        public void Setup()
        {
            _conditionMath = new Mock<IConditionMath>();
            _math = mathFactory.Create(_conditionMath.Object);
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
                ids.Reverse()
                   .Select(i => ConditionName(i, jds.Last())),
                ids.SelectMany(i =>
                        jds.Reverse()
                           .Skip(1)
                           .Select(j => ConditionName(i, j))
                           .Concat([HandlerName(i)])));

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
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
                ids.Select(i => ConditionName(i, jds.Last())),
                ids.SelectMany(i =>
                        jds.Reverse()
                           .Skip(1)
                           .Select(j => ConditionName(i, j))
                           .Concat([HandlerName(i)])));

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void InjectAllConditionsTrueTest(
            [ValueSource(nameof(Ids))] string ids,
            [ValueSource(nameof(Jds))] string jds)
        {
            var chain = SetupChain(_math.InjectChain, ids, jds);
            chain.Execute(token);

            var expectedCallsLog = Enumerable.Concat(
                ids.Reverse()
                   .SelectMany(i => 
                        jds.Reverse()
                           .Select(j => ConditionName(i, j))),
                ids.Select(HandlerName));

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
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
                        jds.Reverse()
                           .Select(j => ConditionName(i, j))
                           .Concat([HandlerName(i)]));

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
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
                ids.SelectMany(i => 
                        jds.Reverse()
                           .Select(j => ConditionName(i, j))),
                ids.Select(HandlerName));

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
        }

        IHandler<T> SetupChain(
            Func<IEnumerable<IHandler<T>>, IHandler<T>> makeChain,
            string ids,
            string jds) => 
                makeChain(Enumerable.Zip(
                    ids.Select(CreateHandler),
                    ids.Select(i => jds.Select(j => CreateCondition(i, j))),
                    (handler, conditions) => conditions.Aggregate(handler, _math.Conditional)));

        void SetupConditionMathAnd() =>
            _conditionMath
                .Setup(o => o.And(It.IsAny<ICondition<T>>(), It.IsAny<ICondition<T>>()))
                .Returns((ICondition<T> a, ICondition<T> b) =>
                {
                    var ab = new Mock<ICondition<T>>();
                    ab.Setup(o => o.Check(token))
                      .Returns(() => a.Check(token) && b.Check(token));

                    return ab.Object;
                });

        static string HandlerName(char i) =>
            $"handler[{i}]";

        IHandler<T> CreateHandler(char i)
        {
            var name = HandlerName(i);
            var handler = new Mock<IHandler<T>> { Name = name };

            handler
                .Setup(o => o.Execute(token))
                .Callback(() => _callsLog.Add(name));

            return handler.Object;
        }

        static string ConditionName(char i, char j) =>
            $"condition[{i}][{j}]";

        ICondition<T> CreateCondition(char i, char j)
        {
            var name = ConditionName(i, j);
            var condition = new Mock<ICondition<T>> { Name = name };

            condition
                .Setup(o => o.Check(token)).Returns(true)
                .Callback(() => _callsLog.Add(name));

            return condition.Object;
        }
    }
}
