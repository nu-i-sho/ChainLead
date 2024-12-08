namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using static ChainLead.Test.Dummy.Index.Common;

    public static partial class Dummy
    {
        public class SingleTypeConditionMath<T>(
                ICollection<Condition<T>, ConditionIndex> conditions)
            : IConditionMath
        {
            readonly Dictionary<string, Func<ConditionIndex, ConditionIndex, ConditionIndex>> _setupForAnyAppend = new();
            readonly Dictionary<(string, ConditionIndex, ConditionIndex), ConditionIndex> _setupForAppend = new();
            readonly Dictionary<ConditionIndex, ConditionIndex> _setupForNot = new();
            ConditionIndex? _setupForMakeCondition;
            ConditionIndex? _setupForTrue;
            ConditionIndex? _setupForFalse;

            public void True_Returns(ConditionIndex condition) =>
                _setupForTrue = condition;

            public void False_Returns(ConditionIndex condition) =>
                _setupForFalse = condition;

            public void MakeCondition_Returns(ConditionIndex condition) =>
                _setupForMakeCondition = condition;

            public ReturnsStep And(ConditionIndex a, ConditionIndex b) =>
                new(x => _setupForAppend.Add((nameof(IConditionMath.And), a, b), x));

            public ImplementsStep And(AnyArg _, AnyArg __) =>
                new(f => _setupForAnyAppend.Add(nameof(IConditionMath.And), f)); 

            public ReturnsStep Or(ConditionIndex a, ConditionIndex b) =>
                new (x => _setupForAppend.Add((nameof(IConditionMath.Or), a, b), x));

            public ImplementsStep Or(AnyArg _, AnyArg __) =>
                new(f => _setupForAnyAppend.Add(nameof(IConditionMath.Or), f));

            public ReturnsStep Not(ConditionIndex a) =>
                new(x => _setupForNot.Add(a, x));

            public class ReturnsStep(Action<ConditionIndex> add)
            {
                public void Returns(ConditionIndex result) =>
                    add(result);
            }

            public class ImplementsStep(
                Action<Func<ConditionIndex, ConditionIndex, ConditionIndex>> add)
            {
                public void Implements(
                    Func<ConditionIndex, ConditionIndex, ConditionIndex> implementation) =>
                        add(implementation);
            }

            static Condition<T> In<U>(ICondition<U> x) => (Condition<T>)x;

            static ICondition<U> Out<U>(Condition<T> x) => (ICondition<U>)x;

            public ICondition<U> True<U>() =>
                Out<U>(conditions.Get(_setupForTrue!));

            public ICondition<U> False<U>() =>
                Out<U>(conditions.Get(_setupForFalse!));

            public bool IsPredictableTrue<U>(ICondition<U> condition) =>
                _setupForTrue != null && 
                conditions.Get(_setupForTrue).Index == In(condition).Index;

            public bool IsPredictableFalse<U>(ICondition<U> condition) =>
                _setupForFalse != null && 
                conditions.Get(_setupForFalse).Index == In(condition).Index;

            public ICondition<U> MakeCondition<U>(Func<U, bool> f)
            {
                var c = conditions.Get(_setupForMakeCondition!);
                c.SetImplementation((Func<T, bool>)(object)f);

                return Out<U>(c);
            }

            public ICondition<U> And<U>(
                ICondition<U> a, ICondition<U> b) =>
                    Append(nameof(IConditionMath.And), a, b);

            public ICondition<U> Or<U>(
                ICondition<U> a, ICondition<U> b) =>
                    Append(nameof(IConditionMath.Or), a, b);
                
            public ICondition<U> Not<U>(ICondition<U> condition) =>
                Out<U>(conditions.Get(_setupForNot[In(condition).Index]));

            ICondition<U> Append<U>(string name, ICondition<U> a, ICondition<U> b)
            {
                var i = In(a).Index;
                var j = In(b).Index;

                ConditionIndex? result = null;
                Func<ConditionIndex, ConditionIndex, ConditionIndex>? implementation = null;

                if (!_setupForAppend.TryGetValue((name, i, j), out result) &&
                    _setupForAnyAppend.TryGetValue(name, out implementation))
                {
                    result = implementation(i, j);
                }

                if (result == null)
                    throw new NotImplementedException(
                        $"{nameof(IConditionMath)}.{nameof(IConditionMath.And)}");

                return Out<U>(conditions.Get(result));
            }
        }
    }
}

