namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using Moq;

    [TestFixture]
    public class ConditionMathTest
    {
        public class Base { }
        public class Derived : Base { }

        public class Conditions
        {
            public required IReadOnlyDictionary<string, ICondition<int>> ForInt { get; init; }
            public required IReadOnlyDictionary<string, ICondition<Base>> ForBase { get; init; }
            public required IReadOnlyDictionary<string, ICondition<Derived>> ForDerived { get; init; }
        }

        IConditionMath _math;

        const string True = "true", False = "false", Mock = "mock";
        Conditions _conditions;

        [SetUp]
        public void Setup()
        {
            _math = new ConditionMath();
            _conditions = new Conditions
            {
                ForInt = new Dictionary<string, ICondition<int>>
                {
                    { True, _math.True<int>() },
                    { False, _math.False<int>() },
                    { Mock, new Mock<ICondition<int>>().Object }
                },
                ForBase = new Dictionary<string, ICondition<Base>>
                {
                    { True, _math.True<Base>() },
                    { False, _math.False<Base>() }
                },
                ForDerived = new Dictionary<string, ICondition<Derived>>
                {
                    { True, _math.True<Derived>() },
                    { False, _math.False<Derived>() },
                    { Mock, new Mock<ICondition<Derived>>().Object }
                },
            };
        }

        [Test]
        public void TrueCheckReturnsTrue()
        {
            var result = _conditions.ForInt[True].Check(678);
            Assert.IsTrue(result);
        }

        [Test]
        public void FalseCheckReturnsTrue()
        {
            var result = _conditions.ForInt[False].Check(903);   
            Assert.IsFalse(result);
        }

        [Test]
        public void TrueIsPredictableTrue()
        {
            var isPredictableTrue = 
                _math.IsPredictableTrue(_conditions.ForInt[True]);   
            
            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void TrueIsNotPredictableFalse()
        {
            var isPredictableFalse =
                _math.IsPredictableFalse(_conditions.ForInt[True]);
            
            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void FalseIsPredictableFalse()
        {
            var isPredictableFalse = 
                _math.IsPredictableFalse(_conditions.ForInt[False]);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void FalseIsNotPredictableTrue()
        {
            var isPredictableTrue = 
                _math.IsPredictableTrue(_conditions.ForInt[False]);

            Assert.IsFalse(isPredictableTrue);
        }


        [Test]
        public void BaseTrueIsPredictableTrueAsDerived()
        {
            var isPredictableTrue =
                _math.IsPredictableTrue<Derived>(_conditions.ForBase[True]);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void BaseTrueIsNotPredictableFalseAsDerived()
        {
            var isPredictableFalse =
                _math.IsPredictableFalse<Derived>(_conditions.ForBase[True]);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void BaseFalseIsNotPredictableTrueAsDerived()
        {
            var isPredictableTrue =
                _math.IsPredictableTrue<Derived>(_conditions.ForBase[False]);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void BaseFalseIsPredictableFalseAsDerived()
        {
            var isPredictableFalse =
                _math.IsPredictableFalse<Derived>(_conditions.ForBase[False]);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void FalseAndSomethingIsPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var falseAndSomething = _math.And(
                _math.False<int>(),
                _conditions.ForInt[something]);

            var isPredictableFalse = _math.IsPredictableFalse(falseAndSomething);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void BaseFalseAndSomethingDerivedIsPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var falseAndSomething = _math.And(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            var isPredictableFalse = _math.IsPredictableFalse(falseAndSomething);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void FalseAndSomethingIsNotPredictableTrue(
            [Values(False, True, Mock)] string something)
        {            
            var falseAndSomething = _math.And(
                _math.False<int>(),
                _conditions.ForInt[something]);

            var isPredictableTrue = _math.IsPredictableTrue(falseAndSomething);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void BaseFalseAndSomethingDerivedIsNotPredictableTrue(
            [Values(False, True, Mock)] string something)
        {
            var falseAndSomething = _math.And(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            var isPredictableTrue = _math.IsPredictableTrue(falseAndSomething);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void FalseAndSomethingCheckIsFalse(
            [Values(False, True, Mock)] string something)
        {
            var falseAndSomething = _math.And(
                _math.False<int>(),
                _conditions.ForInt[something]);

            var result = falseAndSomething.Check(5346);

            Assert.IsFalse(result);
        }

        [Test]
        public void BaseFalseAndSomethingDerivedCheckIsFalse(
            [Values(False, True, Mock)] string something)
        {
            var falseAndSomething = _math.And(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            var result = falseAndSomething.Check(new Derived());

            Assert.IsFalse(result);
        }

        [Test]
        public void SomethingAndFalseIsPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForInt[something],
                _math.False<int>());

            var isPredictableFalse = _math.IsPredictableFalse(somethingAndFalse);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void SomethingDarivedAndBaseFalseIsPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForDerived[something],
                _conditions.ForBase[False]);

            var isPredictableFalse = _math.IsPredictableFalse(somethingAndFalse);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void SomethingAndFalseIsNotPredictableTrue(
            [Values(False, True, Mock)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForInt[something],
                _math.False<int>());

            var isPredictableTrue = _math.IsPredictableTrue(somethingAndFalse);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void SomethingDerivedAndBaseFalseIsNotPredictableTrue(
            [Values(False, True, Mock)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForDerived[something],
                _conditions.ForBase[False]);

            var isPredictableTrue = _math.IsPredictableTrue(somethingAndFalse);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void SomethingAndFalseCheckIsFalse(
            [Values(False, True, Mock)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForInt[something],
                _math.False<int>());

            var result = somethingAndFalse.Check(628);

            Assert.IsFalse(result);
        }

        [Test]
        public void SomethingDerivedAndBaseFalseCheckIsFalse(
            [Values(False, True, Mock)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForDerived[something],
                _conditions.ForBase[False]);

            var result = somethingAndFalse.Check(new Derived());

            Assert.IsFalse(result);
        }

        [Test]
        public void SomethingOrFalseIsSomething()
        {
            var somethingOrFalse = _math.Or(
                _conditions.ForInt[Mock],
                _conditions.ForInt[False]);

            Assert.That(somethingOrFalse,
                Is.SameAs(_conditions.ForInt[Mock]));
        }

        [Test]
        public void SomethingDerivedOrBaseFalseIsSomethingDerived()
        {
            var somethingOrFalse = _math.Or(
                _conditions.ForDerived[Mock],
                _conditions.ForBase[False]);

            Assert.That(somethingOrFalse,
                Is.SameAs(_conditions.ForDerived[Mock]));
        }

        [Test]
        public void SomethingOrFalseCheckIsSomethingCheck(
            [Values(false, true)] bool somethingCheckResult)
        {
            const int arg = 798;
            var something = new Mock<ICondition<int>>();
            something
                .Setup(o => o.Check(arg))
                .Returns(somethingCheckResult);

            var somethingOrFalse = _math.Or(
                something.Object,
                _conditions.ForInt[False]);

            var result = somethingOrFalse.Check(arg);

            Assert.That(result, 
                Is.EqualTo(somethingCheckResult));
        }

        [Test]
        public void SomethingDerivedOrBaseFalseCheckIsSomethingDerivedCheck(
            [Values(false, true)] bool somethingCheckResult)
        {
            var arg = new Derived();
            var something = new Mock<ICondition<Derived>>();
            something
                .Setup(o => o.Check(arg))
                .Returns(somethingCheckResult);

            var somethingOrFalse = _math.Or(
                something.Object,
                _conditions.ForBase[False]);

            var result = somethingOrFalse.Check(arg);

            Assert.That(result, 
                Is.EqualTo(somethingCheckResult));
        }

        [Test]
        public void FalseOrSomethingIsSomething(
            [Values(False, True, Mock)] string something)
        {
            var falseOrSomething = _math.Or(
                _math.False<int>(),
                _conditions.ForInt[something]);

            Assert.That(falseOrSomething, 
                Is.SameAs(_conditions.ForInt[something]));
        }

        [Test]
        public void BaseFalseOrSomethingDerivedIsSomethingDerived(
            [Values(False, True, Mock)] string something)
        {
            var falseOrSomething = _math.Or(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            Assert.That(falseOrSomething,
                Is.SameAs(_conditions.ForDerived[something]));
        }

        [Test]
        public void TrueOrSomethingIsPredictableTrue(
            [Values(False, True, Mock)] string something)
        {
            var trueOrSomething = _math.Or(
                _math.True<int>(),
                _conditions.ForInt[something]);

            var isPredictableTrue = _math.IsPredictableTrue(trueOrSomething);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void BaseTrueOrSomethingDerivedIsPredictableTrue(
            [Values(False, True, Mock)] string something)
        {
            var trueOrSomething = _math.Or(
                _conditions.ForBase[True],
                _conditions.ForDerived[something]);

            var isPredictableTrue = _math.IsPredictableTrue(trueOrSomething);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void TrueOrSomethingIsNotPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var trueOrSomething = _math.Or(
                _math.True<int>(),
                _conditions.ForInt[something]);

            var isPredictableFalse = _math.IsPredictableFalse(trueOrSomething);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void BaseTrueOrSomethingDerivedIsNotPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var trueOrSomething = _math.Or(
                _conditions.ForBase[True],
                _conditions.ForDerived[something]);

            var isPredictableFalse = _math.IsPredictableFalse(trueOrSomething);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void TrueOrSomethingCheckIsTrue(
            [Values(False, True, Mock)] string something)
        {
            var trueOrSomething = _math.Or(
                _math.True<int>(),
                _conditions.ForInt[something]);

            var result = trueOrSomething.Check(325);

            Assert.IsTrue(result);
        }


        [Test]
        public void BaseTrueOrSomethingDerivedCheckIsTrue(
            [Values(False, True, Mock)] string something)
        {
            var trueOrSomething = _math.Or(
                _conditions.ForBase[True],
                _conditions.ForDerived[something]);

            var result = trueOrSomething.Check(new Derived());

            Assert.IsTrue(result);
        }

        [Test]
        public void SomethingOrTrueIsPredictableTrue(
            [Values(False, True, Mock)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForInt[something],
                _math.True<int>());

            var isPredictableTrue = _math.IsPredictableTrue(somethingOrTrue);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void SomethingDerivedOrBaseTrueIsPredictableTrue(
            [Values(False, True, Mock)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForDerived[something],
                _conditions.ForBase[True]);

            var isPredictableTrue = _math.IsPredictableTrue(somethingOrTrue);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void SomethingOrTrueIsNotPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForInt[something],
                _math.True<int>());

            var isPredictableFalse = _math.IsPredictableFalse(somethingOrTrue);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void SomethingDerivedOrBaseTrueIsNotPredictableFalse(
            [Values(False, True, Mock)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForDerived[something],
                _conditions.ForBase[True]);

            var isPredictableFalse = _math.IsPredictableFalse(somethingOrTrue);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void SomethingOrTrueCheckIsTrue(
            [Values(False, True, Mock)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForInt[something],
                _math.True<int>());

            var result = somethingOrTrue.Check(325);

            Assert.IsTrue(result);
        }

        [Test]
        public void SomethingDerivedOrBaseTrueCheckIsTrue(
            [Values(False, True, Mock)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForDerived[something],
                _conditions.ForBase[True]);

            var result = somethingOrTrue.Check(new Derived());

            Assert.IsTrue(result);
        }

        [Test]
        public void SomethingAndTrueIsSomething()
        {
            var somethingAndTrue = _math.And(
                _conditions.ForInt[Mock], 
                _conditions.ForInt[True]);
            
            Assert.That(somethingAndTrue, 
                Is.SameAs(_conditions.ForInt[Mock]));
        }

        [Test]
        public void SomethingDerivedAndBaseTrueIsSomethingDerived()
        {
            var somethingAndTrue = _math.And(
                _conditions.ForDerived[Mock],
                _conditions.ForBase[True]);

            Assert.That(somethingAndTrue,
                Is.SameAs(_conditions.ForDerived[Mock]));
        }

        [Test]
        public void TrueAndSomethingIsSomething()
        {
            var trueAndSomething = _math.And(
                _conditions.ForInt[True],
                _conditions.ForInt[Mock]);

            Assert.That(trueAndSomething,
                Is.SameAs(_conditions.ForInt[Mock]));
        }

        [Test]
        public void BaseTrueAndSomethingDerivedIsSomethingDerived()
        {
            var trueAndSomething = _math.And(
                _conditions.ForBase[True],
                _conditions.ForDerived[Mock]);

            Assert.That(trueAndSomething,
                Is.SameAs(_conditions.ForDerived[Mock]));
        }

        [Test]
        public void NotTrueIsPredictableFalse()
        {
            var notTrue = _math.Not(_conditions.ForInt[True]);
            var isPredictableFalse = _math.IsPredictableFalse(notTrue);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void NotOfBaseTrueIsPredictableFalseByDerivedCheck()
        {
            var notTrue = _math.Not(_conditions.ForBase[True]);
            var isPredictableFalse = _math.IsPredictableFalse<Derived>(notTrue);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void DerivedNotOfBaseTrueIsPredictableFalse()
        {
            var notTrue = _math.Not<Derived>(_conditions.ForBase[True]);
            var isPredictableFalse = _math.IsPredictableFalse(notTrue);

            Assert.IsTrue(isPredictableFalse);
        }

        [Test]
        public void NotTrueIsNotPredictableTrue()
        {
            var notTrue = _math.Not(_conditions.ForInt[True]);
            var isPredictableTrue = _math.IsPredictableTrue(notTrue);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void NotOfBaseTrueIsNotPredictableTrueByDerivedCheck()
        {
            var notTrue = _math.Not(_conditions.ForBase[True]);
            var isPredictableTrue = _math.IsPredictableTrue<Derived>(notTrue);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void DerivedNotOfBaseTrueIsNotPredictableTrue()
        {
            var notTrue = _math.Not<Derived>(_conditions.ForBase[True]);
            var isPredictableTrue = _math.IsPredictableTrue(notTrue);

            Assert.IsFalse(isPredictableTrue);
        }

        [Test]
        public void NotFalseIsPredictableTrue()
        {
            var notFalse = _math.Not(_conditions.ForInt[False]);
            var isPredictableTrue = _math.IsPredictableTrue(notFalse);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void DerivedNotOfBaseFalseIsPredictableTrue()
        {
            var notFalse = _math.Not<Derived>(_conditions.ForBase[False]);
            var isPredictableTrue = _math.IsPredictableTrue(notFalse);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void NotOfBaseFalseIsPredictableTrueByDerivedCheck()
        {
            var notFalse = _math.Not(_conditions.ForBase[False]);
            var isPredictableTrue = _math.IsPredictableTrue<Derived>(notFalse);

            Assert.IsTrue(isPredictableTrue);
        }

        [Test]
        public void NotFalseIsNotPredictableFalse()
        {
            var notFalse = _math.Not(_conditions.ForInt[False]);
            var isPredictableFalse = _math.IsPredictableFalse(notFalse);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void NotOfBaseFalseIsNotPredictableFalseByDerivedCheck()
        {
            var notFalse = _math.Not(_conditions.ForBase[False]);
            var isPredictableFalse = _math.IsPredictableFalse<Derived>(notFalse);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void DerivedNotOfBaseFalseIsNotPredictableFalse()
        {
            var notFalse = _math.Not<Derived>(_conditions.ForBase[False]);
            var isPredictableFalse = _math.IsPredictableFalse(notFalse);

            Assert.IsFalse(isPredictableFalse);
        }

        [Test]
        public void NotTrueCheckIsFalse()
        {
            var notTrue = _math.Not(_conditions.ForInt[True]);
            var result = notTrue.Check(567);

            Assert.IsFalse(result);
        }

        [Test]
        public void NotFalseCheckIsTrue()
        {
            var notFalse = _math.Not(_conditions.ForInt[False]);
            var result = notFalse.Check(567);

            Assert.IsTrue(result);
        }
    }
}
