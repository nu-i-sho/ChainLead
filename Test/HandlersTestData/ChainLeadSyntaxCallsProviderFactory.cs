namespace ChainLead.Test.HandlersTestData
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using ChainLead.Contracts.Syntax;

    public class ChainLeadSyntaxCallsProviderFactory
        : IHandlerMathCallsProviderFactory
    {
        public IHandlerMath Create(IConditionMath conditionMath)
        {
            IHandlerMath math = new HandlerMath(conditionMath);
            ChainLeadSyntax.Configure(math, conditionMath);

            return new Calls();
        }

        public override string ToString() => "\"direct ChainLeadSyntax\"";

        class Calls : IHandlerMath
        {
            public IHandler<T> Zero<T>() =>
                ChainLeadSyntax.Handler<T>.Zero;

            public IHandler<T> MakeHandler<T>(Action<T> action) =>
                ChainLeadSyntax.MakeHandler(action);

            public bool IsZero<T>(IHandler<T> handler) =>
                ChainLeadSyntax.IsZero(handler);

            public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                ChainLeadSyntax.FirstThenSecond(a, b);

            public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                ChainLeadSyntax.FirstCoverSecond(a, b);

            public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                ChainLeadSyntax.FirstWrapSecond(a,b);

            public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                ChainLeadSyntax.InjectFirstIntoSecond(a, b);

            public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                ChainLeadSyntax.JoinFirstWithSecond(a, b);

            public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                ChainLeadSyntax.MergeFirstWithSecond(a, b);

            public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                ChainLeadSyntax.PackFirstInSecond(a, b);

            public IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition) =>
                ChainLeadSyntax.When(handler, condition);

            public IHandler<T> Atomize<T>(IHandler<T> handler) =>
                ChainLeadSyntax.Atomize(handler);
        }
    }
}
