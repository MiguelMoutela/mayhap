using Mayhap.Option;

namespace Mayhap.Maybe
{
    /// <summary>
    /// A structure representing possible value of TValue.
    /// Implicitly casts to <see cref="bool"/> representing success value.
    /// Implicitly casts to TValue.
    /// </summary>
    /// <typeparam name="TValue">Wrapped value type</typeparam>
    public readonly struct Maybe<TValue>
    {
        internal Maybe(IOption<IProblem> error, IOption<TValue> value)
        {
            Error = error;
            Value = value;
        }

        /// <summary>
        /// Gets the wrapped value
        /// </summary>
        public IOption<TValue> Value { get; }

        /// <summary>
        /// Gets the success value
        /// </summary>
        public bool IsSuccessful => !Error.HasValue;

        /// <summary>
        /// Gets error message. Null if is successful.
        /// </summary>
        public IOption<IProblem> Error { get; }

        /// <summary>
        /// To bool implicit cast operator.
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator bool(Maybe<TValue> r) => r.IsSuccessful;

        /// <summary>
        /// To TValue implicit cast operator.
        /// </summary>
        /// <param name="r">Maybe</param>
        public static implicit operator TValue(Maybe<TValue> r) => r.Value.Unwrap();

        /// <summary>
        /// Converts to string
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString() =>
            $"{GetType().Name}{{IsSuccess: {IsSuccessful}, {(IsSuccessful ? "Value: " + Value : "Error: " + Error)}}}";
    }
}