namespace Mayhap.Option
{
    /// <summary>
    /// The option wrapper of type T. If the wrapped object has value it is Some&lt;T&gt;, while when it's empty - None&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">The wrapped type</typeparam>
    public interface IOption<T>
    {
        /// <summary>
        /// Indicates if wrapped object exists.
        /// </summary>
        bool HasValue { get; }
    }
}