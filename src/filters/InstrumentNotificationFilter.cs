using System.Text.Json.Nodes;
using VontobelTest.src.extentions;
using VontobelTest.src.models;

namespace VontobelTest.src.filters
{
    public class InstrumentNotificationFilter : IFilter
    {
        public JsonObject FilterMessage(IBTTermSheet message)
        {
            var obj = new JsonObject
            {
                ["Timespan"] = JsonValue.Create(DateTime.UtcNow.ToString("o")),
                ["ISIN"] = JsonValue.Create(message.GetIsin())
            };

            return obj;
        }

    }
}