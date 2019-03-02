using System;
using System.Collections.Generic;
using Mayhap.Maybe;

namespace Mayhap.Error
{
    /// <summary>
    /// RFC7807 compliant problem details representation
    /// </summary>
    public struct Problem : IProblem
    {
        /// <summary>
        /// Creates a Problem struct instance.
        /// </summary>
        /// <param name="type">The problem type.</param>
        /// <param name="title">The problem title.</param>
        /// <param name="detail">This problem occurence detailed info.</param>
        /// <param name="instance">This problem occurence resource instance.</param>
        /// <param name="status">The problem status code.</param>
        /// <param name="properties">This problem occurence extension properties.</param>
        public Problem(string type,
            string title,
            string detail,
            string instance,
            int? status,
            IReadOnlyDictionary<string, object> properties)
        {
            Type = type;
            Title = title;
            Status = status;
            Detail = detail;
            Instance = instance;
            Properties = properties;
        }

        /// <summary>
        /// An URI reference that identifies the problem type.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Human-readable summary of the problem type.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// A human-readable detailed description of this occurence of the problem.
        /// </summary>
        public string Detail { get; }

        /// <summary>
        /// A URI reference that identifies the resource of this occurence of the problem.
        /// </summary>
        public string Instance { get; }

        /// <summary>
        /// The HTTP status code for this occurrence of the problem.
        /// </summary>
        public int? Status { get; }

        /// <summary>
        /// The extension properties of this occurence of the problem.
        /// </summary>
        public IReadOnlyDictionary<string, object> Properties { get; }

        /// <summary>
        /// Problem factory method
        /// </summary>
        /// <param name="type">The problem type</param>
        /// <returns>Problem builder instance</returns>
        public static ProblemBuilder OfType(Enum type)
            => new ProblemBuilder(type);

        /// <summary>
        /// Creates a problem builder.
        /// </summary>
        /// <returns>Problem builder instance</returns>
        public static ProblemBuilder New()
            => new ProblemBuilder();

        /// <inheritdoc cref="System.Object.ToString()" />
        public override string ToString()
            => $"Problem {{ Type = {Type} }}";
    }
}