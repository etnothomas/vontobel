using System.Xml;
using VontobelTest.src.models;
using System.Collections.Concurrent;

namespace VontobelTest.src.services
{
    public class ProcessMessageService
    {
        private readonly IUserService _userService;
        private readonly ISenderService _senderService;

        private BlockingCollection<IBTTermSheet> _messageQueue = [];

        public ProcessMessageService(IUserService userService, ISenderService senderService, CancellationToken ct)
        {
            _userService = userService;
            _senderService = senderService;
            _ = StartProcessing(ct);
        }
        private void ProcessMessage(IBTTermSheet message)
        {
            ProcessMessage<XmlDocument>(message);
            ProcessMessage<string>(message);
        }
        private void ProcessMessage<T>(IBTTermSheet message)
        {
            foreach (var partner in _userService.GetPartners<T>())
            {
                QueueMessage<T>? formattedMessage = partner.FormatMessage(message);
                Console.WriteLine($"Processing message for partner {partner.Id} with target {partner.target.TargetType} and format {partner.target.TargetFormat}");  
                if (formattedMessage == null)
                {
                    continue;
                }
                _senderService.ProcessMessage(partner, formattedMessage);
            }
        }

        private async Task StartProcessing(CancellationToken ct)
        {
            await Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        var message = _messageQueue.Take(ct);
                        Console.WriteLine($"Dequeued message for processing: ${message}");
                        ProcessMessage(message);
                    }
                    catch (OperationCanceledException)
                    {
                        // Gracefully exit on cancellation
                        break;
                    }
                }
            }, ct);
        }

        public void EnqueueMessage(IBTTermSheet message) => _messageQueue.Add(message);

    }

}