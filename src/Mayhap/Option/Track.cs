using System;
using System.Threading.Tasks;

namespace Mayhap.Option
{
    /// <summary>
    /// The option domain track. Contains useful chaining extension methods for <see cref="IOption{T}"/> type.
    ///
    /// See also:
    /// <seealso cref="IOption{T}"/>,
    /// <seealso cref="Some{T}"/>,
    /// <seealso cref="None{T}"/>
    /// </summary>
    public static class Track
    {
        /// <summary>
        /// Applies the mapping functor the input if it is <see cref="Some{T}"/>.
        /// </summary>
        /// <typeparam name="TInput">The input type.</typeparam>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="input">The input option instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <returns>The output option instance.</returns>
        public static IOption<TOutput> Map<TInput, TOutput>(this IOption<TInput> input, Func<TInput, IOption<TOutput>> functor)
        {
            switch (input)
            {
                case Some<TInput> some:
                    return functor.Invoke(some);
                default:
                    return Optional.None<TOutput>();
            }
        }

        /// <summary>
        /// Applies the mapping functor the input if it is <see cref="Some{T}"/>.
        /// </summary>
        /// <typeparam name="TInput">The input type.</typeparam>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="input">The input option instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <param name="output">The output option instance, same as the returned result.</param>
        /// <returns>The output option instance.</returns>
        public static IOption<TOutput> Map<TInput, TOutput>(this IOption<TInput> input, Func<TInput, IOption<TOutput>> functor, out IOption<TOutput> output)
            => output = input.Map(functor);

        /// <summary>
        /// Applies the mapping functor the input if it is <see cref="Some{T}"/>.
        /// </summary>
        /// <typeparam name="TInput">The input type.</typeparam>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="input">The input option instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <returns>An awaitable task of the output option instance.</returns>
        public static Task<IOption<TOutput>> Map<TInput, TOutput>(this IOption<TInput> input, Func<TInput, Task<IOption<TOutput>>> functor)
        {
            switch (input)
            {
                case Some<TInput> some:
                    return functor(some);
                default:
                    return Task.FromResult((IOption<TOutput>) Optional.None<TOutput>());
            }
        }

        /// <summary>
        /// Applies the mapping functor the input if it is <see cref="Some{T}"/>.
        /// </summary>
        /// <typeparam name="TInput">The input type.</typeparam>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="input">The input option instance.</param>
        /// <param name="functor">The mapping functor.</param>
        /// <param name="output">The output option instance, same as the returned result.</param>
        /// <returns>An awaitable task of the output option instance.</returns>
        public static Task<IOption<TOutput>> Map<TInput, TOutput>(this IOption<TInput> input, Func<TInput, Task<IOption<TOutput>>> functor, out Task<IOption<TOutput>> output)
            => output = input.Map(functor);
    }
}