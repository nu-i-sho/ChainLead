namespace ChainLead.Test.Help
{
    public class ConditionIndex : DummyIndex
    {
        public override char ViewBorder => ':';

        public ConditionIndex(string value) : base(value) { }

        public static new ConditionIndex Make(string value) => new(value);

        public static new ConditionIndex Make(char value) => new(value.ToString());
    }
}
