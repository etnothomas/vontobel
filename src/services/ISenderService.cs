using VontobelTest.src.models;

namespace VontobelTest.src.services
{
        public interface ISenderService
        {
            public void ProcessMessage<T>(Partner<T> partner, QueueMessage<T> message);
        }
}