namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class Index
        {
            public virtual char ViewBorder => '?';

            public string Value { get; }

            public string View => $"{ViewBorder}{Value}{ViewBorder}";

            public Index(string value) => Value = value;

            public static Index Make(string value) => new(value);

            public static Index Make(char value) => new(value.ToString());

            public override string ToString() => View;

            public override bool Equals(object? obj) =>
                obj is Index && ((Index)obj).View == View;

            public override int GetHashCode() =>
                View.GetHashCode();
        }
    }
}
