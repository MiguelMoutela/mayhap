using System;
using System.Collections.Generic;

namespace Mayhap.Error
{
    /// <summary>
    /// The RFC7807 compliant problem details representation.
    /// </summary>
    public struct Problem : IProblem
    {
        /// <summary>
        /// Creates a <see cref="Problem"/> struct instance.
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
        /// Get the problem type.
        /// </summary>
        /// <value>
        /// An URI reference that identifies the problem type.
        /// </value>
        public string Type { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// A human-readable summary of the problem type.
        /// </value>
        public string Title { get; }

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <value>
        /// A human-readable detailed description of this occurence of the problem.
        /// </value>
        public string Detail { get; }

        /// <summary>
        /// Gets the instance of this occurence of the problem.
        /// </summary>
        /// <value>
        /// A URI reference that identifies the resource of this occurence of the problem.
        /// </value>
        public string Instance { get; }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        /// <value>
        /// The HTTP status code for this occurrence of the problem.
        /// </value>
        public int? Status { get; }

        /// <summary>
        /// Gets the extension properties.
        /// </summary>
        /// <value>
        /// The extension properties of this occurence of the problem.
        /// </value>
        public IReadOnlyDictionary<string, object> Properties { get; }

        /// <summary>
        /// Creates an instance of <see cref="ProblemBuilder"/> based on <see cref="Enum"/> problem type.
        /// </summary>
        /// <param name="problemType">The problem type</param>
        /// <returns>An instance of problem builder.</returns>
        public static ProblemBuilder OfType(Enum problemType)
            => new ProblemBuilder(problemType);

        /// <summary>
        /// Creates an instance of <see cref="ProblemBuilder"/>.
        /// </summary>
        /// <returns>An instance of problem builder.</returns>
        public static ProblemBuilder New()
            => new ProblemBuilder();

        /// <summary>
        /// Returns a string representation of current instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
            => $"Problem {{ Type = {Type} }}";
    }
}