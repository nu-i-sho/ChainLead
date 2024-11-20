namespace ChainLead.Test.HandlersTestData
{
    using ChainLead.Contracts;

    public interface IHandlerMathCallsProviderFactory
    {
        IHandlerMath Create(IConditionMath conditionalMath);
    }
}
