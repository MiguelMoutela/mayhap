using System;
using System.Threading.Tasks;

namespace Mayhap.Maybe
{
    /// <summary>
    /// The maybe domain track. Contains useful chaining extension methods for <see cref="Maybe{TValue}"/> type.
    ///
    /// See also: <seealso cref="Maybe{TValue}"/>
    /// </summary>
    public static class Track
    {
        /// <summary>
        /// Applies the mapping functor to the input if it is successful.
        /// </summary>
        /// <typeparam name="TInput">The wrapped input type.</typeparam>
        /// <typeparam name="TOutput">The wrapped output type.</typeparam>
        /// <param name="input">The wrapped input instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <returns>The output maybe instance.</returns>
        public static Maybe<TOutput> Map<TInput, TOutput>(
            this Maybe<TInput> input,
            Func<TInput, Maybe<TOutput>> functor)
            => input ? functor(input) : input.Error.Unwrap().Fail<TOutput>();

        /// <summary>
        /// Applies the mapping functor to the input if it is successful.
        /// </summary>
        /// <typeparam name="TInput">The wrapped input type.</typeparam>
        /// <typeparam name="TOutput">The wrapped output type.</typeparam>
        /// <param name="input">The wrapped input instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <returns>An awaitable task of the output maybe instance.</returns>
        public static Task<Maybe<TOutput>> Map<TInput, TOutput>(
            this Maybe<TInput> input, 
            Func<TInput, Task<Maybe<TOutput>>> functor)
            => input ? functor(input) : Task.FromResult(input.Error.Unwrap().Fail<TOutput>());

        /// <summary>
        /// Applies the mapping functor to the input if it is successful.
        /// </summary>
        /// <typeparam name="TInput">The wrapped input type.</typeparam>
        /// <typeparam name="TOutput">The wrapped output type.</typeparam>
        /// <param name="input">The wrapped input instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <param name="output">The output maybe instance, same as the returned result.</param>
        /// <returns>The output maybe instance.</returns>
        public static Maybe<TOutput> Map<TInput, TOutput>(
            this Maybe<TInput> input,
            Func<TInput, Maybe<TOutput>> functor,
            out Maybe<TOutput> output)
            => output = Map(input, functor);

        /// <summary>
        /// Applies the mapping functor to the input if it is successful.
        /// </summary>
        /// <typeparam name="TInput">The wrapped input type.</typeparam>
        /// <typeparam name="TOutput">The wrapped output type.</typeparam>
        /// <param name="input">The wrapped input instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <param name="output">The output maybe instance, same as the returned result.</param>
        /// <returns>An awaitable task of the output maybe instance.</returns>
        public static Task<Maybe<TOutput>> Map<TInput, TOutput>(
            this Maybe<TInput> input,
            Func<TInput, Task<Maybe<TOutput>>> functor,
            out Task<Maybe<TOutput>> output)
            => output = Map(input, functor);
    }
}