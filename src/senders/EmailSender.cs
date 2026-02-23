using System.Collections.Concurrent;
using VontobelTest.src.models;
using Microsoft.Extensions.Logging;

namespace VontobelTest.src.senders
{
    public class EmailSender : ISender<string>
    {
        private BlockingCollection<QueueMessage<string>> blockingQueue = [];

        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger, CancellationToken ct)
        {
            _logger = logger;

        }
        public async Task StartListener(CancellationToken ct)
        {            
            await Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        var message = blockingQueue.Take(ct);
                        _logger.LogInformation("Dequeued message: {@Message}", message);
                        SendEmail(message.Target.Target, message.Message);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Email sender listener is stopping due to cancellation.");
                        break;
                    }
                }
            }, ct);
        }

        private void SendEmail(string emailAddress, string message)
        {
            try {
                // Simulate email sending
                _logger.LogInformation("Simulating sending email to: {EmailAddress}, message: {Message}", emailAddress, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
            }
        }

        public void EnqueueMessage(QueueMessage<string> queueMessage)
        {
            blockingQueue.Add(queueMessage);
            _logger.LogInformation("Enqueued message: {@QueueMessage}", queueMessage);
        }

    }

}