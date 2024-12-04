namespace ChainLead.Test.Help
{
    using Moq;

    public interface IMocksCollection<TIndex, TMock, TObject>
        : IList<TMock>
        where TMock : Mock<TObject>, IIndexedWith<TIndex>
        where TObject : class
    {
        public IEnumerable<TObject> Objects { get; }

        public TMock this[TIndex i] { get; }

        public IMocksCollection<TIndex, TMock, TObject> this[
            TIndex first, 
            TIndex second, 
            params TIndex[] tail] 
        { 
            get; 
        }
        
        public IMocksCollection<TIndex, TMock, TObject> this[
            IEnumerable<TIndex> indices] 
        { 
            get; 
        }
    }
}
