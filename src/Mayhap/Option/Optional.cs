namespace Mayhap.Option
{
    /// <summary>
    /// The option construction method set.
    /// </summary>
    public static class Optional
    {
        /// <summary>
        /// Creates None&lt;TValue&gt; instance.
        /// </summary>
        /// <typeparam name="TValue">The wrapped value.</typeparam>
        /// <returns>None of TValue</returns>
        public static None<TValue> None<TValue>() => new None<TValue>();

        /// <summary>
        /// Creates Some&lt;TValue&gt; instance.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns>Some of TValue</returns>
        public static Some<TValue> Some<TValue>(this TValue value) => new Some<TValue>(value);
    }
}