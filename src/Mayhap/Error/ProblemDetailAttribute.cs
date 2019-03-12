using System;

namespace Mayhap.Error
{
    /// <summary>
    /// The problem detail attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ProblemDetailAttribute : Attribute
    {
        /// <summary>
        /// The problem detail.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a <see cref="ProblemDetailAttribute"/> instance.
        /// </summary>
        /// <param name="detail">The problem detail.</param>
        public ProblemDetailAttribute(string detail) => Value = detail;
    }
}