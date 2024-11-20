namespace ChainLead.Test.HandlersTestData
{
    using ChainLead.Contracts;

    public interface IHandlerChainingCallsProvider
    {
        IHandler<T> ThenChain<T>(IEnumerable<IHandler<T>> handlers);

        IHandler<T> PackChain<T>(IEnumerable<IHandler<T>> handlers);

        IHandler<T> InjectChain<T>(IEnumerable<IHandler<T>> handlers);

        IHandler<T> CoverChain<T>(IEnumerable<IHandler<T>> handlers);

        IHandler<T> WrapChain<T>(IEnumerable<IHandler<T>> handlers);

        IHandler<T> JoinChain<T>(IEnumerable<IHandler<T>> handlers);

        IHandler<T> MergeChain<T>(IEnumerable<IHandler<T>> handlers);

        IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition);
    }
}
