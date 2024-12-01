namespace ChainLead.Implementation
{
    // do not using ChainLead.Contracts.Syntax;
    using ChainLead.Contracts;
    using System;

    public class HandlerMath(IConditionMath conditionMath)
        : IHandlerMath
    {
        public IHandler<T> Zero<T>() => 
                new Zero<T>();

        public bool IsZero<T>(
            IHandler<T> handler) =>
                handler switch
                {
                    IZero<T> => true,
                    IExtendedHandler<T> x => IsZero(x.Origin),
                    _ => false
                };

        public IHandler<T> MakeHandler<T>(
            Action<T> action) =>
                new Atom<T>(action);

        public IHandler<T> FirstThenSecond<T>(
               IHandler<T> a,
               IHandler<T> b) =>
               0 switch
               {
                   _ when IsZero(a) => b,
                   _ when IsZero(b) => a,
                   _ => new Sum<T>(a, b)
               };

        public IHandler<T> PackFirstInSecond<T>(
               IHandler<T> a,
               IHandler<T> b)
        {
            (b, var condition) = SplitHandlerAndCondition(b);
            return condition != null
                 ? Conditional(FirstThenSecond(a, b), condition)
                 : FirstThenSecond(a, b);
        }

        public IHandler<T> InjectFirstIntoSecond<T>(
               IHandler<T> a,
               IHandler<T> b)
        {
            (b, var condition) = SplitHandlerAndCondition(b);
            return condition != null
                 ? Conditional(InjectFirstIntoSecond(a, b), condition)
                 : FirstThenSecond(a, b);
        }

        public IHandler<T> FirstCoverSecond<T>(
               IHandler<T> a,
               IHandler<T> b)
        {
            (a, var condition) = SplitHandlerAndCondition(a);
            return condition != null
                 ? Conditional(FirstThenSecond(a, b), condition)
                 : FirstThenSecond(a, b);
        }

        public IHandler<T> FirstWrapSecond<T>(
               IHandler<T> a,
               IHandler<T> b)
        {
            (a, var condition) = SplitHandlerAndCondition(a);
            return condition != null
                 ? Conditional(FirstWrapSecond(a, b), condition)
                 : FirstThenSecond(a, b);
        }

        public IHandler<T> JoinFirstWithSecond<T>(
               IHandler<T> a,
               IHandler<T> b)
        {
            var (aHandler, aCondition) = SplitHandlerAndCondition(a);
            var (bHandler, bCondition) = SplitHandlerAndCondition(b);
            var abHandler = FirstThenSecond(aHandler, bHandler);

            return (aCondition, bCondition) switch
            {
                (null, null) => abHandler,
                (null, _) => Conditional(abHandler, bCondition),
                (_, null) => Conditional(abHandler, aCondition),
                
                _ => Conditional(abHandler, 
                        conditionMath.And(aCondition, bCondition)),
            };
        }

        public IHandler<T> MergeFirstWithSecond<T>(
               IHandler<T> a,
               IHandler<T> b)
        {
            var (aHandler, aCondition) = SplitHandlerAndAllConditions(a);
            var (bHandler, bCondition) = SplitHandlerAndAllConditions(b);
            var abHandler = FirstThenSecond(aHandler, bHandler);

            return (aCondition, bCondition) switch
            {
                (null, null) => abHandler,
                (null, _) => Conditional(abHandler, bCondition),
                (_, null) => Conditional(abHandler, aCondition),

                _ => Conditional(abHandler,
                        conditionMath.And(aCondition, bCondition)),
            };
        }

        public IHandler<T> Atomize<T>(
            IHandler<T> handler) =>
                handler is not IAtom<T>
                    ? new Atom<T>(handler.Execute)
                    : handler;

        public IHandler<T> Conditional<T>(
            IHandler<T> handler,
            ICondition<T> condition) =>
                0 switch
                {
                    _ when IsZero(handler) => handler,
                    _ when conditionMath.IsPredictableTrue(condition) => handler,
                    _ => new Conditional<T>(handler, condition)
                };

        (IHandler<T>, ICondition<T>?) SplitHandlerAndCondition<T>(
            IHandler<T> handler) => 
            handler switch
            {
                IExtendedHandler<T> x => SplitHandlerAndCondition(x.Origin),
                Conditional<T> x => (x.Handler, x.Condition),
                _ => (handler, null)
            };

        (IHandler<T>, ICondition<T>?) SplitHandlerAndAllConditions<T>(
            IHandler<T> handler)
        {
            ICondition<T>? accCondition = null;

            (handler, var xCondition) = SplitHandlerAndCondition(handler);
            while (xCondition != null)
            {
                accCondition = accCondition == null ? xCondition
                    : conditionMath.And(accCondition, xCondition);

                (handler, xCondition) = SplitHandlerAndAllConditions(handler);
            }

            return (handler, accCondition);
        }
    }

    file interface IZero<in T> : IHandler<T> { }

    file struct Zero<T> : IZero<T>
    {
        public readonly void Execute(T state) { }
    }

    file interface IAtom<in T> : IHandler<T> { }

    file struct Atom<T>(
        Action<T> imlementation) : IAtom<T>
    {
        public readonly void Execute(T state) => 
            imlementation(state);
    }

    file record struct Sum<T>(
        IHandler<T> Prev, 
        IHandler<T> Next) : IHandler<T>
    {
        public readonly void Execute(T state)
        {
            Prev.Execute(state);
            Next.Execute(state);
        }
    }

    file record struct Conditional<T>(
        IHandler<T> Handler,
        ICondition<T> Condition) : IHandler<T>
    {
        public readonly void Execute(T state)
        {
            if (Condition.Check(state))
                Handler.Execute(state);
        }
    }
}
