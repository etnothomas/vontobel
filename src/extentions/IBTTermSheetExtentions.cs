using VontobelTest.src.models;

namespace VontobelTest.src.extentions;

public static class IBTTermSheetExtentions
{
    extension(IBTTermSheet ibt)
    {
        public string GetEventType() => ibt?.Events?.Event?.EventType ?? string.Empty;
        public string GetIsin() => ibt?.Instrument?.InstrumentIds?.InstrumentId?.Find(i => i.IdSchemeCode == "I-")?.IdValue ?? string.Empty;
        public string GetProductNameFull() => ibt?.Instrument?.ProductNameFull ?? string.Empty;
        public string GetIBTTypeCode() => ibt?.Instrument?.IBTTypeCode ?? string.Empty;

    }
}