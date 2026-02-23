using System.Text.Json.Nodes;

namespace VontobelTest.src.formatters
{

    public interface IFormat<T>
    {
        T FormatMessage(JsonObject message);
    }
}