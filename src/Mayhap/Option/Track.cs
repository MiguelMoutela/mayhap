using System;
using System.Threading.Tasks;

namespace Mayhap.Option
{
    /// <summary>
    /// Option domain track
    /// </summary>
    public static class Track
    {
        /// <summary>
        /// Maps option of TPrev to option of TNext
        /// </summary>
        /// <typeparam name="TPrev">Previous wrapped type.</typeparam>
        /// <typeparam name="TNext">Next wrapped type.</typeparam>
        /// <param name="prev">Previous wrapped object.</param>
        /// <param name="functor">Mapping functor.</param>
        /// <returns>Option of TNext</returns>
        public static IOption<TNext> Map<TPrev, TNext>(this IOption<TPrev> prev, Func<TPrev, IOption<TNext>> functor)
        {
            switch (prev)
            {
                case Some<TPrev> some:
                    return functor.Invoke(some);
                default:
                    return Optional.None<TNext>();
            }
        }

        /// <summary>
        /// Maps option of TPrev to option of TNext
        /// </summary>
        /// <typeparam name="TPrev">Previous wrapped type.</typeparam>
        /// <typeparam name="TNext">Next wrapped type.</typeparam>
        /// <param name="prev">Previous wrapped object.</param>
        /// <param name="functor">Mapping functor.</param>
        /// <param name="next">Option of TNext out parameter.</param>
        /// <returns>Option of TNext.</returns>        
        public static IOption<TNext> Map<TPrev, TNext>(this IOption<TPrev> prev, Func<TPrev, IOption<TNext>> functor, out IOption<TNext> next)
            => next = prev.Map(functor);

        /// <summary>
        /// Maps option of TPrev to option of TNext
        /// </summary>
        /// <typeparam name="TPrev">Previous wrapped type.</typeparam>
        /// <typeparam name="TNext">Next wrapped type.</typeparam>
        /// <param name="prev">Previous wrapped object.</param>
        /// <param name="functor">Mapping functor.</param>
        /// <returns>Awaitable task of option of TNext.</returns>     
        public static Task<IOption<TNext>> Map<TPrev, TNext>(this IOption<TPrev> prev, Func<TPrev, Task<IOption<TNext>>> functor)
        {
            switch (prev)
            {
                case Some<TPrev> some:
                    return functor(some);
                default:
                    return Task.FromResult((IOption<TNext>) Optional.None<TNext>());
            }
        }

        /// <summary>
        /// Maps option of TPrev to option of TNext
        /// </summary>
        /// <typeparam name="TPrev">Previous wrapped type.</typeparam>
        /// <typeparam name="TNext">Next wrapped type.</typeparam>
        /// <param name="prev">Previous wrapped object.</param>
        /// <param name="functor">Mapping functor.</param>
        /// <param name="next">Awaitable task of option of TNext out parameter.</param>
        /// <returns>Awaitable task of option of TNext.</returns>
        public static Task<IOption<TNext>> Map<TPrev, TNext>(this IOption<TPrev> prev, Func<TPrev, Task<IOption<TNext>>> functor, out Task<IOption<TNext>> next)
            => next = prev.Map(functor);
    }
}