namespace ChainLead.Test.Help
{
    public class DummyIndex
    {
        public virtual char ViewBorder => '?';
        
        public string Value { get; }

        public string View => $"{ViewBorder}{Value}{ViewBorder}";

        public DummyIndex(string value) => Value = value;

        public static DummyIndex Make(string value) => new(value);

        public static DummyIndex Make(char value) => new(value.ToString());

        public override string ToString() => View;

        public override bool Equals(object? obj) =>
            obj is DummyIndex && ((DummyIndex)obj).View == View;

        public override int GetHashCode() =>
            View.GetHashCode();
    }
}
