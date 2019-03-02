using System;
using Mayhap.Error;

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
        {
            return new Maybe<TValue>(default, failure);
        }

        /// <summary>
        /// Creates a failed Maybe, with passed string as error message.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="failure">Error type</param>
        /// <returns>Failed Maybe</returns>
        public static Maybe<TValue> Fail<TValue>(this string failure)
        {
            return new Maybe<TValue>(default, Problem.New().WithType(failure).Create());
        }

        /// <summary>
        /// Creates a failed Maybe, with passed string as error message.
        /// </summary>
        /// <typeparam name="TValue">Wrapped value type</typeparam>
        /// <param name="failure">Error type</param>
        /// <returns>Failed Maybe</returns>
        public static Maybe<TValue> Fail<TValue>(this Enum failure)
        {
            return new Maybe<TValue>(default, Problem.OfType(failure).Create());
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