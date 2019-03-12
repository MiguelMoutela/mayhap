using System;
using Mayhap.Error;
using Mayhap.Option;

namespace Mayhap.Maybe
{
    /// <summary>
    /// The maybe model factory methods container.
    /// </summary>
    public static class MaybeFactory
    {
        /// <summary>
        /// Creates a failed <see cref="Maybe{TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The wrapped type.</typeparam>
        /// <param name="error">The error instance.</param>
        /// <returns>The maybe instance.</returns>
        public static Maybe<TValue> Fail<TValue>(this IProblem error)
            => new Maybe<TValue>(error.Some(), Optional.None<TValue>());

        /// <summary>
        /// Creates a failed <see cref="Maybe{TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The wrapped type.</typeparam>
        /// <param name="error">The error type string.</param>
        /// <returns>The maybe instance.</returns>
        public static Maybe<TValue> Fail<TValue>(this string error)
            => new Maybe<TValue>(Problem.New().WithType(error).Create().Some<IProblem>(), Optional.None<TValue>());

        /// <summary>
        /// Creates a failed <see cref="Maybe{TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The wrapped type.</typeparam>
        /// <param name="error">The error type enum.</param>
        /// <returns>The maybe instance.</returns>
        public static Maybe<TValue> Fail<TValue>(this Enum error)
            => new Maybe<TValue>(Problem.OfType(error).Create().Some<IProblem>(), Optional.None<TValue>());

        /// <summary>
        /// Creates a successful <see cref="Maybe{TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The wrapped type.</typeparam>
        /// <param name="data">The instance of wrapped type.</param>
        /// <returns>The maybe instance.</returns>
        public static Maybe<TValue> Success<TValue>(this TValue data)
            => new Maybe<TValue>(Optional.None<IProblem>(), data.Some());
    }
}