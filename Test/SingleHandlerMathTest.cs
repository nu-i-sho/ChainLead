namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Test.Help;
    using Moq;
    using System.Diagnostics.CodeAnalysis;
    using static ChainLead.Test.Help.Constants;
    using static ChainLead.Test.SingleHandlerMathTest;

    [TestFixtureSource(nameof(FixtureCases))]
    public partial class SingleHandlerMathTest(
        ISingleHandlerMathFactory mathFactory)
    {
        public interface IBase;
        public interface IDerived : IBase;

        [NotNull] ChainLeadMocks _mockOf;
        [NotNull] ISingleHandlerMath _math;

        [SetUp]
        [MemberNotNull(nameof(_mockOf))]
        [MemberNotNull(nameof(_math))]
        public void Setup()
        {
            _mockOf = new ChainLeadMocks();
            _math = mathFactory.Create(_mockOf.ConditionMath.Object);
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
        public void MadeHandlerExecutesProvidedAction()
        {
            int x = 0;
            var action = new Action<int>(a => x = a);
            var handler = _math.MakeHandler(action);
            handler.Execute(Arg);

            Assert.That(x, 
                Is.EqualTo(Arg));
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

            Assert.That(checksLog, 
                Is.EqualTo(new[] { Z, Y, X }));
        }

        void AtomizeZeroMakeHandlerThatIsNotZero<T>()
        {
            var zero = _math.Zero<T>();
            zero = _math.Atomize(zero);
            var isZero = _math.IsZero(zero);

            Assert.That(isZero,
                Is.False);
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
    }
}
