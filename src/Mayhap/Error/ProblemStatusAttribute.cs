using System;

namespace Mayhap.Error
{
    /// <summary>
    /// The problem status attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ProblemStatusAttribute : Attribute
    {
        /// <summary>
        /// The problem status.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Creates a ProblemStatusAttribute instance.
        /// </summary>
        /// <param name="status">The problem status.</param>
        public ProblemStatusAttribute(int status) => Value = status;
    }
}