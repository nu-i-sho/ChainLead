namespace ChainLead.Test
{
    using ChainLead.Contracts;

    using Moq;
    using NUnit.Framework.Internal;
    using System;
    using System.Linq;

    using static ChainLead.Test.Utils;
    using static ChainLead.Test.Utils.Appends;
    using static ChainLead.Test.HandlerMathTest;
    using static ChainLead.Test.Dummy.ConditionIndex.Common;
    using static ChainLead.Test.Dummy.HandlerIndex.Common;

    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    [_IX_][_X_][_XI_][_XII_][_XIII_][_XIV_][_XV_][_XVI_]
    [_XVII_][_XVIII_][_XIX_][_XX_][_XXI_][_XXII_][_XXIII_][_XXIV_]
    [_XXV_][_XXVI_][_XXVII_][_XVIII_][_XXIX_][_XXX_][_XXXI_][_XXXII_]
    public partial class HandlerMathTest<T>(string mathFactoryName)
    {
        public interface IBase;
        public interface IDerived : IBase;

        IHandlerMath _math;
        Dummy.Container<T> _dummyOf;
        AppendProvider<T> _appendProvider;
        T _token;

        AppendProvider<T> Do => _appendProvider;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.Get<T>(798);
            _dummyOf = new Dummy.Container<T>(_token);
            _math = HandlerMathFactoryProvider
                .Get(mathFactoryName)
                .Create(_dummyOf.ConditionMath.Object);
            
            _appendProvider = new(_math);
        }

        [Test]
        public void ZeroAppendZeroIsZero(
            [ValueSource(typeof(Appends), nameof(All))] string append)
        {
            var chain = Do[append](
                _math.Zero<T>(), 
                _math.Zero<T>());
            
            Assert.That(_math.IsZero(chain));
        }

        //[Test]
        //public void BaseZeroAppendDerivedZeroIsDerivedZero(
        //    [ValueSource(typeof(Appends), nameof(All))] string append)
        //{
        //    var chain = Do[append](
        //        _math.Zero<object>(), 
        //        _math.Zero<T>());
            
        //    Assert.That(_math.IsZero(chain));
        //}

        //[Test]
        //public void DerivedZeroAppendBaseZeroChainIsDerivedZero(
        //    [ValueSource(typeof(Appends), nameof(All))] string append)
        //{
        //    var chain = Do[append](
        //        _math.Zero<T>(), 
        //        _math.Zero<object>());
            
        //    Assert.That(_math.IsZero(chain));
        //}

        [Test]
        public void AppendIsNotCommutative(
            [ValueSource(typeof(Appends), nameof(All))] string append)
        {

            List<Dummy.HandlerIndex> 
                abExecution = [],
                baExecution = [],
                execution = [];

            _dummyOf.Handlers[A, B].AddLoggingInto(execution);

            var ab = Do[append](_dummyOf.Handlers[A], _dummyOf.Handlers[B]);
            var ba = Do[append](_dummyOf.Handlers[B], _dummyOf.Handlers[A]);

            ab.Execute(_token);
            abExecution.AddRange(execution);

            execution.Clear();

            ba.Execute(_token);
            baExecution.AddRange(execution);

            Assert.That(abExecution,
                Is.Not.EqualTo(baExecution));
        }

        [Test]
        public void AppendIsNotIdempotent(
            [ValueSource(typeof(Appends), nameof(All))] string append,
            [Values(2, 3, 4, 5, 100)] int count)
        {
            Enumerable
                .Repeat(_dummyOf.Handlers[A], count)
                .Aggregate(Do[append])
                .Execute(_token);

            Assert.That(_dummyOf.Handlers[A].WasExecuted(count).Times);
        }

        [Test]
        public void UnconditionalChainIsAssociative(
            [ValueSource(typeof(Appends), nameof(All))] string append)
        {
            List<Dummy.HandlerIndex> 
                ab_cExecution = [],
                a_bcExecution = [],
                execution = [];

            _dummyOf.Handlers[A, B, C].AddLoggingInto(execution);

            IHandler<T> 
                a = _dummyOf.Handlers[A],
                b = _dummyOf.Handlers[B],
                c = _dummyOf.Handlers[C],
                
                ab_c = Do[append](Do[append](a, b), c),
                a_bc = Do[append](a, Do[append](b, c));
            
            ab_c.Execute(_token);
            ab_cExecution.AddRange(execution);

            execution.Clear();

            a_bc.Execute(_token);
            a_bcExecution.AddRange(execution);

            Assert.That(ab_cExecution,
                Is.EqualTo(a_bcExecution));
        }

        [Test]
        public void ChainExecutesHandlersOneByOne(
            [ValueSource(typeof(Appends), nameof(All))] string append,
            [ValueSource(typeof(HandlerMathTest), nameof(Cases1))] Case1 @case)
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[@case.ChainIndices.Distinct()]
                .AddLoggingInto(execution); 

            var chain = @case.ChainIndices
                .Select(_dummyOf.Handlers.Get)
                .Aggregate(Do[append]);

            chain.Execute(_token);

            Assert.That(execution,
                Is.EqualTo(@case.ChainIndices));
        }

        [Test]
        public void ChainExecutesHandlersOneByOneAndIgnoresZeros(
            [ValueSource(typeof(Appends), nameof(All))] string append,
            [ValueSource(typeof(HandlerMathTest), nameof(Cases2))] Case2 @case)
        {
            List<Dummy.HandlerIndex> execution = [];

            var uniqueIndices = @case.ChainIndicesWithNullsAsZeros
                 .Where(NotNull).Select(Denullify).Distinct();

            _dummyOf.Handlers[uniqueIndices]
                 .AddLoggingInto(execution);

            @case.ChainIndicesWithNullsAsZeros
                 .Select(i => i != null
                     ? _dummyOf.Handlers[i]
                     : _math.Zero<T>())
                 .Aggregate(Do[append])
                 .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(@case.ExpectedExecution));
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
            var conditions = _dummyOf.Conditions.Take(setup.Count());
            var handlers = _dummyOf.Handlers.Take(setup.Count());

            conditions.SetResults(setup);

            Enumerable
                .Zip(handlers, conditions, _math.Conditional)
                .Aggregate(_math.FirstThenSecond)
                .Execute(_token);

            Assert.That(conditions.EachWasCheckedOnce());
            Assert.That(handlers.VerifyExecution(setup));
        }

        [TestCaseSource(typeof(HandlerMathTest), nameof(Cases3))]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(Case3 @case)
        {
            _dummyOf.ConditionMath.Setup__And(X, Y, returns: Z);

            var expectedCondition = _dummyOf.Conditions[@case.ExpectedFinalCondition];
            expectedCondition.SetResult(@case.FinalConditionCheckResult);

            IHandler<T> a = _dummyOf.Handlers[A];
            if (@case.AIsConditional)
                a = _math.Conditional(a, _dummyOf.Conditions[X]);

            IHandler<T> b = _dummyOf.Handlers[B];
            if (@case.BIsConditional)
                b = _math.Conditional(b, _dummyOf.Conditions[Y]);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(_token);

            Assert.That(expectedCondition.WasCheckedOnce());
            Assert.That(_dummyOf.Conditions[X, Y, Z].Except([expectedCondition]).NoOneWasChecked());
            Assert.That(_dummyOf.Handlers[A, B].EachWasExecutedOnceWhen(@case.FinalConditionCheckResult).ElseNoOne);
        }

        [TestCaseSource(typeof(HandlerMathTest), nameof(Cases4))]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(Case4 @case)
        {
            Dummy.ConditionIndex 
                aTop = new("A TOP"),
                bTop = new("B TOP"),
                aTop_And_bTop = new("A TOP & B TOP"),
                aBottom = new("A BOTTOM"),
                bBottom = new("B BOTTOM"),
                unexpectedAnd = new("UNEXPECTED &");

            _dummyOf.Conditions.GenerateMore(
                aTop, bTop, aTop_And_bTop, 
                aBottom, bBottom, unexpectedAnd);

            _dummyOf.Conditions[aBottom, bBottom, aTop_And_bTop].SetResults(@case.CheckSetup);

            _dummyOf.ConditionMath.Setup__And__ForAny(returns: unexpectedAnd);
            _dummyOf.ConditionMath.Setup__And(aTop, bTop, returns: aTop_And_bTop);

            IHandler<T> a = _dummyOf.Handlers[A];
            a = _math.Conditional(a, _dummyOf.Conditions[aBottom]);
            a = _math.Conditional(a, _dummyOf.Conditions[aTop]);

            IHandler<T> b = _dummyOf.Handlers[B];
            b = _math.Conditional(b, _dummyOf.Conditions[bBottom]);
            b = _math.Conditional(b, _dummyOf.Conditions[bTop]);

            var ab = _math.JoinFirstWithSecond(a, b);
            ab.Execute(_token);

            Assert.That(_dummyOf.Conditions[aBottom, bBottom, aTop_And_bTop].VerifyChecks(@case.CheckExpected));
            Assert.That(_dummyOf.Conditions[unexpectedAnd, aTop, bTop].NoOneWasChecked());
            Assert.That(_dummyOf.Handlers[A, B].VerifyExecution(@case.ExecutionExpected));
        }

        [TestCaseSource(typeof(HandlerMathTest), nameof(Cases5))]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(Case5 @case)
        {
            _dummyOf.Conditions[@case.ChecksSetup.Keys]
                    .SetResults(@case.ChecksSetup.Values);

            _dummyOf.ConditionMath.Setup__And__ForAny(returns: Q);

            if (@case.AConditions.Any() &&
                @case.BConditions.Any())
                    _dummyOf.ConditionMath.Setup__And(
                        @case.AConditions.Last(),
                        @case.BConditions.Last(),
                        returns: R);

            var a = _dummyOf.Conditions[@case.AConditions]
                .Aggregate(_dummyOf.Handlers[A].Pure, _math.Conditional);

            var b = _dummyOf.Conditions[@case.BConditions]
                .Aggregate(_dummyOf.Handlers[B].Pure, _math.Conditional);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(_token);

            var expectedToCheck = _dummyOf.Conditions[@case.CheckExpected];
            var expectedToExecute = _dummyOf.Handlers[@case.ExecuteExpected];

            Assert.That(expectedToCheck.EachWasCheckedOnce());
            Assert.That(_dummyOf.Conditions.Except(expectedToCheck).NoOneWasChecked());

            Assert.That(expectedToExecute.EachWasExecutedOnce());
            Assert.That(_dummyOf.Handlers.Except(expectedToExecute).NoOneWasExecuted());
        }

        [Test]
        public void MergeFirstWithSecond__ConjunctsAllConditions(
            [ValueSource(typeof(HandlerMathTest), nameof(Cases6))] Case6 @case,
            [Values(true, false)] bool finalConditionResult)
        {
            Dummy.Condition<T>? lastAnd = null;

            _dummyOf.ConditionMath
                .Setup(o => o.And(
                    It.IsAny<ICondition<T>>(),
                    It.IsAny<ICondition<T>>()))
                .Returns((ICondition<T> a, ICondition<T> b) =>
                {
                    if (a is IDummy<Dummy.ConditionIndex> aDummy && 
                        b is IDummy<Dummy.ConditionIndex> bDummy)
                    {
                        Dummy.ConditionIndex i = new(aDummy.Index.Value + bDummy.Index.Value);
                        Dummy.Condition<T> and = new(i, _token);
                        lastAnd = and;

                        return and;
                    }

                    return new Mock<ICondition<T>>().Object;
                });

            var a = _dummyOf.Conditions[@case.AConditions]
                .Aggregate(_dummyOf.Handlers[A].Pure, _math.Conditional);

            var b = _dummyOf.Conditions[@case.BConditions]
                .Aggregate(_dummyOf.Handlers[B].Pure, _math.Conditional);

            var ab = _math.MergeFirstWithSecond(a, b);

            Assert.That(lastAnd, Is.Not.Null);

            var allAnded = lastAnd.Index.Value.Select(Dummy.ConditionIndex.Make);
            var expected = Enumerable.Concat(
                @case.AConditions,
                @case.BConditions);

            Assert.That(allAnded,
                Is.EquivalentTo(expected));

            lastAnd.SetResult(finalConditionResult);

            ab.Execute(_token);

            Assert.That(lastAnd.WasCheckedOnce());
            Assert.That(_dummyOf.Handlers[A, B]
                .EachWasExecutedOnceWhen(finalConditionResult)
                .ElseNoOne);
        }

        [Test]
        public void JoinSomeWithSingleConditionalHandler__PutsConditionOnTopOfResult(
            [Values(false, true)] bool order,
            [Values(false, true)] bool checkResult)
        {
            _dummyOf.Conditions[X].SetResult(checkResult);

            IHandler<T>
                first = _dummyOf.Handlers[B],
                second = _math.Conditional(_dummyOf.Handlers[A], _dummyOf.Conditions[X]);

            (first, second) = order
                ? (first, second)
                : (second, first);

            _math.MergeFirstWithSecond(first, second)
                 .Execute(_token);

            Assert.That(_dummyOf.Conditions[X].WasCheckedOnce());
            Assert.That(_dummyOf.Handlers[A, B].EachWasExecutedOnceWhen(checkResult).ElseNoOne);
        }

        [TestCaseSource(typeof(HandlerMathTest), nameof(Cases7))]
        public void CorrectConditionsCascadeTest(Case7 @case)
        {
            List<Dummy.Index> execution = [];

            _dummyOf.Conditions[@case.AConditions].AddLoggingInto(execution);
            _dummyOf.Conditions[@case.BConditions].AddLoggingInto(execution);
            _dummyOf.Handlers[A, B].AddLoggingInto(execution);

            _dummyOf.Conditions[@case.ChecksSetup.Keys]
                   .SetResults(@case.ChecksSetup.Values);

            var a = _dummyOf.Conditions[@case.AConditions]
                .Aggregate(_dummyOf.Handlers[A].Pure, _math.Conditional);

            var b = _dummyOf.Conditions[@case.BConditions]
                .Aggregate(_dummyOf.Handlers[B].Pure, _math.Conditional);

            Do[@case.Append](a, b).Execute(_token);

            Assert.That(execution,
                Is.EqualTo(@case.ExpectedCallsOrder));
        }

        [Test]
        public void AppendWithAtomizedConditionalHandler__IsTheSameAs__WithRegularHandler(
            [ValueSource(typeof(Appends), nameof(All))] string append,
            [Values(false, true)] bool isFirstArgument,
            [Values(false, true)] bool bottomCheckResult)
        {
            Dummy.HandlerIndex
                atomized = new("ATOMIZED"),
                atom = new("ATOM");

            _dummyOf.Handlers.GenerateMore(atomized, atom);

            List<Dummy.Index>
                executionLog = [],
                expectedLog = 
                    (isFirstArgument, bottomCheckResult) switch
                    {
                        (false, false) => [atom, X, Y, Z],
                        (false, true)  => [atom, X, Y, Z, atomized],
                        (true,  false) => [X, Y, Z, atom],
                        (true,  true)  => [X, Y, Z, atomized, atom]
                    };

            _dummyOf.Handlers.AddLoggingInto(executionLog);
            _dummyOf.Conditions.AddLoggingInto(executionLog); 

            _dummyOf.Conditions[X, Y].SetResults(true);
            _dummyOf.Conditions[Z].SetResult(bottomCheckResult);

            IHandler<T>
                first =  _dummyOf.Handlers[atomized],
                second = _dummyOf.Handlers[atom];

            first = _dummyOf.Conditions[Z, Y, X].Aggregate(first, _math.Conditional);
            first = _math.Atomize(first);

            (first, second) = isFirstArgument
                ? (first, second)
                : (second, first);

            Do[append](first, second).Execute(_token);

            Assert.That(executionLog,
                Is.EqualTo(expectedLog));
        }

        [Test]
        public void AppendedTwoAtomizedConditionalHandlers__IsTheSameAs__AppendedTwoRegularHandlers(
            [ValueSource(typeof(Appends), nameof(All))] string append,
            [Values(false, true)] bool aBottomCheckResult,
            [Values(false, true)] bool bBottomCheckResult)
        {
            List<Dummy.Index>
                executionLog = [],
                expectedLog =
                    (aBottomCheckResult, bBottomCheckResult) switch
                    {
                        (false, false) => [U, V, W,    X, Y, Z   ],
                        (false, true)  => [U, V, W,    X, Y, Z, B],
                        (true,  false) => [U, V, W, A, X, Y, Z   ], 
                        (true,  true)  => [U, V, W, A, X, Y, Z, B] 
                    };

            _dummyOf.Handlers.AddLoggingInto(executionLog);
            _dummyOf.Conditions.AddLoggingInto(executionLog);

            _dummyOf.Conditions[U, V, X, Y].SetResults(true);
            _dummyOf.Conditions[W].SetResult(aBottomCheckResult);
            _dummyOf.Conditions[Z].SetResult(bBottomCheckResult);

            IHandler<T> a = _dummyOf.Handlers[A];
            a = _dummyOf.Conditions[W, V, U].Aggregate(a, _math.Conditional);
            a = _math.Atomize(a);

            IHandler<T> b = _dummyOf.Handlers[B];
            b = _dummyOf.Conditions[Z, Y, X].Aggregate(b, _math.Conditional);
            b = _math.Atomize(b);

            Do[append](a, b).Execute(_token);

            Assert.That(executionLog,
                Is.EqualTo(expectedLog));
        }

        static bool NotNull(object? x) => x != null;

        static T Denullify<T>(T? x) where T : class => x!;
    }
}