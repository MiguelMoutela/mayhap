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
        public override string ToString() => $"{nameof(Maybe)}{{IsSuccess: {IsSuccess}{(IsSuccess ? "" : ", Error: " + Error)}}}";
    }

    public readonly struct Maybe<TValue>
    {
        internal Maybe(TValue value, string failure)
        {
            Value = value;
            Inner = new Maybe(failure);
        }

        internal Maybe(in Maybe inner)
        {
            Value = default;
            Inner = inner;
        }

        public TValue Value { get; }

        public bool IsSuccess => Inner.IsSuccess;

        public string Error => Inner.Error;

        private Maybe Inner { get; }

        [Pure]
        public Maybe<T> Fail<T>() => Inner.Fail<T>();

        public static implicit operator bool(Maybe<TValue> r) => r.Inner.IsSuccess;

        public static implicit operator TValue(Maybe<TValue> r) => r.Value;

        public static implicit operator Maybe(Maybe<TValue> r)
            => r.Inner;

        [Pure]
        public override string ToString() =>
            $"{GetType().Name}{{IsSuccess: {IsSuccess}, {(IsSuccess ? "Value: " + Value.ToString() : "Error: " + Error)}}}";
    }
}
