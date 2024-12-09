namespace ChainLead.Test
{
    using ChainLead.Contracts;

    public static partial class Dummy
    {
        public class SingleTypeHandlerMath<T>(
                ICollection<Handler<T>, HandlerIndex> handlers) 
            : IHandlerMath
        {
            readonly Dictionary<(string, HandlerIndex, HandlerIndex), HandlerIndex> _setupForAppends = [];
            readonly Dictionary<(HandlerIndex, ConditionIndex), HandlerIndex> _setuprForConditional = [];
            readonly Dictionary<HandlerIndex, HandlerIndex> _setupForAtomize = [];
            HandlerIndex? _setupForMakeHandler;
            HandlerIndex? _setupForZero;

            public void Zero_Returns(HandlerIndex handler) =>
                _setupForZero = handler;

            public void MakeHandler_Returns(HandlerIndex handler) =>
                _setupForMakeHandler = handler;

            public ReturnsStep FirstThenSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _setupForAppends.Add(
                            (nameof(IHandlerMath.FirstThenSecond), a, b), 
                            x));

            public ReturnsStep PackFirstInSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _setupForAppends.Add(
                            (nameof(IHandlerMath.PackFirstInSecond), a, b),
                            x));

            public ReturnsStep InjectFirstIntoSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _setupForAppends.Add(
                            (nameof(IHandlerMath.InjectFirstIntoSecond), a, b),
                            x));

            public ReturnsStep FirstCoverSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _setupForAppends.Add(
                            (nameof(IHandlerMath.FirstCoverSecond), a, b),
                            x));

            public ReturnsStep FirstWrapSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _setupForAppends.Add(
                            (nameof(IHandlerMath.FirstWrapSecond), a, b),
                            x));

            public ReturnsStep JoinFirstWithSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _setupForAppends.Add(
                            (nameof(IHandlerMath.JoinFirstWithSecond), a, b),
                            x));

            public ReturnsStep MergeFirstWithSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _setupForAppends.Add(
                            (nameof(IHandlerMath.MergeFirstWithSecond), a, b),
                            x));

            public ReturnsStep Conditional(HandlerIndex handler, ConditionIndex condition) =>
                new(x => _setuprForConditional.Add((handler, condition), x));

            public ReturnsStep Atomize(HandlerIndex handler) =>
                new(x => _setupForAtomize.Add(handler, x));

            public class ReturnsStep(Action<HandlerIndex> add)
            {
                public void Returns(HandlerIndex result) =>
                    add(result);
            }
            
            static Handler<T> In<U>(IHandler<U> x) => (Handler<T>)x;

            static Condition<T> In<U>(ICondition<U> x) => (Condition<T>)x;

            static IHandler<U> Out<U>(IHandler<T> x) => (IHandler<U>)x;

            public IHandler<U> Zero<U>() =>
                Out<U>(handlers.Get(_setupForZero!));

            public bool IsZero<U>(IHandler<U> handler) =>
                _setupForZero != null && 
                handlers.Get(_setupForZero).Index == In(handler).Index;

            public IHandler<U> MakeHandler<U>(Action<U> action)
            {
                var h = handlers.Get(_setupForMakeHandler!);
                h.SetImplementation((Action<T>)(object)action);

                return Out<U>(h);
            }

            public IHandler<U> FirstThenSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_setupForAppends[
                        (nameof(IHandlerMath.FirstThenSecond), In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> PackFirstInSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_setupForAppends[
                        (nameof(IHandlerMath.PackFirstInSecond), In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> InjectFirstIntoSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_setupForAppends[
                        (nameof(IHandlerMath.InjectFirstIntoSecond), In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> FirstCoverSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_setupForAppends[
                        (nameof(IHandlerMath.FirstCoverSecond), In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> FirstWrapSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_setupForAppends[
                        (nameof(IHandlerMath.FirstWrapSecond), In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> JoinFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_setupForAppends[
                        (nameof(IHandlerMath.JoinFirstWithSecond), In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> MergeFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_setupForAppends[
                        (nameof(IHandlerMath.MergeFirstWithSecond), In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> Atomize<U>(
                IHandler<U> handler) =>
                    Out<U>(handlers.Get(_setupForAtomize[
                        In(handler).Index
                        ]));

            public IHandler<U> Conditional<U>(
                IHandler<U> handler, ICondition<U> condition) =>
                    Out<U>(handlers.Get(_setuprForConditional[
                        (In(handler).Index, In(condition).Index)
                        ]));
        }
    }
}
