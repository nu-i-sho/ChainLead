namespace Nuisho.ChainLead.Test
{
    using Utils;
    using static Cases.SingleHandler;
    using static Dummy.ConditionIndex;
    using static Dummy.HandlerIndex;

    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    [_IX_][_X_][_XI_][_XII_][_XIII_][_XIV_][_XV_][_XVI_]
    public class SingleHandlerTest<T>(
        ISingleHandlerMathFactory mathFactory)
    {
        Dummy.Container<T> _dummyOf;
        ISingleHandlerMath _math;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.GetRandom<T>();

            _dummyOf = new (_token);
            _dummyOf.Handlers.Generate(A);

            _math = mathFactory.Create(_dummyOf.ConditionMath);
        }

        [Test]
        public void Zero__DoesNothing() =>
            Assert.DoesNotThrow(() => _math.Zero<T>().Execute(_token));

        [Test]
        public void Zero__IsZero() =>
            Assert.That(_math.IsZero(_math.Zero<T>()));

        [Test]
        public void MadeHandler__Executes_ProvidedAction()
        {
            T? x = default;
            var action = new Action<T>(a => x = a);
            var handler = _math.MakeHandler(action);
            handler.Execute(_token);

            Assert.That(x,
                Is.EqualTo(_token));
        }

        [Test]
        public void ConditionalZero__IsZero()
        {
            _dummyOf.Conditions.Generate(X);
            var conditionalZero = _math.Conditional(
                _math.Zero<T>(),
                _dummyOf.Condition(X));

            Assert.That(_math.IsZero(conditionalZero));
        }

        [Test]
        public void WhenConditionReturnsTrue__HandlerIsExecuted()
        {
            _dummyOf.Conditions.Generate(X);
            _dummyOf.Condition(X).Returns(true);

            _math.Conditional(
                    _dummyOf.Handler(A),
                    _dummyOf.Condition(X))
                 .Execute(_token);

            Assert.That(_dummyOf.Handler(A).WasExecutedOnce);
        }

        [Test]
        public void WhenConditionReturnsFalse__HandlerIs_Not_Executed()
        {
            _dummyOf.Conditions.Generate(X);
            _dummyOf.Condition(X).Returns(false);

            _math.Conditional(
                    _dummyOf.Handler(A),
                    _dummyOf.Condition(X))
                 .Execute(_token);

            Assert.That(_dummyOf.Handler(A).WasNeverExecuted);
        }

        [Test]
        public void WhenTopConditionReturnsFalse__AllOther_Checks_And_Executions_Are_Not_Called()
        {
            _dummyOf.Conditions.Generate(X, Y, Z);
            _dummyOf.Condition(Z).Returns(false);

            _dummyOf.Conditions[X, Y, Z]
                .Aggregate(_dummyOf.Handler(A).Pure, _math.Conditional)
                .Execute(_token);

            using var _ = Assert.EnterMultipleScope();

            Assert.That(_dummyOf.Condition(Z).WasCheckedOnce);

            Assert.That(_dummyOf.Conditions.ThatWereNeverChecked,
                Is.EquivalentTo(_dummyOf.Conditions[X, Y]));

            Assert.That(_dummyOf.Handler(A).WasNeverExecuted);
        }

        [Test]
        public void ChecksAllConditionsUpToFirstFalse__Test(
            [Values(0, 1, 2, 5)] int trueCount,
            [Values(0, 1, 2, 5)] int falseCount)
        {
            _dummyOf.Conditions.Generate(QRSTUVWXYZ.Take(trueCount + falseCount));

            var trues = _dummyOf.Conditions.Take(trueCount);
            var falses = _dummyOf.Conditions.Skip(trueCount).Take(falseCount);

            var falsesThenTrues = falses.Concat(trues);
            var truesThenFalses = falsesThenTrues.Reverse();

            trues.Return(true);
            falses.Return(false);

            falsesThenTrues
                .Aggregate(_dummyOf.Handler(A).Pure, _math.Conditional)
                .Execute(_token);

            var checkedCount = trueCount + int.Min(1, falseCount);

            using var _ = Assert.EnterMultipleScope();

            Assert.That(_dummyOf.Conditions.ThatWereCheckedOnce,
                Is.EquivalentTo(truesThenFalses.Take(checkedCount)));

            Assert.That(_dummyOf.Conditions.ThatWereNeverChecked,
                Is.EquivalentTo(truesThenFalses.Skip(checkedCount)));

            Assert.That(_dummyOf.Handler(A)
                  .WasExecutedOnceWhen(falseCount == 0)
                  .ElseNever);
        }

        [Test]
        public void Checks_Order__Equals_ConditionsReverseAttachingOrder()
        {
            List<Dummy.ConditionIndex> checksLog = [];

            _dummyOf.Conditions.Generate(X, Y, Z);
            _dummyOf.Conditions[X, Y, Z].LogInto(checksLog);
            _dummyOf.Conditions[X, Y, Z].Return(true);
            _dummyOf.Conditions[X, Y, Z]
                .Aggregate(_dummyOf.Handler(A).Pure, _math.Conditional)
                .Execute(_token);

            Assert.That(checksLog,
                Is.EqualTo(new[] { Z, Y, X }));
        }

        [Test]
        public void AtomizeZero__Makes_HandlerThatIs_Not_Zero()
        {
            var zero = _math.Zero<T>();
            zero = _math.Atomize(zero);
            var isZero = _math.IsZero(zero);

            Assert.That(isZero,
                Is.False);
        }

        [Test]
        public void ExtendedZero__IsZero()
        {
            var extendedZero = _math.Zero<T>().AsExtended();
            Assert.That(_math.IsZero(extendedZero));
        }

        [Test]
        public void Extended_Not_Zero__Is_Not_Zero()
        {
            var extended = _dummyOf.Handler(A).AsExtended();
            Assert.That(_math.IsZero(extended),
                Is.False);
        }
    }
}
