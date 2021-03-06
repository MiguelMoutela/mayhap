﻿using System;

namespace Mayhap.Error
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
        /// Creates a <see cref="ProblemTypeAttribute"/> instance.
        /// </summary>
        /// <param name="type">The problem type.</param>
        public ProblemTypeAttribute(string type) => Value = type;
    }
}