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
    }
}