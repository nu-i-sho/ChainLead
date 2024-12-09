using Newtonsoft.Json.Linq;

namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public interface IHandlerCollection<T>
            : ICollection<Handler<T>, HandlerIndex>
        {
            T Token { get; }

            new IHandlerCollection<T> this[
                HandlerIndex first, HandlerIndex second,
                params HandlerIndex[] tail] =>
                    (IHandlerCollection<T>)
                    ((ICollection<Handler<T>, HandlerIndex>)this)[first, second, tail];    

            new IHandlerCollection<T> this[
                IEnumerable<HandlerIndex> indices] =>
                    (IHandlerCollection<T>)
                    ((ICollection<Handler<T>, HandlerIndex>)this)[indices];

            IHandlerCollection<T> ThatWereExecutedOnce =>
                this.Where(x => x.WasExecutedOnce);

            IHandlerCollection<T> ThatWereNeverExecuted =>
                this.Where(x => x.WasNeverExecuted);

            public new interface IMutable :
                ICollection<Handler<T>, HandlerIndex>.IMutable,
                IHandlerCollection<T>
            {
            }
        }
    }
}
