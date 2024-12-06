namespace ChainLead.Test
{
    using ChainLead.Test.Help;

    public partial class HandlerMathTest
    {
        const char CaseBlocksSeparator = '•';

        static string ViewOf(Dummy.Index? x) =>
            x?.Value ?? ".";

        static string ViewOf(bool x) =>
            x ? "I" : "O";

        static string ViewOf(Dummy.Index?[] x) => WithHandleEmptyCollection(x, x =>
            $"[{x.Select(ViewOf).Aggregate(string.Concat)}]");

        static string ViewOf(bool[] x) => WithHandleEmptyCollection(x, x =>
            $"[{x.Select(ViewOf).Aggregate(string.Concat)}]");

        static string ViewOf<TIndex>(Dictionary<TIndex, bool> x)
            where TIndex : Dummy.Index => WithHandleEmptyCollection(x, x =>
            $"[{x.Select(x => $"{{{ViewOf(x.Key)}-{ViewOf(x.Value)}}}")
                 .Aggregate(string.Concat)}]");

        static string WithHandleEmptyCollection<T>(IEnumerable<T> x,
            Func<IEnumerable<T>, string> mainConverter) =>
                x.Any() ? mainConverter(x) : "[]";
    }
}
