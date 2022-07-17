using ResolveCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ResolveCSV.CustomParser
{
    /// <summary>
    /// Custom parser written by Marc Clifton
    /// https://www.codeproject.com/Articles/5256497/Rolling-Your-Own-A-Simple-CSV-Parser-Example
    /// </summary>
    public class CustomParser : ICustomParser
    {
        public List<T> Parse<T>(string filePath, string delimiter) where T : IPopulatedFromCsv, new()
        {
            var data = File.ReadAllText(filePath);
            return ParseData<T>(data, delimiter);
        }

        public List<T> ParseData<T>(string data, string delimiter) where T : IPopulatedFromCsv, new()
        {
            List<T> dataItems = new List<T>();

            var lines = GetLines(data);
            var headers = GetHeaderFields(lines.ElementAt(0), delimiter);
            var propertyList = MapHeaderToProperties<T>(headers);
            lines.Skip(1).ForEach(l => dataItems.Add(Populate<T>(l, delimiter, propertyList)));

            return dataItems;
        }

        private IEnumerable<string> GetLines(string data)
        {
            var lines = data.Split(new char[] { '\r', '\n' })
                            .Where(line => !string.IsNullOrWhiteSpace(line));

            return lines;
        }

        private string[] GetHeaderFields(string header, string delimiter)
        {
            var fields = header.Split(delimiter)
               .Select(field => field.Trim())
               .Where(f =>
                   !string.IsNullOrWhiteSpace(f) ||
                   !string.IsNullOrEmpty(header))
               .ToArray();

            return fields;
        }

        private List<PropertyInfo> MapHeaderToProperties<T>(string[] headerFields)
        {
            var map = new List<PropertyInfo>();
            Type t = typeof(T);

            // Include null properties so these are skipped when parsing the data lines.
            headerFields
                .Select(f =>
                    (
                        f,
                        t.GetProperty(f,
                           BindingFlags.Instance |
                           BindingFlags.Public |
                           BindingFlags.IgnoreCase |
                           BindingFlags.FlattenHierarchy) ?? AliasedProperty(t, f)
                    )
                )
                .ToList()
                .ForEach(fp => map.Add(fp.Item2));

            return map;
        }

        protected virtual PropertyInfo AliasedProperty(Type t, string fieldName)
        {
            var pi = t.GetProperties()
                .Where(p => p.GetCustomAttribute<ColumnMapAttribute>()
                ?.Header
                ?.CaseInsensitiveEquals(fieldName)
                ?? false);
            Assert.That<InvalidOperationException>(pi.Count() <= 1,
                $"{fieldName} is aliased more than once.");

            return pi.FirstOrDefault();
        }

        private T Populate<T>(string line, string delimiter, List<PropertyInfo> props) where T : IPopulatedFromCsv, new()
        {
            T t = Create<T>();
            //ignore delimiter within quotes
            var fieldValues = Regex.Split(line, $"{delimiter}(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            // Unmapped fields will have a null property, and we also skip empty fields, 
            // and trim the field value before doing the type conversion.
            props.ForEach(fieldValues,
               (p, v) => p != null && !String.IsNullOrWhiteSpace(v),
               (p, v) => p.SetValue(t, Converter.Convert(v.Trim(), p.PropertyType)));

            return t;
        }

        public virtual T Create<T>() where T : IPopulatedFromCsv, new()
        { 
            return new T();
        }
    }

    public class ColumnMapAttribute : Attribute
    {
        public string Header { get; set; }
    }
}
