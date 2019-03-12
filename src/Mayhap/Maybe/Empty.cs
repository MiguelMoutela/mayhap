namespace Mayhap.Maybe
{
    /// <summary>
    /// A helper struct made to represent an empty return value in <see cref="Maybe{TValue}"/> structure.
    ///
    /// See also: <seealso cref="Maybe{TValue}"/>.
    /// </summary>
    public struct Empty
    {
        /// <summary>
        /// Creates an instance of <see cref="Empty"/> struct.
        /// </summary>
        /// <returns>The <see cref="Empty"/> structure instance.</returns>
        public static Maybe<Empty> Success() => new Empty().Success();
    }
}