namespace ChainLead.Implementation
{
    //// DO NOT using ChainLead.Contracts.Syntax;
    using ChainLead.Contracts;
    using System;

    public class HandlerMath(IConditionMath conditionMath)
        : IHandlerMath
    {
        public IHandler<T> Zero<T>() => 
            new Zero<T>();

        public bool IsZero<T>(IHandler<T> handler) =>
            handler switch
            {
                IZero<T> => true,
                IExtendedHandler<T> x => IsZero(x.Origin),
                _ => false
            };

        public IHandler<T> MakeHandler<T>(Action<T> action) =>
            new Atom<T>(action);

        public IHandler<T> FirstThenSecond<T>(IHandler<T> prev, IHandler<T> next) =>
            0 switch
            {
                _ when IsZero(prev) => next,
                _ when IsZero(next) => prev,
                _ => new Sum<T>(prev, next)
            };

        public IHandler<T> PackFirstInSecond<T>(IHandler<T> prev, IHandler<T> next)
        {
            (next, var condition) = SplitHandlerAndCondition(next);
            return condition != null
                 ? Conditional(FirstThenSecond(prev, next), condition)
                 : FirstThenSecond(prev, next);
        }

        public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> prev, IHandler<T> next)
        {
            (next, var condition) = SplitHandlerAndCondition(next);
            return condition != null
                 ? Conditional(InjectFirstIntoSecond(prev, next), condition)
                 : FirstThenSecond(prev, next);
        }

        public IHandler<T> FirstCoverSecond<T>(IHandler<T> prev, IHandler<T> next)
        {
            (prev, var condition) = SplitHandlerAndCondition(prev);
            return condition != null
                 ? Conditional(FirstThenSecond(prev, next), condition)
                 : FirstThenSecond(prev, next);
        }

        public IHandler<T> FirstWrapSecond<T>(IHandler<T> prev, IHandler<T> next)
        {
            (prev, var condition) = SplitHandlerAndCondition(prev);
            return condition != null
                 ? Conditional(FirstWrapSecond(prev, next), condition)
                 : FirstThenSecond(prev, next);
        }

        public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next)
        {
            (prev, var prevCondition) = SplitHandlerAndCondition(prev);
            (next, var nextCondition) = SplitHandlerAndCondition(next);
            var result = FirstThenSecond(prev, next);

            return (prevCondition, nextCondition) switch
            {
                (null, null) => result,
                (_, null) => Conditional(result, prevCondition),
                (null, _) => Conditional(result, nextCondition),

                _ => Conditional(result, 
                        conditionMath.And(prevCondition, nextCondition)),
            };
        }

        public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next)
        {
            (prev, var prevCondition) = SplitHandlerAndAllConditions(prev);
            (next, var nextCondition) = SplitHandlerAndAllConditions(next);
            var result = FirstThenSecond(prev, next);

            return (prevCondition, nextCondition) switch
            {
                (null, null) => result,
                (null, _) => Conditional(result, nextCondition),
                (_, null) => Conditional(result, prevCondition),

                _ => Conditional(result,
                        conditionMath.And(prevCondition, nextCondition)),
            };
        }

        public IHandler<T> Atomize<T>(IHandler<T> handler) =>
            handler is not IAtom<T>
                ? new Atom<T>(handler.Execute)
                : handler;

        public IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition) =>
            0 switch
            {
                _ when IsZero(handler) => handler,
                _ when conditionMath.IsPredictableTrue(condition) => handler,
                _ => new Conditional<T>(handler, condition)
            };

        (IHandler<T>, ICondition<T>?) SplitHandlerAndCondition<T>(IHandler<T> handler) => 
            handler switch
            {
                IExtendedHandler<T> x => SplitHandlerAndCondition(x.Origin),
                Conditional<T> x => (x.Handler, x.Condition),
                _ => (handler, null)
            };

        (IHandler<T>, ICondition<T>?) SplitHandlerAndAllConditions<T>(IHandler<T> handler)
        {
            ICondition<T>? accCondition = null;

            (handler, var iCondition) = SplitHandlerAndCondition(handler);
            while (iCondition != null)
            {
                accCondition = accCondition != null
                    ? conditionMath.And(accCondition, iCondition)
                    : iCondition;

                (handler, iCondition) = SplitHandlerAndAllConditions(handler);
            }

            return (handler, accCondition);
        }
    }

    file interface IZero<in T> : IHandler<T>;

    file sealed class Zero<T> : IZero<T>
    {
        public void Execute(T state) { }

        public override string ToString() => "0";
    }

    file interface IAtom<in T> : IHandler<T>;

    file sealed class Atom<T>(
            Action<T> imlementation) 
        : IAtom<T>
    {
        public void Execute(T state) => 
            imlementation(state);
    }

    file sealed class Sum<T>(
            IHandler<T> prev, 
            IHandler<T> next)
        : IHandler<T>
    {
        public void Execute(T state)
        {
            prev.Execute(state);
            next.Execute(state);
        }

        public override string ToString() =>
            $"{prev} + {next}";
    }

    file sealed class Conditional<T>(
            IHandler<T> handler,
            ICondition<T> condition) 
        : IHandler<T>
    {
        public IHandler<T> Handler => handler;

        public ICondition<T> Condition => condition;

        public void Execute(T state)
        {
            if (Condition.Check(state))
                Handler.Execute(state);
        }

        public override string ToString() =>
            $"{Condition}({Handler})";
    }
}
