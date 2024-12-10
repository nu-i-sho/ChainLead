namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public interface IHandlerCollection<T>
            : ICollection<Handler<T>, HandlerIndex>
        {
            T Token { get; }

            IHandlerCollection<T> this[IEnumerable<HandlerIndex> indices] { get; }

            IHandlerCollection<T> this[
                HandlerIndex first, HandlerIndex second,
                params HandlerIndex[] tail] =>
                    this[Enumerable.Concat([first, second], tail)];

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
