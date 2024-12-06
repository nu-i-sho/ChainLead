namespace ChainLead.Test
{
    using System.Linq;

    public static partial class Dummy
    {
        public class ConditionCollection<T> :
            Collection<Condition<T>, ConditionIndex>
        {
            readonly T _expectedArg;

            public ConditionCollection(T expectedArg) 
                : base() => _expectedArg = expectedArg;

            public ConditionCollection(IEnumerable<Condition<T>> items, T expectedArg) 
                : base(items) => _expectedArg = expectedArg;

            public void GenerateMore(ConditionIndex head, params ConditionIndex[] tail)
            {
                Add(new Condition<T>(head, _expectedArg));
                AddRange(tail.Select(x => new Condition<T>(x, _expectedArg)));
            }
        }
    }
}
