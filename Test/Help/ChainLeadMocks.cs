namespace ChainLead.Test.Help
{
    using System.Diagnostics.CodeAnalysis;
    using static Constants;

    class ChainLeadMocks
    {
        public HandlerMathMock HandlerMath { get; }

        public ConditionMathMock ConditionMath { get; }

        public DummyHandlersCollection Handlers { get; }

        public DummyConditionsCollection Conditions { get; }

        public ChainLeadMocks(
            [AllowNull] IEnumerable<HandlerIndex> handlerIndices = null,
            [AllowNull] IEnumerable<ConditionIndex> conditionIndices = null)
        {
            handlerIndices ??= ABCDEFGHIJ;
            conditionIndices ??= QRSTUVWXYZ;

            Handlers = [];
            Conditions = [];

            foreach (var i in handlerIndices)
                Handlers.Add(new DummyHandler(Handlers, i));

            foreach (var i in conditionIndices)
                Conditions.Add(new DummyCondition(i));

            HandlerMath = new HandlerMathMock(Handlers, Conditions);
            ConditionMath = new ConditionMathMock(Conditions);
        }
    }
}
