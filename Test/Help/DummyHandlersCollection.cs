namespace ChainLead.Test.Help
{
    using System.Collections.Generic;

    public class DummyHandlersCollection :
        DummiesCollection<DummyHandler, HandlerIndex>
    {
        public DummyHandlersCollection() 
            : base() { }

        public DummyHandlersCollection(IEnumerable<DummyHandler> items)
            : base(items) { }

        public void GenerateMore(HandlerIndex head, params HandlerIndex[] tail)
        {
            Add(new DummyHandler(this, head));
            AddRange(tail.Select(x => new DummyHandler(this, x)));
        }
    }
}
