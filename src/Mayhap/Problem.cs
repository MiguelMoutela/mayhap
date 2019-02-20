using System;
using System.Collections.Generic;

namespace Mayhap
{
    /// <summary>
    /// RFC7807 compliant problem details representation
    /// </summary>
    public class Problem
    {
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
        public static ProblemBuilder Builder()
            => new ProblemBuilder();
    }
}