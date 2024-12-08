namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public interface ICollection<TDummy, TIndex> 
                : IEnumerable<TDummy>
            where TDummy : IDummy<TIndex>
            where TIndex : Dummy.Index
        {
            TDummy Get(TIndex i);

            ICollection<TDummy, TIndex> this[
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

            public interface IMutable : ICollection<TDummy, TIndex>
            {
                void Add(TIndex i);

                void AddRange(TIndex first, TIndex second, params TIndex[] tail);

                void AddRange(IEnumerable<TIndex> indices);
            }
        }
    }
}
