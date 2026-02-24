using Microsoft.Extensions.Logging;
using VontobelTest.src.senders;
using System.Xml;
using VontobelTest.src.models;

namespace VontobelTest.src.services
{
    public class SenderService : ISenderService
    {
        private readonly ISender<XmlDocument> _xmlFileSender;
        private readonly ISender<string> _emailSender;

        private readonly ILogger<SenderService> _logger;

        public SenderService(XmlFileSender xmlFileSender, EmailSender emailSender, CancellationToken ct, ILogger<SenderService> logger)
        {
            _logger = logger;
            _xmlFileSender = xmlFileSender;
            _emailSender = emailSender;
            _ = InitializeSenders(ct);
        }

        private async Task InitializeSenders(CancellationToken ct)
        {
            Task xmlFileSenderTask = _xmlFileSender.StartListener(ct);
            Task emailSenderTask = _emailSender.StartListener(ct);
            await Task.WhenAll(xmlFileSenderTask, emailSenderTask);
        }

        public void ProcessMessage<T>(Partner<T> partner, QueueMessage<T> message)
        {
            switch((message, partner.Target))
            {
                case (QueueMessage<XmlDocument> xmlMessage, FileTarget _) when partner.Target.TargetFormat == "xml":
                    _xmlFileSender.EnqueueMessage(xmlMessage);
                    break;
                case (QueueMessage<string> emailMessage, EmailTarget _) when partner.Target.TargetFormat == "text":
                    _emailSender.EnqueueMessage(emailMessage);
                    break;
                default:
                    _logger.LogError("Unsupported message type or target format");
                    break;
            }
        }
    }
}