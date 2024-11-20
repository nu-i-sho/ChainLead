﻿namespace ChainLead.Test.HandlersMathTestData
{
    using ChainLead.Contracts;
    using ChainLead.Implementation;
    using ChainLead.Contracts.Syntax;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    public class ChainLeadSytaxReverseCallsProvider
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

        public override string ToString() => "like XThen(b).WhereXIs(a)";

        private class Calls : IHandlerMath
        {
            public IHandler<T> Zero<T>() =>
                Handler<T>.Zero;

            public IHandler<T> MakeHandler<T>(Action<T> action) =>
                ChainLeadSyntax.MakeHandler(action);

            public bool IsZero<T>(IHandler<T> handler) =>
                ChainLeadSyntax.IsZero(handler);

            public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                XThen(b).WhereXIs(a);

            public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                XCover(b).WhereXIs(a);

            public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                XWrap(b).WhereXIs(a);

            public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                InjectXInto(b).WhereXIs(a);

            public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                JoinXWith(b).WhereXIs(a);

            public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                MergeXWith(b).WhereXIs(a);

            public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                PackXIn(b).WhereXIs(a);

            public IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition) =>
                handler.When(condition);
        }
    }
}