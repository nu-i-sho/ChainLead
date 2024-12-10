namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class HandlerCollection<T>(T token) :
            List<Handler<T>>, IHandlerCollection<T>.IMutable
        {
            public T Token => token;

            public ICollection<Handler<T>, HandlerIndex> this[IEnumerable<HandlerIndex> indices]
            { 
                get
                {
                    var slice = new HandlerCollection<T>(token);
                    slice.AddRange(indices.Select(((IHandlerCollection<T>)this).Get));
                    return slice;
                }
            }
            public void Generate(HandlerIndex i) =>
                Add(new Handler<T>(this, i, token));
        }
    }
}
