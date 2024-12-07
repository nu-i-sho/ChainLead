namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using System;

    using Method = Common.Notation.HandlerMath;
    
    public static partial class Dummy
    {
        public class SingleTypeHandlerMath<T>(
                HandlerCollection<T> handlers) 
            : IHandlerMath
        {
            Dictionary<(string, HandlerIndex, HandlerIndex), HandlerIndex> _forAppends = new();
            Dictionary<(HandlerIndex, ConditionIndex), HandlerIndex> _forConditional = new();
            Dictionary<HandlerIndex, HandlerIndex> _forAtomize = new();
            HandlerIndex? _forMakeHandler;
            HandlerIndex? _forZero;

            public ReturnsStep SetZero =>
                new(x => _forZero = x);

            public ReturnsStep SetMakeHandler =>
                new(x => _forMakeHandler = x);

            public ReturnsStep SetFirstThenSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.FirstThenSecond, a, b), x));

            public ReturnsStep SetPackFirstInSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.PackFirstInSecond, a, b), x));

            public ReturnsStep SetInjectFirstIntoSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.InjectFirstIntoSecond, a, b), x));

            public ReturnsStep SetFirstCoverSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.FirstCoverSecond, a, b), x));

            public ReturnsStep SetFirstWrapSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.FirstWrapSecond, a, b), x));

            public ReturnsStep SetJoinFirstWithSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.JoinFirstWithSecond, a, b), x));

            public ReturnsStep SetMergeFirstWithSecond(HandlerIndex a, HandlerIndex b) =>
                new(x => _forAppends.Add((Method.MergeFirstWithSecond, a, b), x));

            public ReturnsStep SetConditional(HandlerIndex handler, ConditionIndex condition) =>
                new(x => _forConditional.Add((handler, condition), x));

            public ReturnsStep SetAtomize(HandlerIndex handler) =>
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
                Out<U>(handlers[_forZero!]);

            public bool IsZero<U>(IHandler<U> handler) =>
                handlers[_forZero!].Index == In(handler).Index;

            public IHandler<U> MakeHandler<U>(Action<U> action)
            {
                var h = handlers[_forMakeHandler!];
                h.SetImplementation((Action<T>)(object)action);

                return Out<U>(h);
            }

            public IHandler<U> FirstThenSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_forAppends[
                        (Method.FirstThenSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> PackFirstInSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_forAppends[
                        (Method.PackFirstInSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> InjectFirstIntoSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_forAppends[
                        (Method.InjectFirstIntoSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> FirstCoverSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_forAppends[
                        (Method.FirstCoverSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> FirstWrapSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_forAppends[
                        (Method.FirstWrapSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> JoinFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_forAppends[
                        (Method.JoinFirstWithSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> MergeFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_forAppends[
                        (Method.MergeFirstWithSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> Atomize<U>(
                IHandler<U> handler) =>
                    Out<U>(handlers[_forAtomize[
                        In(handler).Index
                        ]]);

            public IHandler<U> Conditional<U>(
                IHandler<U> handler, ICondition<U> condition) =>
                    Out<U>(handlers[_forConditional[
                        (In(handler).Index, In(condition).Index)
                        ]]);
        }
    }
}
