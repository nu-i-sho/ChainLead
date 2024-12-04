namespace ChainLead.Test
{
    using ChainLead.Contracts;
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

        const int Arg = 7643;

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
        public void ZeroIsZero()
        {
            var isZero = _math.IsZero(_math.Zero<int>());
            Assert.That(isZero, Is.True);
        }

        [Test]
        public void ZeroForBaseClassIsZeroForDerivedClass()
        {
            var isZero = _math.IsZero<IDerived>(_math.Zero<IBase>());
            Assert.That(isZero, Is.True);
        }

        [Test]
        public void ZeroAppendZeroIsZero(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);
            var zeros = new[] { _math.Zero<int>(), _math.Zero<int>() };
            var zeroZero = append(zeros[0], zeros[1]);
            var isZero = _math.IsZero(zeroZero);

            Assert.That(isZero, Is.True);
        }

        [Test]
        public void BaseZeroAppendDerivedZeroIsDerivedZero(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<IDerived>(by: appendType);
            var chain = append(_math.Zero<IBase>(), _math.Zero<IDerived>());
            var isZero = _math.IsZero(chain);

            Assert.That(isZero, Is.True);
        }

        [Test]
        public void DerivedZeroAppendBaseZeroChainIsDerivedZero(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<IDerived>(by: appendType);
            var chain = append(_math.Zero<IDerived>(), _math.Zero<IBase>());
            var isZero = _math.IsZero(chain);

            Assert.That(isZero, Is.True);
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

            _mockOf.Handlers[A, B].Setup__Execute__LoggingInto(execution);
            
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

            _mockOf.Handlers[A].Verify__Execute(Times.Exactly(count));
        }

        [Test]
        public void UnconditionalChainIsAssociative(
            [ValueSource(typeof(Appends), nameof(All))] string appendType)
        {
            var append = AppendFunc<int>(by: appendType);
            List<HandlerIndex> ab_cExecution = [];
            List<HandlerIndex> a_bcExecution = [];
            List<HandlerIndex> execution = [];

            _mockOf.Handlers[A, B, C].Setup__Execute__LoggingInto(execution);

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
                .Setup__Execute__LoggingInto(execution); 

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
                 .Setup__Execute__LoggingInto(execution);

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
            
            Assert.That(_math.IsZero(conditionalZero), 
                Is.True);
        }

        [Test]
        public void WhenConditionReturnsTrue__HandlerIsExecuted()
        {
            _mockOf.Conditions[X].Setup__Check(returns: true);
            
            _math.Conditional(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Conditions[X].Object)
                 .Execute(Arg);

            _mockOf.Handlers[A].Verify__Execute(Times.Once);
        }

        [Test]
        public void WhenConditionReturnsFalse__HandlerIsNotExecuted()
        {
            _mockOf.Conditions[X].Setup__Check(returns: false);

            _math.Conditional(
                    _mockOf.Handlers[A].Object,
                    _mockOf.Conditions[X].Object)
                 .Execute(Arg);

            _mockOf.Handlers[A].Verify__Execute(Times.Never);
        }

        [Test]
        public void WhenTopConditionReturnsFalse__AllOtherChecksAndExecutionsAreNotCalled()
        {
            _mockOf.Conditions[Z].Setup__Check(returns: false);

            var conditional = _mockOf.Conditions[X, Y, Z].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            conditional.Execute(Arg);

            _mockOf.Conditions[Z].Verify__Check(Times.Once);
            _mockOf.Conditions[X, Y].Verify__Check(Times.Never);
            _mockOf.Handlers[A].Verify__Execute(Times.Never);
        }

        [Test]
        public void ChecksAllConditionsUpToFirstFalse(
            [Values(0, 1, 2, 5)] int trueCount,
            [Values(0, 1, 2, 5)] int falseCount)
        {
            var trues = _mockOf.Conditions.Take(trueCount);
            var falses = _mockOf.Conditions.Skip(trueCount).Take(falseCount);

            trues.Setup__Check(returns: true);
            falses.Setup__Check(returns: false);

            var all = falses.Concat(trues);
            var conditional = all.Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            conditional.Execute(Arg);

            var checkedCount = trueCount + int.Min(1, falseCount);

            all.Reverse().Take(checkedCount).Verify__Check(Times.Once);
            all.Reverse().Skip(checkedCount).Verify__Check(Times.Never);
            _mockOf.Handlers[A].Verify__Execute(OnceWhen(falseCount == 0));
        }

        [Test]
        public void ChecksOrderEqualsConditionsReverseAttachingOrder()
        {
            List<ConditionIndex> checksLog = [];

            _mockOf.Conditions[X, Y, Z].Setup__Check__LoggingInto(checksLog);  
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

            conditions.Setup__Check(returns: setup);

            Enumerable
                .Zip(handlers.Objects, conditions.Objects, _math.Conditional)
                .Aggregate(_math.FirstThenSecond)
                .Execute(Arg);

            conditions.Verify__Check(Times.Once);
            handlers.Verify__Execute(EachWhen(setup));
        }

        [TestCaseSource(nameof(Cases3))]
        public void JoinFirstWithSecondCreatesNewHandlerWithRelevantCondition(Case3 @case)
        {
            _mockOf.ConditionMath.Setup__And(X, Y, returns: Z);

            var expectedCondition = _mockOf.Conditions[@case.ExpectedCondition];
            expectedCondition.Setup__Check(@case.FinalConditionCheckResult);

            var a = _mockOf.Handlers[A].Object;
            if (@case.AIsConditional)
                a = _math.Conditional(a, _mockOf.Conditions[X].Object);

            var b = _mockOf.Handlers[B].Object;
            if (@case.BIsConditional)
                b = _math.Conditional(b, _mockOf.Conditions[Y].Object);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(Arg);

            expectedCondition.Verify__Check(Times.Once);
            _mockOf.Conditions[X, Y, Z].Except([expectedCondition]).Verify__Check(Times.Never);
            _mockOf.Handlers[A, B].Verify__Execute(OnceWhen(@case.HandlersExecutionExpected));
        }

        [TestCaseSource(nameof(Cases4))]
        public void JoinFirstWithSecondConjunctsOnlyTopConditions(Case4 @case)
        {
            var unexpectedAnd = W;
            var aTop = U;
            var bTop = V;
            var aTop_And_bTop = Z;
            var aBottom = X;
            var bBottom = Y;

            _mockOf.Conditions[aBottom, bBottom, aTop_And_bTop]
                .Setup__Check(@case.CheckSetup);

            _mockOf.ConditionMath.Setup__And__ForAny(returns: unexpectedAnd);
            _mockOf.ConditionMath.Setup__And(aTop, bTop, returns: aTop_And_bTop);

            var a = _mockOf.Handlers[A].Object;
            a = _math.Conditional(a, _mockOf.Conditions[aBottom].Object);
            a = _math.Conditional(a, _mockOf.Conditions[aTop].Object);

            var b = _mockOf.Handlers[B].Object;
            b = _math.Conditional(b, _mockOf.Conditions[bBottom].Object);
            b = _math.Conditional(b, _mockOf.Conditions[bTop].Object);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(Arg);

            _mockOf.Conditions[aBottom, bBottom, aTop_And_bTop].Verify__Check(EachWhen(@case.CheckExpected));
            _mockOf.Conditions[unexpectedAnd, aTop, bTop].Verify__Check(Times.Never);
            _mockOf.Handlers[A, B].Verify__Execute(EachWhen(@case.ExecutionExpected));
        }

        [TestCaseSource(nameof(Cases5))]
        public void JoinFirstWithSecondConjunctsOnlyTopConditions(Case5 @case)
        {
            _mockOf.Conditions[@case.ChecksSetup.Keys]
                 .Setup__Check(@case.ChecksSetup.Values);

            _mockOf.ConditionMath.Setup__And__ForAny(returns: Q);

            if (@case.AConditions.Any() &&
                @case.BConditions.Any())
                    _mockOf.ConditionMath.Setup__And(
                        @case.AConditions.Last(),
                        @case.BConditions.Last(),
                        returns: R);

            var a = _mockOf.Conditions[@case.AConditions].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            var b = _mockOf.Conditions[@case.AConditions].Objects
                .Aggregate(_mockOf.Handlers[B].Object, _math.Conditional);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(Arg);

            var expectedToCheck = _mockOf.Conditions[@case.CheckExpected];
            var expectedToExecute = _mockOf.Handlers[@case.ExecuteExpected];

            expectedToCheck.Verify__Check(Times.Once);
            _mockOf.Conditions.Except(expectedToCheck).Verify__Check(Times.Never);

            expectedToExecute.Verify__Execute(Times.Once);
            _mockOf.Handlers.Except(expectedToExecute).Verify__Execute(Times.Never);
        }

        [Test]
        public void MergeFirstWithSecondConjunctsAllConditions(
            [ValueSource(nameof(Cases6))] Case6 @case,
            [Values(true, false)] bool finalConditionResult)
        {
            ConditionMock? lastAnd = null;

            _mockOf.ConditionMath
                .Setup(o => o.And(
                    It.IsAny<ICondition<int>>(),
                    It.IsAny<ICondition<int>>()))
                .Returns((ICondition<int> a, ICondition<int> b) =>
                {
                    var aMock = Mock.Get(a) as ConditionMock;
                    var bMock = Mock.Get(b) as ConditionMock;

                    if (aMock != null && bMock != null)
                    {
                        var i = ConditionIndex.Make(aMock.Index.Value + bMock.Index.Value);
                        var and = new ConditionMock(i);
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

            lastAnd.Setup__Check(returns: finalConditionResult);

            ab.Execute(Arg);

            lastAnd.Verify__Check(Times.Once);
            _mockOf.Handlers[A, B].Verify__Execute(
                OnceWhen(finalConditionResult));
        }

        [Test]
        public void JoinSomeWithSingleConditionalHandlerPutsConditionOnTopOfResult(
            [Values(false, true)] bool order,
            [Values(false, true)] bool checkResult)
        {
            _mockOf.Conditions[X].Setup__Check(checkResult);

            var x = _math.Conditional(
                _mockOf.Handlers[A].Object,
                _mockOf.Conditions[X].Object);

            var y = _mockOf.Handlers[B].Object;

            var z = order
                ? _math.MergeFirstWithSecond(x, y)
                : _math.MergeFirstWithSecond(y, x);

            z.Execute(Arg);

            _mockOf.Conditions[X].Verify__Check(Times.Once);
            _mockOf.Handlers[A, B].Verify__Execute(OnceWhen(checkResult));
        }

        [TestCaseSource(nameof(Cases7))]
        public void CorrectConditionsCascadeTest(Case7 @case)
        {
            List<MockIndex> execution = [];

            _mockOf.Conditions[@case.AConditions].Setup__Check__LoggingInto(execution);
            _mockOf.Conditions[@case.BConditions].Setup__Check__LoggingInto(execution);
            _mockOf.Handlers[A, B].Setup__Execute__LoggingInto(execution);

            _mockOf.Conditions[@case.ChecksSetup.Keys]
                .Setup__Check(returns: @case.ChecksSetup.Values);

            var a = _mockOf.Conditions[@case.AConditions].Objects
                .Aggregate(_mockOf.Handlers[A].Object, _math.Conditional);

            var b = _mockOf.Conditions[@case.AConditions].Objects
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
            List<MockIndex> expectedLog = 
                (reverseHandlersOrder, lastConditionCheckResult) switch
                {
                    (false, false) => [A, X, Y, Z],
                    (false, true)  => [A, X, Y, Z, B],
                    (true,  false) => [X, Y, Z, B],
                    (true,  true)  => [X, Y, Z, A, B]
                };

            List<MockIndex> executionLog = [];

            _mockOf.Conditions.Setup__Check__LoggingInto(executionLog); 
            _mockOf.Handlers.Setup__Execute__LoggingInto(executionLog);

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
        public void AppendTwoAtomizedConditionalHandlerIsTheSameAsTwoRegularHandlers(
            [ValueSource(typeof(Appends), nameof(All))] string appendType,
            [Values(false, true)] bool aBottomCheckResult,
            [Values(false, true)] bool bBottomCheckResult)
        {
            MockIndex[] expectedLog =
                (aBottomCheckResult, bBottomCheckResult) switch
                {
                    (false, false) => [U, V, W, X, Y, Z],
                    (false, true)  => [U, V, W, X, Y, Z, B],
                    (true,  false) => [U, V, W, A, X, Y, Z], 
                    (true,  true)  => [U, V, W, A, X, Y, Z, B] 
                };

            List<MockIndex> execution = [];

            _mockOf.Handlers[A, B].Setup__Execute__LoggingInto(execution);
            _mockOf.Conditions[U, V, W, X, Y, Z].Setup__Check__LoggingInto(execution);

            _mockOf.Conditions[W].Setup__Check(returns: aBottomCheckResult);
            _mockOf.Conditions[Z].Setup__Check(returns: bBottomCheckResult);

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

            var isZero = _math.IsZero(extended.Object);

            Assert.That(isZero, Is.True);
        }

        [Test]
        public void ExtendedNotZeroIsNotZero()
        {
            var extended = new Mock<IExtendedHandler<int>>();
            extended
                .Setup(o => o.Origin)
                .Returns(_mockOf.Handlers[A].Object);

            var isZero = _math.IsZero(extended.Object);

            Assert.That(isZero, Is.False);
        }

        static Func<Times> OnceWhen(bool value) =>
            value ? Times.Once : Times.Never;

        static IEnumerable<Func<Times>> EachWhen(IEnumerable<bool> value) =>
            value.Select(OnceWhen);

        static bool ParseBool(char mask) => mask == '1';

        static bool NotNull(object? x) => x != null;

        static T Denullify<T>(T? x) where T : class => x!;
    }
}