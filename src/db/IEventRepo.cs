namespace VontobelTest.src.db;

using VontobelTest.src.models;

public interface IEventRepo
{
    EventType WriteEvent(EventType e);
    Task WriteEventAsync(EventType e);

}
