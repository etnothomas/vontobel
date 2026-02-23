using System.Xml;
using System.Collections.Concurrent;
using VontobelTest.src.models;
using Microsoft.Extensions.Logging;

namespace VontobelTest.src.senders
{
    public class XmlFileSender : ISender<XmlDocument>
    {   
        
        private BlockingCollection<QueueMessage<XmlDocument>> blockingQueue = [];

        private readonly ILogger<XmlFileSender> _logger;

        public XmlFileSender(ILogger<XmlFileSender> logger, CancellationToken ct)
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
                        Write(message.Target.Target, message.Message);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("XmlFileSender listener is stopping due to cancellation.");
                        break;
                    }
                }
            }, ct);
        }

        private void Write(string directory, XmlDocument message)
        {
            try {
                System.IO.Directory.CreateDirectory(directory);
                string fileName = $"message_{DateTime.UtcNow:yyyyMMddHHmmssfff}.xml";
                var path = Path.Combine(directory, fileName);
                message.Save(path);
                _logger.LogInformation("Wrote message to directory: {Directory}, message: {Message}", directory, message.OuterXml);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write message");
            }
        }

        public void EnqueueMessage(QueueMessage<XmlDocument> queueMessage)
        {
            blockingQueue.Add(queueMessage);
            _logger.LogInformation("Enqueued message: {@QueueMessage}", queueMessage);
        }

    }

}