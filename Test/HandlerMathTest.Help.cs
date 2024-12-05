namespace ChainLead.Test
{
    using ChainLead.Test.Help;

    public partial class HandlerMathTest
    {
        static string ViewOf(DummyIndex? x) =>
            x?.Value ?? ".";

        static string ViewOf(bool x) =>
            x ? "I" : "O";

        static string ViewOf(DummyIndex?[] x) =>
            $"[{x.Select(ViewOf).Aggregate(string.Concat)}]";

        static string ViewOf(bool[] x) =>
            x.Select(ViewOf).Aggregate(string.Concat);

        static string ViewOf<TIndex>(Dictionary<TIndex, bool> x)
            where TIndex : DummyIndex =>
            $"[{x.Select(x => $"{{{ViewOf(x.Key)},{ViewOf(x.Value)}}}")
                 .Aggregate(string.Concat)}";
    }
}
