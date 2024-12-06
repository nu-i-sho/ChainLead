namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using Moq;

    public static partial class Dummy
    {
        public class ConditionMath<T>(
                Dummy.ConditionCollection<T> conditions,
                T tokenArg)
            : Mock<IConditionMath>
        {
            public void Setup__MakeCondition(Dummy.ConditionIndex i)
            {
                Func<T, bool>? predicate = default;

                Setup(o => o.MakeCondition(It.IsAny<Func<T, bool>>()))
               .Returns(conditions[i])
               .Callback((Func<T, bool> f) => predicate = f);
                conditions[i].SetReturn(() => predicate!(tokenArg));
            }

            public void Setup__True(Dummy.ConditionIndex returns) =>
                 Setup(o => o.True<T>())
                .Returns(conditions[returns]);

            public void Setup__False(Dummy.ConditionIndex returns) =>
                 Setup(o => o.False<T>())
                .Returns(conditions[returns]);

            public void Setup__IsPredictableTrue(Dummy.ConditionIndex i, bool returns) =>
                 Setup(o => o.IsPredictableTrue(conditions[i]))
                .Returns(returns);

            public void Setup__IsPredictableFalse(Dummy.ConditionIndex i, bool returns) =>
                 Setup(o => o.IsPredictableFalse(conditions[i]))
                .Returns(returns);

            public void Setup__Or(
                Dummy.ConditionIndex first,
                Dummy.ConditionIndex second,
                Dummy.ConditionIndex returns) =>
                     Setup(o => o.Or(
                         conditions[first],
                         conditions[second]))
                    .Returns(conditions[returns]);

            public void Setup__And__ForAny(
                Dummy.ConditionIndex returns) =>
                     Setup(o => o.And(
                        It.IsAny<ICondition<T>>(),
                        It.IsAny<ICondition<T>>()))
                    .Returns(conditions[returns]);

            public void Setup__And(
                Dummy.ConditionIndex first,
                Dummy.ConditionIndex second,
                Dummy.ConditionIndex returns) =>
                     Setup(o => o.And(
                        conditions[first],
                        conditions[second]))
                    .Returns(conditions[returns]);

            public void Setup__Not(
                Dummy.ConditionIndex i,
                Dummy.ConditionIndex returns) =>
                     Setup(o => o.Not(conditions[i]))
                    .Returns(conditions[returns]);
        }
    }
}
