using System.Text.Json.Nodes;
using VontobelTest.src.models;
using VontobelTest.src.extentions;


namespace VontobelTest.src.filters
{
    public class SimpleMailFilter : IFilter
    {
        public JsonObject FilterMessage(IBTTermSheet message)
        {
            var obj = new JsonObject
            {
                ["ProductNameFull"] = JsonValue.Create(message.GetProductNameFull()),
                ["IBTTypeCode"] = JsonValue.Create(message.GetIBTTypeCode()),
                ["EventType"] = JsonValue.Create(message.GetEventType()),
                ["Isin"] = JsonValue.Create(message.GetIsin()),
            };

            return obj;
        }
    }
}