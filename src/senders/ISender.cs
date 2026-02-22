using VontobelTest.src.models;

namespace VontobelTest.src.senders
{
    public interface ISender<U>
    {
        public Task StartListener(CancellationToken ct);
        public void EnqueueMessage(QueueMessage<U> message);
    }
}