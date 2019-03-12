namespace Mayhap.Option
{
    /// <summary>
    /// Represents an existing instance of the wrapped type.
    /// </summary>
    /// <typeparam name="T">The wrapped type.</typeparam>
    public struct Some<T> : IOption<T>
    {
        /// <summary>
        /// Creates an instance of <see cref="Some{T}"/>.
        /// </summary>
        /// <param name="value">The wrapped instance.</param>
        public Some(T value) => Value = value;

        /// <summary>
        /// Gets the wrapped instance.
        /// </summary>
        /// <value>
        /// The wrapped instance.
        /// </value>
        public T Value { get; }

        /// <summary>
        /// Gets the HasValue value.
        /// </summary>
        /// <value>
        /// Indicates if the wrapped type instance exists.
        /// </value>
        public bool HasValue => true;

        /// <summary>
        /// Unwraps the wrapped instance value.
        /// </summary>
        /// <returns>The wrapped instance.</returns>
        public T Unwrap() => Value;

        /// <summary>
        /// Returns a string representation of current instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"Some(Of {typeof(T).Name}{{ Value = {Value} }})";

        /// <summary>
        /// Implicitly casts to wrapped type.
        /// </summary>
        /// <param name="operand">The operand.</param>
        public static implicit operator T(Some<T> operand) => operand.Value;
    }
}