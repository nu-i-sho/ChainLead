namespace ChainLead.Test.HandlersMathTestData
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;

    public class OriginalHandlerMathCallsProvider
        : IHandlerMathCallsProviderFactory
    {
        public IHandlerMath Create(IConditionMath conditionalMath) =>
            new HandlerMath(conditionalMath);

        public override string ToString() => $"original";
    }
}
