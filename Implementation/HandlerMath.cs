namespace ChainLead.Implementation
{
    // do not using ChainLead.Contracts.Syntax;
    using ChainLead.Contracts;
    using System;

    public class HandlerMath : IHandlerMath
    {
        private readonly IConditionMath _conditionMath;

        public HandlerMath(IConditionMath conditionMath) =>
            _conditionMath = conditionMath;

        public IHandler<T> Zero<T>() => 
                new Zero<T>();

        public bool IsZero<T>(
            IHandler<T> handler) =>
                handler is IZero<T>;

        public IHandler<T> MakeHandler<T>(
            Action<T> action) =>
                new Handler<T>(action);

        public IHandler<T> FirstThenSecond<T>(
               IHandler<T> a,
               IHandler<T> b) =>
               0 switch
               {
                   _ when IsZero(a) => b,
                   _ when IsZero(b) => a,
                   _ => new Sum<T>(a, b)
               };

        public IHandler<T> PutFirstInSecond<T>(
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
                        _conditionMath.And(aCondition, bCondition)),
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
                        _conditionMath.And(aCondition, bCondition)),
            };
        }

        public IHandler<T> Conditional<T>(
            IHandler<T> handler,
            ICondition<T> condition) =>
                0 switch
                {
                    _ when IsZero(handler) => handler,
                    _ when _conditionMath.IsPredictableTrue(condition) => handler,
                    _ => new Conditional<T>(handler, condition)
                };

        private (IHandler<T>, ICondition<T>?) SplitHandlerAndCondition<T>(
            IHandler<T> handler) => 
            handler switch
            {
                Conditional<T> x => (x.Handler, x.Condition),
                _ => (handler, null)
            };

        private (IHandler<T>, ICondition<T>?) SplitHandlerAndAllConditions<T>(
            IHandler<T> handler)
        {
            ICondition<T>? accCondition = null;

            (handler, var xCondition) = SplitHandlerAndCondition(handler);
            while (xCondition != null)
            {
                accCondition = accCondition == null ? xCondition
                    : _conditionMath.And(accCondition, xCondition);

                (handler, xCondition) = SplitHandlerAndAllConditions(handler);
            }

            return (handler, accCondition);
        }
    }

    file interface IZero<in T> : IHandler<T> { }

    file struct Zero<T> : IZero<T>
    {
        public void Execute(T state) { }
    }

    file struct Handler<T>(
        Action<T> imlementation) : IHandler<T>
    {
        public void Execute(T state) => 
            imlementation(state);
    }

    file struct Sum<T>(
        IHandler<T> Prev, 
        IHandler<T> Next) : IHandler<T>
    {
        public void Execute(T state)
        {
            Prev.Execute(state);
            Next.Execute(state);
        }
    }

    file record struct Conditional<T>(
        IHandler<T> Handler,
        ICondition<T> Condition) : IHandler<T>
    {
        public void Execute(T state)
        {
            if (Condition.Check(state))
                Handler.Execute(state);
        }
    }
}
