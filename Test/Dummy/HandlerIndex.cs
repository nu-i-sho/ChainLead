namespace Nuisho.ChainLead.Test
{
    public static partial class Dummy
    {
        public class HandlerIndex(string value) : ChainElementIndex(value)
        {
            public static readonly HandlerIndex
                A = new ("A"), B = new ("B"), C = new ("C"), D = new ("D"),
                E = new ("E"), F = new ("F"), G = new ("G"), H = new ("H"),
                I = new ("I"), J = new ("J");

            public static readonly HandlerIndex[] ABCDEFGHIJ =
                [A, B, C, D, E, F, G, H, I, J];

            public override char ViewBorder => '=';

            public static new HandlerIndex Make(string value) => new (value);

            public static new HandlerIndex Make(char value) => new (value.ToString());

            public static HandlerIndex operator +(HandlerIndex left, HandlerIndex right) =>
                new (left.Value + right.Value);
        }
    }
}
