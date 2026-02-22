using VontobelTest.src.models;

namespace VontobelTest.src.filters
{
    public class MailFilter : IFilter
    {
        public Dictionary<string, string> FilterMessage(IBTTermSheet message)
        {
            return new Dictionary<string, string>
            {
                { "ProductNameFull", GetProductNameFull(message) },
                { "IBTTypeCode", GetIBTTypeCode(message).ToString() },
                { "EventType", GetEventType(message).ToString() },
                { "Isin", GetIsin(message) },
            };
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