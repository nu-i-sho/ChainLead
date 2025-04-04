namespace Nuisho.ChainLead.Test
{
    using Contracts;

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
            IEnumerable<Handler<T>> otherDummies)
        {
            var concatenation = new HandlerCollection<T>(dummies.Token);
            concatenation.AddRange(dummies);
            concatenation.AddRange(otherDummies);
            return concatenation;
        }

        public static IConditionCollection<T> Concat<T>(
            this IConditionCollection<T> dummies,
            IEnumerable<Condition<T>> otherDummies)
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
            IEnumerable<Handler<T>> otherDummies) =>
                dummies[dummies.Indices.Except(otherDummies.Select(x => x.Index))];

        public static IConditionCollection<T> Except<T>(
            this IConditionCollection<T> dummies,
            IEnumerable<Condition<T>> otherDummies) =>
                dummies[dummies.Indices.Except(otherDummies.Select(x => x.Index))];

        public static IHandlerCollection<T> Filter<T>(
            this IHandlerCollection<T> dummies,
            IEnumerable<bool> mask) =>
                dummies[dummies.Indices
                    .Zip(mask, (item, set) => new { item, set })
                    .Where(x => x.set)
                    .Select(x => x.item)];

        public static IConditionCollection<T> Filter<T>(
            this IConditionCollection<T> dummies,
            IEnumerable<bool> mask) =>
                dummies[dummies.Indices
                    .Zip(mask, (item, set) => new { item, set })
                    .Where(x => x.set)
                    .Select(x => x.item)];

        public static IEnumerable<bool> Inverse(
            this IEnumerable<bool> mask) =>
                mask.Select(x => !x);

        public static IExtendedHandler<T> AsExtended<T>(this IHandler<T> origin) => 
            new Extended<T>(origin);

        class Extended<T>(IHandler<T> origin) : IExtendedHandler<T>
        {
            IHandler<T> IExtendedHandler<T>.Origin => origin;
        }
    }
}
