namespace ChainLead.Contracts
{
    public interface IConditionMath
    {
        ICondition<T> True<T>();

        ICondition<T> False<T>();

        ICondition<T> MakeCondition<T>(Func<T, bool> predicate);

        bool IsPredictableTrue<T>(ICondition<T> condition);

        bool IsPredictableFalse<T>(ICondition<T> condition);

        ICondition<T> And<T>(ICondition<T> left, ICondition<T> right);

        ICondition<T> Or<T>(ICondition<T> left, ICondition<T> right);

        ICondition<T> Not<T>(ICondition<T> condition);
    }
}
