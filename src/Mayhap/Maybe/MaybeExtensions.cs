using System;
using Mayhap.Error;
using Mayhap.Option;

namespace Mayhap.Maybe
{
    public static class MaybeExtensions
    {
        /// <summary>
        /// Creates a failed Maybe, with passed string as error message.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="failure">Error</param>
        /// <returns>Failed Maybe</returns>
        public static Maybe<TValue> Fail<TValue>(this IProblem failure)
            => new Maybe<TValue>(failure.Some(), Optional.None<TValue>());

        /// <summary>
        /// Creates a failed Maybe, with passed string as error message.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="failure">Error type</param>
        /// <returns>Failed Maybe</returns>
        public static Maybe<TValue> Fail<TValue>(this string failure)
            => new Maybe<TValue>(Problem.New().WithType(failure).Create().Some<IProblem>(), Optional.None<TValue>());

        /// <summary>
        /// Creates a failed Maybe, with passed string as error message.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="failure">Error type</param>
        /// <returns>Failed Maybe</returns>
        public static Maybe<TValue> Fail<TValue>(this Enum failure)
            => new Maybe<TValue>(Problem.OfType(failure).Create().Some<IProblem>(), Optional.None<TValue>());

        /// <summary>
        /// Creates a successful Maybe of TValue, with Value equal to passed value.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="data">Value object</param>
        /// <returns>Successful Maybe</returns>
        public static Maybe<TValue> Success<TValue>(this TValue data)
            => new Maybe<TValue>(Optional.None<IProblem>(), data.Some());
    }
}