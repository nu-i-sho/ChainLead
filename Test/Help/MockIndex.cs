namespace ChainLead.Test.Help
{
    public class MockIndex
    {
        public virtual char ViewBorder => '?';
        
        public string Value { get; }

        public string View => $"{ViewBorder}{Value}{ViewBorder}";

        protected MockIndex(string value) => Value = value;

        public static MockIndex Make(string value) =>
            new MockIndex(value);

        public static MockIndex Make(char value) =>
            new MockIndex(value.ToString());

        public override string ToString() => View;

        public override bool Equals(object? obj) =>
            obj is MockIndex && ((MockIndex)obj).View == View;

        public override int GetHashCode() =>
            View.GetHashCode();
    }
}
