namespace VontobelTest.src.models
{
    public record QueueMessage<T>(T Message, ITarget? Target);
}