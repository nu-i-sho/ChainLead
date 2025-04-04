namespace Nuisho.ChainLead.Test
{
    using Contracts;
    using ChainLead.Implementation;
    using Utils;

    using static Cases.Common;
    using static Dummy.ConditionIndex;

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
            
            _dummyOf = new Dummy.Container<T>(_token);
            _dummyOf.Conditions.Generate(X, Y);
            
            _math = new ConditionMath();
        }

        [Test]
        public void TrueCheck__ReturnsTrue() =>
            Assert.That(_math.True<T>().Check(_token));

        [Test]
        public void FalseCheck__ReturnsFalse() =>
            Assert.That(_math.False<T>().Check(_token),
                Is.False);

        [Test]
        public void True__IsPredictableTrue() =>
            Assert.That(_math.IsPredictableTrue(_math.True<T>()));

        [Test]
        public void True__IsNotPredictableFalse() =>
            Assert.That(_math.IsPredictableFalse(_math.True<T>()),
                Is.False);

        [Test]
        public void SomeCondition__IsNotPredictableFalse() =>
            Assert.That(_math.IsPredictableFalse(_dummyOf.Condition(X)),
                Is.False);


        [Test]
        public void False__IsPredictableFalse() =>
            Assert.That(_math.IsPredictableFalse(_math.False<T>()));

        [Test]
        public void False__IsNotPredictableTrue() =>
            Assert.That(_math.IsPredictableTrue(_math.False<T>()),
                Is.False);

        [Test]
        public void SomeCondition__IsNotPredictableTrue() =>
            Assert.That(_math.IsPredictableTrue(_dummyOf.Condition(X)),
                Is.False);

        [Test]
        public void False_And_Something__IsPredictableFalse(
            [Values(false, true, null)] bool? another)
        {
            var falseAndSomething = _math.And(_math.False<T>(), Some(another));
            var isPredictableFalse = _math.IsPredictableFalse(falseAndSomething);

            Assert.That(isPredictableFalse);
        }

        [Test]
        public void False_And_Something__IsNotPredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var falseAndSomething = _math.And(_math.False<T>(), Some(another));
            var isPredictableTrue = _math.IsPredictableTrue(falseAndSomething);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void False_And_Something_Check__IsFalse(
            [Values(false, true, null)] bool? another)
        {
            _dummyOf.Condition(X).Returns(true);
            var falseAndSomething = _math.And(_math.False<T>(), Some(another));
           
            Assert.That(falseAndSomething.Check(_token), 
                Is.False);
        }

        [Test]
        public void Something_And_False__IsPredictableFalse(
             [Values(false, true, null)] bool? another)
        {
            var somethingAndFalse = _math.And(Some(another), _math.False<T>());
            var isPredictableFalse = _math.IsPredictableFalse(somethingAndFalse);

            Assert.That(isPredictableFalse);
        }

        [Test]
        public void Something_And_False__Is_Not_PredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var somethingAndFalse = _math.And(Some(another), _math.False<T>());
            var isPredictableTrue = _math.IsPredictableTrue(somethingAndFalse);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void Something_And_False__Check_IsFalse(
            [Values(false, true, null)] bool? another)
        {
            var somethingAndFalse = _math.And(Some(another), _math.False<T>());

            Assert.That(somethingAndFalse.Check(_token),
                Is.False);
        }

        [Test]
        public void Something_And_False_Check__DoesNotCall_CheckOfSomething()
        {
            var somethingAndFalse = _math.And(_dummyOf.Condition(X), _math.False<T>());
            somethingAndFalse.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void False_And_Something_Check__DoesNotCall_CheckOfSomething()
        {
            var falseAndSomething = _math.And(_math.False<T>(), _dummyOf.Condition(X));
            falseAndSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void False_Or_Something_ButNotFalse__Is_Not_PredictableFalse(
            [Values(true, null)] bool? another)
        {
            var falseOrSomething = _math.Or(_math.False<T>(), Some(another));
            var isPredictableFalse = _math.IsPredictableFalse(falseOrSomething);

            Assert.That(isPredictableFalse,
                Is.False);
        }

        [Test]
        public void False_Or_False__IsPredictableFalse()
        {
            var falseOrFalse = _math.Or(_math.False<T>(), _math.False<T>());
            var isPredictableFalse = _math.IsPredictableFalse(falseOrFalse);

            Assert.That(isPredictableFalse);
        }

        [Test]
        public void False_Or_Something_ButNotTrue__Is_Not_PredictableTrue(
            [Values(false, null)] bool? another)
        {
            var falseOrSomething = _math.Or(_math.False<T>(), Some(another));
            var isPredictableTrue = _math.IsPredictableTrue(falseOrSomething);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void False_Or_True__IsPredictableTrue()
        {
            var falseOrTrue = _math.Or(_math.False<T>(), _math.True<T>());
            var isPredictableTrue = _math.IsPredictableTrue(falseOrTrue);

            Assert.That(isPredictableTrue);
        }

        [Test]
        public void False_Or_Something_Check__IsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var falseOrSomething = _math.Or(_math.False<T>(), _dummyOf.Condition(X));

            Assert.That(falseOrSomething.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void False_Or_Something_Check__Calls_CheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var falseOrSomething = _math.Or(_math.False<T>(), _dummyOf.Condition(X));
            falseOrSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void False_Or_Something__IsSomething()
        {
            var falseOrSomething = _math.Or(_math.False<T>(), _dummyOf.Condition(X));
            
            Assert.That(falseOrSomething,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void Something_Or_False_ButNotFalse__Is_Not_PredictableFalse(
            [Values(true, null)] bool? another)
        {
            var somethingOrFalse = _math.Or(Some(another), _math.False<T>());
            var isPredictableFalse = _math.IsPredictableFalse(somethingOrFalse);

            Assert.That(isPredictableFalse,
                Is.False);
        }

        [Test]
        public void Something_ButNotTrue_Or_False__Is_Not_PredictableTrue(
            [Values(false, null)] bool? another)
        {
            var somethingOrFalse = _math.Or(Some(another), _math.False<T>());
            var isPredictableTrue = _math.IsPredictableTrue(somethingOrFalse);

            Assert.That(isPredictableTrue,
                Is.False);
        }

        [Test]
        public void True_Or_False__IsPredictableTrue()
        {
            var trueOrFalse = _math.Or(_math.True<T>(), _math.False<T>());
            var isPredictableTrue = _math.IsPredictableTrue(trueOrFalse);

            Assert.That(isPredictableTrue);
        }

        [Test]
        public void Something_Or_False_Check__IsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var somethingOrFalse = _math.Or(_dummyOf.Condition(X), _math.False<T>());

            Assert.That(somethingOrFalse.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void Something_Or_False_Check__Calls_CheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var somethingOrFalse = _math.Or(_dummyOf.Condition(X), _math.False<T>());
            somethingOrFalse.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void Something_Or_False__IsSomething()
        {
            var somethingOrFalse = _math.Or(_dummyOf.Condition(X), _math.False<T>());

            Assert.That(somethingOrFalse,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void True_Or_Something__IsPredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var trueOrSomething = _math.Or(_math.True<T>(), Some(another));
            var predictableTrue = _math.IsPredictableTrue(trueOrSomething);

            Assert.That(predictableTrue);
        }

        [Test]
        public void True_Or_Something__Is_Not_PredictableFalse(
            [Values(false, true, null)] bool? another)
        {
            var trueOrSomething = _math.Or(_math.True<T>(), Some(another));
            var predictableFalse = _math.IsPredictableFalse(trueOrSomething);

            Assert.That(predictableFalse,
                Is.False);
        }

        [Test]
        public void True_Or_Something_Check__IsTrue(
            [Values(false, true, null)] bool? another)
        {
            var trueOrSomething = _math.Or(_math.True<T>(), Some(another));

            Assert.That(trueOrSomething.Check(_token));
        }

        [Test]
        public void True_Or_Something_Check__DoesNotCall_CheckFromSomething()
        {
            var trueOrSomething = _math.Or(_math.True<T>(), _dummyOf.Condition(X));
            trueOrSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void Something_Or_True__IsPredictableTrue(
            [Values(false, true, null)] bool? another)
        {
            var somethingOrTrue = _math.Or(Some(another), _math.True<T>());
            var predictableTrue = _math.IsPredictableTrue(somethingOrTrue);

            Assert.That(predictableTrue);
        }

        [Test]
        public void Something_Or_True__Is_Not_PredictableFalse(
            [Values(false, true, null)] bool? another)
        {
            var somethingOrTrue = _math.Or(Some(another), _math.True<T>());
            var predictableFalse = _math.IsPredictableFalse(somethingOrTrue);

            Assert.That(predictableFalse,
                Is.False);
        }

        [Test]
        public void Something_Or_True_Check__IsTrue(
            [Values(false, true, null)] bool? another)
        {
            var somethingOrTrue = _math.Or(Some(another), _math.True<T>());

            Assert.That(somethingOrTrue.Check(_token));
        }

        [Test]
        public void Something_Or_True_Check__DoesNotCall_CheckFromSomething()
        {
            var somethingOrTrue = _math.Or(_dummyOf.Condition(X), _math.True<T>());
            somethingOrTrue.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasNeverChecked);
        }

        [Test]
        public void Something_And_True__IsSomething()
        {
            var somethingAndTrue = _math.And(_dummyOf.Condition(X), _math.True<T>());
            
            Assert.That(somethingAndTrue, 
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void True_And_Something__IsSomething()
        {
            var trueAndSomething = _math.And(_math.True<T>(), _dummyOf.Condition(X));

            Assert.That(trueAndSomething,
                Is.SameAs(_dummyOf.Condition(X)));
        }

        [Test]
        public void True_And_Something_Check__IsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var trueAndSomething = _math.And(_math.True<T>(), _dummyOf.Condition(X));

            Assert.That(trueAndSomething.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void True_And_Something_Check__Calls_CheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var trueAndSomething = _math.And(_math.True<T>(), _dummyOf.Condition(X));
            trueAndSomething.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void Something_And_True_Check__IsSomethingCheckResult(
            [Values(false, true)] bool anotherCheckResult)
        {
            _dummyOf.Condition(X).Returns(anotherCheckResult);
            var somethingAndTrue = _math.And(_dummyOf.Condition(X), _math.True<T>());

            Assert.That(somethingAndTrue.Check(_token),
                Is.EqualTo(anotherCheckResult));
        }

        [Test]
        public void Something_And_True_Check__Calls_CheckFromSomething()
        {
            _dummyOf.Condition(X).Returns(true);

            var somethingAndTrue = _math.And(_dummyOf.Condition(X), _math.True<T>());
            somethingAndTrue.Check(_token);

            Assert.That(_dummyOf.Condition(X).WasCheckedOnce);
        }

        [Test]
        public void NotTrue__IsPredictableFalse()
        {
            var notTrue = _math.Not(_math.True<T>());

            Assert.That(_math.IsPredictableFalse(notTrue));
        }

        [Test]
        public void NotTrue__Is_Not_PredictableTrue()
        {
            var notTrue = _math.Not(_math.True<T>());
            
            Assert.That(_math.IsPredictableTrue(notTrue),
                Is.False);
        }

        [Test]
        public void NotFalse__IsPredictableTrue()
        {
            var notFalse = _math.Not(_math.False<T>());

            Assert.That(_math.IsPredictableTrue(notFalse));
        }

        [Test]
        public void NotFalse__Is_Not_PredictableFalse()
        {
            var notFalse = _math.Not(_math.False<T>());
            
            Assert.That(_math.IsPredictableFalse(notFalse), 
                Is.False);
        }

        [Test]
        public void NotTrue_Check__IsFalse()
        {
            var notTrue = _math.Not(_math.True<T>());

            Assert.That(notTrue.Check(_token),
                Is.False);
        }

        [Test]
        public void NotFalse_Check__IsTrue()
        {
            var notFalse = _math.Not(_math.False<T>());
            
            Assert.That(notFalse.Check(_token));
        }

        [Test]
        public void MakeCondition__Calls_EncapsulatedFunc()
        {
            bool called = false;
            var condition = _math.MakeCondition<T>(_ => called = true);

            Assert.Multiple(() =>
            {
                Assert.That(condition.Check(_token));
                Assert.That(called);
            });
        }

        [Test]
        public void MakeCondition__Returns_EncapsulatedFuncResult(
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
