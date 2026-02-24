using Microsoft.Extensions.Logging;
using VontobelTest.src.services;
using VontobelTest.src.senders;
using VontobelTest.src.receivers;
using VontobelTest.src.db;

CancellationTokenSource ct = new();
CancellationToken token = ct.Token;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

string watchedFolder = "/Users/thomasjensen/projects/net-test/watchedfolder"; // replace with your actual XML file path
string partnersDirectory = "/Users/thomasjensen/projects/net-test/VontobelTest/partners/"; // replace with your actual partners directory path

try 
{
    // wire up the services
    Console.WriteLine("Starting application...");
    EventRepo eventRepo = new(loggerFactory.CreateLogger<EventRepo>());
    PartnerService partners = new(partnersDirectory, loggerFactory.CreateLogger<PartnerService>());
    XmlFileSender xmlFileSender = new(loggerFactory.CreateLogger<XmlFileSender>(), token);
    EmailSender emailSender = new(loggerFactory.CreateLogger<EmailSender>(), token);    
    SenderService senders = new(xmlFileSender, emailSender, token, loggerFactory.CreateLogger<SenderService>());
    ProcessMessageService processMessageService = new(partners, senders, eventRepo, loggerFactory.CreateLogger<ProcessMessageService>(), token);
    FileWatcher fileWatcher = new(processMessageService, loggerFactory.CreateLogger<FileWatcher>());
    
    // enqueue the message for processing
    _ = fileWatcher.StartWatching(watchedFolder, token);
    Console.WriteLine($"Listening for new .xml in {watchedFolder}. Please move an IBT file to the directory.  Press enter to exit...");
    Console.Read();
} 
finally 
{
    ct.Cancel();
    Console.WriteLine("Exiting application...");
}