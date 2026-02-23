using System.Text.Json.Nodes;
using VontobelTest.src.models;
using System.Text.Json;

namespace VontobelTest.src.filters
{
    public class SimpleMailFilter : IFilter
    {
        public JsonObject FilterMessage(IBTTermSheet message)
        {
            var obj = new JsonObject
            {
                ["ProductNameFull"] = JsonValue.Create(GetProductNameFull(message)),
                ["IBTTypeCode"] = JsonValue.Create(GetIBTTypeCode(message)),
                ["EventType"] = JsonValue.Create(GetEventType(message)),
                ["Isin"] = JsonValue.Create(GetIsin(message)),
            };

            return obj;
        }

        private string GetProductNameFull(IBTTermSheet message)
        {
            return message?.Instrument?.ProductNameFull ?? string.Empty;
        }

        private string GetIBTTypeCode(IBTTermSheet message)
        {
            return message?.Instrument?.IBTTypeCode ?? string.Empty;
        }

        private string GetEventType(IBTTermSheet message)
        {
            return message?.Events?.Event?.EventType ?? string.Empty;
        }

        private string GetIsin(IBTTermSheet message)
        {
            return message?.Instrument?.InstrumentIds?.InstrumentId?.Find(i => i.IdSchemeCode == "I-")?.IdValue ?? string.Empty;
        }
    }
}