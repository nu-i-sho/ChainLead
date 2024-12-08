namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class ConditionCollection<T> :
            Collection<Condition<T>, ConditionIndex>.Mutable
        {
            readonly T _token;

            public ConditionCollection(T token) 
                : base() => _token = token;

            public ConditionCollection(IEnumerable<Condition<T>> items, T token) 
                : base(items) => _token = token;

            public override void Add(ConditionIndex i) =>
                Add(new Condition<T>(i, _token));
        }
    }
}
