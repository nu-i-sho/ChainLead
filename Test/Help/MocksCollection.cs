namespace ChainLead.Test.Help
{
    using Moq;

    public class MocksCollection<TIndex, TMock, TObject>
        : List<TMock>, IMocksCollection<TIndex, TMock, TObject>
        where TMock : Mock<TObject>, IIndexedWith<TIndex>
        where TObject : class
    {
        public MocksCollection()
            : base() { }

        public MocksCollection(IEnumerable<TMock> items)
            : base(items) { }

        public IEnumerable<TObject> Objects =>
            this.Select(x => x.Object);

        public TMock this[TIndex i] =>
            Find(x => x.Index.Equals(i))!;

        public IMocksCollection<TIndex, TMock, TObject> this[
            TIndex first, TIndex second, params TIndex[] tail] =>
                this[tail.Concat([first, second])];

        public IMocksCollection<TIndex, TMock, TObject> this[IEnumerable<TIndex> indices] =>
            new MocksCollection<TIndex, TMock, TObject>(indices.Select(this.Get));

        public override string ToString() =>
            this.Select(x => x.ToString())
                .Aggregate(string.Concat);
    }
}
