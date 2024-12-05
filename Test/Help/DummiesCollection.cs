namespace ChainLead.Test.Help
{
    public class DummiesCollection<TDummy, TIndex, TObject>
        : List<TDummy>, IDummiesCollection<TDummy, TIndex, TObject>
            where TDummy : IDummy<TIndex, TObject>
            where TIndex : DummyIndex
    {
        public DummiesCollection()
            : base() { }

        public DummiesCollection(IEnumerable<TDummy> items)
            : base(items) { }

        public IEnumerable<TObject> Objects =>
            this.Select(x => x.Object);

        public TDummy this[TIndex i] =>
            Find(x => x.Index.Equals(i))!;

        public IDummiesCollection<TDummy, TIndex, TObject> this[
            TIndex first, TIndex second, params TIndex[] tail] =>
                this[Enumerable.Concat([first, second], tail)];

        public IDummiesCollection<TDummy, TIndex, TObject> this[IEnumerable<TIndex> indices] =>
            new DummiesCollection<TDummy, TIndex, TObject>(indices.Select(this.Get));

        public override string ToString() =>
            this.Select(x => x.ToString())
                .Aggregate(string.Concat);
    }
}
