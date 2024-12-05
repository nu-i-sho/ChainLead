namespace ChainLead.Test.Help
{
    using System.Linq;
    using static ChainLead.Test.Help.DummyHandler;
    
    public static class DummyExtensions
    {
        public static TDummy Get<TDummy, TIndex>(
            this IDummiesCollection<TDummy, TIndex> dummies, TIndex i)
                where TDummy : IDummy<TIndex>
                where TIndex : DummyIndex =>
                    dummies[i];
        
        public static IDummiesCollection<TDummy, TIndex> Where<TDummy, TIndex>(
            this IDummiesCollection<TDummy, TIndex> mocks, Func<TDummy, bool> predicate)
                where TDummy : IDummy<TIndex>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex>(
                        mocks.AsEnumerable().Where(predicate));

        public static IDummiesCollection<TDummy, TIndex> Concat<TDummy, TIndex>(
            this IDummiesCollection<TDummy, TIndex> dummies,
            IEnumerable<TDummy> otherDummies)
                where TDummy : IDummy<TIndex>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex>(
                        dummies.Concat(otherDummies));

        public static IDummiesCollection<TDummy, TIndex> Take<TDummy, TIndex>(
            this IDummiesCollection<TDummy, TIndex> dummies, int count)
                where TDummy : IDummy<TIndex>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex>(
                        dummies.Take(count));

        public static IDummiesCollection<TDummy, TIndex> Skip<TDummy, TIndex>(
            this IDummiesCollection<TDummy, TIndex> dummies, int count)
                where TDummy : IDummy<TIndex>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex>(
                        dummies.Skip(count));

        public static IDummiesCollection<TDummy, TIndex> Reverse<TDummy, TIndex>(
            this IDummiesCollection<TDummy, TIndex> dummies)
                where TDummy : IDummy<TIndex>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex>(
                        dummies.Reverse());

        public static IDummiesCollection<TDummy, TIndex> Except<TDummy, TIndex>(
            this IDummiesCollection<TDummy, TIndex> dummies,
            IEnumerable<TDummy> otherDummies)
                where TDummy : IDummy<TIndex>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex>(
                        dummies.Except(otherDummies));

        public static void SetResults(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition,
            bool value)
        {
            foreach (var c in condition)
                c.SetResult(value);
        }

        public static void SetResults(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition,
            IEnumerable<bool> results)
        {
            foreach (var (c, r) in condition.Zip(results))
                c.SetResult(r);
        }

        public static void AddLoggingInto(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition,
            IList<ConditionIndex> acc)
        {
            foreach (var c in condition)
                c.AddLoggingInto(acc);
        }

        public static void AddLoggingInto(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition,
            IList<DummyIndex> acc)
        {
            foreach (var c in condition)
                c.AddLoggingInto(acc);
        }

        public static bool EachWasCheckedOnce(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition) =>
                condition.Select(x => x.WasCheckedOnce())
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool NoOneWasChecked(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition) =>
                condition.Select(x => x.WasCheckedOnce())
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyChecks(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition,
            bool wasExecutedOnceElseNever) =>
                condition.Select(x => x.VerifyCheck(wasExecutedOnceElseNever))
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyChecks(
            this IDummiesCollection<DummyCondition, ConditionIndex> condition,
            IEnumerable<bool> setup) =>
                condition.Zip(setup, (x, was) => x.VerifyCheck(was))
                    .Aggregate(true, (acc, x) => acc && x);

        public static void AddLoggingInto(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers,
            IList<HandlerIndex> acc)
        {
            foreach (var h in handlers)
                h.AddLoggingInto(acc);
        }

        public static void AddLoggingInto(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers,
            IList<DummyIndex> acc)
        {
            foreach (var h in handlers)
                h.AddLoggingInto(acc);
        }

        public static void AddCallback(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers,
            Action f)
        {
            foreach (var h in handlers)
                h.AddCallback(f);
        }

        public static TimesContinuation EachWasExecuted(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers,
            int times) => new TimesContinuation(
                handlers.Select(h => h.WasExecuted(times).Times)
                    .Aggregate(true, (acc, x) => acc && x));

        public static bool EachWasExecutedOnce(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers) =>
                handlers.Select(h => h.WasExecutedOnce())
                    .Aggregate(true, (acc, x) => acc && x);

        public static ElseNoOneContinuation EachWasExecutedOnceWhen(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers,
            bool condition) =>
                new(handlers.Select(h => h.WasExecutedOnceWhen(condition).ElseNever)
                    .Aggregate(true, (acc, x) => acc && x));

        public class ElseNoOneContinuation(bool answer)
        {
            public bool ElseNoOne => answer;
        }

        public static bool NoOneWasExecuted(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers) =>
                handlers.Select(h => h.WasNeverExecuted())
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyExecution(
            this IDummiesCollection<DummyHandler, HandlerIndex> handlers,
            IEnumerable<bool> setup) =>
                handlers.Zip(setup, (h, x) => h.VerifyExecution(x))
                    .Aggregate(true, (acc, x) => acc && x);
    }
}
