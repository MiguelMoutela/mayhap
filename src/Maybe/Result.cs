using System.Diagnostics.Contracts;

namespace Maybe
{
    internal readonly struct Result
    {
        internal Result(string failure) => Error = failure;

        internal string Error { get; }

        internal bool IsSuccess => Error == null;

        public static implicit operator bool(Result r) => r.IsSuccess;
    }
}
