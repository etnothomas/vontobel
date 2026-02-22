using System.Collections.Concurrent;
using VontobelTest.src.models;

namespace VontobelTest.src.senders
{
    public class EmailSender : ISender<string>
    {
        private BlockingCollection<QueueMessage<string>> blockingQueue = [];
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
                        SendEmail(message.Target.Target, message.Message);
                    }
                    catch (OperationCanceledException)
                    {
                        // Gracefully exit on cancellation
                        break;
                    }
                }
            }, ct);
            
        }

        private static void SendEmail(string emailAddress, string message)
        {
            try {
                // Simulate email sending
                Console.WriteLine($"Simulating sending email to: ${emailAddress}, message: ${message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email: {ex}");
            }
        }

        public void EnqueueMessage(QueueMessage<string> queueMessage)
        {
            blockingQueue.Add(queueMessage);
            Console.WriteLine($"Enqueued message: ${queueMessage}");

        }

    }

}