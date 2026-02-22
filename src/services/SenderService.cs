using VontobelTest.src.senders;
using System.Xml;
using VontobelTest.src.models;

namespace VontobelTest.src.services
{
    public class SenderService : ISenderService
    {
        private readonly ISender<XmlDocument> _xmlFileSender = new XmlFileSender();
        private readonly ISender<string> _emailSender = new EmailSender();

        public SenderService(CancellationToken ct)
        {
            _ = InitializeSenders(ct);
        }

        private async Task InitializeSenders(CancellationToken ct)
        {
            var xmlFileSenderTask = _xmlFileSender.StartListener(ct);
            var emailSenderTask = _emailSender.StartListener(ct);
            await Task.WhenAll(xmlFileSenderTask, emailSenderTask);
        }

        public void ProcessMessage<T>(Partner<T> partner, QueueMessage<T> message)
        {
            switch((message, partner.target))
            {
                case (QueueMessage<XmlDocument> xmlMessage, FileTarget _) when partner.target.TargetFormat == "xml":
                    _xmlFileSender.EnqueueMessage(xmlMessage);
                    break;
                case (QueueMessage<string> emailMessage, EmailTarget _) when partner.target.TargetFormat == "text":
                    _emailSender.EnqueueMessage(emailMessage);
                    break;
                default:
                    Console.Error.Write("Unsupported message type or target format");
                    break;
            }
        }
    }
}