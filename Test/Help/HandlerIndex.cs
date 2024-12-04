namespace ChainLead.Test.Help
{
    public class HandlerIndex : MockIndex
    {
        public override char ViewBorder => '|'; 

        protected HandlerIndex(string value) : base(value) { }

        public static new HandlerIndex Make(string value) => new(value);

        public static new HandlerIndex Make(char value) => new(value.ToString());
    }
}
