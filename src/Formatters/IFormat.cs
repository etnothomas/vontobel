namespace VontobelTest.src.formatters;

using System.Text.Json.Nodes;

public interface IFormat<T>
{
    T FormatMessage(JsonObject message);
}
