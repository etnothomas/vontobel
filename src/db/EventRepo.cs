using System.Net;
using Microsoft.Extensions.Logging;
using VontobelTest.src.models;

namespace VontobelTest.src.db{
    
    public class EventRepo : IEventRepo
    {
        private readonly Dictionary<(int, DateTime), EventType> db = [];

        private readonly Random _random = new();

        private readonly ILogger<EventRepo> _logger;
        public EventRepo(ILogger<EventRepo> logger){
            _logger = logger;
        }

        public EventType WriteEvent(EventType e){
            db.Add((_random.Next(), DateTime.UtcNow), e);
            return e;
        }
        

        public async Task WriteEventAsync(EventType e){
            try
            {
                await Task.Run(() => WriteEvent(e));
            }
            catch (Exception ex){
                _logger.LogError($"Failed to write ${e} with error ${ex}");
            };
        }

    }
}