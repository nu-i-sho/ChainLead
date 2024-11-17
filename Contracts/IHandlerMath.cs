namespace ChainLead.Contracts
{
    public interface IHandlerMath
    {
        IHandler<T> Zero<T>();

        IHandler<T> MakeHandler<T>(Action<T> action);

        bool IsZero<T>(IHandler<T> handler);

        IHandler<T> Join<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> Merge<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> DeepMerge<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> Inject<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> DeepInject<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> Wrap<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> DeepWrap<T>(IHandler<T> a, IHandler<T> b);

        IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition);
    }
}
