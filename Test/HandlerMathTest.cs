namespace ChainLead.Test
{
    // DO NOT using ChainLead.Contracts.Syntax;

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
            
            List<HandlerIndex> 
                abExecution = [],
                baExecution = [],
                execution = [];

            _mockOf.Handlers[A, B].AddLoggingInto(execution);

            var ab = append(_mockOf.Handlers[A], _mockOf.Handlers[B]);
            var ba = append(_mockOf.Handlers[B], _mockOf.Handlers[A]);

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
                .Repeat(_mockOf.Handlers[A], count)
                .Aggregate(AppendFunc<int>(by: appendType))
                .Execute(Arg);

            Assert.That(_mockOf.Handlers[A].WasExecuted(count).Times);
        }

        [Test]
        public void UnconditionalChainIsAssociative(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);
            
            List<HandlerIndex> 
                ab_cExecution = [],
                a_bcExecution = [],
                execution = [];

            _mockOf.Handlers[A, B, C].AddLoggingInto(execution);

            IHandler<int> 
                a = _mockOf.Handlers[A],
                b = _mockOf.Handlers[B],
                c = _mockOf.Handlers[B],
                
                ab_c = append(append(a, b), c),
                a_bc = append(a, append(b, c));
            
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
                .Select(_mockOf.Handlers.Get)
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
                     ? _mockOf.Handlers[i]
                     : _math.Zero<int>())
                 .Aggregate(AppendFunc<int>(by: appendType))
                 .Execute(Arg);

            Assert.That(execution,
                Is.EqualTo(@case.ExpectedExecution));
        }

        [Test]
        public void ConditionalZeroIsZero()
        {
            var conditionalZero = 
                _math.Conditional(_math.Zero<int>(), _mockOf.Conditions[X]);
            
            Assert.That(_math.IsZero(conditionalZero));
        }

        [Test]
        public void WhenConditionReturnsTrue__HandlerIsExecuted()
        {
            _mockOf.Conditions[X].SetResult(true);
            
            _math.Conditional(
                    _mockOf.Handlers[A],
                    _mockOf.Conditions[X])
                 .Execute(Arg);

            Assert.That(_mockOf.Handlers[A].WasExecutedOnce());
        }

        [Test]
        public void WhenConditionReturnsFalse__HandlerIsNotExecuted()
        {
            _mockOf.Conditions[X].SetResult(false);

            _math.Conditional(
                    _mockOf.Handlers[A],
                    _mockOf.Conditions[X])
                 .Execute(Arg);

            Assert.That(_mockOf.Handlers[A].WasNeverExecuted());
        }

        [Test]
        public void WhenTopConditionReturnsFalse__AllOtherChecksAndExecutionsAreNotCalled()
        {
            _mockOf.Conditions[Z].SetResult(false);

            _mockOf.Conditions[X, Y, Z]
                .Aggregate(_mockOf.Handlers[A].Pure, _math.Conditional)
                .Execute(Arg);


            Assert.That(_mockOf.Conditions[Z].WasCheckedOnce());
            Assert.That(_mockOf.Conditions[X, Y].NoOneWasChecked());
            Assert.That(_mockOf.Handlers[A].WasNeverExecuted());
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
            all.Aggregate(_mockOf.Handlers[A].Pure, _math.Conditional)
               .Execute(Arg);

            var checkedCount = trueCount + int.Min(1, falseCount);

            Assert.That(all.Reverse().Take(checkedCount).EachWasCheckedOnce());
            Assert.That(all.Reverse().Skip(checkedCount).NoOneWasChecked());
            Assert.That(_mockOf.Handlers[A].WasExecutedOnceWhen(falseCount == 0).ElseNever);
        }

        [Test]
        public void ChecksOrderEqualsConditionsReverseAttachingOrder()
        {
            List<ConditionIndex> checksLog = [];

            _mockOf.Conditions[X, Y, Z].AddLoggingInto(checksLog);
            _mockOf.Conditions[X, Y, Z].SetResults(true);
            _mockOf.Conditions[X, Y, Z]
                .Aggregate(_mockOf.Handlers[A].Pure, _math.Conditional)
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
            var setup = conditionsSetup.Select(x => x == '1');
            var conditions = _mockOf.Conditions.Take(setup.Count());
            var handlers = _mockOf.Handlers.Take(setup.Count());

            conditions.SetResults(setup);

            Enumerable
                .Zip(handlers, conditions, _math.Conditional)
                .Aggregate(_math.FirstThenSecond)
                .Execute(Arg);

            Assert.That(conditions.EachWasCheckedOnce());
            Assert.That(handlers.VerifyExecution(setup));
        }

        [TestCaseSource(nameof(Cases3))]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(Case3 @case)
        {
            _mockOf.ConditionMath.Setup__And(X, Y, returns: Z);

            var expectedCondition = _mockOf.Conditions[@case.ExpectedFinalCondition];
            expectedCondition.SetResult(@case.FinalConditionCheckResult);

            IHandler<int> a = _mockOf.Handlers[A];
            if (@case.AIsConditional)
                a = _math.Conditional(a, _mockOf.Conditions[X]);

            IHandler<int> b = _mockOf.Handlers[B];
            if (@case.BIsConditional)
                b = _math.Conditional(b, _mockOf.Conditions[Y]);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(Arg);

            Assert.That(expectedCondition.WasCheckedOnce());
            Assert.That(_mockOf.Conditions[X, Y, Z].Except([expectedCondition]).NoOneWasChecked());
            Assert.That(_mockOf.Handlers[A, B].EachWasExecutedOnceWhen(@case.FinalConditionCheckResult).ElseNoOne);
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

            _mockOf.Conditions.GenerateMore(
                aTop, bTop, aTop_And_bTop, 
                aBottom, bBottom, unexpectedAnd);

            _mockOf.Conditions[aBottom, bBottom, aTop_And_bTop].SetResults(@case.CheckSetup);

            _mockOf.ConditionMath.Setup__And__ForAny(returns: unexpectedAnd);
            _mockOf.ConditionMath.Setup__And(aTop, bTop, returns: aTop_And_bTop);

            IHandler<int> a = _mockOf.Handlers[A];
            a = _math.Conditional(a, _mockOf.Conditions[aBottom]);
            a = _math.Conditional(a, _mockOf.Conditions[aTop]);

            IHandler<int> b = _mockOf.Handlers[B];
            b = _math.Conditional(b, _mockOf.Conditions[bBottom]);
            b = _math.Conditional(b, _mockOf.Conditions[bTop]);

            var ab = _math.JoinFirstWithSecond(a, b);
            ab.Execute(Arg);

            Assert.That(_mockOf.Conditions[aBottom, bBottom, aTop_And_bTop].VerifyChecks(@case.CheckExpected));
            Assert.That(_mockOf.Conditions[unexpectedAnd, aTop, bTop].NoOneWasChecked());
            Assert.That(_mockOf.Handlers[A, B].VerifyExecution(@case.ExecutionExpected));
        }

        [TestCaseSource(nameof(Cases5))]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(Case5 @case)
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

            var a = _mockOf.Conditions[@case.AConditions]
                .Aggregate(_mockOf.Handlers[A].Pure, _math.Conditional);

            var b = _mockOf.Conditions[@case.BConditions]
                .Aggregate(_mockOf.Handlers[B].Pure, _math.Conditional);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(Arg);

            var expectedToCheck = _mockOf.Conditions[@case.CheckExpected];
            var expectedToExecute = _mockOf.Handlers[@case.ExecuteExpected];

            Assert.That(expectedToCheck.EachWasCheckedOnce());
            Assert.That(_mockOf.Conditions.Except(expectedToCheck).NoOneWasChecked());

            Assert.That(expectedToExecute.EachWasExecutedOnce());
            Assert.That(_mockOf.Handlers.Except(expectedToExecute).NoOneWasExecuted());
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
                    if (a is IDummy<ConditionIndex> aDummy && 
                        b is IDummy<ConditionIndex> bDummy)
                    {
                        ConditionIndex i = new(aDummy.Index.Value + bDummy.Index.Value);
                        DummyCondition and = new(i);
                        lastAnd = and;

                        return and;
                    }

                    return new Mock<ICondition<int>>().Object;
                });

            var a = _mockOf.Conditions[@case.AConditions]
                .Aggregate(_mockOf.Handlers[A].Pure, _math.Conditional);

            var b = _mockOf.Conditions[@case.BConditions]
                .Aggregate(_mockOf.Handlers[B].Pure, _math.Conditional);

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

            IHandler<int>
                first = _mockOf.Handlers[B],
                second = _math.Conditional(_mockOf.Handlers[A], _mockOf.Conditions[X]);

            (first, second) = order
                ? (first, second)
                : (second, first);

            _math.MergeFirstWithSecond(first, second)
                 .Execute(Arg);

            Assert.That(_mockOf.Conditions[X].WasCheckedOnce());
            Assert.That(_mockOf.Handlers[A, B].EachWasExecutedOnceWhen(checkResult).ElseNoOne);
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

            var a = _mockOf.Conditions[@case.AConditions]
                .Aggregate(_mockOf.Handlers[A].Pure, _math.Conditional);

            var b = _mockOf.Conditions[@case.BConditions]
                .Aggregate(_mockOf.Handlers[B].Pure, _math.Conditional);

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
        public void AppendWithAtomizedConditionalHandler__IsTheSameAs__WithRegularHandler(
            [ValueSource(typeof(Appends), nameof(All))] string appendType,
            [Values(false, true)] bool isFirstArgument,
            [Values(false, true)] bool bottomCheckResult)
        {
            HandlerIndex
                atomized = new("ATOMIZED"),
                atom = new("ATOM");

            _mockOf.Handlers.GenerateMore(atomized, atom);

            List<DummyIndex>
                executionLog = [],
                expectedLog = 
                    (isFirstArgument, bottomCheckResult) switch
                    {
                        (false, false) => [atom, X, Y, Z],
                        (false, true)  => [atom, X, Y, Z, atomized],
                        (true,  false) => [X, Y, Z, atom],
                        (true,  true)  => [X, Y, Z, atomized, atom]
                    };

            _mockOf.Handlers.AddLoggingInto(executionLog);
            _mockOf.Conditions.AddLoggingInto(executionLog); 

            _mockOf.Conditions[X, Y].SetResults(true);
            _mockOf.Conditions[Z].SetResult(bottomCheckResult);

            IHandler<int>
                first =  _mockOf.Handlers[atomized],
                second = _mockOf.Handlers[atom];

            first = _mockOf.Conditions[Z, Y, X].Aggregate(first, _math.Conditional);
            first = _math.Atomize(first);

            (first, second) = isFirstArgument
                ? (first, second)
                : (second, first);

            var append = AppendFunc<int>(by: appendType);
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
            List<DummyIndex>
                executionLog = [],
                expectedLog =
                    (aBottomCheckResult, bBottomCheckResult) switch
                    {
                        (false, false) => [U, V, W,    X, Y, Z   ],
                        (false, true)  => [U, V, W,    X, Y, Z, B],
                        (true,  false) => [U, V, W, A, X, Y, Z   ], 
                        (true,  true)  => [U, V, W, A, X, Y, Z, B] 
                    };

            _mockOf.Handlers.AddLoggingInto(executionLog);
            _mockOf.Conditions.AddLoggingInto(executionLog);

            _mockOf.Conditions[U, V, X, Y].SetResults(true);
            _mockOf.Conditions[W].SetResult(aBottomCheckResult);
            _mockOf.Conditions[Z].SetResult(bBottomCheckResult);

            IHandler<int> a = _mockOf.Handlers[A];
            a = _mockOf.Conditions[W, V, U].Aggregate(a, _math.Conditional);
            a = _math.Atomize(a);

            IHandler<int> b = _mockOf.Handlers[B];
            b = _mockOf.Conditions[Z, Y, X].Aggregate(b, _math.Conditional);
            b = _math.Atomize(b);

            var append = AppendFunc<int>(by: appendType);
            var result = append(a, b);
            result.Execute(Arg);

            Assert.That(executionLog,
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
                .Returns(_mockOf.Handlers[A]);

            Assert.That(_math.IsZero(extended.Object), 
                Is.False);
        }

        static bool NotNull(object? x) => x != null;

        static T Denullify<T>(T? x) where T : class => x!;
    }
}