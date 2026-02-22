using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VontobelTest.src.models;

namespace VontobelTest.src.Formatters
{
    public class MailFormat : IFormat<string>
    {
        public string FormatMessage(Dictionary<string, string> input)
        {
            if (input == null || input.Count == 0)
                return string.Empty;

            var lines = new List<string>(input.Count);
            foreach (var kv in input)
            {
                var key = kv.Key ?? string.Empty;
                var value = kv.Value ?? string.Empty;
                lines.Add($"{key}: {value}");
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}