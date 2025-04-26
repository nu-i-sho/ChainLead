namespace Nuisho.ChainLead.Test;

public static partial class Dummy
{
    public class ChainElementIndex(string value)
    {
        public virtual char ViewBorder => '?';

        public string Value => value;

        public string View => $"{ViewBorder}{Value}{ViewBorder}";

        public static ChainElementIndex Make(string value) => new (value);

        public static ChainElementIndex Make(char value) => new (value.ToString());

        public sealed override string ToString() => View;

        public sealed override bool Equals(object? obj) =>
            obj is ChainElementIndex index && View == index.View;

        public sealed override int GetHashCode() =>
            View.GetHashCode();
    }
}
