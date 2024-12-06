namespace ChainLead.Test.Help
{
    public interface IDummy<TIndex>
        where TIndex : Dummy.Index
    {
        TIndex Index { get; }

        string Name { get; }
    }
}
