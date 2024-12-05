namespace ChainLead.Test.Help
{
    public interface IDummy<TIndex, TObject>
        where TIndex : DummyIndex
    {
        TIndex Index { get; }

        TObject Object { get; }

        string Name { get; }
    }
}
