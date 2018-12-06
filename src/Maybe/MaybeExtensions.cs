namespace Maybe
{
    public static class MaybeExtensions
    {
        public static Maybe<TData> Fail<TData>(this string failure)
        {
            return new Maybe<TData>(default, failure);
        }

        public static Maybe Fail(this string failure)
        {
            return new Maybe(failure);
        }

        public static Maybe<TData> Success<TData>(this TData data)
        {
            return new Maybe<TData>(data, default);
        }
    }
}