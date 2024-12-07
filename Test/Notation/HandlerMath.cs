namespace ChainLead.Test.Common
{
    using ChainLead.Contracts;

    public static partial class Notation
    {
        public static class HandlerMath
        {
            public static readonly string
                FirstThenSecond = nameof(IHandlerMath.FirstThenSecond),
                PackFirstInSecond = nameof(IHandlerMath.PackFirstInSecond),
                InjectFirstIntoSecond = nameof(IHandlerMath.InjectFirstIntoSecond),
                FirstCoverSecond = nameof(IHandlerMath.FirstCoverSecond),
                FirstWrapSecond = nameof(IHandlerMath.FirstWrapSecond),
                JoinFirstWithSecond = nameof(IHandlerMath.JoinFirstWithSecond),
                MergeFirstWithSecond = nameof(IHandlerMath.MergeFirstWithSecond);

            public static readonly string[] AllAppends =
            [
                FirstThenSecond,
                PackFirstInSecond,
                InjectFirstIntoSecond,
                FirstCoverSecond,
                FirstWrapSecond,
                JoinFirstWithSecond,
                MergeFirstWithSecond
            ];
        }
    }
}
