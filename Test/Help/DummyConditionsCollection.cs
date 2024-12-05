namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;

    public class DummyConditionsCollection :
        DummiesCollection<DummyCondition, ConditionIndex>
    {
        public DummyConditionsCollection() 
            : base() { }

        public DummyConditionsCollection(IEnumerable<DummyCondition> items)
            : base(items) { }
    }
}
