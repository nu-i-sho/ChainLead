namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using Moq;

    public static partial class Dummy
    {
        public class ConditionMath<T>(
                ConditionCollection<T> conditions,
                T tokenArg)
            : Mock<IConditionMath>
        {
            public void Setup__MakeCondition(ConditionIndex i)
            {
                Func<T, bool>? predicate = default;

                Setup(o => o.MakeCondition(It.IsAny<Func<T, bool>>()))
               .Returns(conditions[i])
               .Callback((Func<T, bool> f) => predicate = f);
                conditions[i].SetReturn(() => predicate!(tokenArg));
            }

            public void Setup__True(ConditionIndex returns) =>
                 Setup(o => o.True<T>())
                .Returns(conditions[returns]);

            public void Setup__False(ConditionIndex returns) =>
                 Setup(o => o.False<T>())
                .Returns(conditions[returns]);

            public void Setup__IsPredictableTrue(ConditionIndex i, bool returns) =>
                 Setup(o => o.IsPredictableTrue(conditions[i]))
                .Returns(returns);

            public void Setup__IsPredictableFalse(ConditionIndex i, bool returns) =>
                 Setup(o => o.IsPredictableFalse(conditions[i]))
                .Returns(returns);

            public void Setup__Or(
                ConditionIndex first,
                ConditionIndex second,
                ConditionIndex returns) =>
                     Setup(o => o.Or(
                         conditions[first],
                         conditions[second]))
                    .Returns(conditions[returns]);

            public void Setup__And__ForAny(
                ConditionIndex returns) =>
                     Setup(o => o.And(
                        It.IsAny<ICondition<T>>(),
                        It.IsAny<ICondition<T>>()))
                    .Returns(conditions[returns]);

            public void Setup__And(
                ConditionIndex first,
                ConditionIndex second,
                ConditionIndex returns) =>
                     Setup(o => o.And(
                        conditions[first],
                        conditions[second]))
                    .Returns(conditions[returns]);

            public void Setup__Not(
                ConditionIndex i,
                ConditionIndex returns) =>
                     Setup(o => o.Not(conditions[i]))
                    .Returns(conditions[returns]);
        }
    }
}
