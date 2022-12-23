using FtpServer;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddCommandLine(args).Build();
var ftpServerSettings = config.Get<FtpServerSettings>();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArg) =>
{
    eventArg.Cancel = true;
    Console.WriteLine("Shutting down gracefully");
    cts.Cancel();
    cts.Dispose();
};

var server = new FtpServer.FtpServer(ftpServerSettings, cts.Token);
await server.RunServer();