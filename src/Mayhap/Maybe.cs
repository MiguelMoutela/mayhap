﻿using System;

namespace Mayhap
{
    /// <summary>
    /// A structure representing possible value of TValue.
    /// Implicitly casts to <see cref="bool"/> representing success value.
    /// Implicitly casts to TValue.
    /// </summary>
    /// <typeparam name="TValue">Wrapped value type</typeparam>
    public readonly struct Maybe<TValue>
    {
        private readonly Result _result;

        internal Maybe(TValue value, string failure)
        {
            Value = value;
            _result = new Result(failure);
        }

        private Maybe(in Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Conversion is possible only for failed Maybe");
            }

            Value = default;
            _result = result;
        }

        /// <summary>
        /// Gets the wrapped value
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Gets success value
        /// </summary>
        public bool IsSuccess => _result.IsSuccess;

        /// <summary>
        /// Gets error message. Null if is successful.
        /// </summary>
        public string Error => _result.Error;

        /// <summary>
        /// Cast to Maybe of T.
        /// Should be used only for failed Maybe.
        /// </summary>
        /// <typeparam name="T">New wrapped type</typeparam>
        /// <returns></returns>
        public Maybe<T> To<T>() => new Maybe<T>(in _result);

        /// <summary>
        /// To bool implicit cast operator.
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator bool(Maybe<TValue> r) => r._result.IsSuccess;

        /// <summary>
        /// To TValue implicit cast operator.
        /// </summary>
        /// <param name="r">Maybe</param>
        public static implicit operator TValue(Maybe<TValue> r) => r.Value;

        /// <summary>
        /// Converts to string
        /// </summary>
        /// <returns>Str</returns>
        public override string ToString() =>
            $"{GetType().Name}{{IsSuccess: {IsSuccess}, {(IsSuccess ? "Value: " + Value : "Error: " + Error)}}}";
    }
}