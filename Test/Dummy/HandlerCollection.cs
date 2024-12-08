namespace ChainLead.Test
{
    using System.Collections.Generic;

    public static partial class Dummy
    {
        public class HandlerCollection<T> :
            Collection<Handler<T>, HandlerIndex>.Mutable
        {
            readonly T _token;

            public HandlerCollection(T token)
                : base() => _token = token;

            public HandlerCollection(IEnumerable<Handler<T>> items, T token)
                : base(items) => _token = token;

            public override void Add(HandlerIndex i) =>
                Add(new Handler<T>(this, i, _token));
        }
    }
}
