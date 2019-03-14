using Mayhap.Error;
using Mayhap.Option;

namespace Mayhap.Maybe
{
    /// <summary>
    /// Represents a failure aware result of an operation.
    /// </summary>
    /// <typeparam name="TValue">The wrapped type.</typeparam>
    public readonly struct Maybe<TValue>
    {
        internal Maybe(IOption<Problem> error, IOption<TValue> value)
        {
            Error = error;
            Value = value;
        }

        /// <summary>
        /// Gets the wrapped value option.
        /// </summary>
        /// <value>
        /// The wrapped value option.
        /// </value>
        public IOption<TValue> Value { get; }

        /// <summary>
        /// Gets the success value
        /// </summary>
        /// <value>
        /// Indicates if the current instance is successful.
        /// </value>
        public bool IsSuccessful => !Error.HasValue;

        /// <summary>
        /// Gets the error option.
        /// </summary>
        /// <value>
        /// Contains the error optional instance.
        /// </value>
        public IOption<Problem> Error { get; }

        /// <summary>
        /// Implicitly casts the instance to the <see cref="bool"/> type. The result represents instances success flag.
        /// </summary>
        /// <param name="operand">The operand.</param>
        /// <returns>The <see cref="IsSuccessful"/> value.</returns>
        public static implicit operator bool(Maybe<TValue> operand) => operand.IsSuccessful;

        /// <summary>
        /// Implicitly casts the instance to the wrapped type. The result represents the unwrapped value of the wrapped type.
        /// In case of underlying <see cref="None{TValue}"/> the default value of the wrapped type will be returned.
        /// </summary>
        /// <param name="operand">The operand.</param>
        /// <returns>The unwrapped instance of the wrapped type.</returns>
        public static implicit operator TValue(Maybe<TValue> operand) => operand.Value.Unwrap();

        /// <summary>
        /// Returns a string representation of current instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() =>
            $"{GetType().Name}{{IsSuccess: {IsSuccessful}, {(IsSuccessful ? "Value: " + Value : "Error: " + Error)}}}";
    }
}