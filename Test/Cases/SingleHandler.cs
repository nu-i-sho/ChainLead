namespace Nuisho.ChainLead.Test
{
    using Nuisho.ChainLead.Contracts;
    using Nuisho.ChainLead.Contracts.Syntax;
    using Nuisho.ChainLead.Test.Types;

    using static Nuisho.ChainLead.Contracts.Syntax.ChainLeadSyntax;
    
    public static partial class Cases
    {
        public static class SingleHandler
        {
            public class OriginalTestFixtureAttribute(Type t) 
                : TestFixtureAttribute(t, new OriginalMathFactory());

            public class _I_Attribute() : OriginalTestFixtureAttribute(typeof(int));
            public class _II_Attribute() : OriginalTestFixtureAttribute(typeof(string));
            public class _III_Attribute() : OriginalTestFixtureAttribute(typeof(Class));
            public class _IV_Attribute() : OriginalTestFixtureAttribute(typeof(Struct));
            public class _V_Attribute() : OriginalTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _VI_Attribute() : OriginalTestFixtureAttribute(typeof(Record));
            public class _VII_Attribute() : OriginalTestFixtureAttribute(typeof(RecordStruct));
            public class _VIII_Attribute() : OriginalTestFixtureAttribute(typeof(ReadonlyRecordStruct));

            public class SyntaxTestFixtureAttribute(Type t)
                : TestFixtureAttribute(t, new SyntaxMathFactory());

            public class _IX_Attribute() : SyntaxTestFixtureAttribute(typeof(int));
            public class _X_Attribute() : SyntaxTestFixtureAttribute(typeof(string));
            public class _XI_Attribute() : SyntaxTestFixtureAttribute(typeof(Class));
            public class _XII_Attribute() : SyntaxTestFixtureAttribute(typeof(Struct));
            public class _XIII_Attribute() : SyntaxTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _XIV_Attribute() : SyntaxTestFixtureAttribute(typeof(Record));
            public class _XV_Attribute() : SyntaxTestFixtureAttribute(typeof(RecordStruct));
            public class _XVI_Attribute() : SyntaxTestFixtureAttribute(typeof(ReadonlyRecordStruct));

            public interface ISingleHandlerMathFactory
            {
                ISingleHandlerMath Create(IConditionMath conditionMath);
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

            public class OriginalMathFactory
                : ISingleHandlerMathFactory
            {
                public ISingleHandlerMath Create(IConditionMath conditionMath) =>
                    new Product(new Implementation.HandlerMath(conditionMath));

                public override string ToString() => "Original";

                public class Product(IHandlerMath math) : ISingleHandlerMath
                {
                    protected IHandlerMath Math => math;

                    public IHandler<T> MakeHandler<T>(Action<T> action) =>
                        Math.MakeHandler(action);

                    public IHandler<T> Zero<T>() =>
                        Math.Zero<T>();

                    public bool IsZero<T>(IHandler<T> handler) =>
                        Math.IsZero(handler);

                    public IHandler<T> Atomize<T>(IHandler<T> handler) =>
                        Math.Atomize(handler);

                    public IHandler<T> Conditional<T>(
                        IHandler<T> handler,
                        ICondition<T> condition) =>
                            Math.Conditional(handler, condition);
                }
            }

            public class SyntaxMathFactory : ISingleHandlerMathFactory
            {
                public ISingleHandlerMath Create(IConditionMath conditionMath) =>
                    new Product(conditionMath);

                public override string ToString() => "Syntax";

                public class Product : ISingleHandlerMath
                {
                    public Product(IConditionMath conditionMath)
                    {
                        Configure(
                            new Implementation.HandlerMath(conditionMath),
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
            }
        }
    }
}
