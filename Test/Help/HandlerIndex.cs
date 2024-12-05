namespace ChainLead.Test.Help
{
    public class HandlerIndex : DummyIndex
    {
        public override char ViewBorder => '|'; 

        public HandlerIndex(string value) : base(value) { }

        public static new HandlerIndex Make(string value) => new(value);

        public static new HandlerIndex Make(char value) => new(value.ToString());
    }
}
