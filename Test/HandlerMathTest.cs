namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using Moq;
    using NUnit.Framework.Internal;
    using System;
    using System.Linq;

    [TestFixture]
    public class HandlerMathTest
    {
        public class Base { }
        public class Derived : Base { }

        private IHandlerMath _math;

        private IHandler<int> _zeroInt;
        private IHandler<Base> _zeroBase;
        private IHandler<Derived> _zeroDerived;

        private Mock<IHandler<int>> _handler;
        private Mock<ICondition<int>> _condition;

        private const int 
            A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7, I = 8,
            First = A, Last = I, Missing = -1;

        private Mock<IHandler<int>>[] _handlers;
        private Mock<ICondition<int>>[] _conditions;

        private Mock<IConditionMath> _conditionMath;

        private const string
            Join = nameof(Join),
            Merge = nameof(Merge),
            DeepMerge = nameof(DeepMerge),
            Inject = nameof(Inject),
            DeepInject = nameof(DeepInject),
            Wrap = nameof(Wrap),
            DeepWrap = nameof(DeepWrap);

        private static string[] AllAppends =
        {
            Join, Merge, DeepMerge, Inject, DeepInject, Wrap, DeepWrap
        };

        private Func<IHandler<T>, IHandler<T>, IHandler<T>>
            AppendFunc<T>(string by) =>
                by switch
                {
                    Join => _math.Join,
                    Merge => _math.Merge,
                    DeepMerge => _math.DeepMerge,
                    Inject => _math.Inject,
                    DeepInject => _math.DeepInject,
                    Wrap => _math.Wrap,
                    DeepWrap => _math.DeepWrap,
                    _ => throw new ArgumentOutOfRangeException(nameof(by))
                };

        private const int Arg = 7643;

        [SetUp]
        public void Setup()
        {
            _conditionMath = MockPredicateMath();
            _math = new HandlerMath(_conditionMath.Object);

            _zeroInt = _math.Zero<int>();
            _zeroBase = _math.Zero<Base>();
            _zeroDerived = _math.Zero<Derived>();

            _handler = new Mock<IHandler<int>> { Name = nameof(_handler) };
            _condition = new Mock<ICondition<int>> { Name = nameof(_condition) };

            var name = nameof(_handlers);
            _handlers = Enumerable
                .Range(First, Last + 1)
                .Select(IndexToString)
                .Select(i => new Mock<IHandler<int>>{ Name = $"{name}[{i}]" })
                .ToArray();

            name = nameof(_conditions);
            _conditions = Enumerable
                .Range(First, Last + 1)
                .Select(IndexToString)
                .Select(i => new Mock<ICondition<int>> { Name = $"{name}[{i}]" })
                .ToArray();
        }

        private Mock<IConditionMath> MockPredicateMath()
        {
            var math = new Mock<IConditionMath>();

            MockTrue<int>("True");
            MockFalse<int>("False");
            MockTrue<Base>($"True<{nameof(Base)}>");
            MockFalse<Base>($"False<{nameof(Base)}>");
            MockTrue<Derived>($"True<{nameof(Derived)}>");
            MockFalse<Derived>($"False<{nameof(Derived)}>");

            return math;

            ICondition<T> MockTrue<T>(string name)
            {
                var @true = new Mock<ICondition<T>>() { Name = name };

                math.Setup(o => o.True<T>())
                    .Returns(@true.Object);

                math.Setup(o => o.IsPredictableTrue(@true.Object))
                    .Returns(true);

                math.Setup(o => o.And(@true.Object, It.IsAny<ICondition<T>>()))
                    .Returns((ICondition<T> a, ICondition<T> b) => b);

                math.Setup(o => o.And(It.IsAny<ICondition<T>>(), @true.Object))
                    .Returns((ICondition<T> a, ICondition<T> b) => a);

                return @true.Object;
            }

            ICondition<T> MockFalse<T>(string name)
            {
                var @false = new Mock<ICondition<T>> { Name = "False" };

                math.Setup(o => o.False<T>())
                    .Returns(@false.Object);

                math.Setup(o => o.IsPredictableFalse(@false.Object))
                    .Returns(true);

                return @false.Object;
            }
        }

        [Test]
        public void ZeroDoesNothing() =>
            Assert.DoesNotThrow(() => _zeroInt.Execute(5));

        [Test]
        public void ZeroIsZero()
        {
            var isZero = _math.IsZero(_zeroInt);
            Assert.IsTrue(isZero);
        }

        [Test]
        public void ZeroForBaseClassIsZeroForDerivedClass()
        {
            var isZero = _math.IsZero<Derived>(_zeroBase);
            Assert.IsTrue(isZero);
        }

        [Test]
        public void ZeroAppendZeroIsZero(
            [ValueSource(nameof(AllAppends))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);
            var zeroInt = _math.Zero<int>();
            var zeroZero = append(_zeroInt, zeroInt);
            var isZero = _math.IsZero(zeroZero);

            Assert.IsTrue(isZero);
        }

        [Test]
        public void BaseZeroAppendDerivedZeroIsDerivedZero(
            [ValueSource(nameof(AllAppends))] string appendType)
        {
            var append = AppendFunc<Derived>(by: appendType);
            var chain = append(_zeroBase, _zeroDerived);
            var isZero = _math.IsZero(chain);

            Assert.IsTrue(isZero);
        }

        [Test]
        public void DerivedZeroAppendBaseZeroChainIsDerivedZero(
            [ValueSource(nameof(AllAppends))] string appendType)
        {
            var append = AppendFunc<Derived>(by: appendType);
            var chain = append(_zeroDerived, _zeroBase);
            var isZero = _math.IsZero(chain);

            Assert.IsTrue(isZero);
        }

        [Test]
        public void MadeHandlerExecutesProvidedAction()
        {
            int x = 0;
            var action = new Action<int>(a => x = a);
            var handler = _math.MakeHandler(action);
            handler.Execute(Arg);

            Assert.That(x, Is.EqualTo(Arg));
        }

        [Test]
        public void AppendIsNotCommutative(
            [ValueSource(nameof(AllAppends))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);

            var abChainResult = string.Empty;
            var baChainResult = string.Empty;
            var acc = string.Empty;

            _handlers[A]
                .Setup(o => o.Execute(Arg))
                .Callback(() => acc += 'a');

            _handlers[B]
                .Setup(o => o.Execute(Arg))
                .Callback(() => acc += 'b');

            var ab = append(
                _handlers[A].Object,
                _handlers[B].Object);

            var ba = append(
                _handlers[B].Object,
                _handlers[A].Object);

            ab.Execute(Arg);
            abChainResult = acc;

            acc = string.Empty;

            ba.Execute(Arg);
            baChainResult = acc;

            Assert.That(
                abChainResult,
                Is.Not.EqualTo(baChainResult));
        }

        [Test]
        public void AppendIsNotIdempotent(
            [Values(2, 3, 4, 5, 100)] int count,
            [ValueSource(nameof(AllAppends))] string appendType)
        {
            var chain = Enumerable
                .Repeat(_handler.Object, count)
                .Aggregate(AppendFunc<int>(by: appendType));

            chain.Execute(Arg);

            _handler.Verify(
                o => o.Execute(Arg),
                Times.Exactly(count));
        }

        [Test]
        public void UnconditionalChainIsAssociative(
            [ValueSource(nameof(AllAppends))] string appendType)
        {
            var acc = string.Empty;
            var ab_cExecutionResult = string.Empty;
            var a_bcExecutionResult = string.Empty;

            var append = AppendFunc<int>(by: appendType);

            foreach (var h in _handlers)
                h.Setup(o => o.Execute(Arg))
                 .Callback(() => acc += 'a');

            var ab_c =
                append(
                    append(
                        _handlers[A].Object,
                        _handlers[B].Object),
                    _handlers[C].Object);

            var a_bc =
                append(
                    _handlers[A].Object,
                    append(
                        _handlers[B].Object,
                        _handlers[C].Object));

            ab_c.Execute(Arg);
            ab_cExecutionResult = acc;

            acc = string.Empty;

            a_bc.Execute(Arg);
            a_bcExecutionResult = acc;

            Assert.That(
                ab_cExecutionResult,
                Is.EqualTo(a_bcExecutionResult));
        }

        public static string[] OneByOneCases =
        {
            "ab", "abc", "abcd",
            "abcdueiruoewiroiewepwo",
            "aaaaaaaaaaaaaaaaaaaaaaa"
        };

        [Test]
        public void ChainExecutesHandlersOneByOne(
            [ValueSource(nameof(AllAppends))] string appendType,
            [ValueSource(nameof(OneByOneCases))] string handlersSource)
        {
            string executionResult = string.Empty;
            IHandler<int>[] handlers = handlersSource
                .Select(x =>
                {
                    var handler = new Mock<IHandler<int>>();
                    handler
                        .Setup(o => o.Execute(Arg))
                        .Callback(() => executionResult += x);

                    return handler.Object;
                })
                .ToArray();

            var chain = handlers
                .Aggregate(AppendFunc<int>(by: appendType));

            chain.Execute(Arg);

            Assert.That(
                executionResult,
                Is.EqualTo(handlersSource));
        }

        public static (string, string)[] OneByOneWithoutZerosCases =
        {
            ("0a", "a"),
            ("a0", "a"),
            ("0a0", "a"),
            ("0000a000", "a"),
            ("abcd0ue0i00ruo0ewi000r000oie000wepw0o0",
             "abcdueiruoewiroiewepwo"),
            ("a0a0a0a0a0a0a0a0a0aaaa0000aaa0a0aaaaaaa000aaaaaa0aaa0aa00aaaa000",
             "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")
        };

        [Test]
        public void ChainExecutesHandlersOneByOneAndIgnoresZeros(
            [ValueSource(nameof(AllAppends))] string appendType,
            [ValueSource(nameof(OneByOneWithoutZerosCases))] (string, string) @case)
        {
            (var handlersSource, var expectedResult) = @case;

            string executionResult = string.Empty;
            IHandler<int>[] handlers = handlersSource
                .Select(x =>
                {
                    if (x == '0')
                    {
                        return _math.Zero<int>();
                    }

                    var handler = new Mock<IHandler<int>>();
                    handler
                        .Setup(o => o.Execute(Arg))
                        .Callback(() => executionResult += x);

                    return handler.Object;
                })
                .ToArray();

            var chain = handlers
                .Aggregate(AppendFunc<int>(by: appendType));

            chain.Execute(Arg);

            Assert.That(
                executionResult,
                Is.EqualTo(expectedResult));
        }

        [Test]
        public void ConditionalZeroIsZero()
        {
            var conditionalZero = _math.Conditional(_zeroInt, _condition.Object);
            Assert.IsTrue(_math.IsZero(conditionalZero));
        }

        [Test]
        public void WhenConditionReturnsTrue__HandlerIsExecuted()
        {
            _condition
                .Setup(o => o.Check(Arg))
                .Returns(true);

            var conditional = _math
                .Conditional(_handler.Object, _condition.Object);

            conditional.Execute(Arg);

            _handler.Verify(o => o.Execute(Arg), Times.Once);
        }

        [Test]
        public void WhenConditionReturnsFalse__HandlerIsNotExecuted()
        {
            _condition
                .Setup(o => o.Check(Arg))
                .Returns(false);

            var conditional = _math
                .Conditional(_handler.Object, _condition.Object);

            conditional.Execute(Arg);

            _handler.Verify(o => o.Execute(Arg), Times.Never);
        }

        [Test]
        public void WhenTopConditionReturnsFalse__AllOtherChecksAndExecutionsAreNotCalled()
        {
            _conditions[C]
                .Setup(o => o.Check(Arg))
                .Returns(false);

            var conditional = new[] { A, B, C }
                .Select(i => _conditions[i].Object)
                .Aggregate(_handler.Object, _math.Conditional);

            conditional.Execute(Arg);

            _conditions[C].Verify(o => o.Check(Arg), Times.Once);
            _conditions[B].Verify(o => o.Check(Arg), Times.Never);
            _conditions[A].Verify(o => o.Check(Arg), Times.Never);
            _handler.Verify(o => o.Execute(Arg), Times.Never);
        }

        [Test]
        public void ChecksAllConditionsUpToFirstFalse(
            [Values(0, 1, 2, 5)] int trueCount,
            [Values(0, 1, 2, 5)] int falseCount)
        {
            var trues = new Mock<ICondition<int>>[trueCount];
            for (int i = 0; i < trueCount; i++)
            {
                trues[i] = new Mock<ICondition<int>>();
                trues[i].Setup(o => o.Check(Arg))
                        .Returns(true);
            }

            var falses = new Mock<ICondition<int>>[falseCount];
            for (int i = 0; i < falseCount; i++)
            {
                falses[i] = new Mock<ICondition<int>>();
                falses[i].Setup(o => o.Check(Arg))
                         .Returns(false);
            }

            var conditional =
                falses.Concat(trues)
                      .Select(x => x.Object)
                      .Aggregate(_handler.Object, _math.Conditional);

            conditional.Execute(Arg);

            var uncheckedCount = int.Max(0, falseCount - 1);

            foreach (var x in trues.Concat(falses.Skip(uncheckedCount)))
                x.Verify(o => o.Check(Arg), Times.Once);

            foreach (var x in falses.Take(uncheckedCount))
                x.Verify(o => o.Check(Arg), Times.Never);

            _handler.Verify(o => o.Execute(Arg),
                ExecutionExpectedWhen(falseCount == 0));
        }

        [Test]
        public void ChecksOrderEqualsWrappingOrder(
            [Values("123")] string abcMarkers,
            [Values("321")] string expectedLog)
        {
            var checksLog = string.Empty;

            foreach (var i in new[] { A, B, C })
                _conditions[i]
                    .Setup(o => o.Check(Arg)).Returns(true)
                    .Callback(() => checksLog += abcMarkers[i]);

            var h = new[] { A, B, C }
                .Select(i => _conditions[i].Object)
                .Aggregate(_handler.Object, _math.Conditional);

            h.Execute(Arg);

            Assert.That(checksLog, Is.EqualTo(expectedLog));
        }

        [TestCase("00")]
        [TestCase("111")]
        [TestCase("01100011")]
        [TestCase("1111100001001010")]
        [TestCase("0101010111100010000111")]
        public void ConditionalHandlersJoinedToChainExecuteRelevantHandlers(
            string conditionsSetup)
        {
            var chainLength = conditionsSetup.Length;

            var handlers = Enumerable
                .Range(0, chainLength)
                .Select(_ => new Mock<IHandler<int>>())
                .ToArray();

            var conditions = Enumerable
                .Range(0, chainLength)
                .Select(_ => new Mock<ICondition<int>>())
                .ToArray();

            foreach (var x in conditions.Zip(conditionsSetup,
                (condition, result) => new { condition, result }))
            {
                x.condition
                 .Setup(o => o.Check(Arg))
                 .Returns(ParseBool(x.result));
            }

            var chain = Enumerable
                .Zip(handlers.Select(x => x.Object),
                     conditions.Select(x => x.Object),
                     _math.Conditional)
                .Aggregate(_math.Join);

            chain.Execute(Arg);

            for (int i = 0; i < chainLength; i++)
            {
                conditions[i].Verify(o => o.Check(Arg), Times.Once);
                handlers[i].Verify(o => o.Execute(Arg),
                    ExecutionExpectedWhen(conditionsSetup[i] == '1'));
            }
        }

        [TestCase("000", "X1")]
        [TestCase("010", "B0")]
        [TestCase("011", "B1")]
        [TestCase("100", "A0")]
        [TestCase("101", "A1")]
        [TestCase("110", "C0")]
        [TestCase("111", "C1")]
        public void MergeCreatesNewHandlerWithRelevantPredicate2(
            string setup,
            string expectation)
        {
            bool aIsConditional = ParseBool(setup[0]);
            bool bIsConditional = ParseBool(setup[1]);
            bool checkResult = ParseBool(setup[2]);
            int expectedCondition = ParseIndex(expectation[0]);
            bool handlersExecutionExpected = ParseBool(expectation[1]);

            _conditionMath
                .Setup(o => o.And(_conditions[A].Object, _conditions[B].Object))
                .Returns(_conditions[C].Object);

            if (expectedCondition != Missing)
                _conditions[expectedCondition]
                    .Setup(o => o.Check(Arg))
                    .Returns(checkResult);

            var a = aIsConditional
                ? _math.Conditional(_handlers[A].Object, _conditions[A].Object)
                : _handlers[A].Object;

            var b = bIsConditional 
                ? _math.Conditional(_handlers[B].Object, _conditions[B].Object)
                : _handlers[B].Object;

            CheckNothingExecuted();

            var ab = _math.Merge(a, b);

            CheckNothingExecuted();

            ab.Execute(Arg);

            if (expectedCondition != Missing)
                _conditions[expectedCondition]
                    .Verify(o => o.Check(Arg), Times.Once);

            foreach (var i in new[] { A, B, C }
                      .Except(new[] { expectedCondition }))
                _conditions[i].Verify(o => o.Check(Arg), Times.Never);

            foreach (var i in new[] { A, B })
                _handlers[i].Verify(o => o.Execute(Arg),
                    ExecutionExpectedWhen(handlersExecutionExpected));
        }

        //         A-C--F    ABCDEF    A-C
        [TestCase("0-0--0", "000001", "0-0")]
        [TestCase("0-1--0", "000001", "0-0")]
        [TestCase("1-0--0", "000001", "0-0")]
        [TestCase("1-1--0", "000001", "0-0")]
        [TestCase("0-0--1", "101001", "0-0")]
        [TestCase("0-1--1", "101001", "0-1")]
        [TestCase("1-0--1", "101001", "1-0")]
        [TestCase("1-1--1", "101001", "1-1")]
        public void MergeConjunctsOnlyTopConditions(
            string conditionCheckResult,
            string conditionCheckExpected,
            string handlerExecuteExpected)
        {
            var checkExpectations = new[] { A, B, C, D, E, F }
                .ToDictionary(i => i, i => ParseBool(conditionCheckExpected[i]));

            var executeExpectations = new[] { A, C }
                .ToDictionary(i => i, i => ParseBool(handlerExecuteExpected[i]));

            foreach (var i in new[] { A, C, F })
                _conditions[i]
                    .Setup(o => o.Check(Arg))
                    .Returns(ParseBool(conditionCheckResult[i]));

            _conditionMath
                .Setup(x => x.And(
                    It.IsAny<ICondition<int>>(),
                    It.IsAny<ICondition<int>>()))
                .Returns(_conditions[E].Object);

            _conditionMath
                .Setup(x => x.And(
                    _conditions[B].Object,
                    _conditions[D].Object))
                .Returns(_conditions[F].Object);

            var a_a  = _math.Conditional(_handlers[A].Object, _conditions[A].Object);
            var a_ab = _math.Conditional(a_a, _conditions[B].Object);

            var c_c  = _math.Conditional(_handlers[C].Object, _conditions[C].Object);
            var c_cd = _math.Conditional(c_c, _conditions[D].Object);

            CheckNothingExecuted();

            var a_ab_MargedWith_c_cd = _math.Merge(a_ab, c_cd);

            CheckNothingExecuted();

            a_ab_MargedWith_c_cd.Execute(Arg);

            foreach (var i in new[] { A, B, C, D, E, F })
                _conditions[i].Verify(o => o.Check(Arg),
                    ExecutionExpectedWhen(checkExpectations[i]));

            foreach (var i in new[] { A, C })
                _handlers[i].Verify(o => o.Execute(Arg),
                    ExecutionExpectedWhen(executeExpectations[i]));
        }

        [TestCase("", "", "", "", "AB")]
        [TestCase("A", "", "A-0", "A", "")]
        [TestCase("A", "", "A-1", "A", "AB")]
        [TestCase("AC", "", "C-0", "C", "")]
        [TestCase("AC", "", "C-1,A-0", "CA", "B")]
        [TestCase("AC", "", "C-1,A-1", "CA", "AB")]
        [TestCase("ACD", "", "D-0", "D", "")]
        [TestCase("ACD", "", "D-1,C-0", "DC", "B")]
        [TestCase("ACD", "", "D-1,C-1,A-0", "DCA", "B")]
        [TestCase("ACD", "", "D-1,C-1,A-1", "DCA", "AB")]
        
        [TestCase("A", "B", "H-0", "H", "")]
        [TestCase("AC", "B", "H-0", "H", "")]
        [TestCase("ACD", "B", "H-0", "H", "")]
        [TestCase("A", "B", "H-0", "H", "")]
        [TestCase("A", "BC", "H-0", "H", "")]
        [TestCase("A", "BCD", "H-0", "H", "")]
        [TestCase("AC", "BD", "H-0", "H", "")]
        [TestCase("ACD", "BE", "H-0", "H", "")]
        [TestCase("ACD", "BEF", "H-0", "H", "")]
        [TestCase("AC", "BEF", "H-0", "H", "")]
        
        [TestCase("A", "B", "H-1", "H", "AB")]
        [TestCase("AC", "B", "H-1,A-0", "HA", "B")]
        [TestCase("AC", "B", "H-1,A-1", "HA", "AB")]
        [TestCase("ACD", "B", "H-1,C-0", "HC", "B")]
        [TestCase("ACD", "B", "H-1,C-1,A-0", "HCA", "B")]
        [TestCase("ACD", "B", "H-1,C-1,A-1", "HCA", "AB")]

        [TestCase("A", "BC", "H-1,B-0", "HB", "A")]
        [TestCase("A", "BC", "H-1,B-1", "HB", "AB")]
        [TestCase("A", "BCD", "H-1,C-0", "HC", "A")]
        [TestCase("A", "BCD", "H-1,C-1,B-0", "HCB", "A")]
        [TestCase("A", "BCD", "H-1,C-1,B-1", "HCB", "AB")]

        [TestCase("AC", "BD", "H-1,A-0,B-0", "HAB", "")]
        [TestCase("AC", "BD", "H-1,A-1,B-0", "HAB", "A")]
        [TestCase("AC", "BD", "H-1,A-0,B-1", "HAB", "B")]
        [TestCase("AC", "BD", "H-1,A-1,B-1", "HAB", "AB")]

        [TestCase("ACD", "BE", "H-1,C-0,B-0", "HCB", "")]
        [TestCase("ACD", "BE", "H-1,C-1,A-0,B-0", "HCAB", "")]
        [TestCase("ACD", "BE", "H-1,C-1,A-1,B-0", "HCAB", "A")]
        [TestCase("ACD", "BE", "H-1,C-0,B-1", "HCB", "B")]
        [TestCase("ACD", "BE", "H-1,C-1,A-0,B-1", "HCAB", "B")]
        [TestCase("ACD", "BE", "H-1,C-1,A-1,B-1", "HCAB", "AB")]

        [TestCase("ACD", "BEF", "H-1,C-0,E-0", "HCE", "")]
        [TestCase("ACD", "BEF", "H-1,C-1,A-0,E-0", "HCAE", "")]
        [TestCase("ACD", "BEF", "H-1,C-1,A-0,E-1,B-0", "HCAEB", "")]
        [TestCase("ACD", "BEF", "H-1,C-1,A-1,E-0", "HCAE", "A")]
        [TestCase("ACD", "BEF", "H-1,C-1,A-1,E-1,B-0", "HCAEB", "A")]
        [TestCase("ACD", "BEF", "H-1,C-0,E-1,B-0", "HCEB", "")]
        [TestCase("ACD", "BEF", "H-1,C-0,E-1,B-1", "HCEB", "B")]
        [TestCase("ACD", "BEF", "H-1,C-1,A-0,E-1,B-1", "HCAEB", "B")]
        [TestCase("ACD", "BEF", "H-1,C-1,A-1,E-1,B-1", "HCAEB", "AB")]
        public void MergeConjunctsOnlyTopConditions(
            string aHandlerConditions,
            string bHandlerConditions,
            string conditionsChecksSetup,
            string expectedToCheck,
            string expectedToExecute)
        {
            var @checked = expectedToCheck.Select(ParseIndex).ToArray();
            var executed = expectedToExecute.Select(ParseIndex).ToArray();

            foreach (var x in ParseChecksSetup(conditionsChecksSetup))
                _conditions[x.i]
                    .Setup(o => o.Check(Arg))
                    .Returns(x.value);

            _conditionMath
                .Setup(x => x.And(
                    It.IsAny<ICondition<int>>(),
                    It.IsAny<ICondition<int>>()))
                .Returns(_conditions[I].Object);

            if (aHandlerConditions.Any() &&
                bHandlerConditions.Any())
                    _conditionMath
                        .Setup(x => x.And(
                            _conditions[ParseIndex(aHandlerConditions.Last())].Object,
                            _conditions[ParseIndex(bHandlerConditions.Last())].Object))
                        .Returns(_conditions[H].Object);

            var a = aHandlerConditions
                .Select(ParseIndex)
                .Select(i => _conditions[i].Object)
                .Aggregate(_handlers[A].Object, _math.Conditional);

            var b = bHandlerConditions
                .Select(ParseIndex)
                .Select(i => _conditions[i].Object)
                .Aggregate(_handlers[B].Object, _math.Conditional);

            CheckNothingExecuted();

            var ab = _math.Merge(a, b);

            CheckNothingExecuted();

            ab.Execute(Arg);

            for (int i = First; i <= Last; i++)
                _conditions[i]
                    .Verify(o => o.Check(Arg),
                            ExecutionExpectedWhen(@checked.Contains(i)));

            _handlers[A].Verify(o => o.Execute(Arg),
                ExecutionExpectedWhen(executed.Contains(A)));

            _handlers[B].Verify(o => o.Execute(Arg),
                ExecutionExpectedWhen(executed.Contains(B)));
        }
        
        [TestCase("AB", "", false)]
        [TestCase("ABC", "", false)]
        [TestCase("ABCDE", "", false)]
        [TestCase("ABCDEFGH", "", false)]

        [TestCase("", "AB", false)]
        [TestCase("", "ABC", false)]
        [TestCase("", "ABCDE", false)]
        [TestCase("", "ABCDEFGH", false)]

        [TestCase("AB", "", true)]
        [TestCase("ABC", "", true)]
        [TestCase("ABCDE", "", true)]
        [TestCase("ABCDEFGH", "", true)]

        [TestCase("", "AB", true)]
        [TestCase("", "ABC", true)]
        [TestCase("", "ABCDE", true)]
        [TestCase("", "ABCDEFGH", true)]

        [TestCase("A", "B", false)]
        [TestCase("AB", "C", false)]
        [TestCase("AB", "CD", false)]
        [TestCase("ABC", "D", false)]
        [TestCase("ABC", "DE", false)]
        [TestCase("ABC", "DEF", false)]
        [TestCase("ABCDE", "F", false)]
        [TestCase("ABCDE", "FG", false)]
        [TestCase("ABCDE", "FGH", false)]
        [TestCase("ABCDE", "FGHI", false)]
        [TestCase("ABCDEFG", "H", false)]
        [TestCase("ABCDEFG", "HI", false)]
        [TestCase("ABCDEFGH", "I", false)]

        [TestCase("B", "A", false)]
        [TestCase("C", "AB", false)]
        [TestCase("CD", "AB", false)]
        [TestCase("D", "ABC", false)]
        [TestCase("DE", "ABC", false)]
        [TestCase("DEF", "ABC", false)]
        [TestCase("F", "ABCDE", false)]
        [TestCase("FG", "ABCDE", false)]
        [TestCase("FGH", "ABCDE", false)]
        [TestCase("FGHI", "ABCDE", false)]
        [TestCase("H", "ABCDEFG", false)]
        [TestCase("HI", "ABCDEFG", false)]
        [TestCase("I", "ABCDEFGH", false)]

        [TestCase("A", "B", true)]
        [TestCase("AB", "C", true)]
        [TestCase("AB", "CD", true)]
        [TestCase("ABC", "D", true)]
        [TestCase("ABC", "DE", true)]
        [TestCase("ABC", "DEF", true)]
        [TestCase("ABCDE", "F", true)]
        [TestCase("ABCDE", "FG", true)]
        [TestCase("ABCDE", "FGH", true)]
        [TestCase("ABCDE", "FGHI", true)]
        [TestCase("ABCDEFG", "H", true)]
        [TestCase("ABCDEFG", "HI", true)]
        [TestCase("ABCDEFGH", "I", true)]

        [TestCase("B", "A", true)]
        [TestCase("C", "AB", true)]
        [TestCase("CD", "AB", true)]
        [TestCase("D", "ABC", true)]
        [TestCase("DE", "ABC", true)]
        [TestCase("DEF", "ABC", true)]
        [TestCase("F", "ABCDE", true)]
        [TestCase("FG", "ABCDE", true)]
        [TestCase("FGH", "ABCDE", true)]
        [TestCase("FGHI", "ABCDE", true)]
        [TestCase("H", "ABCDEFG", true)]
        [TestCase("HI", "ABCDEFG", true)]
        [TestCase("I", "ABCDEFGH", true)]
        public void DeepMergeConjunctsAllConditions(
            string aHandlerConditions,
            string bHandlerConditions,
            bool finalConditionResult)
        {
            var conditions = Enumerable.Range(First, Last + 1)
                .ToDictionary(IndexToString, i => _conditions[i].Object);

            var finalCondition = new Mock<ICondition<int>>(); 

            _conditionMath
                .Setup(o => o.And(
                    It.IsAny<ICondition<int>>(), 
                    It.IsAny<ICondition<int>>()))
                
                .Returns((ICondition<int> x, ICondition<int> y) =>
                {
                    var i = conditions.First(z => z.Value == x).Key;
                    var j = conditions.First(z => z.Value == y).Key;
                    var ij = new string((i + j).Order().ToArray());

                    finalCondition = new Mock<ICondition<int>>()
                    {
                        Name = $"predicate|{ij}|"
                    };

                    conditions.Add(ij, finalCondition.Object);

                    return finalCondition.Object; 
                });

            var a = aHandlerConditions
                .Select(ParseIndex)
                .Select(i => _conditions[i].Object)
                .Aggregate(_handlers[A].Object, _math.Conditional);

            var b = bHandlerConditions
                .Select(ParseIndex)
                .Select(i => _conditions[i].Object)
                .Aggregate(_handlers[B].Object, _math.Conditional);

            CheckNothingExecuted();

            var ab = _math.DeepMerge(a, b);

            CheckNothingExecuted();

            finalCondition
                .Setup(o => o.Check(Arg))
                .Returns(finalConditionResult);

            ab.Execute(Arg);

            var key = new string(
                (aHandlerConditions + bHandlerConditions)
                    .Order()
                    .ToArray());

            var resultCondition = conditions[key];

            Assert.That(resultCondition,
                Is.SameAs(finalCondition.Object));

            finalCondition.Verify(o => o.Check(Arg), Times.Once);

            _handlers[A].Verify(o => o.Execute(Arg),
                ExecutionExpectedWhen(finalConditionResult));

            _handlers[A].Verify(o => o.Execute(Arg),
                ExecutionExpectedWhen(finalConditionResult));
        }

        [Test]
        public void DeepMergeWithSingleConditionalHandlerPutConditionOnTopOfResult(
            [Values(false, true)] bool order,
            [Values(false, true)] bool checkResult)
        {
            _conditions[A]
                .Setup(o => o.Check(Arg))
                .Returns(checkResult);

            var x = _math.Conditional(_handlers[A].Object, _conditions[A].Object);
            var y = _handlers[B].Object;

            CheckNothingExecuted();

            var z = order
                ? _math.DeepMerge(x, y)
                : _math.DeepMerge(y, x);

            CheckNothingExecuted();

            z.Execute(Arg);

            _conditions[A].Verify(o => o.Check(Arg), Times.Once);

            _handlers[A].Verify(o => o.Execute(Arg),
                ExecutionExpectedWhen(checkResult));

            _handlers[B].Verify(o => o.Execute(Arg),
                ExecutionExpectedWhen(checkResult));
        }

        [TestCase(DeepInject, "", "", "", "[A][B]")]
        [TestCase(DeepInject, "C", "", "C-0", "C[B]")]
        [TestCase(DeepInject, "C", "", "C-1", "C[A][B]")]
        [TestCase(DeepInject, "", "C", "C-0", "C")]
        [TestCase(DeepInject, "", "C", "C-1", "C[A][B]")]
        [TestCase(DeepInject, "CD", "", "D-0", "D[B]")]
        [TestCase(DeepInject, "CD", "", "D-1,C-0", "DC[B]")]
        [TestCase(DeepInject, "CD", "", "D-1,C-1", "DC[A][B]")]
        [TestCase(DeepInject, "", "CD", "D-0", "D")]
        [TestCase(DeepInject, "", "CD", "D-1,C-0", "DC")]
        [TestCase(DeepInject, "", "CD", "D-1,C-1", "DC[A][B]")]
        [TestCase(DeepInject, "CDEF", "", "F-0", "F[B]")]
        [TestCase(DeepInject, "CDEF", "", "F-1,E-0", "FE[B]")]
        [TestCase(DeepInject, "CDEF", "", "F-1,E-1,D-0", "FED[B]")]
        [TestCase(DeepInject, "CDEF", "", "F-1,E-1,D-1,C-0", "FEDC[B]")]
        [TestCase(DeepInject, "CDEF", "", "F-1,E-1,D-1,C-1", "FEDC[A][B]")]
        [TestCase(DeepInject, "", "CDEF", "F-0", "F")]
        [TestCase(DeepInject, "", "CDEF", "F-1,E-0", "FE")]
        [TestCase(DeepInject, "", "CDEF", "F-1,E-1,D-0", "FED")]
        [TestCase(DeepInject, "", "CDEF", "F-1,E-1,D-1,C-0", "FEDC")]
        [TestCase(DeepInject, "", "CDEF", "F-1,E-1,D-1,C-1", "FEDC[A][B]")]

        [TestCase(DeepInject, "C", "D", "D-0", "D")]
        [TestCase(DeepInject, "C", "D", "D-1,C-0", "DC[B]")]
        [TestCase(DeepInject, "C", "D", "D-1,C-1", "DC[A][B]")]
        [TestCase(DeepInject, "CD", "E", "E-0", "E")]
        [TestCase(DeepInject, "CD", "E", "E-1,D-0", "ED[B]")]
        [TestCase(DeepInject, "CD", "E", "E-1,D-1,C-0", "EDC[B]")]
        [TestCase(DeepInject, "CD", "E", "E-1,D-1,C-1", "EDC[A][B]")]
        [TestCase(DeepInject, "C", "DE", "E-0", "E")]
        [TestCase(DeepInject, "C", "DE", "E-1,D-0", "ED")]
        [TestCase(DeepInject, "C", "DE", "E-1,D-1,C-0", "EDC[B]")]
        [TestCase(DeepInject, "C", "DE", "E-1,D-1,C-1", "EDC[A][B]")]
        [TestCase(DeepInject, "CD", "EF", "F-0", "F")]
        [TestCase(DeepInject, "CD", "EF", "F-1,E-0", "FE")]
        [TestCase(DeepInject, "CD", "EF", "F-1,E-1,D-0", "FED[B]")]
        [TestCase(DeepInject, "CD", "EF", "F-1,E-1,D-1,C-0", "FEDC[B]")]
        [TestCase(DeepInject, "CD", "EF", "F-1,E-1,D-1,C-1", "FEDC[A][B]")]
        [TestCase(DeepInject, "CDE", "FGH", "H-0", "H")]
        [TestCase(DeepInject, "CDE", "FGH", "H-1,G-0", "HG")]
        [TestCase(DeepInject, "CDE", "FGH", "H-1,G-1,F-0", "HGF")]
        [TestCase(DeepInject, "CDE", "FGH", "H-1,G-1,F-1,E-0", "HGFE[B]")]
        [TestCase(DeepInject, "CDE", "FGH", "H-1,G-1,F-1,E-1,D-0", "HGFED[B]")]
        [TestCase(DeepInject, "CDE", "FGH", "H-1,G-1,F-1,E-1,D-1,C-0", "HGFEDC[B]")]
        [TestCase(DeepInject, "CDE", "FGH", "H-1,G-1,F-1,E-1,D-1,C-1", "HGFEDC[A][B]")]

        [TestCase(DeepWrap, "", "", "", "[A][B]")]
        [TestCase(DeepWrap, "C", "", "C-0", "C")]
        [TestCase(DeepWrap, "C", "", "C-1", "C[A][B]")]
        [TestCase(DeepWrap, "", "C", "C-0", "[A]C")]
        [TestCase(DeepWrap, "", "C", "C-1", "[A]C[B]")]
        [TestCase(DeepWrap, "CD", "", "D-0", "D")]
        [TestCase(DeepWrap, "CD", "", "D-1,C-0", "DC")]
        [TestCase(DeepWrap, "CD", "", "D-1,C-1", "DC[A][B]")]
        [TestCase(DeepWrap, "", "CD", "D-0", "[A]D")]
        [TestCase(DeepWrap, "", "CD", "D-1,C-0", "[A]DC")]
        [TestCase(DeepWrap, "", "CD", "D-1,C-1", "[A]DC[B]")]
        [TestCase(DeepWrap, "CDEF", "", "F-0", "F")]
        [TestCase(DeepWrap, "CDEF", "", "F-1,E-0", "FE")]
        [TestCase(DeepWrap, "CDEF", "", "F-1,E-1,D-0", "FED")]
        [TestCase(DeepWrap, "CDEF", "", "F-1,E-1,D-1,C-0", "FEDC")]
        [TestCase(DeepWrap, "CDEF", "", "F-1,E-1,D-1,C-1", "FEDC[A][B]")]
        [TestCase(DeepWrap, "", "CDEF", "F-0", "[A]F")]
        [TestCase(DeepWrap, "", "CDEF", "F-1,E-0", "[A]FE")]
        [TestCase(DeepWrap, "", "CDEF", "F-1,E-1,D-0", "[A]FED")]
        [TestCase(DeepWrap, "", "CDEF", "F-1,E-1,D-1,C-0", "[A]FEDC")]
        [TestCase(DeepWrap, "", "CDEF", "F-1,E-1,D-1,C-1", "[A]FEDC[B]")]

        [TestCase(DeepWrap, "C", "D", "C-0", "C")]
        [TestCase(DeepWrap, "C", "D", "C-1,D-0", "C[A]D")]
        [TestCase(DeepWrap, "C", "D", "C-1,D-1", "C[A]D[B]")]
        [TestCase(DeepWrap, "CD", "E", "D-0", "D")]
        [TestCase(DeepWrap, "CD", "E", "D-1,C-0", "DC")]
        [TestCase(DeepWrap, "CD", "E", "D-1,C-1,E-0", "DC[A]E")]
        [TestCase(DeepWrap, "CD", "E", "D-1,C-1,E-1", "DC[A]E[B]")]
        [TestCase(DeepWrap, "C", "DE", "C-0", "C")]
        [TestCase(DeepWrap, "C", "DE", "C-1,E-0", "C[A]E")]
        [TestCase(DeepWrap, "C", "DE", "C-1,E-1,D-0", "C[A]ED")]
        [TestCase(DeepWrap, "C", "DE", "C-1,E-1,D-1", "C[A]ED[B]")]
        [TestCase(DeepWrap, "CD", "EF", "D-0", "D")]
        [TestCase(DeepWrap, "CD", "EF", "D-1,C-0", "DC")]
        [TestCase(DeepWrap, "CD", "EF", "D-1,C-1,F-0", "DC[A]F")]
        [TestCase(DeepWrap, "CD", "EF", "D-1,C-1,F-1,E-0", "DC[A]FE")]
        [TestCase(DeepWrap, "CD", "EF", "D-1,C-1,F-1,E-1", "DC[A]FE[B]")]
        [TestCase(DeepWrap, "CDE", "FGH", "E-0", "E")]
        [TestCase(DeepWrap, "CDE", "FGH", "E-1,D-0", "ED")]
        [TestCase(DeepWrap, "CDE", "FGH", "E-1,D-1,C-0", "EDC")]
        [TestCase(DeepWrap, "CDE", "FGH", "E-1,D-1,C-1,H-0", "EDC[A]H")]
        [TestCase(DeepWrap, "CDE", "FGH", "E-1,D-1,C-1,H-1,G-0", "EDC[A]HG")]
        [TestCase(DeepWrap, "CDE", "FGH", "E-1,D-1,C-1,H-1,G-1,F-0", "EDC[A]HGF")]
        [TestCase(DeepWrap, "CDE", "FGH", "E-1,D-1,C-1,H-1,G-1,F-1", "EDC[A]HGF[B]")]

        [TestCase(Inject, "", "", "", "[A][B]")]
        [TestCase(Inject, "C", "", "C-0", "C[B]")]
        [TestCase(Inject, "C", "", "C-1", "C[A][B]")]
        [TestCase(Inject, "", "C", "C-0", "C")]
        [TestCase(Inject, "", "C", "C-1", "C[A][B]")]
        [TestCase(Inject, "CD", "", "D-0", "D[B]")]
        [TestCase(Inject, "CD", "", "D-1,C-0", "DC[B]")]
        [TestCase(Inject, "CD", "", "D-1,C-1", "DC[A][B]")]
        [TestCase(Inject, "", "CD", "D-0", "D")]
        [TestCase(Inject, "", "CD", "D-1,C-0", "D[A]C")]
        [TestCase(Inject, "", "CD", "D-1,C-1", "D[A]C[B]")]
        [TestCase(Inject, "CDEF", "", "F-0", "F[B]")]
        [TestCase(Inject, "CDEF", "", "F-1,E-0", "FE[B]")]
        [TestCase(Inject, "CDEF", "", "F-1,E-1,D-0", "FED[B]")]
        [TestCase(Inject, "CDEF", "", "F-1,E-1,D-1,C-0", "FEDC[B]")]
        [TestCase(Inject, "CDEF", "", "F-1,E-1,D-1,C-1", "FEDC[A][B]")]
        [TestCase(Inject, "", "CDEF", "F-0", "F")]
        [TestCase(Inject, "", "CDEF", "F-1,E-0", "F[A]E")]
        [TestCase(Inject, "", "CDEF", "F-1,E-1,D-0", "F[A]ED")]
        [TestCase(Inject, "", "CDEF", "F-1,E-1,D-1,C-0", "F[A]EDC")]
        [TestCase(Inject, "", "CDEF", "F-1,E-1,D-1,C-1", "F[A]EDC[B]")]

        [TestCase(Inject, "C", "D", "D-0", "D")]
        [TestCase(Inject, "C", "D", "D-1,C-0", "DC[B]")]
        [TestCase(Inject, "C", "D", "D-1,C-1", "DC[A][B]")]
        [TestCase(Inject, "CD", "E", "E-0", "E")]
        [TestCase(Inject, "CD", "E", "E-1,D-0", "ED[B]")]
        [TestCase(Inject, "CD", "E", "E-1,D-1,C-0", "EDC[B]")]
        [TestCase(Inject, "CD", "E", "E-1,D-1,C-1", "EDC[A][B]")]
        [TestCase(Inject, "C", "DE", "E-0", "E")]
        [TestCase(Inject, "C", "DE", "E-1,C-0,D-0", "ECD")]
        [TestCase(Inject, "C", "DE", "E-1,C-1,D-0", "EC[A]D")]
        [TestCase(Inject, "C", "DE", "E-1,C-1,D-1", "EC[A]D[B]")]
        [TestCase(Inject, "CD", "EF", "F-0", "F")]
        [TestCase(Inject, "CD", "EF", "F-1,D-0,E-0", "FDE")]
        [TestCase(Inject, "CD", "EF", "F-1,D-0,E-1", "FDE[B]")]
        [TestCase(Inject, "CD", "EF", "F-1,D-1,C-0,E-0", "FDCE")]
        [TestCase(Inject, "CD", "EF", "F-1,D-1,C-0,E-1", "FDCE[B]")]
        [TestCase(Inject, "CD", "EF", "F-1,D-1,C-1,E-0", "FDC[A]E")]
        [TestCase(Inject, "CD", "EF", "F-1,D-1,C-1,E-1", "FDC[A]E[B]")]
        [TestCase(Inject, "CDE", "FGH", "H-0", "H")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-0,G-0", "HEG")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-0,G-0", "HEDG")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-1,C-0,G-0", "HEDCG")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-1,C-1,G-0", "HEDC[A]G")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-0,G-0", "HEG")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-0,G-1,F-0", "HEGF")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-0,G-1,F-1", "HEGF[B]")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-0,G-1,F-0", "HEDGF")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-1,C-0,G-1,F-0", "HEDCGF")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-1,C-1,G-1,F-0", "HEDC[A]GF")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-1,C-0,G-1,F-1", "HEDCGF[B]")]
        [TestCase(Inject, "CDE", "FGH", "H-1,E-1,D-1,C-1,G-1,F-1", "HEDC[A]GF[B]")]

        [TestCase(Wrap, "", "", "", "[A][B]")]
        [TestCase(Wrap, "C", "", "C-0", "C")]
        [TestCase(Wrap, "C", "", "C-1", "C[A][B]")]
        [TestCase(Wrap, "", "C", "C-0", "[A]C")]
        [TestCase(Wrap, "", "C", "C-1", "[A]C[B]")]
        [TestCase(Wrap, "CD", "", "D-0", "D")]
        [TestCase(Wrap, "CD", "", "D-1,C-0", "DC[B]")]
        [TestCase(Wrap, "CD", "", "D-1,C-1", "DC[A][B]")]
        [TestCase(Wrap, "", "CD", "D-0", "[A]D")]
        [TestCase(Wrap, "", "CD", "D-1,C-0", "[A]DC")]
        [TestCase(Wrap, "", "CD", "D-1,C-1", "[A]DC[B]")]
        [TestCase(Wrap, "CDEF", "", "F-0", "F")]
        [TestCase(Wrap, "CDEF", "", "F-1,E-0", "FE[B]")]
        [TestCase(Wrap, "CDEF", "", "F-1,E-1,D-0", "FED[B]")]
        [TestCase(Wrap, "CDEF", "", "F-1,E-1,D-1,C-0", "FEDC[B]")]
        [TestCase(Wrap, "CDEF", "", "F-1,E-1,D-1,C-1", "FEDC[A][B]")]
        [TestCase(Wrap, "", "CDEF", "F-0", "[A]F")]
        [TestCase(Wrap, "", "CDEF", "F-1,E-0", "[A]FE")]
        [TestCase(Wrap, "", "CDEF", "F-1,E-1,D-0", "[A]FED")]
        [TestCase(Wrap, "", "CDEF", "F-1,E-1,D-1,C-0", "[A]FEDC")]
        [TestCase(Wrap, "", "CDEF", "F-1,E-1,D-1,C-1", "[A]FEDC[B]")]

        [TestCase(Wrap, "C", "D", "C-0", "C")]
        [TestCase(Wrap, "C", "D", "C-1,D-0", "C[A]D")]
        [TestCase(Wrap, "C", "D", "C-1,D-1", "C[A]D[B]")]
        [TestCase(Wrap, "CD", "E", "D-0", "D")]
        [TestCase(Wrap, "CD", "E", "D-1,C-0,E-0", "DCE")]
        [TestCase(Wrap, "CD", "E", "D-1,C-1,E-0", "DC[A]E")]
        [TestCase(Wrap, "CD", "E", "D-1,C-0,E-1", "DCE[B]")]
        [TestCase(Wrap, "CD", "E", "E-1,D-1,C-1", "DC[A]E[B]")]
        [TestCase(Wrap, "C", "DE", "C-0", "C")]
        [TestCase(Wrap, "C", "DE", "C-1,E-0", "C[A]E")]
        [TestCase(Wrap, "C", "DE", "C-1,E-1,D-0", "C[A]ED")]
        [TestCase(Wrap, "C", "DE", "C-1,E-1,D-1", "C[A]ED[B]")]
        [TestCase(Wrap, "CD", "EF", "D-0", "D")]
        [TestCase(Wrap, "CD", "EF", "D-1,C-0,F-0", "DCF")]
        [TestCase(Wrap, "CD", "EF", "D-1,C-1,F-0", "DC[A]F")]
        [TestCase(Wrap, "CD", "EF", "D-1,C-1,F-1,E-0", "DC[A]FE")]
        [TestCase(Wrap, "CD", "EF", "D-1,C-0,F-1,E-0", "DCFE")]
        [TestCase(Wrap, "CD", "EF", "D-1,C-1,F-1,E-0", "DC[A]FE")]
        [TestCase(Wrap, "CD", "EF", "D-1,C-0,F-1,E-1", "DCFE[B]")]
        [TestCase(Wrap, "CD", "EF", "D-1,C-1,F-1,E-1", "DC[A]FE[B]")]
        [TestCase(Wrap, "CDE", "FGH", "E-0", "E")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-0,H-0", "EDH")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-1,C-0,H-0", "EDCH")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-1,C-1,H-1,G-0", "EDC[A]HG")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-1,C-1,H-1,G-1,F-0", "EDC[A]HGF")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-1,C-1,H-1,G-1,F-1", "EDC[A]HGF[B]")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-0,H-1,G-0", "EDHG")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-0,H-1,G-1,F-0", "EDHGF")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-0,H-1,G-1,F-1", "EDHGF[B]")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-1,C-0,H-1,G-0", "EDCHG")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-1,C-0,H-1,G-1,F-0", "EDCHGF")]
        [TestCase(Wrap, "CDE", "FGH", "E-1,D-1,C-0,H-1,G-1,F-1", "EDCHGF[B]")]
        public void InjectAndWrapCreatesCorrectConditionsÑascade(
            string appendType,
            string aHandlerConditions,
            string bHandlerConditions,
            string conditionsChecksSetup,
            string expectedCallsOrder)
        {
            var checksResult = ParseChecksSetup(conditionsChecksSetup)
                .ToDictionary(x => x.i, x => x.value);

            var callsOrder = string.Empty;

            foreach (var i in new[] { A, B })
                _handlers[i]
                    .Setup(o => o.Execute(Arg))
                    .Callback(() => callsOrder += $"[{IndexToString(i)}]");

            var a = CombineHandler(aHandlerConditions, A);
            var b = CombineHandler(bHandlerConditions, B);

            var append = AppendFunc<int>(by: appendType);

            CheckNothingExecuted();

            var ab = append(a, b);

            CheckNothingExecuted();

            ab.Execute(Arg);

            Assert.That(callsOrder,
                Is.EqualTo(expectedCallsOrder));

            IHandler<int> CombineHandler(
                string conditionsSetup, 
                int handlerIndex) =>
                    conditionsSetup
                        .Select(x =>
                        {
                            var i = ParseIndex(x);
                            var p = _conditions[i];

                            if (checksResult.ContainsKey(i))
                                p.Setup(o => o.Check(Arg))
                                 .Returns(checksResult[i])
                                 .Callback(() => callsOrder += x);
                            else
                                p.Setup(o => o.Check(Arg))
                                 .Callback(() => callsOrder += x);

                            return p.Object;
                        })
                        .Aggregate(
                            _handlers[handlerIndex].Object,
                            _math.Conditional);
        }

        private static IEnumerable<(int i, bool value)> ParseChecksSetup(
            string conditionChecksSetup) =>
                conditionChecksSetup
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split('-'))
                    .Select(x => (i: ParseIndex(x[0][0]), value: ParseBool(x[1][0])));

        private void CheckNothingExecuted()
        {
            foreach (var mock in _conditions)
                mock.Verify(o => o.Check(Arg), Times.Never);

            foreach (var mock in _handlers)
                mock.Verify(o => o.Execute(Arg), Times.Never);
        }

        private static Func<Times> ExecutionExpectedWhen(bool value) =>
            value ? Times.Once : Times.Never;

        private static bool ParseBool(char mask) => mask == '1';

        private static int ParseIndex(char mask) =>
            mask switch
            {
                'X' => Missing,
                _ => mask - 'A'
            };

        private static string IndexToString(int i) =>
            (i switch
            {
                Missing => 'X',
                _ => (char)('A' + i)
            })
            .ToString();
    }
}