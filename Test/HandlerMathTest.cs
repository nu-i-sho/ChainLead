namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using ChainLead.Test.HandlersTestData;
    using ChainLead.Test.Help;

    using Moq;
    using NUnit.Framework.Internal;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using static ChainLead.Test.Help.Constants;
    using static ChainLead.Test.Help.Constants.Appends;

    [TestFixtureSource(nameof(Cases))]
    public partial class HandlerMathTest(IHandlerMathCallsProviderFactory mathFactory)
    {
        public static IEnumerable<IHandlerMathCallsProviderFactory> Cases
        {
            get
            {
                yield return new OriginalHandlerMathCallsProviderFactory();
                yield return new ChainLeadSyntaxCallsProviderFactory();
                yield return new ChainLeadSyntaxSeparatedCallsProviderFactory();
                yield return new ChainLeadSyntaxReverseCallsProviderFactory();
            }
        }

        public interface IBase;
        public interface IDerived : IBase;

        [AllowNull] IHandlerMath _math;
        [AllowNull] ChainLeadMocks _mockOf;

        Func<IHandler<T>, IHandler<T>, IHandler<T>>
            AppendFunc<T>(string by) =>
                by switch
                {
                    FirstThenSecond => _math.FirstThenSecond,
                    PackFirstInSecond => _math.PackFirstInSecond,
                    InjectFirstIntoSecond => _math.InjectFirstIntoSecond,
                    FirstCoverSecond => _math.FirstCoverSecond,
                    FirstWrapSecond => _math.FirstWrapSecond,
                    JoinFirstWithSecond => _math.JoinFirstWithSecond,
                    MergeFirstWithSecond => _math.MergeFirstWithSecond,
                    _ => throw new ArgumentOutOfRangeException(nameof(by))
                };

        [SetUp]
        [MemberNotNull(nameof(_math))]
        [MemberNotNull(nameof(_mockOf))]
        public void Setup()
        {
            _mockOf = new ChainLeadMocks();
            MockConditionMath();
            _math = mathFactory.Create(_mockOf.ConditionMath.Object);
        }

        void MockConditionMath()
        {
            MockTrue<int>("True");
            MockFalse<int>("False");
            MockTrue<IBase>($"True<{nameof(IBase)}>");
            MockFalse<IBase>($"False<{nameof(IBase)}>");
            MockTrue<IDerived>($"True<{nameof(IDerived)}>");
            MockFalse<IDerived>($"False<{nameof(IDerived)}>");

            ICondition<T> MockTrue<T>(string name)
            {
                var @true = new Mock<ICondition<T>>() { Name = name };

                _mockOf.ConditionMath
                    .Setup(o => o.True<T>())
                    .Returns(@true.Object);

                _mockOf.ConditionMath
                    .Setup(o => o.IsPredictableTrue(@true.Object))
                    .Returns(true);

                _mockOf.ConditionMath
                    .Setup(o => o.And(@true.Object, It.IsAny<ICondition<T>>()))
                    .Returns((ICondition<T> a, ICondition<T> b) => b);

                _mockOf.ConditionMath
                    .Setup(o => o.And(It.IsAny<ICondition<T>>(), @true.Object))
                    .Returns((ICondition<T> a, ICondition<T> b) => a);

                return @true.Object;
            }

            ICondition<T> MockFalse<T>(string name)
            {
                var @false = new Mock<ICondition<T>> { Name = name };

                _mockOf.ConditionMath
                    .Setup(o => o.False<T>())
                    .Returns(@false.Object);

                _mockOf.ConditionMath
                    .Setup(o => o.IsPredictableFalse(@false.Object))
                    .Returns(true);

                return @false.Object;
            }
        }

        [Test]
        public void ZeroDoesNothing() =>
            Assert.DoesNotThrow(() => _math.Zero<int>().Execute(Arg));

        [Test]
        public void ZeroIsZero() =>
            Assert.That(_math.IsZero(_math.Zero<int>()));

        [Test]
        public void ZeroForBaseClassIsZeroForDerivedClass() =>
            Assert.That(_math.IsZero<IDerived>(_math.Zero<IBase>()));

        [Test]
        public void ZeroAppendZeroIsZero(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);
            var zeros = new[] { _math.Zero<int>(), _math.Zero<int>() };
            var twoZeros = append(zeros[0], zeros[1]);
            
            Assert.That(_math.IsZero(twoZeros));
        }

        [Test]
        public void BaseZeroAppendDerivedZeroIsDerivedZero(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<IDerived>(by: appendType);
            var chain = append(_math.Zero<IBase>(), _math.Zero<IDerived>());
            
            Assert.That(_math.IsZero(chain));
        }

        [Test]
        public void DerivedZeroAppendBaseZeroChainIsDerivedZero(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<IDerived>(by: appendType);
            var chain = append(_math.Zero<IDerived>(), _math.Zero<IBase>());
            
            Assert.That(_math.IsZero(chain));
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
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);
            List<HandlerIndex> abExecution = [];
            List<HandlerIndex> baExecution = [];
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            
            var ab = append(
                _mockOf.Handlers[A].Object,
                _mockOf.Handlers[B].Object);

            var ba = append(
                _mockOf.Handlers[B].Object,
                _mockOf.Handlers[A].Object);

            ab.Execute(Arg);
            abExecution.AddRange(execution);

            execution.Clear();

            ba.Execute(Arg);
            baExecution.AddRange(execution);

            Assert.That(abExecution,
                Is.Not.EqualTo(baExecution));
        }

        [Test]
        public void AppendIsNotIdempotent(
            [Values(2, 3, 4, 5, 100)] int count,
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            Enumerable
                .Repeat(_mockOf.Handlers[A].Object, count)
                .Aggregate(AppendFunc<int>(by: appendType))
                .Execute(Arg);

            Assert.That(_mockOf.Handlers[A].WasExecuted(count).Times);
        }

        [Test]
        public void UnconditionalChainIsAssociative(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);
            List<HandlerIndex> ab_cExecution = [];
            List<HandlerIndex> a_bcExecution = [];
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            var ab_c =
                append(
                    append(
                        _mockOf.Handlers[A].Object,
                        _mockOf.Handlers[B].Object),
                    _mockOf.Handlers[C].Object);

            var a_bc =
                append(
                    _mockOf.Handlers[A].Object,
                    append(
                        _mockOf.Handlers[B].Object,
                        _mockOf.Handlers[C].Object));

            ab_c.Execute(Arg);
            ab_cExecution.AddRange(execution);

            execution.Clear();

            a_bc.Execute(Arg);
            a_bcExecution.AddRange(execution);

            Assert.That(ab_cExecution,
                Is.EqualTo(a_bcExecution));
        }

        [Test]
        public void ChainExecutesHandlersOneByOne(
            [ValueSource(typeof(Appends), nameof(All))] string appendType,
            [ValueSource(nameof(Cases1))] Case1 @case)
        {
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[@case.ChainIndices.Distinct()]
                .AddLoggingInto(execution); 

            var chain = @case.ChainIndices
                .Select(_mockOf.Handlers.GetObject)
                .Aggregate(AppendFunc<int>(by: appendType));

            chain.Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(@case.ChainIndices));
        }

        [Test]
        public void ChainExecutesHandlersOneByOneAndIgnoresZeros(
            [ValueSource(typeof(Appends), nameof(All))] string appendType,
            [ValueSource(nameof(Cases2))] Case2 @case)
        {
            List<HandlerIndex> execution = [];

            var uniqueIndices = @case.ChainIndicesWithNullsAsZeros
                 .Where(NotNull).Select(Denullify).Distinct();

            _mockOf.Handlers[uniqueIndices]
                 .AddLoggingInto(execution);

            @case.ChainIndicesWithNullsAsZeros
                 .Select(i => i != null
                     ? _mockOf.Handlers[i].Object
                     : _math.Zero<int>())
                 .Aggregate(AppendFunc<int>(by: appendType))
                 .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(@case.ExpectedExecution));
        }

        [Test]
        public void ConditionalZeroIsZero()
        {
            var conditionalZero = _math.Conditional(
                _math.Zero<int>(),
                _mockOf.Conditions[X].Object);
            
            Assert.That(_math.IsZero(conditionalZero));
        }

        [Test]
        public void WhenConditionReturnsTrue__HandlerIsExecuted()
        {
            _mockOf.Conditions[X].SetResult(true);
            
            _math.Conditional(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Conditions[X].Object)
                 .Execute(Arg);

            Assert.That(_mockOf.Handlers[A].WasExecutedOnce());
        }

        [Test]
        public void WhenConditionReturnsFalse__HandlerIsNotExecuted()
        {
            _mockOf.Conditions[X].SetResult(false);

            _math.Conditional(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Conditions[X].Object)
                 .Execute(Arg);

            Assert.That(_mockOf.Handlers[A].WasNeverExecuted());
        }

        [Test]
        public void WhenTopConditionReturnsFalse__AllOtherChecksAndExecutionsAreNotCalled()
        {
            _mockOf.Conditions[Z].SetResult(false);

            var conditional = _mockOf.Conditions[X, Y, Z].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            conditional.Execute(Arg);

            Assert.Multiple(() =>
            {
                Assert.That(_mockOf.Conditions[Z].WasCheckedOnce());
                Assert.That(_mockOf.Conditions[X, Y].NoOneWasChecked());
                Assert.That(_mockOf.Handlers[A].WasNeverExecuted());
            });
        }

        [Test]
        public void ChecksAllConditionsUpToFirstFalse(
            [Values(0, 1, 2, 5)] int trueCount,
            [Values(0, 1, 2, 5)] int falseCount)
        {
            var trues = _mockOf.Conditions.Take(trueCount);
            var falses = _mockOf.Conditions.Skip(trueCount).Take(falseCount);

            trues.SetResults(true);
            falses.SetResults(false);

            var all = falses.Concat(trues);
            var conditional = all.Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            conditional.Execute(Arg);

            var checkedCount = trueCount + int.Min(1, falseCount);

            Assert.Multiple(() =>
            {
                Assert.That(all.Reverse().Take(checkedCount).EachWasCheckedOnce());
                Assert.That(all.Reverse().Skip(checkedCount).NoOneWasChecked());
                Assert.That(_mockOf.Handlers[A].WasExecutedOnceWhen(falseCount == 0).ElseNever);
            });
        }

        [Test]
        public void ChecksOrderEqualsConditionsReverseAttachingOrder()
        {
            List<ConditionIndex> checksLog = [];

            _mockOf.Conditions[X, Y, Z].AddLoggingInto(checksLog);
            _mockOf.Conditions[X, Y, Z].SetResults(true);
            _mockOf.Conditions[X, Y, Z].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional)
                .Execute(Arg);

            Assert.That(checksLog, Is.EqualTo(new[] { Z, Y, X }));
        }

        [TestCase("00")]
        [TestCase("10")]
        [TestCase("01")]
        [TestCase("111")]
        [TestCase("101")]
        [TestCase("010")]
        [TestCase("1011100110")]
        [TestCase("0110110001")]
        [TestCase("0101010101")]
        [TestCase("1111111111")]
        [TestCase("0000000000")]
        public void ConditionalHandlersAddedToChainExecuteRelevantHandlers(
            string conditionsSetup)
        {
            var setup = conditionsSetup.Select(ParseBool);
            var conditions = _mockOf.Conditions.Take(setup.Count());
            var handlers = _mockOf.Handlers.Take(setup.Count());

            conditions.SetResults(setup);

            Enumerable
                .Zip(handlers.Objects, conditions.Objects, _math.Conditional)
                .Aggregate(_math.FirstThenSecond)
                .Execute(Arg);

            Assert.Multiple(() =>
            {
                Assert.That(conditions.EachWasCheckedOnce());
                Assert.That(handlers.VerifyExecution(setup));
            });
        }

        [TestCaseSource(nameof(Cases3))]
        public void JoinFirstWithSecondCreatesNewHandlerWithRelevantCondition(Case3 @case)
        {
            _mockOf.ConditionMath.Setup__And(X, Y, returns: Z);

            var expectedCondition = _mockOf.Conditions[@case.ExpectedCondition];
            expectedCondition.SetResult(@case.FinalConditionCheckResult);

            var a = _mockOf.Handlers[A].Object;
            if (@case.AIsConditional)
                a = _math.Conditional(a, _mockOf.Conditions[X].Object);

            var b = _mockOf.Handlers[B].Object;
            if (@case.BIsConditional)
                b = _math.Conditional(b, _mockOf.Conditions[Y].Object);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(Arg);

            Assert.Multiple(() =>
            {
                Assert.That(expectedCondition.WasCheckedOnce());
                Assert.That(_mockOf.Conditions[X, Y, Z].Except([expectedCondition]).NoOneWasChecked());
                Assert.That(_mockOf.Handlers[A, B].EachWasExecutedOnceWhen(@case.HandlersExecutionExpected).ElseNoOne);
            });
        }

        [TestCaseSource(nameof(Cases4))]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(Case4 @case)
        {
            ConditionIndex 
                aTop = new("A TOP"),
                bTop = new("B TOP"),
                aTop_And_bTop = new("A TOP & B TOP"),
                aBottom = new("A BOTTOM"),
                bBottom = new("B BOTTOM"),
                unexpectedAnd = new("UNEXPECTED &");

            var mockOf = new ChainLeadMocks(conditionIndices: 
                [aTop, bTop, aTop_And_bTop, aBottom, bBottom, unexpectedAnd]);

            IHandlerMath math = new HandlerMath(mockOf.ConditionMath.Object);

            mockOf.Conditions[aBottom, bBottom, aTop_And_bTop].SetResults(@case.CheckSetup);

            mockOf.ConditionMath.Setup__And__ForAny(returns: unexpectedAnd);
            mockOf.ConditionMath.Setup__And(aTop, bTop, returns: aTop_And_bTop);

            var a = mockOf.Handlers[A].Object;
            a = math.Conditional(a, mockOf.Conditions[aBottom].Object);
            a = math.Conditional(a, mockOf.Conditions[aTop].Object);

            var b = _mockOf.Handlers[B].Object;
            b = math.Conditional(b, mockOf.Conditions[bBottom].Object);
            b = math.Conditional(b, mockOf.Conditions[bTop].Object);

            var ab = math.JoinFirstWithSecond(a, b);
            ab.Execute(Arg);

            Assert.Multiple(() =>
            {
                Assert.That(mockOf.Conditions[aBottom, bBottom, aTop_And_bTop].VerifyChecks(@case.CheckExpected));
                Assert.That(mockOf.Conditions[unexpectedAnd, aTop, bTop].NoOneWasChecked());
                Assert.That(mockOf.Handlers[A, B].VerifyExecution(@case.ExecutionExpected));
            });
        }

        [TestCaseSource(nameof(Cases5))]
        public void JoinFirstWithSecondConjunctsOnlyTopConditions(Case5 @case)
        {
            _mockOf.Conditions[@case.ChecksSetup.Keys]
                   .SetResults(@case.ChecksSetup.Values);

            _mockOf.ConditionMath.Setup__And__ForAny(returns: Q);

            if (@case.AConditions.Any() &&
                @case.BConditions.Any())
                    _mockOf.ConditionMath.Setup__And(
                        @case.AConditions.Last(),
                        @case.BConditions.Last(),
                        returns: R);

            var a = _mockOf.Conditions[@case.AConditions].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            var b = _mockOf.Conditions[@case.BConditions].Objects
                .Aggregate(_mockOf.Handlers[B].Object, _math.Conditional);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(Arg);

            var expectedToCheck = _mockOf.Conditions[@case.CheckExpected];
            var expectedToExecute = _mockOf.Handlers[@case.ExecuteExpected];

            Assert.Multiple(() =>
            {
                Assert.That(expectedToCheck.EachWasCheckedOnce());
                Assert.That(_mockOf.Conditions.Except(expectedToCheck).NoOneWasChecked());

                Assert.That(expectedToExecute.EachWasExecutedOnce());
                Assert.That(_mockOf.Handlers.Except(expectedToExecute).NoOneWasExecuted());
            });
        }

        [Test]
        public void MergeFirstWithSecondConjunctsAllConditions(
            [ValueSource(nameof(Cases6))] Case6 @case,
            [Values(true, false)] bool finalConditionResult)
        {
            DummyCondition? lastAnd = null;

            _mockOf.ConditionMath
                .Setup(o => o.And(
                    It.IsAny<ICondition<int>>(),
                    It.IsAny<ICondition<int>>()))
                .Returns((ICondition<int> a, ICondition<int> b) =>
                {
                    if (Mock.Get(a) is DummyCondition aMock && 
                        Mock.Get(b) is DummyCondition bMock)
                    {
                        var i = ConditionIndex.Make(aMock.Index.Value + bMock.Index.Value);
                        var and = new DummyCondition(i);
                        lastAnd = and;

                        return and.Object;
                    }

                    return new Mock<ICondition<int>>().Object;
                });

            var a = _mockOf.Conditions[@case.AConditions].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            var b = _mockOf.Conditions[@case.BConditions].Objects
                .Aggregate(_mockOf.Handlers[B].Object, _math.Conditional);

            var ab = _math.MergeFirstWithSecond(a, b);

            Assert.That(lastAnd, Is.Not.Null);

            var allAnded = lastAnd.Index.Value.Select(ConditionIndex.Make);
            var expected = Enumerable.Concat(
                @case.AConditions,
                @case.BConditions);

            Assert.That(allAnded,
                Is.EquivalentTo(expected));

            lastAnd.SetResult(finalConditionResult);

            ab.Execute(Arg);

            Assert.That(lastAnd.WasCheckedOnce());
            Assert.That(_mockOf.Handlers[A, B].EachWasExecutedOnceWhen(finalConditionResult).ElseNoOne);
        }

        [Test]
        public void JoinSomeWithSingleConditionalHandlerPutsConditionOnTopOfResult(
            [Values(false, true)] bool order,
            [Values(false, true)] bool checkResult)
        {
            _mockOf.Conditions[X].SetResult(checkResult);

            var x = _math.Conditional(
                _mockOf.Handlers[A].Object,
                _mockOf.Conditions[X].Object);

            var y = _mockOf.Handlers[B].Object;

            var z = order
                ? _math.MergeFirstWithSecond(x, y)
                : _math.MergeFirstWithSecond(y, x);

            z.Execute(Arg);

            Assert.Multiple(() =>
            {
                Assert.That(_mockOf.Conditions[X].WasCheckedOnce());
                Assert.That(_mockOf.Handlers[A, B].EachWasExecutedOnceWhen(checkResult).ElseNoOne);
            });
        }

        [TestCaseSource(nameof(Cases7))]
        public void CorrectConditionsCascadeTest(Case7 @case)
        {
            List<DummyIndex> execution = [];

            _mockOf.Conditions[@case.AConditions].AddLoggingInto(execution);
            _mockOf.Conditions[@case.BConditions].AddLoggingInto(execution);
            _mockOf.Handlers[A, B].AddLoggingInto(execution);

            _mockOf.Conditions[@case.ChecksSetup.Keys]
                   .SetResults(@case.ChecksSetup.Values);

            var a = _mockOf.Conditions[@case.AConditions].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            var b = _mockOf.Conditions[@case.BConditions].Objects
                .Aggregate(_mockOf.Handlers[B].Object, _math.Conditional);

            var append = AppendFunc<int>(by: @case.AppendType);
            var ab = append(a, b);
            ab.Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(@case.ExpectedCallsOrder));
        }

        private void AtomizeZeroMakeHandlerThatIsNotZero<T>()
        {
            var zero = _math.Zero<T>();
            zero = _math.Atomize(zero);
            var isZero = _math.IsZero(zero);

            Assert.That(isZero, Is.False);
        }

        [Test]
        public void AtomizeZeroMakeHandlerThatIsNotZero__Int() =>
            AtomizeZeroMakeHandlerThatIsNotZero<int>();

        [Test]
        public void AtomizeZeroMakeHandlerThatIsNotZero__String() =>
            AtomizeZeroMakeHandlerThatIsNotZero<string>();

        [Test]
        public void AtomizeZeroMakeHandlerThatIsNotZero__Base() =>
            AtomizeZeroMakeHandlerThatIsNotZero<IBase>();

        [Test]
        public void AtomizeZeroMakeHandlerThatIsNotZero__Derived() =>
            AtomizeZeroMakeHandlerThatIsNotZero<IDerived>();

        [Test]
        public void AppendWithAtomizedConditionalHandlerIsTheSameAsWithRegularHandler(
            [ValueSource(typeof(Appends), nameof(All))] string appendType,
            [Values(true, false)] bool reverseHandlersOrder,
            [Values(true, false)] bool lastConditionCheckResult)
        {
            List<DummyIndex> expectedLog = 
                (reverseHandlersOrder, lastConditionCheckResult) switch
                {
                    (false, false) => [A, X, Y, Z],
                    (false, true)  => [A, X, Y, Z, B],
                    (true,  false) => [X, Y, Z, B],
                    (true,  true)  => [X, Y, Z, A, B]
                };

            List<DummyIndex> executionLog = [];

            _mockOf.Conditions.AddLoggingInto(executionLog); 
            _mockOf.Handlers.AddLoggingInto(executionLog);

            var atom = _mockOf.Handlers[A].Object;
            var conditional = _mockOf.Conditions[Z, Y, X].Objects 
                .Aggregate(_mockOf.Handlers[B].Object, _math.Conditional);
            
            var atomizedConditional = _math.Atomize(conditional);

            var append = AppendFunc<int>(by: appendType);
            var (first, second) = reverseHandlersOrder
                ? (atomizedConditional, atom)
                : (atom, atomizedConditional);

            var result = append(first, second);

            result.Execute(Arg);

            Assert.That(executionLog,
                Is.EqualTo(expectedLog));
        }

        [Test]
        public void AppendedTwoAtomizedConditionalHandlers__IsTheSameAs__AppendedTwoRegularHandlers(
            [ValueSource(typeof(Appends), nameof(All))] string appendType,
            [Values(false, true)] bool aBottomCheckResult,
            [Values(false, true)] bool bBottomCheckResult)
        {
            DummyIndex[] expectedLog =
                (aBottomCheckResult, bBottomCheckResult) switch
                {
                    (false, false) => [U, V, W, X, Y, Z],
                    (false, true)  => [U, V, W, X, Y, Z, B],
                    (true,  false) => [U, V, W, A, X, Y, Z], 
                    (true,  true)  => [U, V, W, A, X, Y, Z, B] 
                };

            List<DummyIndex> execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);
            _mockOf.Conditions[U, V, W, X, Y, Z].AddLoggingInto(execution);

            _mockOf.Conditions[W].SetResult(aBottomCheckResult);
            _mockOf.Conditions[Z].SetResult(bBottomCheckResult);

            var a = _mockOf.Conditions[W, V, U].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            var b = _mockOf.Conditions[Z, Y, X].Objects
                .Aggregate(_mockOf.Handlers[B].Object, _math.Conditional);

            var append = AppendFunc<int>(by: appendType);
            var result = append(a, b);
            result.Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(expectedLog));
        }

        [Test]
        public void ExtendedZeroIsZero()
        {
            var extended = new Mock<IExtendedHandler<int>>();
            extended
                .Setup(o => o.Origin)
                .Returns(_math.Zero<int>());

            Assert.That(_math.IsZero(extended.Object));
        }

        [Test]
        public void ExtendedNotZeroIsNotZero()
        {
            var extended = new Mock<IExtendedHandler<int>>();
            extended
                .Setup(o => o.Origin)
                .Returns(_mockOf.Handlers[A].Object);

            Assert.That(_math.IsZero(extended.Object));
        }

        static bool ParseBool(char mask) => mask == '1';

        static bool NotNull(object? x) => x != null;

        static T Denullify<T>(T? x) where T : class => x!;
    }
}