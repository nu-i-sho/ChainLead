namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;

    public class DummyConditionsCollection :
        DummiesCollection<DummyCondition, ConditionIndex, ICondition<int>>
    {
        public DummyConditionsCollection() 
            : base() { }

        public DummyConditionsCollection(IEnumerable<DummyCondition> items)
            : base(items) { }
    }
}
