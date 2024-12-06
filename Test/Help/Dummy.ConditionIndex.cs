namespace ChainLead.Test.Help
{
    public static partial class Dummy
    {
        public class ConditionIndex : Index
        {
            public override char ViewBorder => ':';

            public ConditionIndex(string value) : base(value) { }

            public static new ConditionIndex Make(string value) => new(value);

            public static new ConditionIndex Make(char value) => new(value.ToString());

            public static class Common
            {
                public static readonly ConditionIndex Q = new("Q");
                public static readonly ConditionIndex R = new("R");
                public static readonly ConditionIndex S = new("S");
                public static readonly ConditionIndex T = new("T");
                public static readonly ConditionIndex U = new("U");
                public static readonly ConditionIndex V = new("V");
                public static readonly ConditionIndex W = new("W");
                public static readonly ConditionIndex X = new("X");
                public static readonly ConditionIndex Y = new("Y");
                public static readonly ConditionIndex Z = new("Z");

                public static readonly ConditionIndex[] QRSTUVWXYZ = [Q, R, S, T, U, V, W, X, Y, Z];
            }
        }
    }
}
