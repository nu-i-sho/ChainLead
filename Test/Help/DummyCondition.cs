namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using static Constants;

    public class DummyCondition(ConditionIndex index) : 
        IDummy<ConditionIndex>,
        ICondition<int>
    {        
        public ConditionIndex Index => index;
        
        public string Name => index.View;

        public void AddCallback(Action f) =>
            Callback += f;

        public void AddLoggingInto(IList<ConditionIndex> acc) =>
            AddCallback(() => acc.Add(Index));

        public void AddLoggingInto(IList<DummyIndex> acc) =>
            AddCallback(() => acc.Add(Index));

        public void SetResult(bool value) =>
            Return = () => value;

        public void SetReturn(Func<bool> f) =>
            Return = f;

        public bool WasCheckedOnce() =>
            CallsCount == 1;

        public bool WasNeverChecked() =>
            CallsCount == 0;

        public bool VerifyCheck(bool wasExecutedOnceElseNever) =>
            wasExecutedOnceElseNever
                ? WasCheckedOnce()
                : WasNeverChecked();

        public override string ToString() => Name;

        public ICondition<int> Pure => this;

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
