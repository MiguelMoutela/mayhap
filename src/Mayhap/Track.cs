using System;
using System.Threading.Tasks;

namespace Mayhap
{
    public static class Track
    {
        public static Maybe<TNext> Continue<TPrev, TNext>(
            this Maybe<TPrev> previous,
            Func<TPrev, Maybe<TNext>> continuation)
            => previous ? continuation(previous) : previous.To<TNext>();

        public static Task<Maybe<TNext>> Continue<TPrev, TNext>(
            this Maybe<TPrev> previous, 
            Func<TPrev, Task<Maybe<TNext>>> continuation)
            => previous ? continuation(previous) : Task.FromResult(previous.To<TNext>());

        public static Maybe<TNext> Continue<TPrev, TNext>(
            this Maybe<TPrev> previous,
            Func<TPrev, Maybe<TNext>> continuation,
            out Maybe<TNext> next)
            => next = Continue(previous, continuation);

        public static Task<Maybe<TNext>> Continue<TPrev, TNext>(
            this Maybe<TPrev> previous,
            Func<TPrev, Task<Maybe<TNext>>> continuation,
            out Task<Maybe<TNext>> next)
            => next = Continue(previous, continuation);
    }
}