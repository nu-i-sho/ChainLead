namespace ChainLead.Test.Help
{
    public interface IDummy<TIndex>
        where TIndex : DummyIndex
    {
        TIndex Index { get; }

        string Name { get; }
    }
}
