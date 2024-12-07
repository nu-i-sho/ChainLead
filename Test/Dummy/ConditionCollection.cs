namespace ChainLead.Test
{
    using System.Linq;

    public static partial class Dummy
    {
        public class ConditionCollection<T> :
            Collection<Condition<T>, ConditionIndex>
        {
            readonly T _token;

            public ConditionCollection(T token) 
                : base() => _token = token;

            public ConditionCollection(IEnumerable<Condition<T>> items, T token) 
                : base(items) => _token = token;

            public void GenerateMore(ConditionIndex head, params ConditionIndex[] tail)
            {
                Add(new Condition<T>(head, _token));
                AddRange(tail.Select(x => new Condition<T>(x, _token)));
            }

            public void Add(IEnumerable<ConditionIndex> indices) =>
                AddRange(indices.Select(x => new Condition<T>(x, _token)));
        }
    }
}
