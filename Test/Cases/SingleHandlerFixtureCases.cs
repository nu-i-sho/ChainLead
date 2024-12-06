namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Implementation;
    using System;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Cases.Common.Types;

    public static partial class Cases
    {
        public static class SingleHandlerFixtureCases
        {
            public const string Original = "Original";
            public const string Syntax = "Syntax";

            public class _I_Attribute() : TestFixtureAttribute(typeof(int), Original);
            public class _II_Attribute() : TestFixtureAttribute(typeof(string), Original);
            public class _III_Attribute() : TestFixtureAttribute(typeof(Class), Original);
            public class _IV_Attribute() : TestFixtureAttribute(typeof(Struct), Original);
            public class _V_Attribute() : TestFixtureAttribute(typeof(ReadonlyStruct), Original);
            public class _VI_Attribute() : TestFixtureAttribute(typeof(Record), Original);
            public class _VII_Attribute() : TestFixtureAttribute(typeof(RecordStruct), Original);
            public class _VIII_Attribute() : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Original);

            public class _IX_Attribute() : TestFixtureAttribute(typeof(int), Syntax);
            public class _X_Attribute() : TestFixtureAttribute(typeof(string), Syntax);
            public class _XI_Attribute() : TestFixtureAttribute(typeof(Class), Syntax);
            public class _XII_Attribute() : TestFixtureAttribute(typeof(Struct), Syntax);
            public class _XIII_Attribute() : TestFixtureAttribute(typeof(ReadonlyStruct), Syntax);
            public class _XIV_Attribute() : TestFixtureAttribute(typeof(Record), Syntax);
            public class _XV_Attribute() : TestFixtureAttribute(typeof(RecordStruct), Syntax);
            public class _XVI_Attribute() : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Syntax);

            public static class SingleHandlerMathFactoryProvider
            {
                public static ISingleHandlerMathFactory Get(string name) =>
                    name switch
                    {
                        Original => new OriginalMathFactory(),
                        Syntax => new SyntaxMathFactory(),
                        _ => throw new ArgumentOutOfRangeException(nameof(name))
                    };
            }

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

                public override string ToString() => Original;
            }

            public class SyntaxMathFactory : ISingleHandlerMathFactory
            {
                public ISingleHandlerMath Create(IConditionMath conditionMath) =>
                    new Product(conditionMath);

                public class Product : ISingleHandlerMath
                {
                    public Product(IConditionMath conditionMath)
                    {
                        Configure(
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

                public override string ToString() => Syntax;
            }
        }
    }
}
