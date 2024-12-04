namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using Moq;
    using System.Linq;
    using static Constants;

    public static class MockingExtensions
    {
        public static TMock Get<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> mocks, TIndex i)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    mocks[i];

        public static TObject GetObject<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> mocks, TIndex i)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    mocks[i].Object;

        public static IMocksCollection<TIndex, TMock, TObject> Where<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> mocks, Func<TMock, bool> predicate)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    new MocksCollection<TIndex, TMock, TObject>(
                        mocks.AsEnumerable().Where(predicate));

        public static IMocksCollection<TIndex, TMock, TObject> Concat<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> handlers,
            IEnumerable<TMock> other)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    new MocksCollection<TIndex, TMock, TObject>(
                        handlers.AsEnumerable().Concat(other));

        public static IMocksCollection<TIndex, TMock, TObject> Take<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> handlers, int count)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    new MocksCollection<TIndex, TMock, TObject>(
                        handlers.AsEnumerable().Take(count));

        public static IMocksCollection<TIndex, TMock, TObject> Skip<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> handlers, int count)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    new MocksCollection<TIndex, TMock, TObject>(
                        handlers.AsEnumerable().Take(count));

        public static IMocksCollection<TIndex, TMock, TObject> Reverse<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> handlers)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    new MocksCollection<TIndex, TMock, TObject>(
                        handlers.AsEnumerable().Reverse());

        public static IMocksCollection<TIndex, TMock, TObject> Except<TIndex, TMock, TObject>(
            this IMocksCollection<TIndex, TMock, TObject> handlers,
            IEnumerable<TMock> other)
                where TMock : Mock<TObject>, IIndexedWith<TIndex>
                where TObject : class =>
                    new MocksCollection<TIndex, TMock, TObject>(
                        handlers.AsEnumerable().Except(other));

        public static void Setup__Check(
            this IMocksCollection<ConditionIndex, ConditionMock, ICondition<int>> condition,
            bool returns)
        {
            foreach (var c in condition)
                c.Setup__Check(returns);
        }

        public static void Setup__Check(
            this IMocksCollection<ConditionIndex, ConditionMock, ICondition<int>> condition,
            IEnumerable<bool> returns)
        {
            foreach (var (c, r) in condition.Zip(returns))
                c.Setup__Check(r);
        }

        public static void Setup__Check__LoggingInto(
            this IMocksCollection<ConditionIndex, ConditionMock, ICondition<int>> condition,
            IList<ConditionIndex> acc)
        {
            foreach (var c in condition)
                c.Setup__Check__LoggingInto(acc);
        }

        public static void Setup__Check__LoggingInto(
            this IMocksCollection<ConditionIndex, ConditionMock, ICondition<int>> condition,
            IList<MockIndex> acc)
        {
            foreach (var c in condition)
                c.Setup__Check__LoggingInto(acc);
        }

        public static void Verify__Check(
            this IMocksCollection<ConditionIndex, ConditionMock, ICondition<int>> condition,
            Times times)
        {
            foreach (var c in condition)
                c.Verify__Check(times);
        }

        public static void Verify__Check(
            this IMocksCollection<ConditionIndex, ConditionMock, ICondition<int>> condition,
            Func<Times> times)
        {
            foreach (var c in condition)
                c.Verify__Check(times);
        }

        public static void Verify__Check(
            this IMocksCollection<ConditionIndex, ConditionMock, ICondition<int>> condition,
            IEnumerable<Func<Times>> times)
        {
            foreach (var (c, t) in condition.Zip(times))
                c.Verify__Check(t);
        }

        public static void Setup__Execute__Calling(
            this IMocksCollection<HandlerIndex, HandlerMock, IHandler<int>> handlers,
            Action<int> f)
        {
            foreach (var h in handlers)
                h.Setup__Execute__Calling(f);
        }

        public static void Setup__Execute__LoggingInto(
            this IMocksCollection<HandlerIndex, HandlerMock, IHandler<int>> handlers,
            IList<HandlerIndex> acc)
        {
            foreach (var h in handlers)
                h.Setup__Execute__LoggingInto(acc);
        }

        public static void Setup__Execute__LoggingInto(
            this IMocksCollection<HandlerIndex, HandlerMock, IHandler<int>> handlers,
            IList<MockIndex> acc)
        {
            foreach (var h in handlers)
                h.Setup__Execute__LoggingInto(acc);
        }

        public static void Verify__Execute(
            this IMocksCollection<HandlerIndex, HandlerMock, IHandler<int>> handlers,
            Times times)
        {
            foreach (var h in handlers)
                h.Verify__Execute(times);
        }

        public static void Verify__Execute(
            this IMocksCollection<HandlerIndex, HandlerMock, IHandler<int>> handlers,
            Func<Times> times)
        {
            foreach (var h in handlers)
                h.Verify__Execute(times);
        }

        public static void Verify__Execute(
            this IMocksCollection<HandlerIndex, HandlerMock, IHandler<int>> handlers,
            IEnumerable<Func<Times>> times)
        {
            foreach (var (h, t) in handlers.Zip(times))
                h.Verify__Execute(t);
        }
    }
}
