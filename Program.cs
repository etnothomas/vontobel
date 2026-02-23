using Microsoft.Extensions.Logging;
using VontobelTest.src.Parsers;
using VontobelTest.src.services;
using VontobelTest.src.models;
using VontobelTest.src.senders;

CancellationTokenSource ct = new();
CancellationToken token = ct.Token;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

string xmlFilePath = "/Users/thomasjensen/projects/net-test/VontobelTest/IBT.xml"; // replace with your actual XML file path
string partnersDirectory = "/Users/thomasjensen/projects/net-test/VontobelTest/partners/"; // replace with your actual partners directory path

try 
{
    // wire up the services
    Console.WriteLine("Starting application...");
    IBTTermSheet ibt = XmlParser.ParseXml<IBTTermSheet>(xmlFilePath);
    PartnerService partners = new(partnersDirectory, loggerFactory.CreateLogger<PartnerService>());
    XmlFileSender xmlFileSender = new(loggerFactory.CreateLogger<XmlFileSender>(), token);
    EmailSender emailSender = new(loggerFactory.CreateLogger<EmailSender>(), token);    
    SenderService senders = new(xmlFileSender, emailSender, token, loggerFactory.CreateLogger<SenderService>());
    ProcessMessageService processMessageService = new(partners, senders, loggerFactory.CreateLogger<ProcessMessageService>(), token);
    
    // enqueue the message for processing
    processMessageService.EnqueueMessage(ibt);
    Console.WriteLine("Message enqueued for processing. Press enter to exit...");
    Console.Read();
} 
finally 
{
    ct.Cancel();
    Console.WriteLine("Exiting application...");
}