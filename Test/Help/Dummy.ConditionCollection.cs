namespace ChainLead.Test.Help
{
    using System.Linq;

    public static partial class Dummy
    {
        public class ConditionCollection<T> :
            Dummy.Collection<Dummy.Condition<T>, Dummy.ConditionIndex>
        {
            readonly T _expectedArg;

            public ConditionCollection(T expectedArg) 
                : base() => _expectedArg = expectedArg;

            public ConditionCollection(IEnumerable<Dummy.Condition<T>> items, T expectedArg) 
                : base(items) => _expectedArg = expectedArg;

            public void GenerateMore(ConditionIndex head, params ConditionIndex[] tail)
            {
                Add(new Dummy.Condition<T>(head, _expectedArg));
                AddRange(tail.Select(x => new Dummy.Condition<T>(x, _expectedArg)));
            }
        }
    }
}
