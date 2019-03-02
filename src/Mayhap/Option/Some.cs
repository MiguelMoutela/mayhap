namespace Mayhap.Option
{
    /// <summary>
    /// Represents existence of value of type T.
    /// </summary>
    /// <typeparam name="T">The wrapped type</typeparam>
    public struct Some<T> : IOption<T>
    {
        public Some(T value) => Value = value;

        /// <summary>
        /// The wrapped object
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Indicates if wrapped object exists.
        /// </summary>
        public bool HasValue => true;

        /// <summary>
        /// Unwraps the value.
        /// </summary>
        /// <returns>The value.</returns>
        public T Unwrap() => Value;

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"Some(Of {typeof(T).Name}{{ Value = {Value} }})";

        /// <summary>
        /// Implicit cast to wrapped type.
        /// </summary>
        /// <param name="s">Operand</param>
        public static implicit operator T(Some<T> s) => s.Value;
    }
}