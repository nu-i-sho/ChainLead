namespace ChainLead.Contracts.Syntax
{
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

        public static IHandler<T> Then<T>(
            this IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .Join(prev, next);

        public static IHandler<T> Join<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .Join(prev, next);

        public static IPrepositionedFunc<T>.To Join<T>(
            IHandler<T> prev) => new PrepositionedFunc<T>._To(
           (next) =>
                Math.ForHandler
                    .Join(prev, next));

        public static Func<IHandler<T>, IHandler<T>> JoinItTo<T>(
            IHandler<T> next) =>
           (prev) =>
                Math.ForHandler
                    .Join(prev, next);

        public static IHandler<T> Merge<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .Merge(prev, next);

        public static IPrepositionedFunc<T>.With Merge<T>(
            IHandler<T> prev) => new PrepositionedFunc<T>._With(
           (next) =>
                Math.ForHandler
                    .Merge(prev, next));

        public static Func<IHandler<T>, IHandler<T>> MergeItWith<T>(
            IHandler<T> next) =>
           (prev) =>
                Math.ForHandler
                    .Merge(prev, next);

        public static IHandler<T> DeepMerge<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .DeepMerge(prev, next);

        public static IPrepositionedFunc<T>.With DeepMerge<T>(
            IHandler<T> prev) => new PrepositionedFunc<T>._With(
           (next) =>
                Math.ForHandler
                    .DeepMerge(prev, next));

        public static Func<IHandler<T>, IHandler<T>> DeepMergeItWith<T>(
            IHandler<T> next) =>
           (prev) =>
                Math.ForHandler
                    .DeepMerge(prev, next);

        public static IHandler<T> Inject<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .Inject(prev, next);

        public static IPrepositionedFunc<T>.Into Inject<T>(
            IHandler<T> prev) => new PrepositionedFunc<T>._Into(
           (next) =>
                Math.ForHandler
                    .Inject(prev, next));

        public static Func<IHandler<T>, IHandler<T>> InjectItInto<T>(
            IHandler<T> next) =>
           (prev) =>
                Math.ForHandler
                    .Inject(prev, next);

        public static IHandler<T> DeepInject<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .DeepInject(prev, next);

        public static IPrepositionedFunc<T>.Into DeepInject<T>(
            IHandler<T> prev) => new PrepositionedFunc<T>._Into(
           (next) =>
                Math.ForHandler
                    .DeepInject(prev, next));

        public static Func<IHandler<T>, IHandler<T>> DeepInjectItInto<T>(
            IHandler<T> next) =>
           (prev) =>
                Math.ForHandler
                    .DeepInject(prev, next);

        public static IHandler<T> Wrap<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .Wrap(prev, next);

        public static IPrepositionedFunc<T>.Up Wrap<T>(
            IHandler<T> prev) => new PrepositionedFunc<T>._Up(
           (next) =>
                Math.ForHandler
                    .Wrap(prev, next));

        public static Func<IHandler<T>, IHandler<T>> WrapItUp<T>(
            IHandler<T> next) =>
           (prev) =>
                Math.ForHandler
                    .Wrap(prev, next);

        public static IHandler<T> DeepWrap<T>(
            IHandler<T> prev,
            IHandler<T> next) =>
                Math.ForHandler
                    .DeepWrap(prev, next);

        public static IPrepositionedFunc<T>.Up DeepWrap<T>(
            IHandler<T> prev) => new PrepositionedFunc<T>._Up(
           (next) =>
                Math.ForHandler
                    .DeepWrap(prev, next));

        public static Func<IHandler<T>, IHandler<T>> DeepWrapItUp<T>(
            IHandler<T> next) =>
           (prev) =>
                Math.ForHandler
                    .DeepWrap(prev, next);

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

        public static class IPrepositionedFunc<T>
        {
            public interface To
            {
                IHandler<T> To(IHandler<T> next);
            }

            public interface With
            {
                IHandler<T> With(IHandler<T> next);
            }

            public interface Into
            {
                IHandler<T> Into(IHandler<T> next);
            }

            public interface Up
            {
                IHandler<T> Up(IHandler<T> next);
            }
        }

        internal static class PrepositionedFunc<T>
        {
            public record struct _To(
                Func<IHandler<T>, IHandler<T>> f)
                : IPrepositionedFunc<T>.To
            {
                public IHandler<T> To(IHandler<T> next) =>
                    f(next);
            }

            public record struct _With(
                Func<IHandler<T>, IHandler<T>> f)
                : IPrepositionedFunc<T>.With
            {
                public IHandler<T> With(IHandler<T> next) =>
                    f(next);
            }

            public record struct _Into(
                Func<IHandler<T>, IHandler<T>> f)
                : IPrepositionedFunc<T>.Into
            {
                public IHandler<T> Into(IHandler<T> next) =>
                    f(next);
            }

            public record struct _Up(
                Func<IHandler<T>, IHandler<T>> f)
                : IPrepositionedFunc<T>.Up
            {
                public IHandler<T> Up(IHandler<T> next) =>
                    f(next);
            }
        }
    }
}
