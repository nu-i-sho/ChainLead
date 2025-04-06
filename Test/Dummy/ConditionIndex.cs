namespace Nuisho.ChainLead.Test
{
    public static partial class Dummy
    {
        public class ConditionIndex(string value) : ChainElementIndex(value)
        {
            public static readonly ConditionIndex
                Q = new ("Q"), R = new ("R"), S = new ("S"), T = new ("T"),
                U = new ("U"), V = new ("V"), W = new ("W"), X = new ("X"),
                Y = new ("Y"), Z = new ("Z");

            public static readonly ConditionIndex[] QRSTUVWXYZ =
                [Q, R, S, T, U, V, W, X, Y, Z];

            public override char ViewBorder => ':';

            public static new ConditionIndex Make(string value) => new (value);

            public static new ConditionIndex Make(char value) => new (value.ToString());

            public static ConditionIndex operator &(ConditionIndex left, ConditionIndex right) =>
                new ($"{left.Value} & {right.Value}");

            public static ConditionIndex operator |(ConditionIndex left, ConditionIndex right) =>
                new ($"{left.Value} | {right.Value}");
        }
    }
}
