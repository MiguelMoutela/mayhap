using System.Collections.Generic;

namespace Mayhap
{
    public interface IProblem
    {
        /// <summary>
        /// An URI reference that identifies the problem type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Human-readable summary of the problem type.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// A human-readable detailed description of this occurence of the problem.
        /// </summary>
        string Detail { get; }

        /// <summary>
        /// A URI reference that identifies the resource of this occurence of the problem.
        /// </summary>
        string Instance { get; }

        /// <summary>
        /// The HTTP status code for this occurrence of the problem.
        /// </summary>
        int? Status { get; }

        /// <summary>
        /// The extension properties of this occurence of the problem.
        /// </summary>
        IReadOnlyDictionary<string, object> Properties { get; }
    }
}