namespace Nuisho.ChainLead.Test
{
    using Contracts;
    using Contracts.Syntax;
    using Types;

    using static Contracts.Syntax.ChainLeadSyntax;

    public static partial class Cases
    {
        public static class MultipleHandlers
        {
            public class IIndicesAttribute() : 
                ValuesAttribute("AB", "ABC", "ABCD", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            public class JIndicesAttribute() :
                ValuesAttribute("012", "01234", "0123456789");

            public class DirectTestFixtureAttribute(Type t)
                : TestFixtureAttribute(t, new DirectChainingMathFactory());

            public class _I_Attribute() : DirectTestFixtureAttribute(typeof(int));
            public class _II_Attribute() : DirectTestFixtureAttribute(typeof(string));
            public class _III_Attribute() : DirectTestFixtureAttribute(typeof(Class));
            public class _IV_Attribute() : DirectTestFixtureAttribute(typeof(Struct));
            public class _V_Attribute() : DirectTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _VI_Attribute() : DirectTestFixtureAttribute(typeof(Record));
            public class _VII_Attribute() : DirectTestFixtureAttribute(typeof(RecordStruct));
            public class _VIII_Attribute() : DirectTestFixtureAttribute(typeof(ReadonlyRecordStruct));

            public class ReverseTestFixtureAttribute(Type t)
                : TestFixtureAttribute(t, new ReverseChainingMathFactory());

            public class _IX_Attribute() : ReverseTestFixtureAttribute(typeof(int));
            public class _X_Attribute() : ReverseTestFixtureAttribute(typeof(string));
            public class _XI_Attribute() : ReverseTestFixtureAttribute(typeof(Class));
            public class _XII_Attribute() : ReverseTestFixtureAttribute(typeof(Struct));
            public class _XIII_Attribute() : ReverseTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _XIV_Attribute() : ReverseTestFixtureAttribute(typeof(Record));
            public class _XV_Attribute() : ReverseTestFixtureAttribute(typeof(RecordStruct));
            public class _XVI_Attribute() : ReverseTestFixtureAttribute(typeof(ReadonlyRecordStruct));

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
                    IHandlerMath math = new Implementation.HandlerMath(conditionMath);
                    ChainLeadSyntax.Configure(math, conditionMath);

                    return new Product();
                }

                public override string ToString() => "like Use(a).ToMerge(b).ThenMerge(c)";

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
                    IHandlerMath math = new Implementation.HandlerMath(conditionMath);
                    ChainLeadSyntax.Configure(math, conditionMath);

                    return new Product();
                }

                public override string ToString() => "like MergeXWith(b).ThenWith(c).WhereXIs(a)";

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
