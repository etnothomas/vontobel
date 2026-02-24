using VontobelTest.src.models;

namespace VontobelTest.src.db
{
    public interface IEventRepo
    {
        EventType WriteEvent(EventType e);
        Task WriteEventAsync(EventType e);

    }
}