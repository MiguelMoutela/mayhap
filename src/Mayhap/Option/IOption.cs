namespace Mayhap.Option
{
    /// <summary>
    /// Represents an optional occurence of the wrapped type instance.
    /// </summary>
    /// <typeparam name="T">The wrapped type.</typeparam>
    public interface IOption<out T>
    {
        /// <summary>
        /// Gets the HasValue value.
        /// </summary>
        /// <value>
        /// Indicates if the wrapped type instance exists.
        /// </value>
        bool HasValue { get; }

        /// <summary>
        /// Unwraps the wrapped instance value.
        /// </summary>
        /// <returns>The wrapped instance.</returns>
        T Unwrap();
    }
}