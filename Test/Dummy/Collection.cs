namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class Collection<TDummy, TIndex>
            : List<TDummy>, ICollection<TDummy, TIndex>
                where TDummy : IDummy<TIndex>
                where TIndex : Index
        {
            public Collection()
                : base() { }

            public Collection(IEnumerable<TDummy> items)
                : base(items) { }

            public TDummy this[TIndex i] =>
                Find(x => x.Index.Equals(i))!;

            public ICollection<TDummy, TIndex> this[
                TIndex first, TIndex second, params TIndex[] tail] =>
                    this[Enumerable.Concat([first, second], tail)];

            public ICollection<TDummy, TIndex> this[IEnumerable<TIndex> indices] =>
                new Collection<TDummy, TIndex>(indices.Select(this.Get));

            public override string ToString() =>
                this.Select(x => x.ToString())
                    .Aggregate(string.Empty, string.Concat);
        }
    }
}
