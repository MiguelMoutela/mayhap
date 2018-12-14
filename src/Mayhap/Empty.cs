namespace Mayhap
{
    /// <summary>
    /// Represents empty response
    /// </summary>
    public struct Empty
    {
        public static Maybe<Empty> Success() => new Empty().Success();
    }
}