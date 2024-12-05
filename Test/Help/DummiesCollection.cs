namespace ChainLead.Test.Help
{
    public class DummiesCollection<TDummy, TIndex>
        : List<TDummy>, IDummiesCollection<TDummy, TIndex>
            where TDummy : IDummy<TIndex>
            where TIndex : DummyIndex
    {
        public DummiesCollection()
            : base() { }

        public DummiesCollection(IEnumerable<TDummy> items)
            : base(items) { }

        public TDummy this[TIndex i] =>
            Find(x => x.Index.Equals(i))!;

        public IDummiesCollection<TDummy, TIndex> this[
            TIndex first, TIndex second, params TIndex[] tail] =>
                this[Enumerable.Concat([first, second], tail)];

        public IDummiesCollection<TDummy, TIndex> this[IEnumerable<TIndex> indices] =>
            new DummiesCollection<TDummy, TIndex>(indices.Select(this.Get));

        public override string ToString() =>
            this.Select(x => x.ToString())
                .Aggregate(string.Concat);
    }
}
