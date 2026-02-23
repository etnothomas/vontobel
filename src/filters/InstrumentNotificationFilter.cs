using System.Text.Json.Nodes;
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
                ["ISIN"] = JsonValue.Create(GetIsin(message)),
            };

            return obj;
        }

        private string GetIsin(IBTTermSheet message)
        {
            return message?.Instrument?.InstrumentIds?.InstrumentId?.Find(i => i.IdSchemeCode == "I-")?.IdValue ?? string.Empty;
            
        }
    }
}