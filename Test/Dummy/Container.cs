namespace ChainLead.Test
{
    using System.Diagnostics.CodeAnalysis;
    using static ChainLead.Test.Dummy;

    public static partial class Dummy
    {
        public class Container<T>
        {
            public SingleTypeHandlerMath<T> HandlerMath { get; }

            public ConditionMath<T> ConditionMath { get; }

            public HandlerCollection<T> Handlers { get; }

            public ConditionCollection<T> Conditions { get; }

            public Handler<T> Handler(HandlerIndex index) =>
                Handlers[index];

            public Condition<T> Condition(ConditionIndex index) =>
                Conditions[index];

            public Container(T tokenArg,
                [AllowNull] IEnumerable<HandlerIndex> handlerIndices = null,
                [AllowNull] IEnumerable<ConditionIndex> conditionIndices = null)
            {
                handlerIndices ??= HandlerIndex.Common.ABCDEFGHIJ;
                conditionIndices ??= ConditionIndex.Common.QRSTUVWXYZ;

                Handlers = new(tokenArg);
                Conditions = new(tokenArg);

                foreach (var i in handlerIndices)
                    Handlers.Add((Handler<T>)new(Handlers, i, tokenArg));

                foreach (var i in conditionIndices)
                    Conditions.Add((Condition<T>)new(i, tokenArg));

                HandlerMath = new(Handlers);
                ConditionMath = new(Conditions, tokenArg);
            }
        }
    }
}
