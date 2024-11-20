namespace ChainLead.Test.HandlersMathTestData
{
    using ChainLead.Contracts;

    public interface IHandlerMathCallsProviderFactory
    {
        IHandlerMath Create(IConditionMath conditionalMath);
    }
}
