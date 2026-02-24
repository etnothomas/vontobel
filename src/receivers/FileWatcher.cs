using Microsoft.Extensions.Logging;
using VontobelTest.src.Parsers;
using VontobelTest.src.services;
using VontobelTest.src.models;

namespace VontobelTest.src.receivers
{
    public class FileWatcher
    {
        private readonly ProcessMessageService _processMessageService;
        private readonly ILogger<FileWatcher> _logger;
        public FileWatcher(ProcessMessageService processMessageService, ILogger<FileWatcher> logger)
        {
            _processMessageService = processMessageService;
            _logger = logger;
        }

        public async Task StartWatching(string path, CancellationToken ct)
        {
            await Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        FileSystemWatcher fileSystemWatcher = new()
                        {
                            Path = path,
                            Filter = "*.xml",
                            EnableRaisingEvents = true
                        };
                        fileSystemWatcher.Created += new FileSystemEventHandler(OnFileCreated);   
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("FileWatcher is stopping due to cancellation.");
                        break;
                    }
                }
            }, ct);
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"File created: {e.FullPath}");
            if (e.FullPath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var message = XmlParser.ParseXml<IBTTermSheet>(e.FullPath);
                    _processMessageService.EnqueueMessage(message);
                    _logger.LogInformation($"Enqueued XML message from file: {e.FullPath}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to process file: {e.FullPath}");
                }
            }
            else
            {
                _logger.LogWarning($"Unsupported file type: {e.FullPath}");
            }
        }
    }
}