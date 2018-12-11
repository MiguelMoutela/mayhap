using System;
using System.Diagnostics.Contracts;

namespace Mayhap
{
    public readonly struct Maybe<TValue>
    {
        private readonly Result _result;

        internal Maybe(TValue value, string failure)
        {
            Value = value;
            _result = new Result(failure);
        }

        internal Maybe(in Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Conversion is possible only for failed Maybe");
            }

            Value = default;
            _result = result;
        }

        public TValue Value { get; }

        public bool IsSuccess => _result.IsSuccess;

        public string Error => _result.Error;

        [Pure]
        public Maybe<T> To<T>() => new Maybe<T>(in _result);

        public static implicit operator bool(Maybe<TValue> r) => r._result.IsSuccess;

        public static implicit operator TValue(Maybe<TValue> r) => r.Value;

        [Pure]
        public override string ToString() =>
            $"{GetType().Name}{{IsSuccess: {IsSuccess}, {(IsSuccess ? "Value: " + Value : "Error: " + Error)}}}";
    }
}