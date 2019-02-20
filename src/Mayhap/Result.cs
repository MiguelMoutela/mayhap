namespace Mayhap
{
    internal readonly struct Result
    {
        internal Result(IProblem failure) => Error = failure;

        internal IProblem Error { get; }

        internal bool IsSuccess => Error == null;

        public static implicit operator bool(Result r) => r.IsSuccess;
    }
}
