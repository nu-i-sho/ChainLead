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

            public TDummy Get(TIndex i) => 
                Find(x => x.Index.Equals(i))!;

            public ICollection<TDummy, TIndex> this[
                TIndex first, TIndex second, params TIndex[] tail] =>
                    this[Enumerable.Concat([first, second], tail)];

            public ICollection<TDummy, TIndex> this[IEnumerable<TIndex> indices] =>
                new Collection<TDummy, TIndex>(indices.Select(Get));
            
            public override string ToString() =>
                this.Select(x => x.Index.ToString())
                    .Aggregate(string.Empty, string.Concat);

            public abstract class Mutable :
                Collection<TDummy, TIndex>,
                ICollection<TDummy, TIndex>.IMutable
            {
                protected Mutable()
                    : base() { }

                protected Mutable(IEnumerable<TDummy> items)
                    : base(items) { }

                public abstract void Add(TIndex i);

                public void AddRange(TIndex first, TIndex second, params TIndex[] tail)
                {
                    Add(first);
                    Add(second);

                    foreach (var i in tail)
                        Add(i);
                }

                public void AddRange(IEnumerable<TIndex> indices)
                {
                    foreach (var i in indices)
                        Add(i);
                }
            }
        }
    }
}
