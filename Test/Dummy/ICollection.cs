namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public interface ICollection<TDummy, TIndex> 
                : IList<TDummy>
            where TDummy : IDummy<TIndex>
            where TIndex : Dummy.Index
        {
            public TDummy this[TIndex indices]
            {
                get;
            }

            public ICollection<TDummy, TIndex> this[
                TIndex first,
                TIndex second,
                params TIndex[] tail]
            {
                get;
            }

            public ICollection<TDummy, TIndex> this[
                IEnumerable<TIndex> indices]
            {
                get;
            }
        }
    }
}
