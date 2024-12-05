namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Implementation;
    using System;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    public partial class SingleHandlerMathTest
    {
        public static IEnumerable<ISingleHandlerMathFactory> FixtureCases
        {
            get
            {
                yield return new OriginalMathFactory();
                yield return new SyntaxMathFactory();
            }
        }

        public interface ISingleHandlerMath
        {
            IHandler<T> Zero<T>();

            IHandler<T> MakeHandler<T>(Action<T> action);

            bool IsZero<T>(IHandler<T> handler);

            IHandler<T> Atomize<T>(IHandler<T> handler);

            IHandler<T> Conditional<T>(
                IHandler<T> handler, 
                ICondition<T> condition);
        }

        public interface ISingleHandlerMathFactory
        {
            ISingleHandlerMath Create(IConditionMath conditionMath);
        }

        public class OriginalMathFactory 
            : ISingleHandlerMathFactory
        {
            public ISingleHandlerMath Create(IConditionMath conditionMath) => 
                new Product(new HandlerMath(conditionMath));

            public class Product(IHandlerMath math) : ISingleHandlerMath
            {
                public IHandler<T> MakeHandler<T>(Action<T> action) => 
                    math.MakeHandler(action);

                public IHandler<T> Zero<T>() => 
                    math.Zero<T>();

                public bool IsZero<T>(IHandler<T> handler) => 
                    math.IsZero(handler);

                public IHandler<T> Atomize<T>(IHandler<T> handler) => 
                    math.Atomize(handler);

                public IHandler<T> Conditional<T>(
                    IHandler<T> handler, 
                    ICondition<T> condition) =>
                        math.Conditional(handler, condition);
            }

            public override string ToString() => "Original";
        }

        public class SyntaxMathFactory : ISingleHandlerMathFactory
        {
            public ISingleHandlerMath Create(IConditionMath conditionMath) => 
                new Product(conditionMath);

            public class Product : ISingleHandlerMath
            {
                public Product(IConditionMath conditionMath)
                {
                    ChainLeadSyntax.Configure(
                        new HandlerMath(conditionMath),
                        conditionMath);
                }

                public IHandler<T> MakeHandler<T>(Action<T> action) =>
                    ChainLeadSyntax.MakeHandler(action);

                public IHandler<T> Zero<T>() => 
                    Handler<T>.Zero;

                public bool IsZero<T>(IHandler<T> handler) =>
                    handler.IsZero();

                public IHandler<T> Atomize<T>(IHandler<T> handler) =>
                    handler.Atomize();

                public IHandler<T> Conditional<T>(
                    IHandler<T> handler,
                    ICondition<T> condition) =>
                        handler.When(condition);
            }

            public override string ToString() => "Syntax";
        }
    }
}
