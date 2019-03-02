using System;

namespace Mayhap.Error
{
    /// <summary>
    /// The problem property attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ProblemPropertyAttribute : Attribute
    {
        /// <summary>
        /// The problem property name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The problem property value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Creates a ProblemPropertyAttribute instance.
        /// </summary>
        /// <param name="name">The problem property name.</param>
        /// <param name="value">The problem property value.</param>
        public ProblemPropertyAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}