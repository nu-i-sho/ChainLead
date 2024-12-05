namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using Moq;

    using static Constants;

    public class DummyCondition(ConditionIndex index) : 
        IDummy<ConditionIndex, ICondition<int>>
    {
        private readonly Dummy _dummy = new();

        public ICondition<int> Object => _dummy;
        
        public ConditionIndex Index => index;
        
        public string Name => index.View;

        public void AddCallback(Action f) =>
            _dummy.Callback += f;

        public void AddLoggingInto(IList<ConditionIndex> acc) =>
            AddCallback(() => acc.Add(Index));

        public void AddLoggingInto(IList<DummyIndex> acc) =>
            AddCallback(() => acc.Add(Index));

        public void SetResult(bool value) =>
            _dummy.Return = () => value;

        public void SetReturn(Func<bool> f) =>
            _dummy.Return = f;

        public bool WasCheckedOnce() =>
            _dummy.CallsCount == 1;

        public bool WasNeverChecked() =>
            _dummy.CallsCount == 1;

        public bool VerifyCheck(bool wasExecutedOnceElseNever) =>
            wasExecutedOnceElseNever
                ? WasCheckedOnce()
                : WasNeverChecked();

        private class Dummy : ICondition<int>
        {
            public int CallsCount { get; private set; } = 0;

            public Action Callback { get; set; } = () =>
            {
                /* INITIALY DO NOTHING */
            };

            public Func<bool> Return { get; set; } = () => false;

            public bool Check(int state)
            {
                if (state == Arg)
                {
                    Callback();
                    CallsCount++;

                    return Return();
                }

                return false;
            }
        }
    }
}
