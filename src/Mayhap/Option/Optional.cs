namespace Mayhap.Option
{
    /// <summary>
    /// The option model factory methods container.
    /// </summary>
    public static class Optional
    {
        /// <summary>
        /// Creates an instance of <see cref="None{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">The wrapped type.</typeparam>
        /// <returns>The option instance.</returns>
        public static None<TValue> None<TValue>() => new None<TValue>();

        /// <summary>
        /// Creates an instance of <see cref="Some{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">The wrapped type.</typeparam>
        /// <param name="value">The wrapped type instance.</param>
        /// <returns>The option instance.</returns>
        public static Some<TValue> Some<TValue>(this TValue value) => new Some<TValue>(value);
    }
}