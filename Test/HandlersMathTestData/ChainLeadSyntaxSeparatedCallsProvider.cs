namespace ChainLead.Test.HandlersMathTestData
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using ChainLead.Contracts.Syntax;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    public class ChainLeadSyntaxSeparatedCallsProvider
       : IHandlerMathCallsProviderFactory
    {
        public IHandlerMath Create(IConditionMath conditionalMath)
        {
            IHandlerMath math = new HandlerMath(conditionalMath);
            ConfigureChainLeadSyntax
                .WithHandlerMath(math)
                .AndWithConditionMath(conditionalMath);

            return new Calls();
        }

        public override string ToString() => "like a.Then(b)";

        private class Calls : IHandlerMath
        {
            public IHandler<T> Zero<T>() =>
                Handler<T>.Zero;

            public IHandler<T> MakeHandler<T>(Action<T> action) =>
                ChainLeadSyntax.MakeHandler(action);

            public bool IsZero<T>(IHandler<T> handler) =>
                ChainLeadSyntax.IsZero(handler);

            public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                a.Then(b);

            public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                Use(a).ToCover(b);

            public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                Use(a).ToWrap(b);

            public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                Inject(a).Into(b);

            public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                Join(a).With(b);

            public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                Merge(a).With(b);

            public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                Pack(a).In(b);

            public IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition) =>
                handler.When(condition);
        }
    }
}
