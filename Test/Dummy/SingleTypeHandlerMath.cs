namespace ChainLead.Test
{
    using ChainLead.Contracts;

    public static partial class Dummy
    {
        public class SingleTypeHandlerMath<T>(
                ICollection<Handler<T>, HandlerIndex> handlers) 
            : IHandlerMath
        {
            readonly Dictionary<(string, HandlerIndex, HandlerIndex), HandlerIndex> _appendsSetup = [];
            readonly Dictionary<(HandlerIndex, ConditionIndex), HandlerIndex> _conditionalSetup = [];
            readonly Dictionary<HandlerIndex, HandlerIndex> _atomizeSetup = [];
            HandlerIndex? _makeHandlerSetup;
            HandlerIndex? _zeroSetup;

            public void Zero_Returns(HandlerIndex handler) =>
                _zeroSetup = handler;

            public void MakeHandler_Returns(HandlerIndex handler) =>
                _makeHandlerSetup = handler;

            public ReturnsStep FirstThenSecond(HandlerIndex forPrev, HandlerIndex andNext)
            {
                var forFuncWithArgs = (nameof(IHandlerMath.FirstThenSecond), forPrev, andNext);
                return new(returnResult => _appendsSetup.Add(forFuncWithArgs, returnResult));
            }

            public ReturnsStep PackFirstInSecond(HandlerIndex forPrev, HandlerIndex andNext)
            {
                var forFuncWithArgs = (nameof(IHandlerMath.PackFirstInSecond), forPrev, andNext);
                return new(returnResult => _appendsSetup.Add(forFuncWithArgs, returnResult));
            }

            public ReturnsStep InjectFirstIntoSecond(HandlerIndex forPrev, HandlerIndex andNext)
            {
                var forFuncWithArgs = (nameof(IHandlerMath.InjectFirstIntoSecond), forPrev, andNext);
                return new(returnResult => _appendsSetup.Add(forFuncWithArgs, returnResult));
            }

            public ReturnsStep FirstCoverSecond(HandlerIndex forPrev, HandlerIndex andNext)
            {
                var forFuncWithArgs = (nameof(IHandlerMath.FirstCoverSecond), forPrev, andNext);
                return new (returnResult => _appendsSetup.Add(forFuncWithArgs, returnResult));
            }

            public ReturnsStep FirstWrapSecond(HandlerIndex forPrev, HandlerIndex forNext)
            {
                var forFuncWithArgs = (nameof(IHandlerMath.FirstWrapSecond), forPrev, forNext);
                return new(returnResult => _appendsSetup.Add(forFuncWithArgs, returnResult));
            }

            public ReturnsStep JoinFirstWithSecond(HandlerIndex forPrev, HandlerIndex andNext)
            {
                var forFuncWithArgs = (nameof(IHandlerMath.JoinFirstWithSecond), forPrev, andNext);
                return new (returnResult => _appendsSetup.Add(forFuncWithArgs, returnResult));
            }

            public ReturnsStep MergeFirstWithSecond(HandlerIndex forPrev, HandlerIndex andNext)
            {
                var forFuncWithArgs = (nameof(IHandlerMath.MergeFirstWithSecond), forPrev, andNext);
                return new(returnResult => _appendsSetup.Add(forFuncWithArgs, returnResult));
            }

            public ReturnsStep Conditional(HandlerIndex handler, ConditionIndex condition) =>
                new(result => _conditionalSetup.Add((handler, condition), result));

            public ReturnsStep Atomize(HandlerIndex handler) =>
                new(result => _atomizeSetup.Add(handler, result));

            public class ReturnsStep(Action<HandlerIndex> add)
            {
                public void Returns(HandlerIndex result) =>
                    add(result);
            }
            
            static HandlerIndex In<U>(IHandler<U> x) => ((Handler<T>)x).Index;

            static ConditionIndex In<U>(ICondition<U> x) => ((Condition<T>)x).Index;

            IHandler<U> Out<U>(HandlerIndex x) => (IHandler<U>)handlers.Get(x);

            public IHandler<U> Zero<U>() =>
                Out<U>(_zeroSetup!);

            public bool IsZero<U>(IHandler<U> handler) =>
                _zeroSetup != null && 
                handlers.Get(_zeroSetup).Index == In(handler);

            public IHandler<U> MakeHandler<U>(Action<U> action)
            {
                var h = handlers.Get(_makeHandlerSetup!);
                h.SetImplementation((Action<T>)(object)action);

                return Out<U>(h.Index);
            }

            public IHandler<U> FirstThenSecond<U>(IHandler<U> prev, IHandler<U> next)
            {
                var forFuncAndArgs = (nameof(IHandlerMath.FirstThenSecond), In(prev), In(next));
                return Out<U>(_appendsSetup[forFuncAndArgs]);
            }

            public IHandler<U> PackFirstInSecond<U>(IHandler<U> prev, IHandler<U> next)
            {
                var forFuncAndArgs = (nameof(IHandlerMath.PackFirstInSecond), In(prev), In(next));
                return Out<U>(_appendsSetup[forFuncAndArgs]);
            }

            public IHandler<U> InjectFirstIntoSecond<U>(IHandler<U> prev, IHandler<U> next)
            {
                var forFuncAndArgs = (nameof(IHandlerMath.InjectFirstIntoSecond), In(prev), In(next));
                return Out<U>(_appendsSetup[forFuncAndArgs]);
            }

            public IHandler<U> FirstCoverSecond<U>(IHandler<U> prev, IHandler<U> next)
            {
                var forFuncAndArgs = (nameof(IHandlerMath.FirstCoverSecond), In(prev), In(next));
                return Out<U>(_appendsSetup[forFuncAndArgs]);
            }

            public IHandler<U> FirstWrapSecond<U>(IHandler<U> prev, IHandler<U> next)
            {
                var forFuncAndArgs = (nameof(IHandlerMath.FirstWrapSecond), In(prev), In(next));
                return Out<U>(_appendsSetup[forFuncAndArgs]);
            }

            public IHandler<U> JoinFirstWithSecond<U>(IHandler<U> prev, IHandler<U> next)
            {
                var forFuncAndArgs = (nameof(IHandlerMath.JoinFirstWithSecond), In(prev), In(next));
                return Out<U>(_appendsSetup[forFuncAndArgs]);
            }

            public IHandler<U> MergeFirstWithSecond<U>(IHandler<U> prev, IHandler<U> next)
            {
                var forFuncAndArgs = (nameof(IHandlerMath.MergeFirstWithSecond), In(prev), In(next));
                return Out<U>(_appendsSetup[forFuncAndArgs]);
            }

            public IHandler<U> Atomize<U>(IHandler<U> handler) =>
                Out<U>(_atomizeSetup[In(handler)]);

            public IHandler<U> Conditional<U>(IHandler<U> handler, ICondition<U> condition) =>
                Out<U>(_conditionalSetup[(In(handler), In(condition))]);
        }
    }
}
