namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Implementation;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Cases.Common.Types;
    using static ChainLead.Test.Cases.Common;

    public static partial class Cases
    {
        public static class MultipleHandlersFixtureCases
        {
            public const string Direct = "Direct";
            public const string Reverse = "Reverse";
            public class TestFixtureAttribute(Type t, string mathName)
                : NUnit.Framework.TestFixtureAttribute(t,
                    GetMathFactory(mathName),
                    TokensProvider.Get(t, 28532))
            {
                static IMultipleHandlersMathFactory GetMathFactory(string name) =>
                    name switch
                    {
                        Direct => new DirectChainingMathFactory(),
                        Reverse => new ReverseChainingMathFactory(),
                        _ => throw new ArgumentOutOfRangeException(nameof(name))
                    };
            }

            public class _I_Attribute() : TestFixtureAttribute(typeof(int), Direct);
            public class _II_Attribute() : TestFixtureAttribute(typeof(string), Direct);
            public class _III_Attribute() : TestFixtureAttribute(typeof(Class), Direct);
            public class _IV_Attribute() : TestFixtureAttribute(typeof(Struct), Direct);
            public class _V_Attribute() : TestFixtureAttribute(typeof(ReadonlyStruct), Direct);
            public class _VI_Attribute() : TestFixtureAttribute(typeof(Record), Direct);
            public class _VII_Attribute() : TestFixtureAttribute(typeof(RecordStruct), Direct);
            public class _VIII_Attribute() : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Direct);

            public class _IX_Attribute() : TestFixtureAttribute(typeof(int), Reverse);
            public class _X_Attribute() : TestFixtureAttribute(typeof(string), Reverse);
            public class _XI_Attribute() : TestFixtureAttribute(typeof(Class), Reverse);
            public class _XII_Attribute() : TestFixtureAttribute(typeof(Struct), Reverse);
            public class _XIII_Attribute() : TestFixtureAttribute(typeof(ReadonlyStruct), Reverse);
            public class _XIV_Attribute() : TestFixtureAttribute(typeof(Record), Reverse);
            public class _XV_Attribute() : TestFixtureAttribute(typeof(RecordStruct), Reverse);
            public class _XVI_Attribute() : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Reverse);

            public interface IMultipleHandlersMathFactory
            {
                IMultipleHandlersMath Create(IConditionMath conditionMath);
            }

            public interface IMultipleHandlersMath
            {
                IHandler<T> ThenChain<T>(IEnumerable<IHandler<T>> handlers);

                IHandler<T> PackChain<T>(IEnumerable<IHandler<T>> handlers);

                IHandler<T> InjectChain<T>(IEnumerable<IHandler<T>> handlers);

                IHandler<T> CoverChain<T>(IEnumerable<IHandler<T>> handlers);

                IHandler<T> WrapChain<T>(IEnumerable<IHandler<T>> handlers);

                IHandler<T> JoinChain<T>(IEnumerable<IHandler<T>> handlers);

                IHandler<T> MergeChain<T>(IEnumerable<IHandler<T>> handlers);

                IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition);
            }

            public class DirectChainingMathFactory
                : IMultipleHandlersMathFactory
            {
                public IMultipleHandlersMath Create(IConditionMath conditionMath)
                {
                    IHandlerMath math = new HandlerMath(conditionMath);
                    ChainLeadSyntax.Configure(math, conditionMath);

                    return new Product();
                }

                public override string ToString() => "\"like Use(a).ToMerge(b).ThenMerge(c)\"";

                class Product : IMultipleHandlersMath
                {
                    public IHandler<T> ThenChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = First(handlers).Then(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.Then(handler);

                        return chain;
                    }

                    public IHandler<T> CoverChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = Use(First(handlers)).ToCover(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenCover(handler);

                        return chain;
                    }

                    public IHandler<T> InjectChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = Inject(First(handlers)).Into(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenInto(handler);

                        return chain;
                    }

                    public IHandler<T> JoinChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = Join(First(handlers)).With(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenWith(handler);

                        return chain;
                    }

                    public IHandler<T> MergeChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = Merge(First(handlers)).With(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenWith(handler);

                        return chain;
                    }

                    public IHandler<T> PackChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = Pack(First(handlers)).In(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenIn(handler);

                        return chain;
                    }

                    public IHandler<T> WrapChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = Use(First(handlers)).ToWrap(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenWrap(handler);

                        return chain;
                    }

                    public IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition) =>
                        handler.When(condition);
                }
            }

            public class ReverseChainingMathFactory
                : IMultipleHandlersMathFactory
            {
                public IMultipleHandlersMath Create(IConditionMath conditionMath)
                {
                    IHandlerMath math = new HandlerMath(conditionMath);
                    ChainLeadSyntax.Configure(math, conditionMath);

                    return new Product();
                }

                public override string ToString() => "\"like MergeXWith[b].ThenWith[c].WhereXIs[c]\"";

                class Product : IMultipleHandlersMath
                {
                    public IHandler<T> ThenChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = XThen(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.Then(handler);

                        return chain.WhereXIs(First(handlers));
                    }

                    public IHandler<T> CoverChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = XCover(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenCover(handler);

                        return chain.WhereXIs(First(handlers));
                    }

                    public IHandler<T> InjectChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = InjectXInto(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenInto(handler);

                        return chain.WhereXIs(First(handlers));
                    }

                    public IHandler<T> JoinChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = JoinXWith(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenWith(handler);

                        return chain.WhereXIs(First(handlers));
                    }

                    public IHandler<T> MergeChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = MergeXWith(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenWith(handler);

                        return chain.WhereXIs(First(handlers));
                    }

                    public IHandler<T> PackChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = PackXIn(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenIn(handler);

                        return chain.WhereXIs(First(handlers));
                    }

                    public IHandler<T> WrapChain<T>(IEnumerable<IHandler<T>> handlers)
                    {
                        var chain = XWrap(Second(handlers));
                        foreach (var handler in Tail(handlers))
                            chain = chain.ThenWrap(handler);

                        return chain.WhereXIs(First(handlers));
                    }

                    public IHandler<T> Conditional<T>(IHandler<T> handler, ICondition<T> condition) =>
                        handler.When(condition);
                }
            }

            static T First<T>(IEnumerable<T> source) =>
                source.First();

            static T Second<T>(IEnumerable<T> source) =>
                source.Skip(1).First();

            static IEnumerable<T> Tail<T>(IEnumerable<T> source) =>
                source.Skip(2);
        }
    }
}
