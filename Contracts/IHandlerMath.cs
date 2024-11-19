namespace ChainLead.Contracts
{
    public interface IHandlerMath
    {
        IHandler<T> Zero<T>();

        IHandler<T> MakeHandler<T>(Action<T> action);

        bool IsZero<T>(IHandler<T> handler);

        IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> PutFirstInSecond<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition);
    }
}
