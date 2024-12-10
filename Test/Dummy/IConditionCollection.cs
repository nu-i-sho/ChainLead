namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public interface IConditionCollection<T>
            : ICollection<Condition<T>, ConditionIndex>
        {
            T Token { get; }

            new IConditionCollection<T> this[
                ConditionIndex first, ConditionIndex second,
                params ConditionIndex[] tail] =>
                    (IConditionCollection<T>)
                    ((ICollection<Condition<T>, ConditionIndex>)this)[first, second, tail];

            new IConditionCollection<T> this[
                IEnumerable<ConditionIndex> indices] =>
                    (IConditionCollection<T>)
                    ((ICollection<Condition<T>, ConditionIndex>)this)[indices];

            ICollection<Condition<T>, ConditionIndex> ThatWereCheckedOnce =>
                this.Where(x => x.WasCheckedOnce);

            ICollection<Condition<T>, ConditionIndex> ThatWereNeverChecked =>
                this.Where(x => x.WasNeverChecked);

            public void Return(bool value)
            {
                foreach (var c in this)
                    c.Returns(value);
            }

            public void Return(IEnumerable<bool> results)
            {
                foreach (var (c, r) in this.Zip(results))
                    c.Returns(r);
            }

            public new interface IMutable :
                ICollection<Condition<T>, ConditionIndex>.IMutable,
                IConditionCollection<T>
            {
            }
        }
    }
}
