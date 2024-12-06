namespace ChainLead.Test
{
    using System.Collections.Generic;

    public static partial class Dummy
    {
        public class HandlerCollection<T> :
            Collection<Handler<T>, HandlerIndex>
        {
            readonly T _expectedArg;

            public HandlerCollection(T expectedArg)
                : base() => _expectedArg = expectedArg;

            public HandlerCollection(IEnumerable<Handler<T>> items, T expectedArg)
                : base(items) => _expectedArg = expectedArg;

            public void GenerateMore(HandlerIndex head, params HandlerIndex[] tail)
            {
                Add(new Handler<T>(this, head, _expectedArg));
                AddRange(tail.Select(x => new Handler<T>(this, x, _expectedArg)));
            }
        }
    }
}
