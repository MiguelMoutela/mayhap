using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mayhap
{
    /// <summary>
    /// Problem structure builder
    /// </summary>
    /// <typeparam name="TTypeEnum">Problem type enum</typeparam>
    public class ProblemBuilder<TTypeEnum>
    {
        private string _typeString;
        private string _title;
        private string _detail;
        private string _instance;
        private int? _status;
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Creates ProblemBuilder instance
        /// </summary>
        public ProblemBuilder()
        {
        }

        internal ProblemBuilder(TTypeEnum type)
        {
            _typeString = ExtractAttribute<ProblemTypeAttribute>(type)?.Value ?? type.ToString();
            _title = ExtractAttribute<ProblemTitleAttribute>(type)?.Value;
            _detail = ExtractAttribute<ProblemDetailAttribute>(type)?.Value;
            _instance = ExtractAttribute<ProblemInstanceAttribute>(type)?.Value;
            _status = ExtractAttribute<ProblemStatusAttribute>(type)?.Value;
            _properties = ExtractProperties(type);
        }

        /// <summary>
        /// Creates previously set up Problem instance.
        /// </summary>
        /// <returns>A Problem structure instance.</returns>
        public Problem Create() => new Problem(_typeString, _title, _detail, _instance, _status, _properties);

        private Dictionary<string, object> ExtractProperties(TTypeEnum type)
        {
            var properties = ExtractAttributes<ProblemPropertyAttribute>(type)
                .ToDictionary(a => a.Name, a => a.Value);
            return properties.Any() ? properties : null;
        }

        private T ExtractAttribute<T>(TTypeEnum type) =>
            ExtractAttributes<T>(type)
                .SingleOrDefault();

        private IEnumerable<T> ExtractAttributes<T>(TTypeEnum type) =>
            typeof(TTypeEnum).GetTypeInfo().GetMember(type.ToString())
                .SelectMany(f => f.GetCustomAttributes(false))
                .Where(a => a is T)
                .Cast<T>();
    }
}