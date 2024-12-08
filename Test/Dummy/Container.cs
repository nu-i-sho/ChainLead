namespace ChainLead.Test
{
    using System.Diagnostics.CodeAnalysis;

    public static partial class Dummy
    {
        public class Container<T>
        {
            public SingleTypeHandlerMath<T> HandlerMath { get; }

            public SingleTypeConditionMath<T> ConditionMath { get; }

            public ICollection<Handler<T>, HandlerIndex>.IMutable Handlers { get; }

            public ICollection<Condition<T>, ConditionIndex>.IMutable Conditions { get; }

            public Handler<T> Handler(HandlerIndex index) =>
                Handlers.Get(index);

            public Condition<T> Condition(ConditionIndex index) =>
                Conditions.Get(index);

            public Container(T token,
                [AllowNull] IEnumerable<HandlerIndex> handlerIndices = null,
                [AllowNull] IEnumerable<ConditionIndex> conditionIndices = null)
            {
                handlerIndices ??= HandlerIndex.Common.ABCDEFGHIJ;
                conditionIndices ??= ConditionIndex.Common.QRSTUVWXYZ;

                Handlers = new HandlerCollection<T>(token);
                Conditions = new ConditionCollection<T>(token);

                Handlers.AddRange(handlerIndices);
                Conditions.AddRange(conditionIndices);

                HandlerMath = new(Handlers);
                ConditionMath = new(Conditions);
            }
        }
    }
}
