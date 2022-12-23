using FtpClient;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddCommandLine(args).Build();
var ftpClientSettings = config.Get<FtpClientSettings>();

var cts = new CancellationTokenSource();

var client = new FtpClient.FtpClient(ftpClientSettings!, cts);
 await client.RunClient();