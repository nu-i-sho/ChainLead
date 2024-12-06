﻿namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using Moq;

    using static ChainLead.Test.Cases.SingleHandlerTest;
    using static ChainLead.Test.Dummy.HandlerIndex.Common;
    using static ChainLead.Test.Dummy.ConditionIndex.Common;
    using static ChainLead.Test.Cases.Common;

    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    [_IX_][_X_][_XI_][_XII_][_XIII_][_XIV_][_XV_][_XVI_]
    public class SingleHandlerTest<T>(
        string mathFactoryName)
    {
        Dummy.Container<T> _dummyOf;
        ISingleHandlerMath _math;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.Get<T>(546823);
            _dummyOf = new(_token);
            _math = SingleHandlerMathFactoryProvider
                .Get(mathFactoryName)
                .Create(_dummyOf.ConditionMath.Object);
        }

        [Test]
        public void ZeroDoesNothing() =>
            Assert.DoesNotThrow(() => _math.Zero<T>().Execute(_token));

        [Test]
        public void ZeroIsZero() =>
            Assert.That(_math.IsZero(_math.Zero<T>()));

        //[Test]
        //public void ZeroForBaseClassIsZeroForDerivedClass() =>
        //    Assert.That(_math.IsZero<IDerived>(_math.Zero<IBase>()));

        [Test]
        public void MadeHandlerExecutesProvidedAction()
        {
            T? x = default;
            var action = new Action<T>(a => x = a);
            var handler = _math.MakeHandler(action);
            handler.Execute(_token);

            Assert.That(x, 
                Is.EqualTo(_token));
        }

        [Test]
        public void ConditionalZeroIsZero()
        {
            var conditionalZero =
                _math.Conditional(_math.Zero<T>(), _dummyOf.Conditions[X]);

            Assert.That(_math.IsZero(conditionalZero));
        }

        [Test]
        public void WhenConditionReturnsTrue__HandlerIsExecuted()
        {
            _dummyOf.Conditions[X].SetResult(true);

            _math.Conditional(
                    _dummyOf.Handlers[A],
                    _dummyOf.Conditions[X])
                 .Execute(_token);

            Assert.That(_dummyOf.Handlers[A].WasExecutedOnce());
        }

        [Test]
        public void WhenConditionReturnsFalse__HandlerIsNotExecuted()
        {
            _dummyOf.Conditions[X].SetResult(false);

            _math.Conditional(
                    _dummyOf.Handlers[A],
                    _dummyOf.Conditions[X])
                 .Execute(_token);

            Assert.That(_dummyOf.Handlers[A].WasNeverExecuted());
        }

        [Test]
        public void WhenTopConditionReturnsFalse__AllOtherChecksAndExecutionsAreNotCalled()
        {
            _dummyOf.Conditions[Z].SetResult(false);

            _dummyOf.Conditions[X, Y, Z]
                .Aggregate(_dummyOf.Handlers[A].Pure, _math.Conditional)
                .Execute(_token);

            Assert.That(_dummyOf.Conditions[Z].WasCheckedOnce());
            Assert.That(_dummyOf.Conditions[X, Y].NoOneWasChecked());
            Assert.That(_dummyOf.Handlers[A].WasNeverExecuted());
        }

        [Test]
        public void ChecksAllConditionsUpToFirstFalse(
            [Values(0, 1, 2, 5)] int trueCount,
            [Values(0, 1, 2, 5)] int falseCount)
        {
            var trues = _dummyOf.Conditions.Take(trueCount);
            var falses = _dummyOf.Conditions.Skip(trueCount).Take(falseCount);

            trues.SetResults(true);
            falses.SetResults(false);

            var all = falses.Concat(trues);
            all.Aggregate(_dummyOf.Handlers[A].Pure, _math.Conditional)
               .Execute(_token);

            var checkedCount = trueCount + int.Min(1, falseCount);

            Assert.That(all.Reverse().Take(checkedCount).EachWasCheckedOnce());
            Assert.That(all.Reverse().Skip(checkedCount).NoOneWasChecked());
            Assert.That(_dummyOf.Handlers[A].WasExecutedOnceWhen(falseCount == 0).ElseNever);
        }

        [Test]
        public void ChecksOrderEqualsConditionsReverseAttachingOrder()
        {
            List<Dummy.ConditionIndex> checksLog = [];

            _dummyOf.Conditions[X, Y, Z].AddLoggingInto(checksLog);
            _dummyOf.Conditions[X, Y, Z].SetResults(true);
            _dummyOf.Conditions[X, Y, Z]
                .Aggregate(_dummyOf.Handlers[A].Pure, _math.Conditional)
                .Execute(_token);

            Assert.That(checksLog, 
                Is.EqualTo(new[] { Z, Y, X }));
        }

        [Test]
        public void AtomizeZeroMakeHandlerThatIsNotZero()
        {
            var zero = _math.Zero<T>();
            zero = _math.Atomize(zero);
            var isZero = _math.IsZero(zero);

            Assert.That(isZero,
                Is.False);
        }

        [Test]
        public void ExtendedZeroIsZero()
        {
            var extended = new Mock<IExtendedHandler<T>>();
            extended.Setup(o => o.Origin)
                    .Returns(_math.Zero<T>());

            Assert.That(_math.IsZero(extended.Object));
        }

        [Test]
        public void ExtendedNotZeroIsNotZero()
        {
            var extended = new Mock<IExtendedHandler<T>>();
            extended.Setup(o => o.Origin)
                    .Returns(_dummyOf.Handlers[A]);

            Assert.That(_math.IsZero(extended.Object),
                Is.False);
        }
    }
}