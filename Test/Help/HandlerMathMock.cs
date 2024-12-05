namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using Moq;

    using static Constants;

    public class HandlerMathMock(
        DummyHandlersCollection handlers,
        DummyConditionsCollection conditions)
        : Mock<IHandlerMath>
    {
        public void Setup__Zero(HandlerIndex returns) =>
             Setup(o => o.Zero<int>())
            .Returns(handlers[returns].Object);

        public void Setup__IsZero(HandlerIndex i, bool returns) =>
             Setup(o => o.IsZero(handlers[i].Object))
            .Returns(returns);

        public void Setup__MakeHandler(HandlerIndex i)
        {
            Action<int>? action = default;

            Setup(o => o.MakeHandler(It.IsAny<Action<int>>()))
           .Returns(handlers[i].Object)
           .Callback((Action<int> f) => action = f);

            handlers[i].AddCallback(() => action!(Arg));
        }

        public void Setup__FirstThenSecond(
            HandlerIndex first,
            HandlerIndex second,
            HandlerIndex returns) =>
                 Setup(o => o.FirstThenSecond(
                    handlers[first].Object,
                    handlers[second].Object))
                .Returns(handlers[returns].Object);

        public void Setup__JoinFirstWithSecond(
            HandlerIndex first,
            HandlerIndex second,
            HandlerIndex returns) =>
                 Setup(o => o.JoinFirstWithSecond(
                    handlers[first].Object,
                    handlers[second].Object))
                .Returns(handlers[returns].Object);

        public void Setup__MergeFirstWithSecond(
            HandlerIndex first,
            HandlerIndex second,
            HandlerIndex returns) =>
                 Setup(o => o.MergeFirstWithSecond(
                    handlers[first].Object,
                    handlers[second].Object))
                .Returns(handlers[returns].Object);

        public void Setup__PackFirstInSecond(
            HandlerIndex first,
            HandlerIndex second,
            HandlerIndex returns) =>
                 Setup(o => o.PackFirstInSecond(
                    handlers[first].Object,
                    handlers[second].Object))
                .Returns(handlers[returns].Object);

        public void Setup__InjectFirstIntoSecond(
            HandlerIndex first,
            HandlerIndex second,
            HandlerIndex returns) =>
                 Setup(o => o.InjectFirstIntoSecond(
                    handlers[first].Object,
                    handlers[second].Object))
                .Returns(handlers[returns].Object);

        public void Setup__FirstCoverSecond(
            HandlerIndex first,
            HandlerIndex second,
            HandlerIndex returns) =>
                 Setup(o => o.FirstCoverSecond(
                    handlers[first].Object,
                    handlers[second].Object))
                .Returns(handlers[returns].Object);

        public void Setup__FirstWrapSecond(
            HandlerIndex first,
            HandlerIndex second,
            HandlerIndex returns) =>
                Setup(o => o.FirstWrapSecond(
                    handlers[first].Object,
                    handlers[second].Object))
               .Returns(handlers[returns].Object);

        public void Setup__Conditional(
            HandlerIndex handler,
            ConditionIndex condition,
            HandlerIndex returns) =>
                 Setup(o => o.Conditional(
                     handlers[handler].Object,
                     conditions[condition].Object))
                .Returns(handlers[returns].Object);

        public void Setup__Atomize(
            HandlerIndex i,
            HandlerIndex returns) =>
                 Setup(o => o.Atomize(handlers[i].Object))
                .Returns(handlers[returns].Object);
    }
}
