using static ChainLead.Test.Dummy;

namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public static ICollection<TDummy, TIndex> Where<TDummy, TIndex>(
            this ICollection<TDummy, TIndex> dummies, 
            Func<TDummy, bool> predicate)
            
                where TDummy : IDummy<TIndex>
                where TIndex : Index =>

                    new Collection<TDummy, TIndex>(
                        dummies.AsEnumerable()
                               .Where(predicate));


        public static ICollection<TDummy, TIndex> Concat<TDummy, TIndex>(
            this ICollection<TDummy, TIndex> dummies,
            IEnumerable<TDummy> otherDummies)

                where TDummy : IDummy<TIndex>
                where TIndex : Index =>

                    new Collection<TDummy, TIndex>(
                        dummies.AsEnumerable()
                               .Concat(otherDummies));


        public static ICollection<TDummy, TIndex> Take<TDummy, TIndex>(
            this ICollection<TDummy, TIndex> dummies,
            int count)

                where TDummy : IDummy<TIndex>
                where TIndex : Index =>

                    new Collection<TDummy, TIndex>(
                        dummies.AsEnumerable()
                               .Take(count));


        public static ICollection<TDummy, TIndex> Skip<TDummy, TIndex>(
            this ICollection<TDummy, TIndex> dummies, 
            int count)

                where TDummy : IDummy<TIndex>
                where TIndex : Index =>

                    new Collection<TDummy, TIndex>(
                        dummies.AsEnumerable()
                               .Skip(count));


        public static ICollection<TDummy, TIndex> Reverse<TDummy, TIndex>(
            this ICollection<TDummy, TIndex> dummies)
                
                where TDummy : IDummy<TIndex>
                where TIndex : Index =>

                    new Collection<TDummy, TIndex>(
                        dummies.AsEnumerable()
                               .Reverse());


        public static ICollection<TDummy, TIndex> Except<TDummy, TIndex>(
            this ICollection<TDummy, TIndex> dummies,
            IEnumerable<TDummy> otherDummies)
                
                where TDummy : IDummy<TIndex>
                where TIndex : Index =>
                    
                    new Collection<TDummy, TIndex>(
                        dummies.AsEnumerable()
                               .Except(otherDummies));


        public static void Return<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            bool value)
        {
            foreach (var c in condition)
                c.Returns(value);
        }

        public static void Return<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            IEnumerable<bool> results)
        {
            foreach (var (c, r) in condition.Zip(results))
                c.Returns(r);
        }

        public static void AddLoggingInto<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            IList<ConditionIndex> acc)
        {
            foreach (var c in condition)
                c.LogsInto(acc);
        }

        public static void LogInto<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            IList<Index> acc)
        {
            foreach (var c in condition)
                c.LogsInto(acc);
        }

        public static bool EachWasCheckedOnce<T>(
            this ICollection<Condition<T>, ConditionIndex> condition) =>
                condition.Select(x => x.WasCheckedOnce())
                         .Aggregate(true, (acc, x) => acc && x);

        public static bool WereNeverChecked<T>(
            this ICollection<Condition<T>, ConditionIndex> condition) =>
                condition.Select(x => x.WasNeverChecked())
                         .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyChecks<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            bool wasExecutedOnceElseNever) =>
                condition.Select(x => x.VerifyCheck(wasExecutedOnceElseNever))
                         .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyChecks<T>(
            this ICollection<Condition<T>, ConditionIndex> condition,
            IEnumerable<bool> setup) =>
                condition.Zip(setup, (x, was) => x.VerifyCheck(was))
                         .Aggregate(true, (acc, x) => acc && x);

        public static void LogInto<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers,
            IList<HandlerIndex> acc)
        {
            foreach (var h in handlers)
                h.LogsInto(acc);
        }

        public static void LogInto<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers,
            IList<Index> acc)
        {
            foreach (var h in handlers)
                h.LogsInto(acc);
        }

        public static void AddCallback<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers,
            Action f)
        {
            foreach (var h in handlers)
                h.AddCallback(f);
        }

        public static Handler<T>.TimesContinuation EachWasExecuted<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers,
            int times) => new(
                handlers.Select(h => h.WasExecuted(times).Times)
                        .Aggregate(true, (acc, x) => acc && x));

        public static bool EachWasExecutedOnce<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers) =>
                handlers.Select(h => h.WasExecutedOnce())
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

        public static bool NoOneWasExecuted<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers) =>
                handlers.Select(h => h.WasNeverExecuted())
                        .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyExecution<T>(
            this ICollection<Handler<T>, HandlerIndex> handlers,
            IEnumerable<bool> setup) =>
                handlers.Zip(setup, (h, x) => h.VerifyExecution(x))
                        .Aggregate(true, (acc, x) => acc && x);
    }
}
