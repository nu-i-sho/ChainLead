namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class ConditionCollection<T>(T token) :
            List<Condition<T>>, IConditionCollection<T>.IMutable
        {
            public T Token => token;

            public ICollection<Condition<T>, ConditionIndex> this[IEnumerable<ConditionIndex> indices]
            {
                get
                {
                    var slice = new ConditionCollection<T>(token);
                    slice.AddRange(indices.Select(((IConditionCollection<T>)this).Get));
                    return slice;
                }
            }
            public void Generate(ConditionIndex i) =>
                Add(new Condition<T>(i, token));
        }
    }
}
