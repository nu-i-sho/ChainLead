namespace ChainLead.Test.Help
{
    public interface IDummiesCollection<TDummy, TIndex>
        : IList<TDummy>
        where TDummy : IDummy<TIndex>
        where TIndex : DummyIndex
    {
        public TDummy this[TIndex i] { get; }

        public IDummiesCollection<TDummy, TIndex> this[
            TIndex first, 
            TIndex second, 
            params TIndex[] tail] 
        { 
            get; 
        }
        
        public IDummiesCollection<TDummy, TIndex> this[
            IEnumerable<TIndex> indices] 
        { 
            get; 
        }
    }
}
