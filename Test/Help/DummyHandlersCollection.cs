namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using System.Collections.Generic;

    public class DummyHandlersCollection :
        DummiesCollection<DummyHandler, HandlerIndex>
    {
        public DummyHandlersCollection() 
            : base() { }

        public DummyHandlersCollection(IEnumerable<DummyHandler> items)
            : base(items) { }
    }
}
