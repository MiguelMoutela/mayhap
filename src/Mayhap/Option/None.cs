namespace Mayhap.Option
{
    /// <summary>
    /// Represents the absence of the wrapped type instance.
    /// </summary>
    /// <typeparam name="T">The wrapped type.</typeparam>
    public struct None<T> : IOption<T>
    {
        /// <summary>
        /// Gets the HasValue value.
        /// </summary>
        /// <value>
        /// Indicates if the wrapped type instance exists.
        /// </value>
        public bool HasValue => false;

        /// <summary>
        /// Unwraps the wrapped instance value.
        /// </summary>
        /// <returns>The wrapped instance.</returns>
        public T Unwrap() => default;

        /// <summary>
        /// Returns a string representation of current instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"None(Of {typeof(T).Name})";

        /// <summary>
        /// Implicitly casts to wrapped type.
        /// </summary>
        /// <param name="operand">The operand.</param>
        public static implicit operator T(None<T> operand) => default;
    }
}