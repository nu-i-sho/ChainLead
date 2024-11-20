namespace ChainLead.Test.HandlersTestData
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;

    public class OriginalHandlerMathCallsProviderFactory
        : IHandlerMathCallsProviderFactory
    {
        public IHandlerMath Create(IConditionMath conditionMath) =>
            new HandlerMath(conditionMath);

        public override string ToString() => $"original";
    }
}
