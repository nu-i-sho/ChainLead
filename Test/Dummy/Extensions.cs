using ChainLead.Contracts;
using static ChainLead.Test.Dummy;

namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public static IHandlerCollection<T> Where<T>(
            this IHandlerCollection<T> dummies,
            Func<Handler<T>, bool> predicate) =>
                dummies[dummies.AsEnumerable()
                    .Where(predicate)
                    .Select(x => x.Index)];

        public static IConditionCollection<T> Where<T>(
            this IConditionCollection<T> dummies,
            Func<Condition<T>, bool> predicate) =>
                dummies[dummies.AsEnumerable()
                    .Where(predicate)
                    .Select(x => x.Index)];

        public static IHandlerCollection<T> Concat<T>(
            this IHandlerCollection<T> dummies,
            IHandlerCollection<T> otherDummies)
        {
            var concatenation = new HandlerCollection<T>(dummies.Token);
            concatenation.AddRange(dummies);
            concatenation.AddRange(otherDummies);
            return concatenation;
        }

        public static IConditionCollection<T> Concat<T>(
            this IConditionCollection<T> dummies,
            IConditionCollection<T> otherDummies)
        {
            var concatenation = new ConditionCollection<T>(dummies.Token);
            concatenation.AddRange(dummies);
            concatenation.AddRange(otherDummies);
            return concatenation;
        }

        public static IHandlerCollection<T> Take<T>(
            this IHandlerCollection<T> dummies, int count) =>
                dummies[dummies.Indices.Take(count)];

        public static IConditionCollection<T> Take<T>(
            this IConditionCollection<T> dummies, int count) =>
                dummies[dummies.Indices.Take(count)];

        public static IHandlerCollection<T> Skip<T>(
            this IHandlerCollection<T> dummies, int count) =>
                dummies[dummies.Indices.Skip(count)];

        public static IConditionCollection<T> Skip<T>(
            this IConditionCollection<T> dummies, int count) =>
                dummies[dummies.Indices.Skip(count)];

        public static IHandlerCollection<T> Reverse<T>(
            this IHandlerCollection<T> dummies) =>
                dummies[dummies.Indices.Reverse()];

        public static IConditionCollection<T> Reverse<T>(
            this IConditionCollection<T> dummies) =>
                dummies[dummies.Indices.Reverse()];

        public static IHandlerCollection<T> Except<T>(
            this IHandlerCollection<T> dummies,
            IHandlerCollection<T> otherDummies) =>
                dummies[dummies.Indices.Except(otherDummies.Indices)];

        public static IConditionCollection<T> Except<T>(
            this IConditionCollection<T> dummies,
            IConditionCollection<T> otherDummies) =>
                dummies[dummies.Indices.Except(otherDummies.Indices)];

        public static bool EachWasCheckedWhen<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            bool checkedCondition) =>
                condition.Select(x => x.WasCheckedOnceWhen(checkedCondition).ElseNever)
                         .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyChecks<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            IEnumerable<bool> setup) =>
                condition.Zip(setup, (x, was) => x.WasCheckedOnceWhen(was).ElseNever)
                         .Aggregate(true, (acc, x) => acc && x);

        public static ElseNoOneContinuation EachWasExecutedOnceWhen<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers,
            bool condition) => new(
                handlers.Select(h => h.WasExecutedOnceWhen(condition).ElseNever)
                        .Aggregate(true, (acc, x) => acc && x));

        public class ElseNoOneContinuation(bool answer)
        {
            public bool ElseNoOne => answer;
        }

        public static bool VerifyExecution<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers,
            IEnumerable<bool> setup) =>
                handlers.Zip(setup, (h, x) => h.WasExecutedOnceWhen(x).ElseNever)
                        .Aggregate(true, (acc, x) => acc && x);

        public static IExtendedHandler<T> AsExtended<T>(this IHandler<T> origin) => 
            new Extended<T>(origin);

        class Extended<T>(IHandler<T> origin) : IExtendedHandler<T>
        {
            IHandler<T> IExtendedHandler<T>.Origin => origin;
        }
    }
}
