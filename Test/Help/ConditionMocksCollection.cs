namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;

    public class ConditionMocksCollection :
        MocksCollection<ConditionIndex, ConditionMock, ICondition<int>>
    {
        public ConditionMocksCollection() 
            : base() { }

        public ConditionMocksCollection(IEnumerable<ConditionMock> items)
            : base(items) { }
    }
}
