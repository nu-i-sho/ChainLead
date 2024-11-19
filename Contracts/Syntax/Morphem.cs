namespace ChainLead.Contracts.Syntax
{
    public static class Morphem
    {
        public interface IReverseCall<T>
        {
            IHandler<T> WhereXIs(IHandler<T> x);
        }

        public interface IThenReverseCall<T> : IReverseCall<T>
        {
            IThenReverseCall<T> Then(IHandler<T> next);
        }

        public interface IIn<T>
        {
            IThenIn<T> In(IHandler<T> next);
        }

        public interface IThenIn<T> : IExtendedHandler<T>
        {
            IThenIn<T> ThenIn(IHandler<T> next);
        }

        public interface IThenInReverseCall<T> : IReverseCall<T>
        {
            IThenInReverseCall<T> ThenIn(IHandler<T> next);
        }

        public interface IInto<T>
        {
            IThenInto<T> Into(IHandler<T> next);
        }

        public interface IThenInto<T> : IExtendedHandler<T>
        {
            IThenInto<T> ThenInto(IHandler<T> next);
        }

        public interface IThenIntoReverseCall<T> : IReverseCall<T>
        {
            IThenIntoReverseCall<T> ThenInto(IHandler<T> next);
        }

        public interface IToCoverOrWrap<T>
        {
            IThenCover<T> ToCover(IHandler<T> next);

            IThenWrap<T> ToWrap(IHandler<T> next);
        }

        public interface IThenCover<T> : IExtendedHandler<T>
        {
            IThenCover<T> ThenCover(IHandler<T> next);
        }

        public interface IThenWrap<T> : IExtendedHandler<T>
        {
            IThenWrap<T> ThenWrap(IHandler<T> next);
        }

        public interface IThenCoverReverseCall<T>
            : IReverseCall<T>
        {
            IThenCoverReverseCall<T> ThenCover(IHandler<T> next);
        }

        public interface IThenWrapReverseCall<T>
            : IReverseCall<T>
        {
            IThenWrapReverseCall<T> ThenWrap(IHandler<T> next);
        }

        public interface IWith<T>
        {
            IThenWith<T> With(IHandler<T> next);
        }

        public interface IThenWith<T> : IExtendedHandler<T>
        {
            IThenWith<T> ThenWith(IHandler<T> next);
        }

        public interface IThenWithReverseCall<T>
            : IReverseCall<T>
        {
            IThenWithReverseCall<T> ThenWith(IHandler<T> next);
        }
    }
}
