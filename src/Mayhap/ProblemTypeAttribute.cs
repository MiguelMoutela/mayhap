using System;

namespace Mayhap
{
    /// <summary>
    /// The problem type attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ProblemTypeAttribute : Attribute
    {
        /// <summary>
        /// The problem type.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a ProblemTypeAttribute instance.
        /// </summary>
        /// <param name="type">The problem type.</param>
        public ProblemTypeAttribute(string type) => Value = type;
    }
}