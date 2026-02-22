using System.Xml;
using System.Collections.Concurrent;
using VontobelTest.src.models;

namespace VontobelTest.src.senders
{
    public class XmlFileSender : ISender<XmlDocument>
    {   
        
        private BlockingCollection<QueueMessage<XmlDocument>> blockingQueue = [];
        public async Task StartListener(CancellationToken ct)
        {            
            await Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        var message = blockingQueue.Take(ct);
                        Console.WriteLine($"Dequeued message: ${message}");
                        Write(message.Target.Target, message.Message);
                    }
                    catch (OperationCanceledException)
                    {
                        // Gracefully exit on cancellation
                        break;
                    }
                }
            }, ct);
            
        }

        private static void Write(string directory, XmlDocument message)
        {
            try {
                System.IO.Directory.CreateDirectory(directory);
                string fileName = $"message_{DateTime.UtcNow:yyyyMMddHHmmssfff}.xml";
                var path = Path.Combine(directory, fileName);
                message.Save(path);
                Console.WriteLine($"Wrote message to directory: ${directory}, message: ${message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write message: {ex}");
            }
        }

        public void EnqueueMessage(QueueMessage<XmlDocument> queueMessage)
        {
            blockingQueue.Add(queueMessage);
            Console.WriteLine($"Enqueued message: ${queueMessage}");

        }

    }

}