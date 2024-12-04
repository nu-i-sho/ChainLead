namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using System.Collections.Generic;

    public class HandlerMocksCollection :
        MocksCollection<HandlerIndex, HandlerMock, IHandler<int>>
    {
        public HandlerMocksCollection() 
            : base() { }

        public HandlerMocksCollection(IEnumerable<HandlerMock> items)
            : base(items) { }
    }
}
