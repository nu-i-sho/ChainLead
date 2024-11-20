namespace ChainLead.Test.HandlersTestData
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    public class ChainLeadSyntaxReversedChainingCallsProviderFactory
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

        public override string ToString() => "like MergeXWith(b).ThenWith(c).WhereXIs(c)";

        class Calls : IHandlerChainingCallsProvider
        {
            public IHandler<T> ThenChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = XThen(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.Then(handler);

                return chain.WhereXIs(First(handlers));
            }

            public IHandler<T> CoverChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = XCover(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenCover(handler);

                return chain.WhereXIs(First(handlers));
            }

            public IHandler<T> InjectChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = InjectXInto(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenInto(handler);

                return chain.WhereXIs(First(handlers));
            }

            public IHandler<T> JoinChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = JoinXWith(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenWith(handler);

                return chain.WhereXIs(First(handlers));
            }

            public IHandler<T> MergeChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = MergeXWith(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenWith(handler);

                return chain.WhereXIs(First(handlers));
            }

            public IHandler<T> PackChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = PackXIn(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenIn(handler);

                return chain.WhereXIs(First(handlers));
            }

            public IHandler<T> WrapChain<T>(IEnumerable<IHandler<T>> handlers)
            {
                var chain = XWrap(Second(handlers));
                foreach (var handler in Tail(handlers))
                    chain = chain.ThenWrap(handler);

                return chain.WhereXIs(First(handlers));
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
