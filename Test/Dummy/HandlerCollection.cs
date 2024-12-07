namespace ChainLead.Test
{
    using System.Collections.Generic;
    using static ChainLead.Test.Dummy;

    public static partial class Dummy
    {
        public class HandlerCollection<T> :
            Collection<Handler<T>, HandlerIndex>
        {
            readonly T _token;

            public HandlerCollection(T expectedArg)
                : base() => _token = expectedArg;

            public HandlerCollection(IEnumerable<Handler<T>> items, T token)
                : base(items) => _token = token;

            public void AddRange(HandlerIndex head, params HandlerIndex[] tail)
            {
                Add(new Handler<T>(this, head, _token));
                AddRange(tail.Select(x => new Handler<T>(this, x, _token)));
            }
        }
    }
}
