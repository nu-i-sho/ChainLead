using ChainLead.Contracts;

namespace ChainLead.Test.Help
{
    public static partial class Dummy
    {
        public class Handler<T>(
                Dummy.HandlerCollection<T> handlers,
                Dummy.HandlerIndex index,
                T tokenArg) :
            IDummy<Dummy.HandlerIndex>,
            IHandler<T>
        {
            public Dummy.HandlerIndex Index => index;

            public string Name => index.View;

            public void AddCallback(Action f) =>
                Callback += f;

            public void AddLoggingInto(IList<Dummy.HandlerIndex> acc) =>
                AddCallback(() => acc.Add(Index));

            public void AddLoggingInto(IList<Index> acc) =>
                AddCallback(() => acc.Add(Index));

            public void AddDelegationTo(params Dummy.HandlerIndex[] indexes) =>
                    AddCallback(() =>
                    {
                        foreach (var i in indexes)
                            handlers[i].Execute(tokenArg);
                    });

            public bool WasExecutedOnce() =>
                CallsCount == 1;

            public bool WasNeverExecuted() =>
                CallsCount == 0;

            public TimesContinuation WasExecuted(int times) =>
                new(CallsCount == times);

            public class TimesContinuation(bool answer)
            {
                public bool Times => answer;
            }

            public ElseNeverContinuation WasExecutedOnceWhen(bool condition) =>
                new(condition
                    ? WasExecutedOnce()
                    : WasNeverExecuted());

            public class ElseNeverContinuation(bool answer)
            {
                public bool ElseNever => answer;
            }

            public bool VerifyExecution(bool wasExecutedOnceElseNever) =>
                wasExecutedOnceElseNever
                    ? WasExecutedOnce()
                    : WasNeverExecuted();

            public override string ToString() => Name;

            public IHandler<T> Pure => this;

            private int CallsCount { get; set; } = 0;

            private Action Callback { get; set; } = () =>
            {
                /* INITIALY DO NOTHING */
            };

            public void Execute(T state)
            {
                if (state?.Equals(tokenArg) ?? false)
                {
                    Callback();
                    CallsCount++;
                }
            }
        }
    }
}
