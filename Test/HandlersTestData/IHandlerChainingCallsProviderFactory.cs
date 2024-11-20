namespace ChainLead.Test.HandlersTestData
{
    using ChainLead.Contracts;

    public interface IHandlerChainingCallsProviderFactory
    {
        IHandlerChainingCallsProvider Create(IConditionMath conditionMath);
    }
}
