namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class HandlerIndex(string value) : Index(value)
        {
            public override char ViewBorder => '|';

            public static new HandlerIndex Make(string value) => new(value);

            public static new HandlerIndex Make(char value) => new(value.ToString());

            public static HandlerIndex operator +(HandlerIndex left, HandlerIndex right) =>
                new(left.Value + right.Value);

            public static new class Common
            {
                public static readonly HandlerIndex A = new("A");
                public static readonly HandlerIndex B = new("B");
                public static readonly HandlerIndex C = new("C");
                public static readonly HandlerIndex D = new("D");
                public static readonly HandlerIndex E = new("E");
                public static readonly HandlerIndex F = new("F");
                public static readonly HandlerIndex G = new("G");
                public static readonly HandlerIndex H = new("H");
                public static readonly HandlerIndex I = new("I");
                public static readonly HandlerIndex J = new("J");

                public static readonly HandlerIndex[] ABCDEFGHIJ = [A, B, C, D, E, F, G, H, I, J];
            }
        }
    }
}
