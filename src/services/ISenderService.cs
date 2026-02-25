namespace VontobelTest.src.services;

using VontobelTest.src.models;


public interface ISenderService
{
    public void ProcessMessage<T>(Partner<T> partner, QueueMessage<T> message);
}
