namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using Moq;

    public static partial class Dummy
    {
        public class ConditionMath<T>(
                ICollection<Condition<T>, ConditionIndex>.Mutable conditions,
                T token)
            : Mock<IConditionMath>
        {
            public void Setup__MakeCondition(ConditionIndex i)
            {
                Func<T, bool>? predicate = default;

                Setup(o => o.MakeCondition(It.IsAny<Func<T, bool>>()))
               .Returns(conditions.Get(i))
               .Callback((Func<T, bool> f) => predicate = f);
                conditions.Get(i).SetReturn(() => predicate!(token));
            }

            public void Setup__True(ConditionIndex returns) =>
                 Setup(o => o.True<T>())
                .Returns(conditions.Get(returns));

            public void Setup__False(ConditionIndex returns) =>
                 Setup(o => o.False<T>())
                .Returns(conditions.Get(returns));

            public void Setup__IsPredictableTrue(ConditionIndex i, bool returns) =>
                 Setup(o => o.IsPredictableTrue(conditions.Get(i)))
                .Returns(returns);

            public void Setup__IsPredictableFalse(ConditionIndex i, bool returns) =>
                 Setup(o => o.IsPredictableFalse(conditions.Get(i)))
                .Returns(returns);

            public void Setup__Or(
                ConditionIndex first,
                ConditionIndex second,
                ConditionIndex returns) =>
                     Setup(o => o.Or(
                         conditions.Get(first),
                         conditions.Get(second)))
                    .Returns(conditions.Get(returns));

            public void Setup__And__ForAny(
                ConditionIndex returns) =>
                     Setup(o => o.And(
                        It.IsAny<ICondition<T>>(),
                        It.IsAny<ICondition<T>>()))
                    .Returns(conditions.Get(returns));

            public void Setup__And(
                ConditionIndex first,
                ConditionIndex second,
                ConditionIndex returns) =>
                     Setup(o => o.And(
                        conditions.Get(first),
                        conditions.Get(second)))
                    .Returns(conditions.Get(returns));

            public void Setup__Not(
                ConditionIndex i,
                ConditionIndex returns) =>
                     Setup(o => o.Not(conditions.Get(i)))
                    .Returns(conditions.Get(returns));
        }
    }
}
