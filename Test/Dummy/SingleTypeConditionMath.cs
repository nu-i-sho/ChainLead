namespace Nuisho.ChainLead.Test
{
    using Contracts;
    using static Test.Dummy.Common;

    public static partial class Dummy
    {
        public class SingleTypeConditionMath<T>(
                IConditionCollection<T> conditions)
            : IConditionMath
        {
            readonly Dictionary<string, Func<ConditionIndex, ConditionIndex, ConditionIndex>> _anyAppendSetup = [];
            readonly Dictionary<(string, ConditionIndex, ConditionIndex), ConditionIndex> _appendSetup = [];
            readonly Dictionary<ConditionIndex, ConditionIndex> _notSetup = [];
            ConditionIndex? _makeConditionSetup;
            ConditionIndex? _trueSetup;
            ConditionIndex? _falseSetup;

            public void True_Returns(ConditionIndex condition) =>
                _trueSetup = condition;

            public void False_Returns(ConditionIndex condition) =>
                _falseSetup = condition;

            public void MakeCondition_Returns(ConditionIndex condition) =>
                _makeConditionSetup = condition;

            public ReturnsStep And(ConditionIndex forLeft, ConditionIndex andRight)
            {
                var forFuncAndArgs = (nameof(IConditionMath.And), forLeft, andRight);
                return new (returnResult => _appendSetup.Add(forFuncAndArgs, returnResult));
            }

            public ImplementsStep And(AnyArg _, AnyArg __) =>
                new (implementation => _anyAppendSetup
                    .Add(nameof(IConditionMath.And), implementation));

            public ReturnsStep Or(ConditionIndex forLeft, ConditionIndex andRight)
            {
                var forFuncAndArgs = (nameof(IConditionMath.Or), forLeft, andRight);
                return new (returnResult => _appendSetup.Add(forFuncAndArgs, returnResult));
            }

            public ImplementsStep Or(AnyArg _, AnyArg __) =>
                new (implementation => _anyAppendSetup
                    .Add(nameof(IConditionMath.Or), implementation));

            public ReturnsStep Not(ConditionIndex forCondition) =>
                new (returnResult => _notSetup.Add(forCondition, returnResult));

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

            static ConditionIndex In<U>(ICondition<U> x) => ((Condition<T>)x).Index;

            ICondition<U> Out<U>(ConditionIndex i) => (ICondition<U>)conditions.Get(i);

            public ICondition<U> True<U>() => Out<U>(_trueSetup!);

            public ICondition<U> False<U>() => Out<U>(_falseSetup!);

            public bool IsPredictableTrue<U>(ICondition<U> condition) =>
                _trueSetup != null &&
                conditions.Get(_trueSetup).Index == In(condition);

            public bool IsPredictableFalse<U>(ICondition<U> condition) =>
                _falseSetup != null &&
                conditions.Get(_falseSetup).Index == In(condition);

            public ICondition<U> MakeCondition<U>(Func<U, bool> predicate)
            {
                var condition = conditions.Get(_makeConditionSetup!);
                condition.SetImplementation((Func<T, bool>)(object)predicate);

                return Out<U>(condition.Index);
            }

            public ICondition<U> And<U>(ICondition<U> left, ICondition<U> right) =>
                Append(nameof(IConditionMath.And), left, right);

            public ICondition<U> Or<U>(ICondition<U> left, ICondition<U> right) =>
                Append(nameof(IConditionMath.Or), left, right);

            public ICondition<U> Not<U>(ICondition<U> condition) =>
                Out<U>(_notSetup[In(condition)]);

            ICondition<U> Append<U>(string name, ICondition<U> left, ICondition<U> right)
            {
                ConditionIndex? result = null;
                Func<ConditionIndex, ConditionIndex, ConditionIndex>? implementation = null;

                if (!_appendSetup.TryGetValue((name, In(left), In(right)), out result) &&
                    _anyAppendSetup.TryGetValue(name, out implementation))
                {
                    result = implementation(In(left), In(right));
                }

                if (result == null)
                    throw new NotImplementedException(
                        $"{nameof(IConditionMath)}.{nameof(IConditionMath.And)}");

                return Out<U>(result);
            }
        }
    }
}
