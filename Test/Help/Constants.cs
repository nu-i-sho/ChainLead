using ChainLead.Contracts;

namespace ChainLead.Test.Help
{
    public static class Constants
    {
        public const int Arg = 7568;

        public static readonly HandlerIndex A = HandlerIndex.Make("A");
        public static readonly HandlerIndex B = HandlerIndex.Make("B");
        public static readonly HandlerIndex C = HandlerIndex.Make("C");
        public static readonly HandlerIndex D = HandlerIndex.Make("D");
        public static readonly HandlerIndex E = HandlerIndex.Make("E");
        public static readonly HandlerIndex F = HandlerIndex.Make("F");
        public static readonly HandlerIndex G = HandlerIndex.Make("G");
        public static readonly HandlerIndex H = HandlerIndex.Make("H");
        public static readonly HandlerIndex I = HandlerIndex.Make("I");
        public static readonly HandlerIndex J = HandlerIndex.Make("J");

        public static readonly ConditionIndex Q = ConditionIndex.Make("Q");
        public static readonly ConditionIndex R = ConditionIndex.Make("R");
        public static readonly ConditionIndex S = ConditionIndex.Make("S");
        public static readonly ConditionIndex T = ConditionIndex.Make("T");
        public static readonly ConditionIndex U = ConditionIndex.Make("U");
        public static readonly ConditionIndex V = ConditionIndex.Make("V");
        public static readonly ConditionIndex W = ConditionIndex.Make("W");
        public static readonly ConditionIndex X = ConditionIndex.Make("X");
        public static readonly ConditionIndex Y = ConditionIndex.Make("Y");
        public static readonly ConditionIndex Z = ConditionIndex.Make("Z");

        public static readonly HandlerIndex[] ABCDEFGHIJ = [A, B, C, D, E, F, G, H, I, J];
        public static readonly ConditionIndex[] QRSTUVWXYZ = [Q, R, S, T, U, V, W, X, Y, Z];


        public static class Appends
        {
            public const string
                FirstThenSecond = nameof(IHandlerMath.FirstThenSecond),
                PackFirstInSecond = nameof(IHandlerMath.PackFirstInSecond),
                InjectFirstIntoSecond = nameof(IHandlerMath.InjectFirstIntoSecond),
                FirstCoverSecond = nameof(IHandlerMath.FirstCoverSecond),
                FirstWrapSecond = nameof(IHandlerMath.FirstWrapSecond),
                JoinFirstWithSecond = nameof(IHandlerMath.JoinFirstWithSecond),
                MergeFirstWithSecond = nameof(IHandlerMath.MergeFirstWithSecond);

            public static readonly string[] All =
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
