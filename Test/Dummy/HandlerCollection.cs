﻿namespace ChainLead.Test
{
    using System.Collections;

    public static partial class Dummy
    {
        public class HandlerCollection<T>(T token) :
            Dictionary<HandlerIndex, Handler<T>>, IHandlerCollection<T>.IMutable
        {
            public T Token => token;

            public ICollection<Handler<T>, HandlerIndex> this[IEnumerable<HandlerIndex> indices]
            { 
                get
                {
                    var slice = new HandlerCollection<T>(token);
                    foreach (var i in indices) 
                        slice.Add(i, ((IHandlerCollection<T>)this).Get(i));

                    return slice;
                }
            }

            public void AddRange(IEnumerable<Handler<T>> handlers)
            {
                foreach (var h in handlers)
                    Add(h.Index, h);
            }

            Handler<T> ICollection<Handler<T>, HandlerIndex>.Get(HandlerIndex i) => this[i];

            public void Generate(HandlerIndex i) =>
                Add(i, new Handler<T>(this, i, token));

            IEnumerator<Handler<T>> IEnumerable<Handler<T>>.GetEnumerator() =>
                Values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() =>
                Values.GetEnumerator();
        }
    }
}
