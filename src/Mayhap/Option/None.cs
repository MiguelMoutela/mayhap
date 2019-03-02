namespace Mayhap.Option
{
    /// <summary>
    /// Represents absence of value of type T.
    /// </summary>
    /// <typeparam name="T">Wrapped type</typeparam>
    public struct None<T> : IOption<T>
    {
        /// <summary>
        /// Indicates if wrapped object exists.
        /// </summary>
        public bool HasValue => false;

        /// <summary>
        /// Unwraps the value.
        /// </summary>
        /// <returns>The value.</returns>
        public T Unwrap() => default;

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"None(Of {typeof(T).Name})";

        /// <summary>
        /// Implicit cast to wrapped type.
        /// </summary>
        /// <param name="s">Operand</param>
        public static implicit operator T(None<T> s) => default;
    }
}