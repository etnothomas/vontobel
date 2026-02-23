using System.Xml;
using VontobelTest.src.models;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace VontobelTest.src.services
{
    public class ProcessMessageService
    {
        private readonly IPartnerService _partnerService;
        private readonly ISenderService _senderService;
        private readonly ILogger<ProcessMessageService> _logger;

        private BlockingCollection<IBTTermSheet> _messageQueue = [];

        public ProcessMessageService(IPartnerService partnerService, ISenderService senderService, ILogger<ProcessMessageService> logger, CancellationToken ct)
        {
            _partnerService = partnerService;
            _senderService = senderService;
            _logger = logger;
            _ = StartProcessing(ct);
        }
        private void ProcessMessageForAllPartners(IBTTermSheet message)
        {
            ProcessMessage<XmlDocument>(message);
            ProcessMessage<string>(message);
        }
        private void ProcessMessage<T>(IBTTermSheet message)
        {
            foreach (var partner in _partnerService.GetPartners<T>())
            {
                QueueMessage<T>? formattedMessage = partner.FormatMessage(message);
                _logger.LogInformation("Processing message for partner {PartnerId} with target {TargetType} and format {TargetFormat}", partner.Id, partner.target.TargetType, partner.target.TargetFormat);
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
                        _logger.LogInformation("Dequeued message for processing: {@Message}", message);
                        ProcessMessageForAllPartners(message);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("ProcessMessageService is stopping due to cancellation.");
                        break;
                    }
                }
            }, ct);
        }

        public void EnqueueMessage(IBTTermSheet message) => _messageQueue.Add(message);

    }

}