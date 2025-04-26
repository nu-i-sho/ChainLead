namespace Nuisho.ChainLead.Contracts;

public interface IHandler<in T>
{
    void Execute(T state);
}
