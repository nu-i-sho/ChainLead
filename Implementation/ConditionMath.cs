namespace ChainLead.Implementation
{
    //// DO NOT using ChainLead.Contracts.Syntax;
    using ChainLead.Contracts;
    using System;

    public class ConditionMath : IConditionMath
    {
        public ICondition<T> False<T>() =>
                new False<T>();

        public ICondition<T> True<T>() =>
                new True<T>();

        public ICondition<T> MakeCondition<T>(
            Func<T, bool> predicate) =>
                new Condition<T>(predicate);

        public bool IsPredictableTrue<T>(
            ICondition<T> condition) =>
                condition is ITrue<T>;

        public bool IsPredictableFalse<T>(
            ICondition<T> condition) =>
                condition is IFalse<T>;

        public ICondition<T> And<T>(
            ICondition<T> a,
            ICondition<T> b)
        {
            if (IsPredictableFalse(a)) return a;
            if (IsPredictableTrue(a)) return b;
            if (IsPredictableFalse(b)) return b;
            if (IsPredictableTrue(b)) return a;

            return new And<T>(a, b);
        }

        public ICondition<T> Or<T>(
            ICondition<T> a,
            ICondition<T> b)
        {
            if (IsPredictableFalse(a)) return b;
            if (IsPredictableTrue(a)) return a;
            if (IsPredictableFalse(b)) return a;
            if (IsPredictableTrue(b)) return b;

            return new Or<T>(a, b);
        }

        public ICondition<T> Not<T>(
            ICondition<T> condition)
        {
            if (IsPredictableFalse(condition)) return True<T>();
            if (IsPredictableTrue(condition)) return False<T>();

            return new Not<T>(condition);
        }
    }

    file interface ITrue<in T> : ICondition<T> { }

    file struct True<T> : ITrue<T>
    {
        public readonly bool Check(T _) => true;
    }

    file interface IFalse<in T> : ICondition<T> { }

    file struct False<T> : IFalse<T>
    {
        public readonly bool Check(T _) => false;
    }

    file struct Condition<T>(
        Func<T, bool> check) : ICondition<T>
    {
        public readonly bool Check(T state) => check(state);
    }

    file struct And<T>(
        ICondition<T> a,
        ICondition<T> b) : ICondition<T>
    {
        public readonly bool Check(T state) =>
            a.Check(state) && b.Check(state);
    }

    file struct Or<T>(
        ICondition<T> a,
        ICondition<T> b) : ICondition<T>
    {
        public readonly bool Check(T state) =>
            a.Check(state) || b.Check(state);
    }

    file struct Not<T>(
        ICondition<T> condition) : ICondition<T>
    {
        public readonly bool Check(T state) =>
            !condition.Check(state);
    }
}
