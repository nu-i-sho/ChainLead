namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Test.HandlersTestData;
    using Moq;

    using static System.Linq.Enumerable;

    [TestFixtureSource(nameof(Cases))]
    public class HandlerChainsTest(IHandlerChainingCallsProviderFactory mathFactory)
    {
        const int Arg = 24309;
        static readonly string[] Ids = 
            ["AB", "ABC", "ABCD", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"];

        static readonly int[] _012 = [0, 1, 2];

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
        public void PackAllConditionsTrueTest([ValueSource(nameof(Ids))] string ids)
        {
            var chain = SetupChain(_math.PackChain, ids);
            chain.Execute(Arg);

            var expectedCallsLog = Enumerable
                .Concat(
                    ids.Reverse()
                       .Select(c => ConditionName(c, _012.Last())),
                    ids.SelectMany(c => 
                            _012.Reverse().Skip(1)
                                .Select(i => ConditionName(c, i))
                                .Concat([HandlerName(c)])))
                .ToList();

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void JoinAllConditionsTrueTest([ValueSource(nameof(Ids))] string ids)
        {
            SetupConditionMathAnd();
            PackAllConditionsTrueTest(ids);
        }

        [Test]
        public void InjectAllConditionsTrueTest([ValueSource(nameof(Ids))] string ids)
        {
            var chain = SetupChain(_math.InjectChain, ids);
            chain.Execute(Arg);

            var expectedCallsLog = Enumerable
                .Concat(
                    ids.Reverse()
                       .SelectMany(c => _012.Reverse()
                           .Select(i => ConditionName(c, i))),
                    ids.Select(HandlerName))
                .ToList();

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void CoverWrapAndThenAllConditionsTrueTest(
            [Values(nameof(IHandlerChainingCallsProvider.CoverChain), 
                    nameof(IHandlerChainingCallsProvider.WrapChain),
                    nameof(IHandlerChainingCallsProvider.ThenChain))] 
                string chainCallName,
            [ValueSource(nameof(Ids))]
                string ids)
        {
            var chain = SetupChain(
                chainCallName switch
                {
                    nameof(IHandlerChainingCallsProvider.CoverChain) => _math.CoverChain,
                    nameof(IHandlerChainingCallsProvider.WrapChain) => _math.WrapChain,
                    nameof(IHandlerChainingCallsProvider.ThenChain) => _math.ThenChain,
                    _ => throw new ArgumentException(chainCallName)
                }, 
                ids);

            chain.Execute(Arg);

            var expectedCallsLog =
                ids.SelectMany(c =>
                        _012.Reverse()
                            .Select(i => ConditionName(c, i))
                            .Concat([HandlerName(c)]))
                    .ToList();

            Assert.That(_callsLog, Is.EqualTo(expectedCallsLog));
        }

        [Test]
        public void MergeAllConditionsTrueTest([ValueSource(nameof(Ids))] string ids)
        {
            SetupConditionMathAnd();

            var chain = SetupChain(_math.CoverChain, ids);
            chain.Execute(Arg);

            var expectedCallsLog = Enumerable.Concat(
                ids.SelectMany(c => _012.Select(i => ConditionName(c, i))),
                ids.Select(HandlerName));
        }

        IHandler<int> SetupChain(
            Func<IEnumerable<IHandler<int>>, IHandler<int>> makeChain,
            string ids) => makeChain(Enumerable.Zip(
                ids.Select(CreateHandler),
                ids.Select(c => _012.Select(i => CreateCondition(c, i))),
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

        static string HandlerName(char c) =>
            $"handler[{c}]";

        IHandler<int> CreateHandler(char c)
        {
            var name = HandlerName(c);
            var handler = new Mock<IHandler<int>> { Name = name };

            handler
                .Setup(o => o.Execute(Arg))
                .Callback(() => _callsLog.Add(name));

            return handler.Object;
        }

        static string ConditionName(char c, int i) =>
            $"condition[{c}][{i}]";

        ICondition<int> CreateCondition(char c, int i)
        {
            var name = ConditionName(c, i);
            var condition = new Mock<ICondition<int>> { Name = name };

            condition
                .Setup(o => o.Check(Arg)).Returns(true)
                .Callback(() => _callsLog.Add(name));

            return condition.Object;
        }
    }
}
