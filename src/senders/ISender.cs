using VontobelTest.src.models;

namespace VontobelTest.src.senders
{
    public interface ISender<U>
    {
        public Task StartListener(CancellationToken ct);
        public void EnqueueMessage(QueueMessage<U> message);

        public void DoWithRetry(Action action, TimeSpan sleepPeriod, int tryCount = 3)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(tryCount);
            while (true) {
            try {
                action();
                break; // success!
            }  catch {
                if (--tryCount == 0)
                    throw;
                Thread.Sleep(sleepPeriod);
                }
            }
        }


    }
}