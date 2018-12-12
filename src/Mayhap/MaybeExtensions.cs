namespace Mayhap
{
    public static class MaybeExtensions
    {
        /// <summary>
        /// Creates a failed Maybe, with passed string as error message.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="failure">Error message</param>
        /// <returns>Failed Maybe</returns>
        public static Maybe<TValue> Fail<TValue>(this string failure)
        {
            return new Maybe<TValue>(default, failure);
        }

        /// <summary>
        /// Creates a successful Maybe of TValue, with Value equal to passed value.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="data">Value object</param>
        /// <returns>Successful Maybe</returns>
        public static Maybe<TValue> Success<TValue>(this TValue data)
        {
            return new Maybe<TValue>(data, default);
        }
    }
}