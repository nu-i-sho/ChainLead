﻿namespace ChainLead.Contracts
{
    public interface IConditionMath
    {
        ICondition<T> True<T>();

        ICondition<T> False<T>();

        ICondition<T> MakeCondition<T>(Func<T, bool> predicate);

        bool IsPredictableTrue<T>(ICondition<T> condition);

        bool IsPredictableFalse<T>(ICondition<T> condition);

        ICondition<T> And<T>(ICondition<T> a, ICondition<T> b);

        ICondition<T> Or<T>(ICondition<T> a, ICondition<T> b);

        ICondition<T> Not<T>(ICondition<T> handler);
    }
}
