namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using System.Linq;
    using static ChainLead.Test.Help.DummyHandler;
    
    public static class DummyExtensions
    {
        public static TDummy Get<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> dummies, TIndex i)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    dummies[i];
        public static TObject GetObject<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> dummies, TIndex i)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    dummies[i].Object;

        public static IDummiesCollection<TDummy, TIndex, TObject> Where<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> mocks, Func<TDummy, bool> predicate)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex, TObject>(
                        mocks.AsEnumerable().Where(predicate));

        public static IDummiesCollection<TDummy, TIndex, TObject> Concat<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> dummies,
            IEnumerable<TDummy> otherDummies)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex, TObject>(
                        dummies.Concat(otherDummies));

        public static IDummiesCollection<TDummy, TIndex, TObject> Take<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> dummies, int count)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex, TObject>(
                        dummies.Take(count));

        public static IDummiesCollection<TDummy, TIndex, TObject> Skip<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> dummies, int count)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex, TObject>(
                        dummies.Skip(count));

        public static IDummiesCollection<TDummy, TIndex, TObject> Reverse<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> dummies)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex, TObject>(
                        dummies.Reverse());

        public static IDummiesCollection<TDummy, TIndex, TObject> Except<TDummy, TIndex, TObject>(
            this IDummiesCollection<TDummy, TIndex, TObject> dummies,
            IEnumerable<TDummy> otherDummies)
                where TDummy : IDummy<TIndex, TObject>
                where TIndex : DummyIndex =>
                    new DummiesCollection<TDummy, TIndex, TObject>(
                        dummies.Except(otherDummies));

        public static void SetResults(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition,
            bool value)
        {
            foreach (var c in condition)
                c.SetResult(value);
        }

        public static void SetResults(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition,
            IEnumerable<bool> results)
        {
            foreach (var (c, r) in condition.Zip(results))
                c.SetResult(r);
        }

        public static void AddLoggingInto(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition,
            IList<ConditionIndex> acc)
        {
            foreach (var c in condition)
                c.AddLoggingInto(acc);
        }

        public static void AddLoggingInto(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition,
            IList<DummyIndex> acc)
        {
            foreach (var c in condition)
                c.AddLoggingInto(acc);
        }

        public static bool EachWasCheckedOnce(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition) =>
                condition.Select(x => x.WasCheckedOnce())
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool NoOneWasChecked(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition) =>
                condition.Select(x => x.WasCheckedOnce())
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyChecks(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition,
            bool wasExecutedOnceElseNever) =>
                condition.Select(x => x.VerifyCheck(wasExecutedOnceElseNever))
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyChecks(
            this IDummiesCollection<DummyCondition, ConditionIndex, ICondition<int>> condition,
            IEnumerable<bool> setup) =>
                condition.Zip(setup, (x, was) => x.VerifyCheck(was))
                    .Aggregate(true, (acc, x) => acc && x);

        public static void AddLoggingInto(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers,
            IList<HandlerIndex> acc)
        {
            foreach (var h in handlers)
                h.AddLoggingInto(acc);
        }

        public static void AddLoggingInto(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers,
            IList<DummyIndex> acc)
        {
            foreach (var h in handlers)
                h.AddLoggingInto(acc);
        }

        public static void AddCallback(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers,
            Action f)
        {
            foreach (var h in handlers)
                h.AddCallback(f);
        }

        public static TimesContinuation EachWasExecuted(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers,
            int times) => new TimesContinuation(
                handlers.Select(h => h.WasExecuted(times).Times)
                    .Aggregate(true, (acc, x) => acc && x));

        public static bool EachWasExecutedOnce(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers) =>
                handlers.Select(h => h.WasExecutedOnce())
                    .Aggregate(true, (acc, x) => acc && x);

        public static ElseNoOneContinuation EachWasExecutedOnceWhen(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers,
            bool condition) =>
                new(handlers.Select(h => h.WasExecutedOnceWhen(condition).ElseNever)
                    .Aggregate(true, (acc, x) => acc && x));

        public class ElseNoOneContinuation(bool answer)
        {
            public bool ElseNoOne => answer;
        }

        public static bool NoOneWasExecuted(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers) =>
                handlers.Select(h => h.WasNeverExecuted())
                    .Aggregate(true, (acc, x) => acc && x);

        public static bool VerifyExecution(
            this IDummiesCollection<DummyHandler, HandlerIndex, IHandler<int>> handlers,
            IEnumerable<bool> setup) =>
                handlers.Zip(setup, (h, x) => h.VerifyExecution(x))
                    .Aggregate(true, (acc, x) => acc && x);
    }
}
