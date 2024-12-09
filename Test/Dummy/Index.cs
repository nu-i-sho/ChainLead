namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class Index(string value)
        {
            public virtual char ViewBorder => '?';

            public string Value => value;

            public string View => $"{ViewBorder}{Value}{ViewBorder}";

            public static Index Make(string value) => new(value);

            public static Index Make(char value) => new(value.ToString());

            public override string ToString() => View;

            public override bool Equals(object? obj) =>
                obj is Index index && index.View == View;

            public override int GetHashCode() =>
                View.GetHashCode();

            public static class Common
            {
                public record AnyArg;

                public static readonly AnyArg Any = new();
            }
        }
    }
}
