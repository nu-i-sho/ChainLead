﻿namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Test.HandlersTestData;
    using Moq;
    using System.Diagnostics.CodeAnalysis;

    using static System.Linq.Enumerable;

    [TestFixtureSource(nameof(Cases))]
    public class HandlerChainsTest(IHandlerChainingCallsProviderFactory mathFactory)
    {
        const int Arg = 24309;

        static readonly string[] Ids = 
            ["AB", "ABC", "ABCD", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"];

        static readonly string[] Jds =
            ["012", "01234", "01234567890"];

        Mock<IConditionMath> _conditionMath;
        IHandlerChainingCallsProvider _math;
        List<string> _callsLog;

        public static IEnumerable<IHandlerChainingCallsProviderFactory> Cases
        {
            get
            {
                yield return new ChainLeadSyntaxDirectChainingCallsProviderFactory();
                yield return new ChainLeadSyntaxReversedChainingCallsProviderFactory();
            }
        }

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
            chain.Execute(Arg);

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
            chain.Execute(Arg);

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
            chain.Execute(Arg);

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
            Func<IEnumerable<IHandler<int>>, IHandler<int>> append,
            string ids,
            string jds)
        {
            var chain = SetupChain(append, ids, jds);

            chain.Execute(Arg);

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
            chain.Execute(Arg);

            var expectedCallsLog = Enumerable.Concat(
                ids.SelectMany(i => 
                        jds.Reverse()
                           .Select(j => ConditionName(i, j))),
                ids.Select(HandlerName));

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
        }

        IHandler<int> SetupChain(
            Func<IEnumerable<IHandler<int>>, IHandler<int>> makeChain,
            string ids,
            string jds) => 
                makeChain(Enumerable.Zip(
                    ids.Select(CreateHandler),
                    ids.Select(i => jds.Select(j => CreateCondition(i, j))),
                    (handler, conditions) => conditions.Aggregate(handler, _math.Conditional)));

        void SetupConditionMathAnd() =>
            _conditionMath
                .Setup(o => o.And(It.IsAny<ICondition<int>>(), It.IsAny<ICondition<int>>()))
                .Returns((ICondition<int> a, ICondition<int> b) =>
                {
                    var ab = new Mock<ICondition<int>>();
                    ab.Setup(o => o.Check(Arg))
                      .Returns(() => a.Check(Arg) && b.Check(Arg));

                    return ab.Object;
                });

        static string HandlerName(char i) =>
            $"handler[{i}]";

        IHandler<int> CreateHandler(char i)
        {
            var name = HandlerName(i);
            var handler = new Mock<IHandler<int>> { Name = name };

            handler
                .Setup(o => o.Execute(Arg))
                .Callback(() => _callsLog.Add(name));

            return handler.Object;
        }

        static string ConditionName(char i, char j) =>
            $"condition[{i}][{j}]";

        ICondition<int> CreateCondition(char i, char j)
        {
            var name = ConditionName(i, j);
            var condition = new Mock<ICondition<int>> { Name = name };

            condition
                .Setup(o => o.Check(Arg)).Returns(true)
                .Callback(() => _callsLog.Add(name));

            return condition.Object;
        }
    }
}
