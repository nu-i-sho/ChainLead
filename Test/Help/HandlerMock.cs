namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using Moq;

    using static Constants;

    public class HandlerMock : 
        Mock<IHandler<int>>,
        IIndexedWith<HandlerIndex>
    {
        private readonly HandlerMocksCollection _handlers;

        public HandlerMock(
            HandlerMocksCollection handlers,
            HandlerIndex index)
        {
            _handlers = handlers;
            Name = index.View;
            Index = index;
        }

        public HandlerIndex Index { get; }

        public void Setup__Execute__Calling(Action<int> f) =>
             Setup(o => o.Execute(Arg))
            .Callback((int x) => f(x));

        public void Setup__Execute__LoggingInto(IList<HandlerIndex> acc) =>
             Setup(o => o.Execute(Arg))
            .Callback(() => acc.Add(Index));

        public void Setup__Execute__LoggingInto(IList<MockIndex> acc) =>
             Setup(o => o.Execute(Arg))
            .Callback(() => acc.Add(Index));

        public void Setup__Execute__DelegatingTo(params HandlerIndex[] indexes) =>
             Setup(o => o.Execute(Arg))
            .Callback(() =>
            {
                foreach (var i in indexes)
                    _handlers[i].Object.Execute(Arg);
            });

        public void Verify__Execute(Times times) =>
            Verify(o => o.Execute(Arg), times);

        public void Verify__Execute(Func<Times> times) =>
            Verify(o => o.Execute(Arg), times);
    }
}
