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
    }
}