namespace ChainLead.Test.Help
{
    using System.Diagnostics.CodeAnalysis;
    using static Constants;

    class ChainLeadMocks
    {
        public HandlerMathMock HandlerMath { get; }

        public ConditionMathMock ConditionMath { get; }

        public HandlerMocksCollection Handlers { get; }

        public ConditionMocksCollection Conditions { get; }

        public ChainLeadMocks(
            [AllowNull] IEnumerable<HandlerIndex> handlerIndices = null,
            [AllowNull] IEnumerable<ConditionIndex> conditionIndices = null)
        {
            handlerIndices ??= ABCDEFGHIJ;
            conditionIndices ??= QRSTUVWXYZ;

            Handlers = new HandlerMocksCollection();
            Conditions = new ConditionMocksCollection();

            foreach (var i in handlerIndices)
                Handlers.Add(new HandlerMock(Handlers, i));

            foreach (var i in conditionIndices)
                Conditions.Add(new ConditionMock(i));

            HandlerMath = new HandlerMathMock(Handlers, Conditions);
            ConditionMath = new ConditionMathMock(Conditions);
        }
    }
}
