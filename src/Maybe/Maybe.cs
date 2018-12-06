namespace Maybe
{
    public readonly struct Maybe
    {
        internal Maybe(string failure) => Error = failure;

        public string Error { get; }

        public bool IsSuccess => Error == null;

        public static Maybe Success() => new Maybe(null);

        public Maybe<T> Fail<T>() => Error.Fail<T>();

        public static implicit operator bool(Maybe r) => r.IsSuccess;

        public override string ToString() => $"{nameof(Maybe)}{{{(IsSuccess ? "Success" : "Failure")}{(IsSuccess ? "" : ": " + Error)}}}";
    }

    public readonly struct Maybe<TData>
    {
        internal Maybe(TData data, string failure)
        {
            Data = data;
            Error = failure;
        }

        public TData Data { get; }

        public string Error { get; }

        public bool IsSuccess => Error == null;

        public void Deconstruct(out TData data, out string failure)
        {
            data = Data;
            failure = Error;
        }

        public Maybe<T> Fail<T>() => Error.Fail<T>();

        public static implicit operator bool(Maybe<TData> r) => r.IsSuccess;

        public static implicit operator TData(Maybe<TData> r) => r.Data;

        public static implicit operator Maybe(Maybe<TData> r)
            => r ? Maybe.Success() : r.Error.Fail();

        public override string ToString() =>
            $"{nameof(Maybe)}{{{(IsSuccess ? "Success" : "Failure")}: {(IsSuccess ? Data.ToString() : Error)}}}";
    }
}
