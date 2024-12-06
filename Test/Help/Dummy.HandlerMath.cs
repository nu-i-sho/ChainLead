using ChainLead.Contracts;
using Moq;

namespace ChainLead.Test.Help
{
    public static partial class Dummy
    {
        public class HandlerMath<T>(
                Dummy.HandlerCollection<T> handlers,
                Dummy.ConditionCollection<T> conditions,
                T expectedArg)
            : Mock<IHandlerMath>
        {
            public void Setup__Zero(Dummy.HandlerIndex returns) =>
                 Setup(o => o.Zero<T>())
                .Returns(handlers[returns]);

            public void Setup__IsZero(Dummy.HandlerIndex i, bool returns) =>
                 Setup(o => o.IsZero(handlers[i]))
                .Returns(returns);

            public void Setup__MakeHandler(Dummy.HandlerIndex i)
            {
                Action<T>? action = default;

                Setup(o => o.MakeHandler(It.IsAny<Action<T>>()))
               .Returns(handlers[i])
               .Callback((Action<T> f) => action = f);

                handlers[i].AddCallback(() => action!(expectedArg));
            }

            public void Setup__FirstThenSecond(
                Dummy.HandlerIndex first,
                Dummy.HandlerIndex second,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.FirstThenSecond(
                        handlers[first],
                        handlers[second]))
                    .Returns(handlers[returns]);

            public void Setup__JoinFirstWithSecond(
                Dummy.HandlerIndex first,
                Dummy.HandlerIndex second,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.JoinFirstWithSecond(
                        handlers[first],
                        handlers[second]))
                    .Returns(handlers[returns]);

            public void Setup__MergeFirstWithSecond(
                Dummy.HandlerIndex first,
                Dummy.HandlerIndex second,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.MergeFirstWithSecond(
                        handlers[first],
                        handlers[second]))
                    .Returns(handlers[returns]);

            public void Setup__PackFirstInSecond(
                Dummy.HandlerIndex first,
                Dummy.HandlerIndex second,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.PackFirstInSecond(
                        handlers[first],
                        handlers[second]))
                    .Returns(handlers[returns]);

            public void Setup__InjectFirstIntoSecond(
                Dummy.HandlerIndex first,
                Dummy.HandlerIndex second,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.InjectFirstIntoSecond(
                        handlers[first],
                        handlers[second]))
                    .Returns(handlers[returns]);

            public void Setup__FirstCoverSecond(
                Dummy.HandlerIndex first,
                Dummy.HandlerIndex second,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.FirstCoverSecond(
                        handlers[first],
                        handlers[second]))
                    .Returns(handlers[returns]);

            public void Setup__FirstWrapSecond(
                Dummy.HandlerIndex first,
                Dummy.HandlerIndex second,
                Dummy.HandlerIndex returns) =>
                    Setup(o => o.FirstWrapSecond(
                        handlers[first],
                        handlers[second]))
                   .Returns(handlers[returns]);

            public void Setup__Conditional(
                Dummy.HandlerIndex handler,
                Dummy.ConditionIndex condition,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.Conditional(
                         handlers[handler],
                         conditions[condition]))
                    .Returns(handlers[returns]);

            public void Setup__Atomize(
                Dummy.HandlerIndex i,
                Dummy.HandlerIndex returns) =>
                     Setup(o => o.Atomize(handlers[i]))
                    .Returns(handlers[returns]);
        }
    }
}
