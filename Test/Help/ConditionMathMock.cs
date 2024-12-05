namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using Moq;

    using static Constants;

    public class ConditionMathMock(DummyConditionsCollection conditions)
         : Mock<IConditionMath>
    {
        public void Setup__MakeCondition(ConditionIndex i)
        {
            Func<int, bool>? predicate = default;

            Setup(o => o.MakeCondition(It.IsAny<Func<int, bool>>()))
           .Returns(conditions[i].Object)
           .Callback((Func<int, bool> f) => predicate = f);
            conditions[i].SetReturn(() => predicate!(Arg));
        }

        public void Setup__True(ConditionIndex returns) =>
             Setup(o => o.True<int>())
            .Returns(conditions[returns].Object);

        public void Setup__False(ConditionIndex returns) =>
             Setup(o => o.False<int>())
            .Returns(conditions[returns].Object);

        public void Setup__IsPredictableTrue(ConditionIndex i, bool returns) =>
             Setup(o => o.IsPredictableTrue(conditions[i].Object))
            .Returns(returns);

        public void Setup__IsPredictableFalse(ConditionIndex i, bool returns) =>
             Setup(o => o.IsPredictableFalse(conditions[i].Object))
            .Returns(returns);

        public void Setup__Or(
            ConditionIndex first,
            ConditionIndex second,
            ConditionIndex returns) =>
                 Setup(o => o.Or(
                    conditions[first].Object,
                    conditions[second].Object))
                .Returns(conditions[returns].Object);

        public void Setup__And__ForAny(
            ConditionIndex returns) =>
                 Setup(o => o.And(
                    It.IsAny<ICondition<int>>(),
                    It.IsAny<ICondition<int>>()))
                .Returns(conditions[returns].Object);

        public void Setup__And(
            ConditionIndex first,
            ConditionIndex second,
            ConditionIndex returns) =>
                 Setup(o => o.And(
                    conditions[first].Object,
                    conditions[second].Object))
                .Returns(conditions[returns].Object);

        public void Setup__Not(
            ConditionIndex i,
            ConditionIndex returns) =>
                 Setup(o => o.Not(conditions[i].Object))
                .Returns(conditions[returns].Object);
    }
}
