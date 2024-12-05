namespace ChainLead.Test.Help
{
    using System.Linq;

    public class DummyConditionsCollection :
        DummiesCollection<DummyCondition, ConditionIndex>
    {
        public DummyConditionsCollection() 
            : base() { }

        public DummyConditionsCollection(IEnumerable<DummyCondition> items)
            : base(items) { }

        public void GenerateMore(ConditionIndex head, params ConditionIndex[] tail)
        {
            Add(new DummyCondition(head));
            AddRange(tail.Select(x => new DummyCondition(x)));
        }
    }
}
