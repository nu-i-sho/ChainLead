namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public class Container<T>
        {
            public SingleTypeHandlerMath<T> HandlerMath { get; }

            public SingleTypeConditionMath<T> ConditionMath { get; }

            public IHandlerCollection<T>.IMutable Handlers { get; }

            public IConditionCollection<T>.IMutable Conditions { get; }

            public Handler<T> Handler(HandlerIndex index) =>
                Handlers.Get(index);

            public Condition<T> Condition(ConditionIndex index) =>
                Conditions.Get(index);

            public Container(T token)
            {
                Handlers = new HandlerCollection<T>(token);
                Conditions = new ConditionCollection<T>(token);

                HandlerMath = new(Handlers);
                ConditionMath = new(Conditions);
            }
        }
    }
}
