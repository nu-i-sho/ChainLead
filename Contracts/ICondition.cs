namespace ChainLead.Contracts
{
    public interface ICondition<in T>
    {
        bool Check(T obj);
    }
}
