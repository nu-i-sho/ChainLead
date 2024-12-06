namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;

    public static partial class Dummy
    {
        public class Condition<T>(
                Dummy.ConditionIndex index,
                T expectedArg) :
            IDummy<Dummy.ConditionIndex>,
            ICondition<T>
        {
            public Dummy.ConditionIndex Index => index;

            public string Name => index.View;

            public void AddCallback(Action f) =>
                Callback += f;

            public void AddLoggingInto(IList<Dummy.ConditionIndex> acc) =>
                AddCallback(() => acc.Add(Index));

            public void AddLoggingInto(IList<Dummy.Index> acc) =>
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

            public ICondition<T> Pure => this;

            public int CallsCount { get; private set; } = 0;

            public Action Callback { get; set; } = () =>
            {
                /* INITIALY DO NOTHING */
            };

            public Func<bool> Return { get; set; } = () => false;

            public bool Check(T state)
            {
                if (state?.Equals(state) ?? false)
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
