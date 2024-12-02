namespace ChainLead.Contracts
{
    public interface IExtendedHandler<in T> : IHandler<T>
    {
        internal IHandler<T> Origin { get; }

        void IHandler<T>.Execute(T state) => Origin.Execute(state);
    }
}
