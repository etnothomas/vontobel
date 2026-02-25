namespace VontobelTest.src.services;

using System.Xml;
using VontobelTest.src.models;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using VontobelTest.src.db;
using VontobelTest.src.extentions;



public class ProcessMessageService
{
    private readonly IPartnerService _partnerService;
    private readonly ISenderService _senderService;
    private readonly ILogger<ProcessMessageService> _logger;
    private readonly IEventRepo _eventRepo;
    private BlockingCollection<IBTTermSheet> _messageQueue = [];

    public ProcessMessageService(IPartnerService partnerService, ISenderService senderService, IEventRepo eventRepo, ILogger<ProcessMessageService> logger, CancellationToken ct)
    {
        _partnerService = partnerService;
        _senderService = senderService;
        _eventRepo = eventRepo;
        _logger = logger;
        _ = StartProcessing(ct);
    }
    private void ProcessMessageForAllPartners(IBTTermSheet message)
    {
        EventType et = new(message.GetEventType());
        Task task = _eventRepo.WriteEventAsync(et)
            .ContinueWith(_ =>
            {
                ProcessMessage<XmlDocument>(message);
                ProcessMessage<string>(message);
            }, TaskContinuationOptions.NotOnFaulted);
    }
    private void ProcessMessage<T>(IBTTermSheet message)
    {
        foreach (var partner in _partnerService.GetPartners<T>())
        {
            QueueMessage<T>? formattedMessage = partner.FormatMessage(message);
            _logger.LogInformation("Processing message for partner {PartnerId} with target {TargetType} and format {TargetFormat}", partner.Id, partner.Target.TargetType, partner.Target.TargetFormat);
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

