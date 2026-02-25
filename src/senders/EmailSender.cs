namespace VontobelTest.src.senders;

using System.Collections.Concurrent;
using VontobelTest.src.models;
using Microsoft.Extensions.Logging;
using System.Net.Mail;


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
        await Task.Run(async () =>
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var message = blockingQueue.Take(ct);
                    _logger.LogInformation("Dequeued message: {@Message}", message);
                    if (message.Target is not null)
                    {
                        int returnCode = await SendEmail(message.Target.Target, message.Message);
                        if (returnCode == 0) throw new SmtpException($"mail failed for message {message}");
                    }
                    else throw new NullReferenceException($"Target is null for message {message}");
                }
                catch (SmtpException s)
                {
                    _logger.LogError("Failed to send mail. Error: {s}", s);
                }
                catch (NullReferenceException n)
                {
                    _logger.LogError("Target is null for message. Error: {n}", n);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Email sender listener is stopping due to cancellation.");
                }
            }
        }, ct);
    }



    private async Task<int> SendEmail(string emailAddress, string message)
    {
        return await Task.Run(() =>
        {
            try
            {
                // Simulate email sending
                _logger.LogInformation("Simulating sending email to: {EmailAddress}, message: {Message}", emailAddress, message);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        });
    }

    public void EnqueueMessage(QueueMessage<string> queueMessage)
    {
        blockingQueue.Add(queueMessage);
        _logger.LogInformation("Enqueued message: {QueueMessage}", queueMessage);
    }

}