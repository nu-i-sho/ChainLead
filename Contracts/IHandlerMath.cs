namespace ChainLead.Contracts
{
    public interface IHandlerMath
    {
        IHandler<T> Zero<T>();

        IHandler<T> MakeHandler<T>(Action<T> action);

        bool IsZero<T>(IHandler<T> handler);

        IHandler<T> FirstThenSecond<T>(IHandler<T> prev, IHandler<T> next);

        IHandler<T> PackFirstInSecond<T>(IHandler<T> prev, IHandler<T> next);

        IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> prev, IHandler<T> next);

        IHandler<T> FirstCoverSecond<T>(IHandler<T> prev, IHandler<T> next);

        IHandler<T> FirstWrapSecond<T>(IHandler<T> prev, IHandler<T> next);

        IHandler<T> JoinFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next);

        IHandler<T> MergeFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next);

        IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition);

        IHandler<T> Atomize<T>(IHandler<T> handler);
    }
}
