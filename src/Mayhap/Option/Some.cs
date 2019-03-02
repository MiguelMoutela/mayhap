namespace Mayhap.Option
{
    /// <summary>
    /// Represents existence of value of type T.
    /// </summary>
    /// <typeparam name="T">The wrapped type</typeparam>
    public struct Some<T> : IOption<T>
    {
        /// <summary>
        /// Creates an instance of Some&lt;T&gt;
        /// </summary>
        /// <param name="value">The value</param>
        public Some(T value) => Value = value;

        /// <summary>
        /// The wrapped object
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Indicates if wrapped object exists.
        /// </summary>
        public bool HasValue => true;
    }
}