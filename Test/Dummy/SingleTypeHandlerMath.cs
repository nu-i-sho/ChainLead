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
            readonly InternalSetupBuilder _setup = new();

            public SetupBuilder Setup => _setup;

            public class SetupBuilder
            {
                protected Dictionary<(string, HandlerIndex, HandlerIndex), HandlerIndex> _forAppends = new();
                protected Dictionary<(HandlerIndex, ConditionIndex), HandlerIndex> _forConditional = new();
                protected Dictionary<HandlerIndex, HandlerIndex> _forAtomize = new();
                protected HandlerIndex? _forMakeHandler;
                protected HandlerIndex? _forZero;

                public void Zero(HandlerIndex handler) =>
                    _forZero = handler;

                public void MakeHandler(HandlerIndex handler) =>
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
            }

            class InternalSetupBuilder : SetupBuilder
            {
                public Dictionary<(string, HandlerIndex, HandlerIndex), HandlerIndex> ForAppends => _forAppends;
                public Dictionary<(HandlerIndex, ConditionIndex), HandlerIndex> ForConditional => _forConditional;
                public Dictionary<HandlerIndex, HandlerIndex> ForAtomize => _forAtomize;
                public HandlerIndex? ForMakeHandler => _forMakeHandler;
                public HandlerIndex? ForZero => _forZero;
            }

            static Handler<T> In<U>(IHandler<U> x) => (Handler<T>)x;

            static Condition<T> In<U>(ICondition<U> x) => (Condition<T>)x;

            static IHandler<U> Out<U>(IHandler<T> x) => (IHandler<U>)x;

            public IHandler<U> Zero<U>() =>
                Out<U>(handlers[_setup.ForZero!]);

            public bool IsZero<U>(IHandler<U> handler) =>
                handlers[_setup.ForZero!].Index == In(handler).Index;

            public IHandler<U> MakeHandler<U>(Action<U> action)
            {
                var h = handlers[_setup.ForMakeHandler!];
                h.SetImplementation((Action<T>)(object)action);

                return Out<U>(h);
            }

            public IHandler<U> FirstThenSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_setup.ForAppends[
                        (Method.FirstThenSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> PackFirstInSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_setup.ForAppends[
                        (Method.PackFirstInSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> InjectFirstIntoSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_setup.ForAppends[
                        (Method.InjectFirstIntoSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> FirstCoverSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_setup.ForAppends[
                        (Method.FirstCoverSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> FirstWrapSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_setup.ForAppends[
                        (Method.FirstWrapSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> JoinFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_setup.ForAppends[
                        (Method.JoinFirstWithSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> MergeFirstWithSecond<U>(
                IHandler<U> a, IHandler<U> b) =>
                    Out<U>(handlers[_setup.ForAppends[
                        (Method.MergeFirstWithSecond, In(a).Index, In(b).Index)
                        ]]);

            public IHandler<U> Atomize<U>(IHandler<U> handler) =>
                Out<U>(handlers[_setup.ForAtomize[In(handler).Index]]);

            public IHandler<U> Conditional<U>(
                IHandler<U> handler, ICondition<U> condition) =>
                    Out<U>(handlers[_setup.ForConditional[
                        (In(handler).Index, In(condition).Index)
                        ]]);
        }
    }
}
