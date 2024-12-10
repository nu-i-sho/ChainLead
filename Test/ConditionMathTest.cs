namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using ChainLead.Test.Utils;
    using static ChainLead.Test.Cases.Common;
    using static ChainLead.Test.Dummy.ConditionIndex;

    [_I_][_II_][_III_][_IV_][_V_][_VI_][_VII_][_VIII_]
    public class ConditionMathTest<T>
    {
        Dummy.Container<T> _dummyOf;
        IConditionMath _math;
        T _token;

        [SetUp]
        public void Setup()
        {
            _token = TokensProvider.GetRandom<T>();
            
            _dummyOf = new(_token);
            _dummyOf.Conditions.Generate(X, Y);
            
            _math = new ConditionMath();
        }

        [Test]
        public void TrueCheckReturnsTrue() =>
            Assert.That(_math.True<T>().Check(_token));

        [Test]
        public void FalseCheckReturnsFalse() =>
            Assert.That(_math.False<T>().Check(_token),
                Is.False);

        [Test]
        public void TrueIsPredictableTrue() =>
            Assert.That(_math.IsPredictableTrue(_math.True<T>()));

        [Test]
        public void TrueIsNotPredictableFalse() =>
            Assert.That(_math.IsPredictableFalse(_math.True<T>()),
                Is.False);

        [Test]
        public void SomeConditionIsNotPredictableFalse() =>
            Assert.That(_math.IsPredictableFalse(_dummyOf.Condition(X)),
                Is.False);

        [Test]
        public void FalseIsPredictableFalse() =>
            Assert.That(_math.IsPredictableFalse(_math.False<T>()));

        [Test]
        public void FalseIsNotPredictableTrue() =>
            Assert.That(_math.IsPredictableTrue(_math.False<T>()),
                Is.False);

        [Test]
        public void SomeConditionIsNotPredictableTrue() =>
            Assert.That(_math.IsPredictableTrue(_dummyOf.Condition(X)),
                Is.False);

        [Test]
        public void FalseAndSomethingIsPredictableFalse(
            [Values(false, true, null)] bool? another)
        {
            var falseAndSomething = _math.And(_math.False<T>(), Some(another));
            var isPredictableFalse = _math.IsPredictableFalse(falseAndSomething);

            Assert.That(isPredictableFalse);
        }

        [Test]
        public void FalseAndSomethingIsNotPredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var falseAndSomething = _math.And(_math.False<T>(), Some(another));
            var isPredictableTrue = _math.IsPredictableTrue(falseAndSomething);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void FalseAndSomethingCheckIsFalse(
            [Values(false, true, null)] bool? another)
        {
            _dummyOf.Condition(X).Returns(true);
            var falseAndSomething = _math.And(_math.False<T>(), Some(another));
           
            Assert.That(falseAndSomething.Check(_token), 
                Is.False);
        }

        [Test]
        public void SomethingAndFalseIsPredictableFalse(
             [Values(false, true, null)] bool? another)
        {
            var somethingAndFalse = _math.And(Some(another), _math.False<T>());
            var isPredictableFalse = _math.IsPredictableFalse(somethingAndFalse);

            Assert.That(isPredictableFalse);
        }

        [Test]
        public void SomethingAndFalseIsNotPredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var somethingAndFalse = _math.And(Some(another), _math.False<T>());
            var isPredictableTrue = _math.IsPredictableTrue(somethingAndFalse);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void SomethingAndFalseCheckIsFalse(
            [Values(false, true, null)] bool? another)
        {
            var somethingAndFalse = _math.And(Some(another), _math.False<T>());

            Assert.That(somethingAndFalse.Check(_token),
                Is.False);
        }

        [Test]
        public void SomethingAndFalseCheckDoesNotCallCheckOfSomething()
        {
            var somethingAndFalse = _math.And(_dummyOf.Condition(X), _math.False<T>());
            somethingAndFalse.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void FalseAndSomethingCheckDoesNotCallCheckOfSomething()
        {
            var falseAndSomething = _math.And(_math.False<T>(), _dummyOf.Condition(X));
            falseAndSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void FalseOrSomethingButNotFalseIsNotPredictableFalse(
            [Values(true, null)] bool? another)
        {
            var falseOrSomething = _math.Or(_math.False<T>(), Some(another));
            var isPredictableFalse = _math.IsPredictableFalse(falseOrSomething);

            Assert.That(isPredictableFalse,
                Is.False);
        }

        [Test]
        public void FalseOrFalseIsPredictableFalse()
        {
            var falseOrFalse = _math.Or(_math.False<T>(), _math.False<T>());
            var isPredictableFalse = _math.IsPredictableFalse(falseOrFalse);

            Assert.That(isPredictableFalse);
        }

        [Test]
        public void FalseOrSomethingButNotTrueIsNotNotPredictableTrue(
            [Values(false, null)] bool? another)
        {
            var falseOrSomething = _math.Or(_math.False<T>(), Some(another));
            var isPredictableTrue = _math.IsPredictableTrue(falseOrSomething);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void FalseOrTrueIsPredictableTrue()
        {
            var falseOrTrue = _math.Or(_math.False<T>(), _math.True<T>());
            var isPredictableTrue = _math.IsPredictableTrue(falseOrTrue);

            Assert.That(isPredictableTrue);
        }

        [Test]
        public void FalseOrSomethingCheckIsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var falseOrSomething = _math.Or(_math.False<T>(), _dummyOf.Condition(X));

            Assert.That(falseOrSomething.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void FalseOrSomethingCheckCallsCheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var falseOrSomething = _math.Or(_math.False<T>(), _dummyOf.Condition(X));
            falseOrSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void FalseOrSomethingIsSomething()
        {
            var falseOrSomething = _math.Or(_math.False<T>(), _dummyOf.Condition(X));
            
            Assert.That(falseOrSomething,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void SomethingOrFalseButNotFalseIsNotPredictableFalse(
            [Values(true, null)] bool? another)
        {
            var somethingOrFalse = _math.Or(Some(another), _math.False<T>());
            var isPredictableFalse = _math.IsPredictableFalse(somethingOrFalse);

            Assert.That(isPredictableFalse,
                Is.False);
        }

        [Test]
        public void SomethingButNotTrueOrFalseIsNotNotPredictableTrue(
            [Values(false, null)] bool? another)
        {
            var somethingOrFalse = _math.Or(Some(another), _math.False<T>());
            var isPredictableTrue = _math.IsPredictableTrue(somethingOrFalse);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void TrueOrFalseIsPredictableTrue()
        {
            var trueOrFalse = _math.Or(_math.True<T>(), _math.False<T>());
            var isPredictableTrue = _math.IsPredictableTrue(trueOrFalse);

            Assert.That(isPredictableTrue);
        }

        [Test]
        public void SomethingOrFalseCheckIsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var somethingOrFalse = _math.Or(_dummyOf.Condition(X), _math.False<T>());

            Assert.That(somethingOrFalse.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void SomethingOrFalseCheckCallsCheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var somethingOrFalse = _math.Or(_dummyOf.Condition(X), _math.False<T>());
            somethingOrFalse.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void SomethingOrFalseIsSomething()
        {
            var somethingOrFalse = _math.Or(_dummyOf.Condition(X), _math.False<T>());

            Assert.That(somethingOrFalse,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void TrueOrSomethingIsPredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var trueOrSomething = _math.Or(_math.True<T>(), Some(another));
            var predictableTrue = _math.IsPredictableTrue(trueOrSomething);

            Assert.That(predictableTrue);
        }

        [Test]
        public void TrueOrSomethingIsNotPredictableFalse(
            [Values(false, true, null)] bool? another)
        {
            var trueOrSomething = _math.Or(_math.True<T>(), Some(another));
            var predictableFalse = _math.IsPredictableFalse(trueOrSomething);

            Assert.That(predictableFalse,
                Is.False);
        }

        [Test]
        public void TrueOrSomethingCheckIsTrue(
            [Values(false, true, null)] bool? another)
        {
            var trueOrSomething = _math.Or(_math.True<T>(), Some(another));

            Assert.That(trueOrSomething.Check(_token));
        }

        [Test]
        public void TrueOrSomethingCheckDoesNotCallCheckFromSomething()
        {
            var trueOrSomething = _math.Or(_math.True<T>(), _dummyOf.Condition(X));
            trueOrSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void SomethingOrTrueIsPredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var somethingOrTrue = _math.Or(Some(another), _math.True<T>());
            var predictableTrue = _math.IsPredictableTrue(somethingOrTrue);

            Assert.That(predictableTrue);
        }

        [Test]
        public void SomethingOrTrueIsNotPredictableFalse(
            [Values(false, true, null)] bool? another)
        {
            var somethingOrTrue = _math.Or(Some(another), _math.True<T>());
            var predictableFalse = _math.IsPredictableFalse(somethingOrTrue);

            Assert.That(predictableFalse,
                Is.False);
        }

        [Test]
        public void SomethingOrTrueCheckIsTrue(
            [Values(false, true, null)] bool? another)
        {
            var somethingOrTrue = _math.Or(Some(another), _math.True<T>());

            Assert.That(somethingOrTrue.Check(_token));
        }

        [Test]
        public void SomethingOrTrueCheckDoesNotCallCheckFromSomething()
        {
            var somethingOrTrue = _math.Or(_dummyOf.Condition(X), _math.True<T>());
            somethingOrTrue.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void SomethingAndTrueIsSomething()
        {
            var somethingAndTrue = _math.And(_dummyOf.Condition(X), _math.True<T>());
            
            Assert.That(somethingAndTrue, 
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void TrueAndSomethingIsSomething()
        {
            var trueAndSomething = _math.And(_math.True<T>(), _dummyOf.Condition(X));

            Assert.That(trueAndSomething,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void TrueAndSomethingCheckIsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var trueAndSomething = _math.And(_math.True<T>(), _dummyOf.Condition(X));

            Assert.That(trueAndSomething.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void TrueAndSomethingCheckCallsCheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var trueAndSomething = _math.And(_math.True<T>(), _dummyOf.Condition(X));
            trueAndSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void SomethingAndTrueCheckIsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var somethingAndTrue = _math.And(_dummyOf.Condition(X), _math.True<T>());

            Assert.That(somethingAndTrue.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void SomethingAndTrueCheckCallsCheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var somethingAndTrue = _math.And(_dummyOf.Condition(X), _math.True<T>());
            somethingAndTrue.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void NotTrueIsPredictableFalse()
        {
            var notTrue = _math.Not(_math.True<T>());

            Assert.That(_math.IsPredictableFalse(notTrue));
        }

        [Test]
        public void NotTrueIsNotPredictableTrue()
        {
            var notTrue = _math.Not(_math.True<T>());
            
            Assert.That(_math.IsPredictableTrue(notTrue),
                Is.False);
        }

        [Test]
        public void NotFalseIsPredictableTrue()
        {
            var notFalse = _math.Not(_math.False<T>());

            Assert.That(_math.IsPredictableTrue(notFalse));
        }

        [Test]
        public void NotFalseIsNotPredictableFalse()
        {
            var notFalse = _math.Not(_math.False<T>());
            
            Assert.That(_math.IsPredictableFalse(notFalse), 
                Is.False);
        }

        [Test]
        public void NotTrueCheckIsFalse()
        {
            var notTrue = _math.Not(_math.True<T>());

            Assert.That(notTrue.Check(_token),
                Is.False);
        }

        [Test]
        public void NotFalseCheckIsTrue()
        {
            var notFalse = _math.Not(_math.False<T>());
            
            Assert.That(notFalse.Check(_token));
        }

        [Test]
        public void MakeConditionCallsEncapsulatedFunc()
        {
            bool called = false;
            var condition = _math.MakeCondition<T>(_ => called = true);

            Assert.That(condition.Check(_token));
            Assert.That(called);
        }

        [Test]
        public void MakeConditionReturnsEncapsulatedFuncResult(
            [Values(false, true)] bool funcResult)
        {
            var condition = _math.MakeCondition<T>(_ => funcResult);

            Assert.That(condition.Check(_token), 
                Is.EqualTo(funcResult));
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void NotConditionCheckTest(
            bool checkResult,
            bool expectedCheckResultOfNot)
        {
            _dummyOf.Condition(X).Returns(checkResult);

            Assert.That(_math.Not(_dummyOf.Condition(X)).Check(_token),
                Is.EqualTo(expectedCheckResultOfNot));
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void AndConditionCheckTest(
            bool xResult,
            bool yResult,
            bool xyExpectedResult)
        {
            _dummyOf.Condition(X).Returns(xResult);
            _dummyOf.Condition(Y).Returns(yResult);

            var xAndY = _math.And(
                _dummyOf.Condition(X),
                _dummyOf.Condition(Y));

            Assert.That(xAndY.Check(_token),
                Is.EqualTo(xyExpectedResult));
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, true)]
        public void OrConditionCheckTest(
            bool xResult,
            bool yResult,
            bool xyExpectedResult)
        {
            _dummyOf.Condition(X).Returns(xResult);
            _dummyOf.Condition(Y).Returns(yResult);

            var xOrY = _math.Or(
                _dummyOf.Condition(X),
                _dummyOf.Condition(Y));

            Assert.That(xOrY.Check(_token),
                Is.EqualTo(xyExpectedResult));
        }

        ICondition<T> Some(bool? marker) =>
            marker switch
            {
                null => _dummyOf.Condition(X),
                false => _math.False<T>(),
                true => _math.True<T>(),
            };
    }
}
