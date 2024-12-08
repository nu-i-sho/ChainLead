namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using System;

    using Method = Common.Notation.HandlerMath;
    
    public static partial class Dummy
    {
        public class SingleTypeHandlerMath<T>(
                ICollection<Handler<T>, HandlerIndex>.Mutable handlers) 
            : IHandlerMath
        {
            Dictionary<(string, HandlerIndex, HandlerIndex), HandlerIndex> _forAppends = new();
            Dictionary<(HandlerIndex, ConditionIndex), HandlerIndex> _forConditional = new();
            Dictionary<HandlerIndex, HandlerIndex> _forAtomize = new();
            HandlerIndex? _forMakeHandler;
            HandlerIndex? _forZero;

            public void Zero_Returns(HandlerIndex handler) =>
                _forZero = handler;

            public void MakeHandler_Returns(HandlerIndex handler) =>
                _forMakeHandler = handler;

            public ReturnsStep FirstThenSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.FirstThenSecond, a, b), x));

            public ReturnsStep PackFirstInSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.PackFirstInSecond, a, b), x));

            public ReturnsStep InjectFirstIntoSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.InjectFirstIntoSecond, a, b), x));

            public ReturnsStep FirstCoverSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.FirstCoverSecond, a, b), x));

            public ReturnsStep FirstWrapSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.FirstWrapSecond, a, b), x));

            public ReturnsStep JoinFirstWithSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.JoinFirstWithSecond, a, b), x));

            public ReturnsStep MergeFirstWithSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.MergeFirstWithSecond, a, b), x));

            public ReturnsStep Conditional(HandlerIndex handler, ConditionIndex condition) =>
                new(x => _forConditional.Add((handler, condition), x));

            public ReturnsStep Atomize(HandlerIndex handler) =>
                new(x => _forAtomize.Add(handler, x));

            public class ReturnsStep(Action<HandlerIndex> add)
            {
                public void Returns(HandlerIndex result) =>
                    add(result);
            }
            
            static Handler<T> In<U>(IHandler<U> x) => (Handler<T>)x;

            static Condition<T> In<U>(ICondition<U> x) => (Condition<T>)x;

            static IHandler<U> Out<U>(IHandler<T> x) => (IHandler<U>)x;

            public IHandler<U> Zero<U>() =>
                Out<U>(handlers.Get(_forZero!));

            public bool IsZero<U>(IHandler<U> handler) =>
                handlers.Get(_forZero!).Index == In(handler).Index;

            public IHandler<U> MakeHandler<U>(Action<U> action)
            {
                var h = handlers.Get(_forMakeHandler!);
                h.SetImplementation((Action<T>)(object)action);

                return Out<U>(h);
            }

            public IHandler<U> FirstThenSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_forAppends[
                        (Method.FirstThenSecond, In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> PackFirstInSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_forAppends[
                        (Method.PackFirstInSecond, In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> InjectFirstIntoSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_forAppends[
                        (Method.InjectFirstIntoSecond, In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> FirstCoverSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_forAppends[
                        (Method.FirstCoverSecond, In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> FirstWrapSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_forAppends[
                        (Method.FirstWrapSecond, In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> JoinFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_forAppends[
                        (Method.JoinFirstWithSecond, In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> MergeFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers.Get(_forAppends[
                        (Method.MergeFirstWithSecond, In(a).Index, In(b).Index)
                        ]));

            public IHandler<U> Atomize<U>(
                IHandler<U> handler) =>
                    Out<U>(handlers.Get(_forAtomize[
                        In(handler).Index
                        ]));

            public IHandler<U> Conditional<U>(
                IHandler<U> handler, ICondition<U> condition) =>
                    Out<U>(handlers.Get(_forConditional[
                        (In(handler).Index, In(condition).Index)
                        ]));
        }
    }
}
