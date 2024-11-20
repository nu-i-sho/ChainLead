namespace ChainLead.Test.HandlersTestData
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    public class ChainLeadSyntaxDirectChainingCallsProviderFactory
        : IHandlerChainingCallsProviderFactory
    {
        public IHandlerChainingCallsProvider Create(IConditionMath conditionMath)
        {
            IHandlerMath math = new HandlerMath(conditionMath);
            ConfigureChainLeadSyntax
                .WithHandlerMath(math)
                .AndWithConditionMath(conditionMath);

            return new Calls();
        }

        public override string ToString() => "\"like Use(a).ToMerge(b).ThenMerge(c)\"";

        class Calls : IHandlerChainingCallsProvider
        {
            public IHandler<T> ThenChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = First(handlers).Then(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.Then(handler);

                return chain;
            }

            public IHandler<T> CoverChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = Use(First(handlers)).ToCover(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenCover(handler);

                return chain;
            }

            public IHandler<T> InjectChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = Inject(First(handlers)).Into(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenInto(handler);

                return chain;
            }

            public IHandler<T> JoinChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = Join(First(handlers)).With(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenWith(handler);

                return chain;
            }

            public IHandler<T> MergeChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = Merge(First(handlers)).With(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenWith(handler);

                return chain;
            }

            public IHandler<T> PackChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = Pack(First(handlers)).In(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenIn(handler);

                return chain;
            }

            public IHandler<T> WrapChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = Use(First(handlers)).ToWrap(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenWrap(handler);

                return chain;
            }

            public IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition) =>
                handler.When(condition);
        }

        static T First<T>(IEnumerable<T> source) =>
            source.First();

        static T Second<T>(IEnumerable<T> source) =>
            source.Skip(1).First();

        static IEnumerable<T> Tail<T>(IEnumerable<T> source) =>
            source.Skip(2);
    }
}
