namespace ChainLead.Contracts.Syntax
{
    using ChainLead.Contracts;

    file static class Math
    {
        public static IHandlerMath ForHandler;

        public static IConditionMath ForCondition;
    }

    public static class ChainLeadSyntax
    {
        public static FirstConfigurationStep ConfigureChainLeadSyntax => new();

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

        public static bool IsPredictableTrue<T>(
            this ICondition<T> condition) => 
                Math.ForCondition.IsPredictableTrue(condition);

        public static bool IsPredictableFalse<T>(
            this ICondition<T> condition) =>
                Math.ForCondition.IsPredictableFalse(condition);

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

        public static Morphem.IThenReverseCall<T> XThen<T>(
            IHandler<T> next) =>
                new ThenReverseCallMorphem<T>(new Acc<T>(
                    Math.ForHandler.FirstThenSecond,
                    next));

        public static IHandler<T> PackFirstInSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .PackFirstInSecond(prev, next);

        public static Morphem.IIn<T> Pack<T>(
            IHandler<T> prev) =>
                new InMorphem<T>(
                    Math.ForHandler.PackFirstInSecond,
                    prev);

        public static Morphem.IThenInReverseCall<T> PackXIn<T>(
            IHandler<T> next) =>
                new ThenInReverseCallMorphem<T>(new Acc<T>(
                    Math.ForHandler.PackFirstInSecond,
                    next));

        public static IHandler<T> InjectFirstIntoSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .InjectFirstIntoSecond(prev, next);

        public static Morphem.IInto<T> Inject<T>(
            IHandler<T> prev) =>
                new IntoMorphem<T>(
                    Math.ForHandler.InjectFirstIntoSecond,
                    prev);

        public static Morphem.IThenIntoReverseCall<T> InjectXInto<T>(
            IHandler<T> next) =>
                new ThenIntoReverseCallMorphem<T>(new Acc<T>(
                    Math.ForHandler.InjectFirstIntoSecond, 
                    next));

        public static IHandler<T> FirstCoverSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .FirstCoverSecond(prev, next);

        public static Morphem.IToCoverOrWrap<T> Use<T>(
            IHandler<T> prev) =>
                new ToCoverOrWrapMorphem<T>(
                    Math.ForHandler.FirstCoverSecond,
                    Math.ForHandler.FirstWrapSecond,
                    prev);

        public static Morphem.IThenCoverReverseCall<T> XCover<T>(
            IHandler<T> next) =>
                new ThenCoverReverseCallMorphem<T>(new Acc<T>(
                    Math.ForHandler.FirstCoverSecond,
                    next));

        public static IHandler<T> FirstWrapSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .FirstWrapSecond(prev, next);

        public static Morphem.IThenWrapReverseCall<T> XWrap<T>(
            IHandler<T> next) =>
                new ThenWrapReverseCallMorphem<T>(new Acc<T>(
                    Math.ForHandler.FirstWrapSecond,
                    next));

        public static IHandler<T> JoinFirstWithSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .JoinFirstWithSecond(prev, next);

        public static Morphem.IWith<T> Join<T>(
            IHandler<T> prev) => 
                new WithMorphem<T>(
                    Math.ForHandler.JoinFirstWithSecond,
                    prev);

        public static Morphem.IThenWithReverseCall<T> JoinXWith<T>(
            IHandler<T> next) =>
                new ThenWithReverseCallMorphem<T>(new Acc<T>(
                    Math.ForHandler.JoinFirstWithSecond,
                    next));

        public static IHandler<T> MergeFirstWithSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .MergeFirstWithSecond(prev, next);

        public static Morphem.IWith<T> Merge<T>(
            IHandler<T> prev) => 
                new WithMorphem<T>(
                    Math.ForHandler.MergeFirstWithSecond,
                    prev);

        public static Morphem.IThenWithReverseCall<T> MergeXWith<T>(
            IHandler<T> next) =>
                new ThenWithReverseCallMorphem<T>(new Acc<T>(
                    Math.ForHandler.MergeFirstWithSecond,
                    next));

        public static IHandler<T> Atomize<T>(
            this IHandler<T> handler) =>
                Math.ForHandler
                    .Atomize(handler);

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

    file readonly struct Acc<T>
    {
        readonly Func<IHandler<T>, IHandler<T>, IHandler<T>> _append;
        readonly Func<IHandler<T>, IHandler<T>> _call;

        Acc(
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

    file readonly struct ThenReverseCallMorphem<T>(Acc<T> acc)
        : Morphem.IThenReverseCall<T>
    {
        public Morphem.IThenReverseCall<T> Then(IHandler<T> next) => 
           new ThenReverseCallMorphem<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file readonly struct InMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IIn<T>
    {
        public Morphem.IThenIn<T> In(IHandler<T> next) =>
           new ThenInMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenInMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IThenIn<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morphem.IThenIn<T> ThenIn(IHandler<T> next) =>
           new ThenInMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenInReverseCallMorphem<T>(Acc<T> acc) 
        : Morphem.IThenInReverseCall<T>
    {
        public Morphem.IThenInReverseCall<T> ThenIn(IHandler<T> next) =>
           new ThenInReverseCallMorphem<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file readonly struct IntoMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IInto<T>
    {
        public Morphem.IThenInto<T> Into(IHandler<T> next) =>
           new ThenIntoMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenIntoMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IThenInto<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morphem.IThenInto<T> ThenInto(IHandler<T> next) =>
           new ThenIntoMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenIntoReverseCallMorphem<T>(Acc<T> acc) 
        : Morphem.IThenIntoReverseCall<T>
    {
        public Morphem.IThenIntoReverseCall<T> ThenInto(IHandler<T> next) =>
           new ThenIntoReverseCallMorphem<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file readonly struct ToCoverOrWrapMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> cover,
            Func<IHandler<T>, IHandler<T>, IHandler<T>> wrap,
            IHandler<T> prev) 
        : Morphem.IToCoverOrWrap<T>
    {
        public Morphem.IThenCover<T> ToCover(IHandler<T> next) =>
           new ThenCoverMorphem<T>(cover, cover(prev, next));

        public Morphem.IThenWrap<T> ToWrap(IHandler<T> next) =>
           new ThenWrapMorphem<T>(wrap, wrap(prev, next));
    }

    file readonly struct ThenCoverMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IThenCover<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morphem.IThenCover<T> ThenCover(IHandler<T> next) =>
           new ThenCoverMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenWrapMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IThenWrap<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morphem.IThenWrap<T> ThenWrap(IHandler<T> next) =>
           new ThenWrapMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenCoverReverseCallMorphem<T>(Acc<T> acc) : 
        Morphem.IThenCoverReverseCall<T>
    {
        public Morphem.IThenCoverReverseCall<T> ThenCover(IHandler<T> next) =>
           new ThenCoverReverseCallMorphem<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file readonly struct ThenWrapReverseCallMorphem<T>(Acc<T> acc) 
        : Morphem.IThenWrapReverseCall<T>
    {
        public Morphem.IThenWrapReverseCall<T> ThenWrap(IHandler<T> next) =>
           new ThenWrapReverseCallMorphem<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file readonly struct WithMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IWith<T>
    {
        public Morphem.IThenWith<T> With(IHandler<T> next) =>
           new ThenWithMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenWithMorphem<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev) 
        : Morphem.IThenWith<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morphem.IThenWith<T> ThenWith(IHandler<T> next) =>
           new ThenWithMorphem<T>(append, append(prev, next));
    }

    file readonly struct ThenWithReverseCallMorphem<T>(Acc<T> acc) 
        : Morphem.IThenWithReverseCall<T>
    {
        public Morphem.IThenWithReverseCall<T> ThenWith(IHandler<T> next) =>
           new ThenWithReverseCallMorphem<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }
}
