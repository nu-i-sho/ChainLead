namespace ChainLead.Test
{
    using ChainLead.Contracts;

    public static partial class Dummy
    {
        public class Handler<T>(
                IHandlerCollection<T> handlers,
                HandlerIndex index,
                T token) :
            IChainElement<HandlerIndex>,
            IHandler<T>
        {
            public HandlerIndex Index => index;

            public void SetImplementation(Action<T> f) =>
                Implementation = f;

            public void AddCallback(Action f) =>
                Callback += f;

            public void LogsInto(IList<HandlerIndex> log) =>
                AddCallback(() => log.Add(Index));

            public void LogsInto(IList<ChainElementIndex> log) =>
                AddCallback(() => log.Add(Index));

            public void DelegatesTo(params HandlerIndex[] indexes) =>
                AddCallback(() =>
                {
                    foreach (var i in indexes)
                        handlers.Get(i).Execute(token);
                });

            public bool WasExecutedOnce =>
                CallsCount == 1;

            public bool WasNeverExecuted =>
                CallsCount == 0;

            public TimesContinuation WasExecuted(int times) =>
                new(CallsCount == times);

            public class TimesContinuation(bool answer)
            {
                public bool Times => answer;
            }

            public ElseNeverContinuation WasExecutedOnceWhen(
                bool executedCondition) =>
                 new(executedCondition 
                        ? WasExecutedOnce
                        : WasNeverExecuted);

            public class ElseNeverContinuation(bool answer)
            {
                public bool ElseNever => answer;
            }

            public override string ToString() => $"h[{index.View}]";

            public IHandler<T> Pure => this;

            int CallsCount { get; set; } = 0;

            Action<T> Implementation { get; set; } = _ =>
            {
                /* INITIALY DO NOTHING */
            };

            Action Callback { get; set; } = () =>
            {
                /* INITIALY DO NOTHING */
            };

            public void Execute(T state)
            {
                if (state?.Equals(token) ?? false)
                {
                    Implementation(state);
                    Callback();
                    CallsCount++;
                }
            }
        }
    }
}
