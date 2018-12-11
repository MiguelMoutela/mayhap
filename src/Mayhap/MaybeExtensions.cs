namespace Mayhap
{
    public static class MaybeExtensions
    {
        public static Maybe<TValue> Fail<TValue>(this string failure)
        {
            return new Maybe<TValue>(default, failure);
        }

        public static Maybe<TValue> Success<TValue>(this TValue data)
        {
            return new Maybe<TValue>(data, default);
        }
    }
}