namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public interface IChainElement<TIndex>
            where TIndex : ChainElementIndex
        {
            TIndex Index { get; }

            void LogsInto(IList<ChainElementIndex> log);

            void LogsInto(IList<TIndex> log);

            void AddCallback(Action f);
        }
    }
}
