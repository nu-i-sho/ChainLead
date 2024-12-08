namespace ChainLead.Test
{
    using ChainLead.Contracts;

    public static partial class Dummy
    {
        public class Condition<T>(
                ConditionIndex index,
                T token) :
            IDummy<ConditionIndex>,
            ICondition<T>
        {
            public ConditionIndex Index => index;

            public string Name => index.View;

            public void AddCallback(Action f) =>
                Callback += f;

            public void AddLoggingInto(IList<ConditionIndex> acc) =>
                AddCallback(() => acc.Add(Index));

            public void AddLoggingInto(IList<Index> acc) =>
                AddCallback(() => acc.Add(Index));

            public void Returns(bool value) =>
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

            int CallsCount { get; set; } = 0;

            Action Callback { get; set; } = () =>
            {
                /* INITIALY DO NOTHING */
            };

            Func<bool> Return { get; set; } = () => false;

            public bool Check(T x)
            {
                if (x?.Equals(token) ?? false)
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
