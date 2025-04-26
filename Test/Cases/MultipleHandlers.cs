namespace Nuisho.ChainLead.Test;

using Contracts;
using Contracts.Syntax;
using Types;

using static Contracts.Syntax.ChainLeadSyntax;

public static partial class Cases
{
    public static class MultipleHandlers
    {
        public sealed class IIndicesAttribute() :
            ValuesAttribute("AB", "ABC", "ABCD", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

        public sealed class JIndicesAttribute() :
            ValuesAttribute("012", "01234", "0123456789");

        public abstract class DirectTestFixtureAttribute(Type t)
            : TestFixtureAttribute(t, new DirectChainingMathFactory());

        public sealed class _I_Attribute() : DirectTestFixtureAttribute(typeof(int));
        public sealed class _II_Attribute() : DirectTestFixtureAttribute(typeof(string));
        public sealed class _III_Attribute() : DirectTestFixtureAttribute(typeof(Class));
        public sealed class _IV_Attribute() : DirectTestFixtureAttribute(typeof(Struct));
        public sealed class _V_Attribute() : DirectTestFixtureAttribute(typeof(ReadonlyStruct));
        public sealed class _VI_Attribute() : DirectTestFixtureAttribute(typeof(Record));
        public sealed class _VII_Attribute() : DirectTestFixtureAttribute(typeof(RecordStruct));
        public sealed class _VIII_Attribute() : DirectTestFixtureAttribute(typeof(ReadonlyRecordStruct));

        public abstract class ReverseTestFixtureAttribute(Type t)
            : TestFixtureAttribute(t, new ReverseChainingMathFactory());

        public sealed class _IX_Attribute() : ReverseTestFixtureAttribute(typeof(int));
        public sealed class _X_Attribute() : ReverseTestFixtureAttribute(typeof(string));
        public sealed class _XI_Attribute() : ReverseTestFixtureAttribute(typeof(Class));
        public sealed class _XII_Attribute() : ReverseTestFixtureAttribute(typeof(Struct));
        public sealed class _XIII_Attribute() : ReverseTestFixtureAttribute(typeof(ReadonlyStruct));
        public sealed class _XIV_Attribute() : ReverseTestFixtureAttribute(typeof(Record));
        public sealed class _XV_Attribute() : ReverseTestFixtureAttribute(typeof(RecordStruct));
        public sealed class _XVI_Attribute() : ReverseTestFixtureAttribute(typeof(ReadonlyRecordStruct));

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

        public sealed class DirectChainingMathFactory
            : IMultipleHandlersMathFactory
        {
            public IMultipleHandlersMath Create(IConditionMath conditionMath)
            {
                IHandlerMath math = new Implementation.HandlerMath(conditionMath);
                ChainLeadSyntax.Configure(math, conditionMath);

                return new Product();
            }

            public override string ToString() => "like Use(a).ToMerge(b).ThenMerge(c)";

            sealed class Product : IMultipleHandlersMath
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

        public sealed class ReverseChainingMathFactory
            : IMultipleHandlersMathFactory
        {
            public IMultipleHandlersMath Create(IConditionMath conditionMath)
            {
                IHandlerMath math = new Implementation.HandlerMath(conditionMath);
                ChainLeadSyntax.Configure(math, conditionMath);

                return new Product();
            }

            public override string ToString() => "like MergeXWith(b).ThenWith(c).WhereXIs(a)";

            sealed class Product : IMultipleHandlersMath
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
