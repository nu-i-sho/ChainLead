namespace ChainLead.Contracts.Syntax
{
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    file static class Math
    {
        public static IHandlerMath ForHandler;

        public static IConditionMath ForCondition;
    }

    public static class ChainLeadSyntax
    {
        public static FirstConfigurationStep ConfigureChainLeadSyntax =>
            new FirstConfigurationStep();

        public class FirstConfigurationStep
        {
            internal FirstConfigurationStep() { }

            public SecondConfigurationStep WithHandlerMath(IHandlerMath math)
            {
                Math.ForHandler = math;
                return new SecondConfigurationStep();
            }
        }

        public class SecondConfigurationStep
        {
            internal SecondConfigurationStep() { }

            public void AndWithConditionMath(IConditionMath math) =>
                Math.ForCondition = math;
        }

        public static class Handler<T>
        {
            public static IHandler<T> Zero =>
                Math.ForHandler
                    .Zero<T>();
        }

        public static class Condition<T>
        {
            public static ICondition<T> True =>
                Math.ForCondition
                    .True<T>();

            public static ICondition<T> False =>
                Math.ForCondition
                    .False<T>();
        }

        public static IHandler<T> MakeHandler<T>(
            Action<T> action) =>
                Math.ForHandler
                    .MakeHandler(action);

        public static IHandler<T> AsHandler<T>(
            this Action<T> action) =>
                Math.ForHandler
                    .MakeHandler(action);

        public static ICondition<T> MakeCondition<T>(
            Func<T, bool> predicate) =>
                Math.ForCondition
                    .MakeCondition(predicate);

        public static ICondition<T> AsCondition<T>(
            this Func<T, bool> predicate) =>
                Math.ForCondition
                    .MakeCondition(predicate);

        public static ICondition<T> AsCondition<T>(
            this Predicate<T> predicate) =>
                Math.ForCondition
                    .MakeCondition(predicate);

        public static bool IsZero<T>(
            this IHandler<T> handler) =>
                Math.ForHandler
                    .IsZero(handler);

        public static IHandler<T> FirstThenSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .FirstThenSecond(prev, next);

        public static IHandler<T> Then<T>(
            this IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .FirstThenSecond(prev, next);

        public interface IReverseCall<T>
        {
            IHandler<T> WhereXIs(IHandler<T> x);
        }

        public interface IThenReverseCall<T> : IReverseCall<T>
        {
            IThenReverseCall<T> Then(IHandler<T> next);
        }

        public static IThenReverseCall<T> XThen<T>(
            IHandler<T> next) =>
                new ThenReverseCall<T>(new Acc<T>(
                    Math.ForHandler.FirstThenSecond,
                    next));

        public static IHandler<T> PutFirstInSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .PutFirstInSecond(prev, next);

        public interface IIn<T>
        {
            IThenIn<T> In(IHandler<T> next);
        }

        public interface IThenIn<T> : IHandler<T>
        {
            IThenIn<T> ThenIn(IHandler<T> next);
        }

        public static IIn<T> Put<T>(
            IHandler<T> prev) => new In<T>(
           (IHandler<T> next) =>
                Math.ForHandler
                    .PutFirstInSecond(prev, next));

        public interface IThenInReverseCall<T> : IReverseCall<T>
        {
            IThenInReverseCall<T> ThenIn(IHandler<T> next);
        }

        public static IThenInReverseCall<T> PutXIn<T>(
            IHandler<T> next) =>
                new ThenInReverseCall<T>(new Acc<T>(
                    Math.ForHandler.PutFirstInSecond,
                    next));

        public static IHandler<T> InjectFirstIntoSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .InjectFirstIntoSecond(prev, next);

        public interface IInto<T>
        {
            IThenInto<T> Into(IHandler<T> next);
        }

        public interface IThenInto<T> : IHandler<T>
        {
            IThenInto<T> ThenInto(IHandler<T> next);
        }

        public static IInto<T> Inject<T>(
            IHandler<T> prev) => new Into<T>(
           (IHandler<T> next) =>
                Math.ForHandler
                    .InjectFirstIntoSecond(prev, next));

        public interface IThenIntoReverseCall<T> : IReverseCall<T>
        {
            IThenIntoReverseCall<T> ThenInto(IHandler<T> next);
        }

        public static IThenIntoReverseCall<T> InjectXInto<T>(
            IHandler<T> next) =>
                new ThenIntoReverseCall<T>(new Acc<T>(
                    Math.ForHandler.InjectFirstIntoSecond, 
                    next));

        public static IHandler<T> FirstCoverSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .FirstCoverSecond(prev, next);

        public interface IToCoverOrWrap<T>
        {
            IThenCover<T> ToCover(IHandler<T> next);

            IThenWrap<T> ToWrap(IHandler<T> next);
        }

        public interface IThenCover<T> : IHandler<T>
        {
            IThenCover<T> ThenCover(IHandler<T> next);
        }

        public interface IThenWrap<T> : IHandler<T>
        {
            IThenWrap<T> ThenWrap(IHandler<T> next);
        }

        public static IToCoverOrWrap<T> Use<T>(
            IHandler<T> prev) => new ToCoverOrWrap<T>(
           (IHandler<T> next) =>
                Math.ForHandler
                    .FirstCoverSecond(prev, next),
           (IHandler<T> next) =>
                Math.ForHandler
                    .FirstWrapSecond(prev, next));

        public interface IThenCoverReverseCall<T>
            : IReverseCall<T>
        {
            IThenCoverReverseCall<T> ThenCover(IHandler<T> next);
        }

        public static IThenCoverReverseCall<T> XCover<T>(
            IHandler<T> next) =>
                new ThenCoverReverseCall<T>(new Acc<T>(
                    Math.ForHandler.FirstCoverSecond,
                    next));

        public static IHandler<T> FirstWrapSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .FirstWrapSecond(prev, next);

        public interface IThenWrapReverseCall<T>
            : IReverseCall<T>
        {
            IThenWrapReverseCall<T> ThenWrap(IHandler<T> next);
        }

        public static IThenWrapReverseCall<T> XWrap<T>(
            IHandler<T> next) =>
                new ThenWrapReverseCall<T>(new Acc<T>(
                    Math.ForHandler.FirstWrapSecond,
                    next));

        public static IHandler<T> JoinFirstWithSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .JoinFirstWithSecond(prev, next);

        public interface IWith<T>
        {
            IThenWith<T> With(IHandler<T> next);
        }

        public interface IThenWith<T> : IHandler<T>
        {
            IThenWith<T> ThenWith(IHandler<T> next);
        }

        public static IWith<T> Join<T>(
            IHandler<T> prev) => new With<T>(
           (IHandler<T> next) =>
                Math.ForHandler
                    .JoinFirstWithSecond(prev, next));

        public interface IThenWithReverseCall<T>
            : IReverseCall<T>
        {
            IThenWithReverseCall<T> ThenWith(IHandler<T> next);
        }

        public static IThenWithReverseCall<T> JoinXWith<T>(
            IHandler<T> next) =>
                new ThenWithReverseCall<T>(new Acc<T>(
                    Math.ForHandler.JoinFirstWithSecond,
                    next));

        public static IHandler<T> MergeFirstWithSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .MergeFirstWithSecond(prev, next);

        public static IWith<T> Merge<T>(
            IHandler<T> prev) => new With<T>(
           (IHandler<T> next) =>
                Math.ForHandler
                    .MergeFirstWithSecond(prev, next));

        public static IThenWithReverseCall<T> MergeXWith<T>(
            IHandler<T> next) =>
                new ThenWithReverseCall<T>(new Acc<T>(
                    Math.ForHandler.MergeFirstWithSecond,
                    next));

        public static IHandler<T> When<T>(
            this IHandler<T> handler,
            ICondition<T> condition) =>
                Math.ForHandler
                    .Conditional(handler, condition);

        public static Func<IHandler<T>, IHandler<T>> WithConditionThat<T>(
            ICondition<T> condition) => handler =>
                Math.ForHandler
                    .Conditional(handler, condition);

        public static ICondition<T> And<T>(
            this ICondition<T> a,
            ICondition<T> b) =>
                Math.ForCondition
                    .And(a, b);

        public static ICondition<T> Or<T>(
            this ICondition<T> a,
            ICondition<T> b) =>
                Math.ForCondition
                    .Or(a, b);

        public static ICondition<T> Not<T>(
            ICondition<T> handler) =>
                Math.ForCondition
                    .Not(handler);

        public static Func<T, T, T> Reverse<T>(
            Func<T, T, T> f) => (a, b) =>
                f(b, a);
    }

    file struct Acc<T>
    {
        private readonly Func<IHandler<T>, IHandler<T>, IHandler<T>> _append;
        private readonly Func<IHandler<T>, IHandler<T>> _call;

        private Acc(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            Func<IHandler<T>, IHandler<T>> call)
        {
            _append = append;
            _call = call;
        }

        public Acc(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> next) : this(append,
           (IHandler<T> head) => append(head, next))
        {
        }

        public Acc<T> Add(IHandler<T> next)
        {
            var (a, c) = (_append, _call);
            return new Acc<T>(_append, head => a(c(head), next));
        }

        public IHandler<T> Close(IHandler<T> head) =>
            _call(head);
    }

    file struct ThenReverseCall<T>(Acc<T> acc) : IThenReverseCall<T>
    {
        public IThenReverseCall<T> Then(IHandler<T> next) => 
            new ThenReverseCall<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
            acc.Close(head);
    }

    file struct In<T>(
        Func<IHandler<T>, IHandler<T>> call) : IIn<T>
    {
        IThenIn<T> IIn<T>.In(IHandler<T> next) =>
            new ThenIn<T>(call(next), call);
    }

    file struct ThenIn<T>(
        IHandler<T> current,
        Func<IHandler<T>, IHandler<T>> call) : IThenIn<T>
    {
        IThenIn<T> IThenIn<T>.ThenIn(IHandler<T> next) =>
            new ThenIn<T>(call(next), call);

        public void Execute(T state) =>
            current.Execute(state);
    }

    file struct ThenInReverseCall<T>(Acc<T> acc) : IThenInReverseCall<T>
    {
        public IThenInReverseCall<T> ThenIn(IHandler<T> next) =>
            new ThenInReverseCall<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
            acc.Close(head);
    }

    file struct Into<T>(
        Func<IHandler<T>, IHandler<T>> call) : IInto<T>
    {
        IThenInto<T> IInto<T>.Into(IHandler<T> next) =>
            new ThenInto<T>(call(next), call);
    }

    file struct ThenInto<T>(
        IHandler<T> current,
        Func<IHandler<T>, IHandler<T>> call) : IThenInto<T>
    {
        IThenInto<T> IThenInto<T>.ThenInto(IHandler<T> next) =>
            new ThenInto<T>(call(next), call);

        public void Execute(T state) =>
            current.Execute(state);
    }

    file struct ThenIntoReverseCall<T>(Acc<T> acc) : IThenIntoReverseCall<T>
    {
        public IThenIntoReverseCall<T> ThenInto(IHandler<T> next) =>
            new ThenIntoReverseCall<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
            acc.Close(head);
    }

    file struct ToCoverOrWrap<T>(
        Func<IHandler<T>, IHandler<T>> cover,
        Func<IHandler<T>, IHandler<T>> wrap) : IToCoverOrWrap<T>
    {
        public IThenCover<T> ToCover(IHandler<T> next) =>
            new ThenCover<T>(cover(next), cover);

        public IThenWrap<T> ToWrap(IHandler<T> next) =>
            new ThenWrap<T>(wrap(next), wrap);
    }

    file struct ThenCover<T>(
        IHandler<T> current,
        Func<IHandler<T>, IHandler<T>> call) : IThenCover<T>
    {
        IThenCover<T> IThenCover<T>.ThenCover(IHandler<T> next) =>
            new ThenCover<T>(call(next), call);

        public void Execute(T state) =>
            current.Execute(state);
    }

    file struct ThenWrap<T>(
        IHandler<T> current,
        Func<IHandler<T>, IHandler<T>> call) : IThenWrap<T>
    {
        IThenWrap<T> IThenWrap<T>.ThenWrap(IHandler<T> next) =>
            new ThenWrap<T>(call(next), call);

        public void Execute(T state) =>
            current.Execute(state);
    }

    file struct ThenCoverReverseCall<T>(Acc<T> acc) : IThenCoverReverseCall<T>
    {
        public IThenCoverReverseCall<T> ThenCover(IHandler<T> next) =>
            new ThenCoverReverseCall<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
            acc.Close(head);
    }

    file struct ThenWrapReverseCall<T>(Acc<T> acc) : IThenWrapReverseCall<T>
    {
        public IThenWrapReverseCall<T> ThenWrap(IHandler<T> next) =>
            new ThenWrapReverseCall<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
            acc.Close(head);
    }

    file struct With<T>(
        Func<IHandler<T>, IHandler<T>> call) : IWith<T>
    {
        IThenWith<T> IWith<T>.With(IHandler<T> next) =>
            new ThenWith<T>(call(next), call);
    }

    file struct ThenWith<T>(
        IHandler<T> current,
        Func<IHandler<T>, IHandler<T>> call) : IThenWith<T>
    {
        IThenWith<T> IThenWith<T>.ThenWith(IHandler<T> next) =>
            new ThenWith<T>(call(next), call);

        public void Execute(T state) =>
            current.Execute(state);
    }

    file struct ThenWithReverseCall<T>(Acc<T> acc) : IThenWithReverseCall<T>
    {
        public IThenWithReverseCall<T> ThenWith(IHandler<T> next) =>
            new ThenWithReverseCall<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
            acc.Close(head);
    }
}
