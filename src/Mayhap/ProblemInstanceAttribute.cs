using System;

namespace Mayhap
{
    /// <summary>
    /// The problem instance attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ProblemInstanceAttribute : Attribute
    {
        /// <summary>
        /// The problem instance.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a ProblemInstanceAttribute instance.
        /// </summary>
        /// <param name="instance">The problem instance.</param>
        public ProblemInstanceAttribute(string instance) => Value = instance;
    }
}