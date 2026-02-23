using System.Text.Json.Nodes;

namespace VontobelTest.src.formatters
{
    public class SimpleMailFormat : IFormat<string>
    {
        public string FormatMessage(JsonObject input)
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