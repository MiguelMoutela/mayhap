using System;
using System.Threading.Tasks;

namespace Maybe
{
    public class Track
    {
        public static Maybe<T> Continue<T>(Maybe previous, Func<Maybe<T>> next)
            => previous.IsSuccess ? next() : previous.Fail<T>();

        public static Maybe Continue(Maybe previous, Func<Maybe> next)
            => previous.IsSuccess ? next() : previous;

        public static Maybe<T> Continue<T>(Maybe previous, Func<T> next)
            => previous.IsSuccess ? next().Success() : previous.Fail<T>();

        public static Task<Maybe<T>> Continue<T>(Maybe previous, Func<Task<Maybe<T>>> next)
            => previous.IsSuccess ? next() : Task.FromResult(previous.Fail<T>());

        public static Task<Maybe> Continue(Maybe previous, Func<Task<Maybe>> next)
            => previous.IsSuccess ? next() : Task.FromResult(previous);

        public static async Task<Maybe<T>> Continue<T>(Maybe previous, Func<Task<T>> next)
            => previous.IsSuccess ? (await next()).Success() : previous.Fail<T>();
    }
}