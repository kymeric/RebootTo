using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RebootTo
{
    public class BootEntry
    {
        private static readonly Regex PropertyExpression = new Regex(@"(?<name>\S+)\s+(?<value>[^$]+)", RegexOptions.Compiled);

        public BootEntry(string[] lines)
        {
            Type = lines[0];
            var properties = lines
                .Skip(2)
                .Select(el => PropertyExpression.Match(el))
                .Where(m => m.Success)
                .ToDictionary(m => m.Result("${name}"), m => m.Result("${value}"));

            Identifier = GetValue(properties, "identifier");
            Description = GetValue(properties, "description");
        }

        private string GetValue(Dictionary<string, string> properties, string name)
        {
            string ret = null;
            properties.TryGetValue(name, out ret);
            return ret;
        }

        public string Type { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
    }
}