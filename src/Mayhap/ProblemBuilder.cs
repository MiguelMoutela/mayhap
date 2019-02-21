using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mayhap
{
    /// <summary>
    /// Problem structure builder
    /// </summary>
    public class ProblemBuilder
    {
        private string _typeString;
        private string _title;
        private string _detail;
        private string _instance;
        private int? _status;
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Creates a ProblemBuilder instance.
        /// </summary>
        public ProblemBuilder()
        {
            _typeString = "about:blank";
            _properties = new Dictionary<string, object>();
        }

        internal ProblemBuilder(Enum type)
        {
            _typeString = ExtractAttribute<ProblemTypeAttribute>(type)?.Value ?? type.ToString();
            _title = ExtractAttribute<ProblemTitleAttribute>(type)?.Value;
            _detail = ExtractAttribute<ProblemDetailAttribute>(type)?.Value;
            _instance = ExtractAttribute<ProblemInstanceAttribute>(type)?.Value;
            _status = ExtractAttribute<ProblemStatusAttribute>(type)?.Value;
            _properties = ExtractProperties(type);
        }

        /// <summary>
        /// Sets the output Problem instances type.
        /// </summary>
        /// <param name="type">The problem type.</param>
        /// <returns>An instance of ProblemBuilder.</returns>
        public ProblemBuilder WithType(string type)
        {
            _typeString = type;
            return this;
        }

        /// <summary>
        /// Sets the output Problem instances title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>An instance of ProblemBuilder.</returns>
        public ProblemBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        /// <summary>
        /// Sets the output Problem instances detail.
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <returns>An instance of ProblemBuilder.</returns>
        public ProblemBuilder WithDetail(string detail)
        {
            _detail = detail;
            return this;
        }

        /// <summary>
        /// Sets the output Problem instances instance property.
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <returns>An instance of ProblemBuilder.</returns>
        public ProblemBuilder WithInstance(string instance)
        {
            _instance = instance;
            return this;
        }

        /// <summary>
        /// Sets the output Problem instances status.
        /// </summary>
        /// <param name="status">The status</param>
        /// <returns>An instance of ProblemBuilder.</returns>
        public ProblemBuilder WithStatus(int? status)
        {
            _status = status;
            return this;
        }

        /// <summary>
        /// Sets the output Problem instances extension properties.
        /// </summary>
        /// <param name="properties">The extension properties.</param>
        /// <returns>An instance of ProblemBuilder.</returns>
        public ProblemBuilder WithProperties(Dictionary<string, object> properties)
        {
            foreach (var property in properties)
            {
                WithProperty(property.Key, property.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets the output Problem instances extension property.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value</param>
        /// <returns>An instance of ProblemBuilder.</returns>
        public ProblemBuilder WithProperty(string name, object value)
        {
            _properties[name] = value;
            return this;
        }

        /// <summary>
        /// Creates previously set up Problem instance.
        /// </summary>
        /// <returns>A Problem structure instance.</returns>
        public Problem Create()
        {
            var properties = _properties.Any() ? _properties : null;
            return new Problem(_typeString, _title, _detail, _instance, _status, properties);
        }

        private Dictionary<string, object> ExtractProperties(Enum type) =>
            ExtractAttributes<ProblemPropertyAttribute>(type)
                .ToDictionary(a => a.Name, a => a.Value);

        private T ExtractAttribute<T>(Enum type) =>
            ExtractAttributes<T>(type)
                .SingleOrDefault();

        private IEnumerable<T> ExtractAttributes<T>(Enum type)
        {
            return type.GetType().GetTypeInfo().GetMember(type.ToString())
                .SelectMany(f => f.GetCustomAttributes(false))
                .Where(a => a is T)
                .Cast<T>();
        }
    }
}