using VontobelTest.src.Parsers;
using VontobelTest.src.services;
using VontobelTest.src.models;

Console.WriteLine("Starting application...");
var ibt = XmlParser.ParseXml<IBTTermSheet>("/Users/thomasjensen/projects/net-test/VontobelTest/IBT.xml");
var ct = new CancellationTokenSource().Token;
var users = new UserService("/Users/thomasjensen/projects/net-test/VontobelTest/partners/");
var senders = new SenderService(ct);
var processMessageService = new ProcessMessageService(users, senders, ct);
processMessageService.EnqueueMessage(ibt);
Console.WriteLine("Message enqueued for processing. Press any key to exit...");
Console.Read();
