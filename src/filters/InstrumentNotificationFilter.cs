using VontobelTest.src.models;

namespace VontobelTest.src.filters
{
    public class InstrumentNotificationFilter : IFilter
    {
        public Dictionary<string, string> FilterMessage(IBTTermSheet message)
        {
            return new Dictionary<string, string>
            {
                { "Timespan", DateTime.UtcNow.ToString("o") },
                { "ISIN", GetIsin(message) },

            };
        }

        private string GetIsin(IBTTermSheet message)
        {
            return message?.Instrument?.InstrumentIds?.InstrumentId?.Find(i => i.IdSchemeCode == "I-")?.IdValue ?? string.Empty;
            
        }
    }
}