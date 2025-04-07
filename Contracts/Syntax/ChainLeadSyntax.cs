namespace Nuisho.ChainLead.Contracts.Syntax
{
    using System.Data;

    public static class ChainLeadSyntax
    {
        static IHandlerMath? _handlerMath;
        static IConditionMath? _conditionMath;

        static Exception NotConfigured =>
            throw new NoNullAllowedException(
                $"{nameof(ChainLeadSyntax)} is not configured.");

        static IHandlerMath HandlerMath =>
            _handlerMath ?? throw NotConfigured;

        static IConditionMath ConditionMath =>
            _conditionMath ?? throw NotConfigured;

        public static void Configure(
            IHandlerMath handlerMath,
            IConditionMath conditionMath)
        {
            _handlerMath = handlerMath;
            _conditionMath = conditionMath;
        }

        public static class Handler<T>
        {
            public static IHandler<T> Zero =>
                HandlerMath.Zero<T>();
        }

        public static class Condition<T>
        {
            public static ICondition<T> True =>
                ConditionMath.True<T>();

            public static ICondition<T> False =>
                ConditionMath.False<T>();
        }

        public static IHandler<T> MakeHandler<T>(Action<T> action) =>
            HandlerMath.MakeHandler(action);

        public static IHandler<T> AsHandler<T>(this Action<T> action) =>
            HandlerMath.MakeHandler(action);

        public static ICondition<T> MakeCondition<T>(Func<T, bool> predicate) =>
            ConditionMath.MakeCondition(predicate);

        public static ICondition<T> AsCondition<T>(this Func<T, bool> predicate) =>
            ConditionMath.MakeCondition(predicate);

        public static ICondition<T> AsCondition<T>(this Predicate<T> predicate) =>
            ConditionMath.MakeCondition(new Func<T, bool>(predicate));

        public static bool IsZero<T>(this IHandler<T> handler) =>
            HandlerMath.IsZero(handler);

        public static bool IsPredictableTrue<T>(this ICondition<T> condition) =>
            ConditionMath.IsPredictableTrue(condition);

        public static bool IsPredictableFalse<T>(this ICondition<T> condition) =>
            ConditionMath.IsPredictableFalse(condition);

        public static IHandler<T> FirstThenSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .FirstThenSecond(prev, next);

        public static IHandler<T> Then<T>(
            this IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .FirstThenSecond(prev, next);

        public static Morpheme.IThenReverseCall<T> XThen<T>(IHandler<T> next) =>
            new ThenReverseCallMorpheme<T>(new Acc<T>(
                HandlerMath.FirstThenSecond,
                next));

        public static IHandler<T> PackFirstInSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .PackFirstInSecond(prev, next);

        public static Morpheme.IIn<T> Pack<T>(IHandler<T> prev) =>
            new InMorpheme<T>(
                HandlerMath.PackFirstInSecond,
                prev);

        public static Morpheme.IThenInReverseCall<T> PackXIn<T>(IHandler<T> next) =>
            new ThenInReverseCallMorpheme<T>(new Acc<T>(
                HandlerMath.PackFirstInSecond,
                next));

        public static IHandler<T> InjectFirstIntoSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .InjectFirstIntoSecond(prev, next);

        public static Morpheme.IInto<T> Inject<T>(IHandler<T> prev) =>
            new IntoMorpheme<T>(
                HandlerMath.InjectFirstIntoSecond,
                prev);

        public static Morpheme.IThenIntoReverseCall<T> InjectXInto<T>(IHandler<T> next) =>
            new ThenIntoReverseCallMorpheme<T>(new Acc<T>(
                HandlerMath.InjectFirstIntoSecond,
                next));

        public static IHandler<T> FirstCoverSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .FirstCoverSecond(prev, next);

        public static Morpheme.IToCoverOrWrap<T> Use<T>(IHandler<T> prev) =>
            new ToCoverOrWrapMorpheme<T>(
                HandlerMath.FirstCoverSecond,
                HandlerMath.FirstWrapSecond,
                prev);

        public static Morpheme.IThenCoverReverseCall<T> XCover<T>(IHandler<T> next) =>
            new ThenCoverReverseCallMorpheme<T>(new Acc<T>(
                HandlerMath.FirstCoverSecond,
                next));

        public static IHandler<T> FirstWrapSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .FirstWrapSecond(prev, next);

        public static Morpheme.IThenWrapReverseCall<T> XWrap<T>(IHandler<T> next) =>
            new ThenWrapReverseCallMorpheme<T>(new Acc<T>(
                HandlerMath.FirstWrapSecond,
                next));

        public static IHandler<T> JoinFirstWithSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .JoinFirstWithSecond(prev, next);

        public static Morpheme.IWith<T> Join<T>(IHandler<T> prev) =>
            new WithMorpheme<T>(
                HandlerMath.JoinFirstWithSecond,
                prev);

        public static Morpheme.IThenWithReverseCall<T> JoinXWith<T>(IHandler<T> next) =>
            new ThenWithReverseCallMorpheme<T>(new Acc<T>(
                HandlerMath.JoinFirstWithSecond,
                next));

        public static IHandler<T> MergeFirstWithSecond<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                HandlerMath
                    .MergeFirstWithSecond(prev, next);

        public static Morpheme.IWith<T> Merge<T>(IHandler<T> prev) =>
            new WithMorpheme<T>(
                HandlerMath.MergeFirstWithSecond,
                prev);

        public static Morpheme.IThenWithReverseCall<T> MergeXWith<T>(IHandler<T> next) =>
            new ThenWithReverseCallMorpheme<T>(new Acc<T>(
                HandlerMath.MergeFirstWithSecond,
                next));

        public static IHandler<T> Atomize<T>(this IHandler<T> handler) =>
            HandlerMath.Atomize(handler);

        public static IHandler<T> When<T>(
            this IHandler<T> handler,
            ICondition<T> condition) =>
                HandlerMath
                    .Conditional(handler, condition);

        public static Func<IHandler<T>, IHandler<T>> WithConditionThat<T>(
            ICondition<T> condition) => handler =>
                HandlerMath
                    .Conditional(handler, condition);

        public static ICondition<T> And<T>(
            this ICondition<T> a,
            ICondition<T> b) =>
                ConditionMath.And(a, b);

        public static ICondition<T> Or<T>(
            this ICondition<T> a,
            ICondition<T> b) =>
                ConditionMath.Or(a, b);

        public static ICondition<T> Not<T>(ICondition<T> handler) =>
            ConditionMath.Not(handler);

        public static Func<T, T, T> Reverse<T>(
            Func<T, T, T> f) => (a, b) =>
                f(b, a);
    }

    file sealed class Acc<T>
    {
        readonly Func<IHandler<T>, IHandler<T>, IHandler<T>> _append;
        readonly Func<IHandler<T>, IHandler<T>> _call;

        Acc(Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
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

    file sealed class ThenReverseCallMorpheme<T>(Acc<T> acc)
        : Morpheme.IThenReverseCall<T>
    {
        public Morpheme.IThenReverseCall<T> Then(IHandler<T> next) =>
           new ThenReverseCallMorpheme<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file sealed class InMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IIn<T>
    {
        public Morpheme.IThenIn<T> In(IHandler<T> next) =>
           new ThenInMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenInMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IThenIn<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morpheme.IThenIn<T> ThenIn(IHandler<T> next) =>
           new ThenInMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenInReverseCallMorpheme<T>(Acc<T> acc)
        : Morpheme.IThenInReverseCall<T>
    {
        public Morpheme.IThenInReverseCall<T> ThenIn(IHandler<T> next) =>
           new ThenInReverseCallMorpheme<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file sealed class IntoMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IInto<T>
    {
        public Morpheme.IThenInto<T> Into(IHandler<T> next) =>
           new ThenIntoMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenIntoMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IThenInto<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morpheme.IThenInto<T> ThenInto(IHandler<T> next) =>
           new ThenIntoMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenIntoReverseCallMorpheme<T>(Acc<T> acc)
        : Morpheme.IThenIntoReverseCall<T>
    {
        public Morpheme.IThenIntoReverseCall<T> ThenInto(IHandler<T> next) =>
           new ThenIntoReverseCallMorpheme<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file sealed class ToCoverOrWrapMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> cover,
            Func<IHandler<T>, IHandler<T>, IHandler<T>> wrap,
            IHandler<T> prev)
        : Morpheme.IToCoverOrWrap<T>
    {
        public Morpheme.IThenCover<T> ToCover(IHandler<T> next) =>
           new ThenCoverMorpheme<T>(cover, cover(prev, next));

        public Morpheme.IThenWrap<T> ToWrap(IHandler<T> next) =>
           new ThenWrapMorpheme<T>(wrap, wrap(prev, next));
    }

    file sealed class ThenCoverMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IThenCover<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morpheme.IThenCover<T> ThenCover(IHandler<T> next) =>
           new ThenCoverMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenWrapMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IThenWrap<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morpheme.IThenWrap<T> ThenWrap(IHandler<T> next) =>
           new ThenWrapMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenCoverReverseCallMorpheme<T>(Acc<T> acc) :
        Morpheme.IThenCoverReverseCall<T>
    {
        public Morpheme.IThenCoverReverseCall<T> ThenCover(IHandler<T> next) =>
           new ThenCoverReverseCallMorpheme<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file sealed class ThenWrapReverseCallMorpheme<T>(Acc<T> acc)
        : Morpheme.IThenWrapReverseCall<T>
    {
        public Morpheme.IThenWrapReverseCall<T> ThenWrap(IHandler<T> next) =>
           new ThenWrapReverseCallMorpheme<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }

    file sealed class WithMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IWith<T>
    {
        public Morpheme.IThenWith<T> With(IHandler<T> next) =>
           new ThenWithMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenWithMorpheme<T>(
            Func<IHandler<T>, IHandler<T>, IHandler<T>> append,
            IHandler<T> prev)
        : Morpheme.IThenWith<T>
    {
        IHandler<T> IExtendedHandler<T>.Origin => prev;

        public Morpheme.IThenWith<T> ThenWith(IHandler<T> next) =>
           new ThenWithMorpheme<T>(append, append(prev, next));
    }

    file sealed class ThenWithReverseCallMorpheme<T>(Acc<T> acc)
        : Morpheme.IThenWithReverseCall<T>
    {
        public Morpheme.IThenWithReverseCall<T> ThenWith(IHandler<T> next) =>
           new ThenWithReverseCallMorpheme<T>(acc.Add(next));

        public IHandler<T> WhereXIs(IHandler<T> head) =>
           acc.Close(head);
    }
}
