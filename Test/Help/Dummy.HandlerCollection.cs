namespace ChainLead.Test.Help
{
    using System.Collections.Generic;

    public static partial class Dummy
    {
        public class HandlerCollection<T> :
            Dummy.Collection<Dummy.Handler<T>, Dummy.HandlerIndex>
        {
            readonly T _expectedArg;

            public HandlerCollection(T expectedArg)
                : base() => _expectedArg = expectedArg;

            public HandlerCollection(IEnumerable<Dummy.Handler<T>> items, T expectedArg)
                : base(items) => _expectedArg = expectedArg;

            public void GenerateMore(HandlerIndex head, params HandlerIndex[] tail)
            {
                Add(new Dummy.Handler<T>(this, head, _expectedArg));
                AddRange(tail.Select(x => new Dummy.Handler<T>(this, x, _expectedArg)));
            }
        }
    }
}
