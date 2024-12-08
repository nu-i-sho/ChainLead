namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using Moq;

    [TestFixture]
    public class ConditionMathTest
    {
        public class Base;
        public class Derived : Base;

        public class Conditions
        {
            public required IReadOnlyDictionary<string, ICondition<int>> ForInt { get; init; }
            public required IReadOnlyDictionary<string, ICondition<Base>> ForBase { get; init; }
            public required IReadOnlyDictionary<string, ICondition<Derived>> ForDerived { get; init; }
        }

        IConditionMath _math;

        const int Arg = 7938;
        const string True = "true", False = "false", A = "A", B = "B";

        Dictionary<string, Mock<ICondition<int>>> _conditionMocks; 
        Conditions _conditions;

        [SetUp]
        public void Setup()
        {
            _math = new ConditionMath();

            _conditionMocks = new Dictionary<string, Mock<ICondition<int>>>
            {
                { A, new Mock<ICondition<int>>() { Name = A } },
                { B, new Mock<ICondition<int>>() { Name = B } },
            };

            _conditions = new Conditions
            {
                ForInt = new Dictionary<string, ICondition<int>>
                {
                    { True, _math.True<int>() },
                    { False, _math.False<int>() },
                    { A, _conditionMocks[A].Object }
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
                    { A, new Mock<ICondition<Derived>>().Object }
                },
            };
        }

        [Test]
        public void TrueCheckReturnsTrue()
        {
            var result = _conditions.ForInt[True].Check(Arg);
            Assert.That(result, Is.True);
        }

        [Test]
        public void FalseCheckReturnsTrue()
        {
            var result = _conditions.ForInt[False].Check(Arg);   
            Assert.That(result, Is.False);
        }

        [Test]
        public void TrueIsPredictableTrue()
        {
            var predictableTrue = 
                _math.IsPredictableTrue(_conditions.ForInt[True]);   
            
            Assert.That(predictableTrue, Is.True);
        }

        [Test]
        public void TrueIsNotPredictableFalse()
        {
            var predictableFalse =
                _math.IsPredictableFalse(_conditions.ForInt[True]);
            
            Assert.That(predictableFalse, Is.False);
        }

        [Test]
        public void FalseIsPredictableFalse()
        {
            var predictableFalse = 
                _math.IsPredictableFalse(_conditions.ForInt[False]);

            Assert.That(predictableFalse, Is.True);
        }

        [Test]
        public void FalseIsNotPredictableTrue()
        {
            var predictableTrue = 
                _math.IsPredictableTrue(_conditions.ForInt[False]);

            Assert.That(predictableTrue, Is.False);
        }


        [Test]
        public void BaseTrueIsPredictableTrueAsDerived()
        {
            var predictableTrue =
                _math.IsPredictableTrue<Derived>(_conditions.ForBase[True]);

            Assert.That(predictableTrue, Is.True);
        }

        [Test]
        public void BaseTrueIsNotPredictableFalseAsDerived()
        {
            var predictableFalse =
                _math.IsPredictableFalse<Derived>(_conditions.ForBase[True]);

            Assert.That(predictableFalse, Is.False);
        }

        [Test]
        public void BaseFalseIsNotPredictableTrueAsDerived()
        {
            var predictableTrue =
                _math.IsPredictableTrue<Derived>(_conditions.ForBase[False]);

            Assert.That(predictableTrue, Is.False);
        }

        [Test]
        public void BaseFalseIsPredictableFalseAsDerived()
        {
            var predictableFalse =
                _math.IsPredictableFalse<Derived>(_conditions.ForBase[False]);

            Assert.That(predictableFalse, Is.True);
        }

        [Test]
        public void FalseAndSomethingIsPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var falseAndSomething = _math.And(
                _math.False<int>(),
                _conditions.ForInt[something]);

            var predictableFalse = _math.IsPredictableFalse(falseAndSomething);

            Assert.That(predictableFalse, Is.True);
        }

        [Test]
        public void BaseFalseAndSomethingDerivedIsPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var falseAndSomething = _math.And(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            var predictableFalse = _math.IsPredictableFalse(falseAndSomething);

            Assert.That(predictableFalse, Is.True);
        }

        [Test]
        public void FalseAndSomethingIsNotPredictableTrue(
            [Values(False, True, A)] string something)
        {            
            var falseAndSomething = _math.And(
                _math.False<int>(),
                _conditions.ForInt[something]);

            var predictableTrue = _math.IsPredictableTrue(falseAndSomething);

            Assert.That(predictableTrue, Is.False);
        }

        [Test]
        public void BaseFalseAndSomethingDerivedIsNotPredictableTrue(
            [Values(False, True, A)] string something)
        {
            var falseAndSomething = _math.And(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            var predictableTrue = _math.IsPredictableTrue(falseAndSomething);

            Assert.That(predictableTrue, Is.False);
        }

        [Test]
        public void FalseAndSomethingCheckIsFalse(
            [Values(False, True, A)] string something)
        {
            var falseAndSomething = _math.And(
                _math.False<int>(),
                _conditions.ForInt[something]);

            var result = falseAndSomething.Check(5346);

            Assert.That(result, Is.False);
        }

        [Test]
        public void BaseFalseAndSomethingDerivedCheckIsFalse(
            [Values(False, True, A)] string something)
        {
            var falseAndSomething = _math.And(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            var result = falseAndSomething.Check(new Derived());

            Assert.That(result, Is.False);
        }

        [Test]
        public void SomethingAndFalseIsPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForInt[something],
                _math.False<int>());

            var predictableFalse = _math.IsPredictableFalse(somethingAndFalse);

            Assert.That(predictableFalse, Is.True);
        }

        [Test]
        public void SomethingDerivedAndBaseFalseIsPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForDerived[something],
                _conditions.ForBase[False]);

            var predictableFalse = _math.IsPredictableFalse(somethingAndFalse);

            Assert.That(predictableFalse, Is.True);
        }

        [Test]
        public void SomethingAndFalseIsNotPredictableTrue(
            [Values(False, True, A)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForInt[something],
                _math.False<int>());

            var isPredictableTrue = _math.IsPredictableTrue(somethingAndFalse);

            Assert.That(isPredictableTrue, Is.False);
        }

        [Test]
        public void SomethingDerivedAndBaseFalseIsNotPredictableTrue(
            [Values(False, True, A)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForDerived[something],
                _conditions.ForBase[False]);

            var predictableTrue = _math.IsPredictableTrue(somethingAndFalse);

            Assert.That(predictableTrue, Is.False);
        }

        [Test]
        public void SomethingAndFalseCheckIsFalse(
            [Values(False, True, A)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForInt[something],
                _math.False<int>());

            var result = somethingAndFalse.Check(Arg);

            Assert.That(result, Is.False);
        }

        [Test]
        public void SomethingDerivedAndBaseFalseCheckIsFalse(
            [Values(False, True, A)] string something)
        {
            var somethingAndFalse = _math.And(
                _conditions.ForDerived[something],
                _conditions.ForBase[False]);

            var result = somethingAndFalse.Check(new Derived());

            Assert.That(result, Is.False);
        }

        [Test]
        public void SomethingOrFalseIsSomething()
        {
            var somethingOrFalse = _math.Or(
                _conditions.ForInt[A],
                _conditions.ForInt[False]);

            Assert.That(somethingOrFalse,
                Is.SameAs(_conditions.ForInt[A]));
        }

        [Test]
        public void SomethingDerivedOrBaseFalseIsSomethingDerived()
        {
            var somethingOrFalse = _math.Or(
                _conditions.ForDerived[A],
                _conditions.ForBase[False]);

            Assert.That(somethingOrFalse,
                Is.SameAs(_conditions.ForDerived[A]));
        }

        [Test]
        public void SomethingOrFalseCheckIsSomethingCheck(
            [Values(false, true)] bool somethingCheckResult)
        {
            var something = _conditionMocks[A];
            something
                .Setup(o => o.Check(Arg))
                .Returns(somethingCheckResult);

            var somethingOrFalse = _math.Or(
                something.Object,
                _conditions.ForInt[False]);

            var result = somethingOrFalse.Check(Arg);

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
            [Values(False, True, A)] string something)
        {
            var falseOrSomething = _math.Or(
                _math.False<int>(),
                _conditions.ForInt[something]);

            Assert.That(falseOrSomething, 
                Is.SameAs(_conditions.ForInt[something]));
        }

        [Test]
        public void BaseFalseOrSomethingDerivedIsSomethingDerived(
            [Values(False, True, A)] string something)
        {
            var falseOrSomething = _math.Or(
                _conditions.ForBase[False],
                _conditions.ForDerived[something]);

            Assert.That(falseOrSomething,
                Is.SameAs(_conditions.ForDerived[something]));
        }

        [Test]
        public void TrueOrSomethingIsPredictableTrue(
            [Values(False, True, A)] string something)
        {
            var trueOrSomething = _math.Or(
                _math.True<int>(),
                _conditions.ForInt[something]);

            var predictableTrue = _math.IsPredictableTrue(trueOrSomething);

            Assert.That(predictableTrue, Is.True);
        }

        [Test]
        public void BaseTrueOrSomethingDerivedIsPredictableTrue(
            [Values(False, True, A)] string something)
        {
            var trueOrSomething = _math.Or(
                _conditions.ForBase[True],
                _conditions.ForDerived[something]);

            var predictableTrue = _math.IsPredictableTrue(trueOrSomething);

            Assert.That(predictableTrue, Is.True);
        }

        [Test]
        public void TrueOrSomethingIsNotPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var trueOrSomething = _math.Or(
                _math.True<int>(),
                _conditions.ForInt[something]);

            var predictableFalse = _math.IsPredictableFalse(trueOrSomething);

            Assert.That(predictableFalse, Is.False);
        }

        [Test]
        public void BaseTrueOrSomethingDerivedIsNotPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var trueOrSomething = _math.Or(
                _conditions.ForBase[True],
                _conditions.ForDerived[something]);

            var predictableFalse = _math.IsPredictableFalse(trueOrSomething);

            Assert.That(predictableFalse, Is.False);
        }

        [Test]
        public void TrueOrSomethingCheckIsTrue(
            [Values(False, True, A)] string something)
        {
            var trueOrSomething = _math.Or(
                _math.True<int>(),
                _conditions.ForInt[something]);

            var result = trueOrSomething.Check(325);

            Assert.That(result, Is.True);
        }

        [Test]
        public void BaseTrueOrSomethingDerivedCheckIsTrue(
            [Values(False, True, A)] string something)
        {
            var trueOrSomething = _math.Or(
                _conditions.ForBase[True],
                _conditions.ForDerived[something]);

            var result = trueOrSomething.Check(new Derived());

            Assert.That(result, Is.True);
        }

        [Test]
        public void SomethingOrTrueIsPredictableTrue(
            [Values(False, True, A)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForInt[something],
                _math.True<int>());

            var predictableTrue = _math.IsPredictableTrue(somethingOrTrue);

            Assert.That(predictableTrue, Is.True);
        }

        [Test]
        public void SomethingDerivedOrBaseTrueIsPredictableTrue(
            [Values(False, True, A)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForDerived[something],
                _conditions.ForBase[True]);

            var predictableTrue = _math.IsPredictableTrue(somethingOrTrue);

            Assert.That(predictableTrue, Is.True);
        }

        [Test]
        public void SomethingOrTrueIsNotPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForInt[something],
                _math.True<int>());

            var predictableFalse = _math.IsPredictableFalse(somethingOrTrue);

            Assert.That(predictableFalse, Is.False);
        }

        [Test]
        public void SomethingDerivedOrBaseTrueIsNotPredictableFalse(
            [Values(False, True, A)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForDerived[something],
                _conditions.ForBase[True]);

            var predictableFalse = _math.IsPredictableFalse(somethingOrTrue);

            Assert.That(predictableFalse, Is.False);
        }

        [Test]
        public void SomethingOrTrueCheckIsTrue(
            [Values(False, True, A)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForInt[something],
                _math.True<int>());

            var result = somethingOrTrue.Check(Arg);

            Assert.That(result, Is.True);
        }

        [Test]
        public void SomethingDerivedOrBaseTrueCheckIsTrue(
            [Values(False, True, A)] string something)
        {
            var somethingOrTrue = _math.Or(
                _conditions.ForDerived[something],
                _conditions.ForBase[True]);

            var result = somethingOrTrue.Check(new Derived());

            Assert.That(result, Is.True);
        }

        [Test]
        public void SomethingAndTrueIsSomething()
        {
            var somethingAndTrue = _math.And(
                _conditions.ForInt[A], 
                _conditions.ForInt[True]);
            
            Assert.That(somethingAndTrue, 
                Is.SameAs(_conditions.ForInt[A]));
        }

        [Test]
        public void SomethingDerivedAndBaseTrueIsSomethingDerived()
        {
            var somethingAndTrue = _math.And(
                _conditions.ForDerived[A],
                _conditions.ForBase[True]);

            Assert.That(somethingAndTrue,
                Is.SameAs(_conditions.ForDerived[A]));
        }

        [Test]
        public void TrueAndSomethingIsSomething()
        {
            var trueAndSomething = _math.And(
                _conditions.ForInt[True],
                _conditions.ForInt[A]);

            Assert.That(trueAndSomething,
                Is.SameAs(_conditions.ForInt[A]));
        }

        [Test]
        public void BaseTrueAndSomethingDerivedIsSomethingDerived()
        {
            var trueAndSomething = _math.And(
                _conditions.ForBase[True],
                _conditions.ForDerived[A]);

            Assert.That(trueAndSomething,
                Is.SameAs(_conditions.ForDerived[A]));
        }

        [Test]
        public void NotTrueIsPredictableFalse()
        {
            var notTrue = _math.Not(_conditions.ForInt[True]);
            var predictableFalse = _math.IsPredictableFalse(notTrue);

            Assert.That(predictableFalse);
        }

        [Test]
        public void NotOfBaseTrueIsPredictableFalseByDerivedCheck()
        {
            var notTrue = _math.Not(_conditions.ForBase[True]);
            var predictableFalse = _math.IsPredictableFalse<Derived>(notTrue);

            Assert.That(predictableFalse);
        }

        [Test]
        public void DerivedNotOfBaseTrueIsPredictableFalse()
        {
            var notTrue = _math.Not<Derived>(_conditions.ForBase[True]);
            var predictableFalse = _math.IsPredictableFalse(notTrue);

            Assert.That(predictableFalse);
        }

        [Test]
        public void NotTrueIsNotPredictableTrue()
        {
            var notTrue = _math.Not(_conditions.ForInt[True]);
            var predictableTrue = _math.IsPredictableTrue(notTrue);

            Assert.That(predictableTrue,
                Is.False);
        }

        [Test]
        public void NotOfBaseTrueIsNotPredictableTrueByDerivedCheck()
        {
            var notTrue = _math.Not(_conditions.ForBase[True]);
            var predictableTrue = _math.IsPredictableTrue<Derived>(notTrue);

            Assert.That(predictableTrue,
                Is.False);
        }

        [Test]
        public void DerivedNotOfBaseTrueIsNotPredictableTrue()
        {
            var notTrue = _math.Not<Derived>(_conditions.ForBase[True]);
            var predictableTrue = _math.IsPredictableTrue(notTrue);

            Assert.That(predictableTrue, 
                Is.False);
        }

        [Test]
        public void NotFalseIsPredictableTrue()
        {
            var notFalse = _math.Not(_conditions.ForInt[False]);
            var predictableTrue = _math.IsPredictableTrue(notFalse);

            Assert.That(predictableTrue);
        }

        [Test]
        public void DerivedNotOfBaseFalseIsPredictableTrue()
        {
            var notFalse = _math.Not<Derived>(_conditions.ForBase[False]);
            var predictableTrue = _math.IsPredictableTrue(notFalse);

            Assert.That(predictableTrue);
        }

        [Test]
        public void NotOfBaseFalseIsPredictableTrueByDerivedCheck()
        {
            var notFalse = _math.Not(_conditions.ForBase[False]);
            var predictableTrue = _math.IsPredictableTrue<Derived>(notFalse);

            Assert.That(predictableTrue);
        }

        [Test]
        public void NotFalseIsNotPredictableFalse()
        {
            var notFalse = _math.Not(_conditions.ForInt[False]);
            var predictableFalse = _math.IsPredictableFalse(notFalse);

            Assert.That(predictableFalse, 
                Is.False);
        }

        [Test]
        public void NotOfBaseFalseIsNotPredictableFalseByDerivedCheck()
        {
            var notFalse = _math.Not(_conditions.ForBase[False]);
            var predictableFalse = _math.IsPredictableFalse<Derived>(notFalse);

            Assert.That(predictableFalse,
                Is.False);
        }

        [Test]
        public void DerivedNotOfBaseFalseIsNotPredictableFalse()
        {
            var notFalse = _math.Not<Derived>(_conditions.ForBase[False]);
            var predictableFalse = _math.IsPredictableFalse(notFalse);

            Assert.That(predictableFalse, Is.False);
        }

        [Test]
        public void NotTrueCheckIsFalse()
        {
            var notTrue = _math.Not(_conditions.ForInt[True]);
            var result = notTrue.Check(Arg);

            Assert.That(result, Is.False);
        }

        [Test]
        public void NotFalseCheckIsTrue()
        {
            var notFalse = _math.Not(_conditions.ForInt[False]);
            var result = notFalse.Check(Arg);

            Assert.That(result, Is.True);
        }

        [Test]
        public void MakeConditionCallsEncapsulatedFunc()
        {
            bool called = false;
            var condition = _math.MakeCondition<int>(_ => called = true);

            condition.Check(Arg);

            Assert.That(called, Is.True);
        }

        [Test]
        public void MakeConditionReturnsEncapsulatedFuncResult(
            [Values(false, true)] bool funcResult)
        {
            var condition = _math.MakeCondition<int>(_ => funcResult);
            var result = condition.Check(Arg);

            Assert.That(result, Is.EqualTo(funcResult));
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void NotConditionCheckTest(
            bool aResult,
            bool notAExpectedResult)
        {
            _conditionMocks[A]
                .Setup(o => o.Check(Arg))
                .Returns(aResult);

            var notA = _math.Not(_conditionMocks[A].Object);
            var notAResult = notA.Check(Arg);

            Assert.That(notAResult,
                Is.EqualTo(notAExpectedResult));
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void AndConditionCheckTest(
            bool aResult,
            bool bResult,
            bool abExpectedResult)
        {
            _conditionMocks[A]
                .Setup(o => o.Check(Arg))
                .Returns(aResult);

            _conditionMocks[B]
                .Setup(o => o.Check(Arg))
                .Returns(bResult);

            var ab = _math.And(
                _conditionMocks[A].Object,
                _conditionMocks[B].Object);

            var abResult = ab.Check(Arg);

            Assert.That(abResult, 
                Is.EqualTo(abExpectedResult));
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, true)]
        public void OrConditionCheckTest(
            bool aResult,
            bool bResult,
            bool abExpectedResult)
        {
            _conditionMocks[A]
                .Setup(o => o.Check(Arg))
                .Returns(aResult);

            _conditionMocks[B]
                .Setup(o => o.Check(Arg))
                .Returns(bResult);

            var ab = _math.Or(
                _conditionMocks[A].Object,
                _conditionMocks[B].Object);

            var abResult = ab.Check(Arg);

            Assert.That(abResult,
                Is.EqualTo(abExpectedResult));
        }
    }
}
