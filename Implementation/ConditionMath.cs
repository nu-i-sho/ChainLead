namespace ChainLead.Implementation
{
    //// DO NOT using ChainLead.Contracts.Syntax; HERE
    using ChainLead.Contracts;
    using System;

    public class ConditionMath : IConditionMath
    {
        public ICondition<T> False<T>() => new False<T>();

        public ICondition<T> True<T>() => new True<T>();

        public ICondition<T> MakeCondition<T>(Func<T, bool> predicate) =>
            new Condition<T>(predicate);

        public bool IsPredictableTrue<T>(ICondition<T> condition) =>
            condition is ITrue<T>;

        public bool IsPredictableFalse<T>(ICondition<T> condition) =>
            condition is IFalse<T>;

        public ICondition<T> And<T>(ICondition<T> left, ICondition<T> right)
        {
            if (IsPredictableFalse(left)) return left;
            if (IsPredictableTrue(left)) return right;
            if (IsPredictableFalse(right)) return right;
            if (IsPredictableTrue(right)) return left;

            return new And<T>(left, right);
        }

        public ICondition<T> Or<T>(ICondition<T> left, ICondition<T> right)
        {
            if (IsPredictableFalse(left)) return right;
            if (IsPredictableTrue(left)) return left;
            if (IsPredictableFalse(right)) return left;
            if (IsPredictableTrue(right)) return right;

            return new Or<T>(left, right);
        }

        public ICondition<T> Not<T>(ICondition<T> condition)
        {
            if (IsPredictableFalse(condition)) return True<T>();
            if (IsPredictableTrue(condition)) return False<T>();

            return new Not<T>(condition);
        }
    }

    file interface ITrue<in T> : ICondition<T> { }

    file sealed class True<T> : ITrue<T>
    {
        public bool Check(T _) => true;
    }

    file interface IFalse<in T> : ICondition<T> { }

    file sealed class False<T> : IFalse<T>
    {
        public bool Check(T _) => false;
    }

    file sealed class Condition<T>(
            Func<T, bool> check) 
        : ICondition<T>
    {
        public bool Check(T state) => check(state);
    }

    file sealed class And<T>(
            ICondition<T> left,
            ICondition<T> right) 
        : ICondition<T>
    {
        public bool Check(T state) =>
            left.Check(state) && right.Check(state);
    }

    file sealed class Or<T>(
            ICondition<T> left,
            ICondition<T> right) 
        : ICondition<T>
    {
        public bool Check(T state) =>
            left.Check(state) || right.Check(state);
    }

    file sealed class Not<T>(
            ICondition<T> condition) 
        : ICondition<T>
    {
        public bool Check(T state) =>
            !condition.Check(state);
    }
}
