namespace ChainLead.Test.Help
{
    public interface IDummiesCollection<TDummy, TIndex, TObject>
        : IList<TDummy>
        where TDummy : IDummy<TIndex, TObject>
        where TIndex : DummyIndex
    {
        public IEnumerable<TObject> Objects { get; }

        public TDummy this[TIndex i] { get; }

        public IDummiesCollection<TDummy, TIndex, TObject> this[
            TIndex first, 
            TIndex second, 
            params TIndex[] tail] 
        { 
            get; 
        }
        
        public IDummiesCollection<TDummy, TIndex, TObject> this[
            IEnumerable<TIndex> indices] 
        { 
            get; 
        }
    }
}
