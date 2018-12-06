using System.Diagnostics.Contracts;

namespace Maybe
{
    public readonly struct Maybe
    {
        internal Maybe(string failure) => Error = failure;

        public string Error { get; }

        public bool IsSuccess => Error == null;

        public static Maybe Success() => new Maybe(null);

        [Pure]
        public Maybe<T> Fail<T>() => new Maybe<T>(in this);

        public static implicit operator bool(Maybe r) => r.IsSuccess;

        [Pure]
        public override string ToString() => $"{nameof(Maybe)}{{{(IsSuccess ? "Success" : "Failure")}{(IsSuccess ? "" : ": " + Error)}}}";
    }

    public readonly struct Maybe<TData>
    {
        internal Maybe(TData data, string failure)
        {
            Data = data;
            Inner = new Maybe(failure);
        }

        internal Maybe(in Maybe inner)
        {
            Data = default;
            Inner = inner;
        }

        public TData Data { get; }

        private Maybe Inner { get; }

        public void Deconstruct(out TData data, out string failure)
        {
            data = Data;
            failure = Inner.Error;
        }

        [Pure]
        public Maybe<T> Fail<T>() => Inner.Fail<T>();

        public static implicit operator bool(Maybe<TData> r) => r.Inner.IsSuccess;

        public static implicit operator TData(Maybe<TData> r) => r.Data;

        public static implicit operator Maybe(Maybe<TData> r)
            => r.Inner;

        [Pure]
        public override string ToString() =>
            $"{nameof(Maybe)}{{{(Inner.IsSuccess ? "Success" : "Failure")}: {(Inner.IsSuccess ? Data.ToString() : Inner.Error)}}}";
    }
}
