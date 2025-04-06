namespace Nuisho.ChainLead.Test
{
    using Contracts;

    public static partial class Dummy
    {
        public class Condition<T>(
                ConditionIndex index,
                T token) :
            IChainElement<ConditionIndex>,
            ICondition<T>
        {
            public ConditionIndex Index => index;

            public void SetImplementation(Func<T, bool> f) =>
                Implementation = f;

            public void AddCallback(Action f) =>
                Callback += f;

            public void LogsInto(IList<ConditionIndex> log) =>
                AddCallback(() => log.Add(Index));

            public void LogsInto(IList<ChainElementIndex> log) =>
                AddCallback(() => log.Add(Index));

            public void Returns(bool value) =>
                Implementation = _ => value;

            public bool WasCheckedOnce =>
                CallsCount == 1;

            public bool WasNeverChecked =>
                CallsCount == 0;

            public ElseNeverContinuation WasCheckedOnceWhen(
                bool checkedCondition) =>
                new (checkedCondition
                        ? WasCheckedOnce
                        : WasNeverChecked);

            public class ElseNeverContinuation(bool answer)
            {
                public bool ElseNever => answer;
            }

            public override string ToString() => $"c<{index.View}>";

            public override bool Equals(object? obj) =>
                obj is Condition<T> handler && Index == handler.Index;

            public override int GetHashCode() =>
                index.GetHashCode();

            public ICondition<T> Pure => this;

            int CallsCount { get; set; } = 0;

            Action Callback { get; set; } = () =>
            {
                /* INITIALY DO NOTHING */
            };

            Func<T, bool> Implementation { get; set; } = _ => false;

            public bool Check(T state)
            {
                if (state?.Equals(token) ?? false)
                {
                    var result = Implementation(state);

                    Callback();
                    CallsCount++;

                    return result;
                }

                return false;
            }
        }
    }
}
