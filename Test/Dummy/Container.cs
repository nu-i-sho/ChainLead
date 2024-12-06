namespace ChainLead.Test
{
    using System.Diagnostics.CodeAnalysis;

    public static partial class Dummy
    {
        public class Container<T>
        {
            public HandlerMath<T> HandlerMath { get; }

            public ConditionMath<T> ConditionMath { get; }

            public HandlerCollection<T> Handlers { get; }

            public ConditionCollection<T> Conditions { get; }

            public Container(T tokenArg,
                [AllowNull] IEnumerable<HandlerIndex> handlerIndices = null,
                [AllowNull] IEnumerable<ConditionIndex> conditionIndices = null)
            {
                handlerIndices ??= HandlerIndex.Common.ABCDEFGHIJ;
                conditionIndices ??= ConditionIndex.Common.QRSTUVWXYZ;

                Handlers = new(tokenArg);
                Conditions = new(tokenArg);

                foreach (var i in handlerIndices)
                    Handlers.Add(new(Handlers, i, tokenArg));

                foreach (var i in conditionIndices)
                    Conditions.Add(new(i, tokenArg));

                HandlerMath = new(Handlers, Conditions, tokenArg);
                ConditionMath = new(Conditions, tokenArg);
            }
        }
    }
}
