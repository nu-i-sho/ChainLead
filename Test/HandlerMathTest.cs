namespace ChainLead.Test
{
    using ChainLead.Contracts;
    
    using Moq;
    using NUnit.Framework.Internal;
    using System;
    using System.Linq;

    using static ChainLead.Test.Cases.Common;
    using static ChainLead.Test.Cases.HandlerMathFixtureCases;
    using static ChainLead.Test.Cases.HandlerMathTestCases;
    using static ChainLead.Test.Dummy.ConditionIndex.Common;
    using static ChainLead.Test.Dummy.HandlerIndex.Common;
    using static ChainLead.Test.Dummy.Index.Common;


    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    [_IX_][_X_][_XI_][_XII_][_XIII_][_XIV_][_XV_][_XVI_]
    [_XVII_][_XVIII_][_XIX_][_XX_][_XXI_][_XXII_][_XXIII_][_XXIV_]
    [_XXV_][_XXVI_][_XXVII_][_XVIII_][_XXIX_][_XXX_][_XXXI_][_XXXII_]
    public partial class HandlerMathTest<T>(
        IHandlerMathFactory mathFactory)
    {
        IHandlerMath _math;
        Dummy.Container<T> _dummyOf;
        Func<string, AppendOf> _do;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.GetRandom<T>();
            _dummyOf = new Dummy.Container<T>(_token);
            _math = mathFactory.Create(_dummyOf.ConditionMath);

            _do = append =>
                  append switch
                  {
                      Appends.FirstThenSecond => new(_math.FirstThenSecond),
                      Appends.PackFirstInSecond => new(_math.PackFirstInSecond),
                      Appends.InjectFirstIntoSecond => new(_math.InjectFirstIntoSecond),
                      Appends.FirstCoverSecond => new(_math.FirstCoverSecond),
                      Appends.FirstWrapSecond => new(_math.FirstWrapSecond),
                      Appends.JoinFirstWithSecond => new(_math.JoinFirstWithSecond),
                      Appends.MergeFirstWithSecond => new(_math.MergeFirstWithSecond),
                      _ => throw new ArgumentOutOfRangeException(nameof(append))
                  };
        }

        [Test]
        public void ZeroAppendZeroIsZero(
            [AllAppends] string append)
        {
            var chain = _do(append).Of(
                _math.Zero<T>(), 
                _math.Zero<T>());
            
            Assert.That(_math.IsZero(chain));
        }

        [Test]
        public void AppendIsNotCommutative(
            [AllAppends] string append)
        {
            List<Dummy.HandlerIndex> 
                abExecution = [],
                baExecution = [],
                execution = [];

            _dummyOf.Handlers[A, B].LogInto(execution);

            IHandler<T>
                a = _dummyOf.Handler(A),
                b = _dummyOf.Handler(B),

                ab = _do(append).Of(a, b),
                ba = _do(append).Of(b, a);

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
            [AllAppends] string append,
            [Values(2, 3, 4, 5, 100)] int count)
        {
            Enumerable
                .Repeat<IHandler<T>>(_dummyOf.Handler(A), count)
                .Aggregate(_do(append).Of)
                .Execute(_token);

            Assert.That(_dummyOf.Handler(A)
                  .WasExecuted(count).Times);
        }

        [Test]
        public void UnconditionalChainIsAssociative(
            [AllAppends] string append)
        {
            List<Dummy.HandlerIndex> 
                ab_cExecution = [],
                a_bcExecution = [],
                execution = [];

            _dummyOf.Handlers[A, B, C].LogInto(execution);

            IHandler<T> 
                a = _dummyOf.Handler(A),
                b = _dummyOf.Handler(B),
                c = _dummyOf.Handler(C),
                
                ab = _do(append).Of(a, b),
                bc = _do(append).Of(b, c),

                ab_c = _do(append).Of(ab, c),
                a_bc = _do(append).Of(a, bc);

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
            [AllAppends] string append,
            [BlueCases] BlueCase @case)
        {
            List<Dummy.HandlerIndex> execution = [];

            _dummyOf.Handlers[@case.ChainIndices.Distinct()]
                .LogInto(execution); 

            var chain = @case.ChainIndices
                .Select(_dummyOf.Handlers.Get).Cast<IHandler<T>>()
                .Aggregate(_do(append).Of);

            chain.Execute(_token);

            Assert.That(execution,
                Is.EqualTo(@case.ChainIndices));
        }

        [Test]
        public void ChainExecutesHandlersOneByOneAndIgnoresZeros(
            [AllAppends] string append,
            [RedCases] RedCase @case)
        {
            List<Dummy.HandlerIndex> execution = [];

            var uniqueIndices = @case.ChainIndicesWithNullsAsZeros
                 .Where(NotNull).Select(Denullify).Distinct();

            _dummyOf.Handlers[uniqueIndices]
                 .LogInto(execution);

            @case.ChainIndicesWithNullsAsZeros
                 .Select(i => i != null
                     ? _dummyOf.Handler(i)
                     : _math.Zero<T>())
                 .Aggregate(_do(append).Of)
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

            conditions.Return(setup);

            Enumerable
                .Zip(handlers, conditions, _math.Conditional)
                .Aggregate(_math.FirstThenSecond)
                .Execute(_token);

            Assert.That(conditions.EachWasCheckedOnce());
            Assert.That(handlers.VerifyExecution(setup));
        }

        [Test]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(
            [GreenCases] GreenCase @case)
        {
            _dummyOf.Conditions.Add(X & Y);
            _dummyOf.ConditionMath.And(X, Y).Returns(X & Y);

            var expectedCondition = _dummyOf.Condition(@case.ExpectedFinalCondition);
            expectedCondition.Returns(@case.FinalConditionCheckResult);

            IHandler<T> a = _dummyOf.Handler(A);
            if (@case.AIsConditional)
                a = _math.Conditional(a, _dummyOf.Condition(X));

            IHandler<T> b = _dummyOf.Handler(B);
            if (@case.BIsConditional)
                b = _math.Conditional(b, _dummyOf.Condition(Y));

            _math.JoinFirstWithSecond(a, b)
                 .Execute(_token);

            Assert.That(expectedCondition
                  .WasCheckedOnce());

            Assert.That(_dummyOf.Conditions[X, Y, X & Y].Except([expectedCondition])
                  .WereNeverChecked());

            Assert.That(_dummyOf.Handlers[A, B]
                  .EachWasExecutedOnceWhen(@case.FinalConditionCheckResult)
                  .ElseNoOne);
        }

        [Test]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(
            [OrangeCases] OrangeCase @case)
        {
            Dummy.ConditionIndex
                aTop = new("A TOP"),
                bTop = new("B TOP"),
                aBottom = new("A BOTTOM"),
                bBottom = new("B BOTTOM");

            _dummyOf.Conditions.AddRange(
                aTop, bTop, aTop&bTop, aBottom, bBottom);

            _dummyOf.Conditions[aBottom, bBottom, aTop&bTop].Return(@case.CheckSetup);

            _dummyOf.ConditionMath.And(aTop, bTop).Returns(aTop&bTop);

            IHandler<T> a = _dummyOf.Handler(A);
            a = _math.Conditional(a, _dummyOf.Condition(aBottom));
            a = _math.Conditional(a, _dummyOf.Condition(aTop));

            IHandler<T> b = _dummyOf.Handler(B);
            b = _math.Conditional(b, _dummyOf.Condition(bBottom));
            b = _math.Conditional(b, _dummyOf.Condition(bTop));

            var ab = _math.JoinFirstWithSecond(a, b);
            ab.Execute(_token);

            Assert.That(_dummyOf.Conditions[aBottom, bBottom, aTop&bTop]
                  .VerifyChecks(@case.CheckExpected));

            Assert.That(_dummyOf.Handlers[A, B]
                  .VerifyExecution(@case.ExecutionExpected));
        }

        [Test]
        public void JoinFirstWithSecond__ConjunctsOnlyTopConditions(
            [YellowCases] YellowCase @case)
        {
            _dummyOf.Conditions[@case.ChecksSetup.Keys]
                    .Return(@case.ChecksSetup.Values);

            if (@case.AConditions.Any() &&
                @case.BConditions.Any())
                    _dummyOf.ConditionMath.And(
                            @case.AConditions.Last(),
                            @case.BConditions.Last())
                        .Returns(R);

            var a = _dummyOf.Conditions[@case.AConditions]
                .Aggregate(_dummyOf.Handler(A).Pure, _math.Conditional);

            var b = _dummyOf.Conditions[@case.BConditions]
                .Aggregate(_dummyOf.Handler(B).Pure, _math.Conditional);

            _math.JoinFirstWithSecond(a, b)
                 .Execute(_token);

            var expectedToCheck = _dummyOf.Conditions[@case.CheckExpected];
            var expectedToExecute = _dummyOf.Handlers[@case.ExecuteExpected];

            Assert.That(expectedToCheck
                  .EachWasCheckedOnce());

            Assert.That(_dummyOf.Conditions.Except(expectedToCheck)
                  .WereNeverChecked());

            Assert.That(expectedToExecute
                  .EachWasExecutedOnce());

            Assert.That(_dummyOf.Handlers.Except(expectedToExecute)
                  .NoOneWasExecuted());
        }

        [Test]
        public void MergeFirstWithSecond__ConjunctsAllConditions(
            [WhiteCases] WhiteCase @case,
            [Values(true, false)] bool finalConditionResult)
        {
            _dummyOf.ConditionMath.And(Any, Any).Implements((x, y) =>
                {
                    _dummyOf.Conditions.Add(x & y);
                    _dummyOf.Condition(x & y).AddCallback(() =>
                    {
                        _dummyOf.Condition(x).Check(_token);
                        _dummyOf.Condition(y).Check(_token);
                    });
                    
                    return x & y;
                });

            var a = _dummyOf.Conditions[@case.AConditions]
                .Aggregate(_dummyOf.Handler(A).Pure, _math.Conditional);

            var b = _dummyOf.Conditions[@case.BConditions]
                .Aggregate(_dummyOf.Handler(B).Pure, _math.Conditional);

            var ab = _math.MergeFirstWithSecond(a, b);

            _dummyOf.Conditions.Last()
                .Returns(finalConditionResult);

            ab.Execute(_token);

            var all = Enumerable.Concat(@case.AConditions, @case.BConditions);
            Assert.That(_dummyOf.Conditions[all].EachWasCheckedOnce());
            Assert.That(_dummyOf.Handlers[A, B]
                  .EachWasExecutedOnceWhen(finalConditionResult)
                  .ElseNoOne);
        }

        [Test]
        public void JoinSomeWithSingleConditionalHandler__PutsConditionOnTopOfResult(
            [Values(false, true)] bool order,
            [Values(false, true)] bool checkResult)
        {
            _dummyOf.Condition(X).Returns(checkResult);

            IHandler<T>
                first = _dummyOf.Handler(B),
                second = _math.Conditional(
                    _dummyOf.Handler(A), 
                    _dummyOf.Condition(X));

            (first, second) = order
                ? (first, second)
                : (second, first);

            _math.MergeFirstWithSecond(first, second)
                 .Execute(_token);

            Assert.That(_dummyOf.Condition(X)
                  .WasCheckedOnce());

            Assert.That(_dummyOf.Handlers[A, B]
                  .EachWasExecutedOnceWhen(checkResult).ElseNoOne);
        }


        [Test]
        public void CorrectConditionsCascadeTest(
            [BlackCases] BlackCase @case)
        {
            List<Dummy.Index> execution = [];

            _dummyOf.Conditions[@case.AConditions].LogInto(execution);
            _dummyOf.Conditions[@case.BConditions].LogInto(execution);
            _dummyOf.Handlers[A, B].LogInto(execution);

            _dummyOf.Conditions[@case.ChecksSetup.Keys]
                   .Return(@case.ChecksSetup.Values);

            var a = _dummyOf.Conditions[@case.AConditions]
                .Aggregate(_dummyOf.Handler(A).Pure, _math.Conditional);

            var b = _dummyOf.Conditions[@case.BConditions]
                .Aggregate(_dummyOf.Handler(B).Pure, _math.Conditional);

            _do(@case.Append).Of(a, b)
                .Execute(_token);

            Assert.That(execution,
                Is.EqualTo(@case.ExpectedCallsOrder));
        }

        [Test]
        public void AppendWithAtomizedConditionalHandler__IsTheSameAs__WithRegularHandler(
            [AllAppends] string append,
            [Values(false, true)] bool isFirstArgument,
            [Values(false, true)] bool bottomCheckResult)
        {
            Dummy.HandlerIndex
                atomized = new("ATOMIZED"),
                atom = new("ATOM");

            _dummyOf.Handlers.AddRange(atomized, atom);

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

            _dummyOf.Handlers.LogInto(executionLog);
            _dummyOf.Conditions.LogInto(executionLog); 

            _dummyOf.Conditions[X, Y].Return(true);
            _dummyOf.Condition(Z).Returns(bottomCheckResult);

            IHandler<T>
                first =  _dummyOf.Handler(atomized),
                second = _dummyOf.Handler(atom);

            first = _dummyOf.Conditions[Z, Y, X].Aggregate(first, _math.Conditional);
            first = _math.Atomize(first);

            (first, second) = isFirstArgument
                ? (first, second)
                : (second, first);

            _do(append).Of(first, second)
                .Execute(_token);

            Assert.That(executionLog,
                Is.EqualTo(expectedLog));
        }

        [Test]
        public void AppendedTwoAtomizedConditionalHandlers__IsTheSameAs__AppendedTwoRegularHandlers(
            [AllAppends] string append,
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

            _dummyOf.Handlers.LogInto(executionLog);
            _dummyOf.Conditions.LogInto(executionLog);

            _dummyOf.Conditions[U, V, X, Y].Return(true);
            _dummyOf.Condition(W).Returns(aBottomCheckResult);
            _dummyOf.Condition(Z).Returns(bBottomCheckResult);

            IHandler<T> a = _dummyOf.Handler(A);
            a = _dummyOf.Conditions[W, V, U].Aggregate(a, _math.Conditional);
            a = _math.Atomize(a);

            IHandler<T> b = _dummyOf.Handler(B);
            b = _dummyOf.Conditions[Z, Y, X].Aggregate(b, _math.Conditional);
            b = _math.Atomize(b);

            _do(append).Of(a, b)
                .Execute(_token);

            Assert.That(executionLog,
                Is.EqualTo(expectedLog));
        }

        static bool NotNull(object? x) => x != null;

        static T Denullify<T>(T? x) where T : class => x!;

        class AppendOf(Func<IHandler<T>, IHandler<T>, IHandler<T>> f)
        {
            public IHandler<T> Of(IHandler<T> a, IHandler<T> b) => f(a, b);
        }
    }
}