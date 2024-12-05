namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using static Constants;

    public class DummyHandler(
            DummyHandlersCollection handlers,
            HandlerIndex index) : 
        IDummy<HandlerIndex, IHandler<int>>
    {
        private readonly Dummy _dummy = new();

        public IHandler<int> Object => _dummy;

        public HandlerIndex Index => index;

        public string Name => index.View;
        
        public void AddCallback(Action f) =>
            _dummy.Callback += f;

        public void AddLoggingInto(IList<HandlerIndex> acc) =>
            AddCallback(() => acc.Add(Index));

        public void AddLoggingInto(IList<DummyIndex> acc) =>
            AddCallback(() => acc.Add(Index));

        public void AddDelegationTo(params HandlerIndex[] indexes) =>
                AddCallback(() =>
                {
                    foreach (var i in indexes)
                        handlers[i].Object.Execute(Arg);
                });

        public bool WasExecutedOnce() =>
            _dummy.CallsCount == 1;

        public bool WasNeverExecuted() =>
            _dummy.CallsCount == 0;

        public TimesContinuation WasExecuted(int times) =>
            new(_dummy.CallsCount == times);

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

        private class Dummy : IHandler<int>
        {
            public int CallsCount { get; private set; } = 0;  

            public Action Callback { get; set; } = () => 
            {
                /* INITIALY DO NOTHING */ 
            };

            public void Execute(int state)
            {
                if (state == Arg)
                {
                    Callback();
                    CallsCount++;
                }
            }
        }
    }
}
