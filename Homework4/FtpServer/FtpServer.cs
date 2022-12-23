using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FtpServer;

public class FtpServer
{
    private readonly FtpServerSettings _settings;
    private readonly CancellationToken _token;

    public FtpServer(FtpServerSettings settings, CancellationToken cancellationToken)
    {
        _settings = settings;
        _token = cancellationToken;
    }

    private static async Task ListAsync(string path, NetworkStream stream)
    {
        if (!Directory.Exists(path))
        {
            await WriteStringToStreamAsync("-1", stream);
            return;
        }

        var response = new StringBuilder();
        var dirs = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);

        response.Append($"{dirs.Length + files.Length} ");
        foreach (var dir in dirs) response.Append($"{dir} true ");

        foreach (var file in files) response.Append($"{file} false ");

        await WriteStringToStreamAsync(response.ToString(), stream);
    }

    private static async Task GetAsync(string path, NetworkStream stream)
    {
        if (!File.Exists(path))
        {
            await WriteStringToStreamAsync("-1 ", stream);
            return;
        }

        var file = new FileInfo(path);
        var fileSize = file.Length;
        await WriteStringToStreamAsync($"{fileSize} ", stream);

        await using var fileStream = file.OpenRead();
        await fileStream.CopyToAsync(stream);
    }

    private static async Task WriteStringToStreamAsync(string str, Stream stream)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        await stream.WriteAsync(bytes);
    }

    public async Task RunServer()
    {
        Directory.SetCurrentDirectory(_settings.Directory);

        var serverIps = await Dns.GetHostEntryAsync(_settings.ServerIp);
        var endPoint = new IPEndPoint(serverIps.AddressList[0], _settings.Port);
        var ftpServer = new TcpListener(endPoint);
        ftpServer.Start();
        Console.WriteLine($"FTP server started on {_settings.ServerIp}:{_settings.Port} from {_settings.Directory}");

        while (true)
        {
            try
            {
                var client = await ftpServer.AcceptTcpClientAsync(_token);
                var task = StartServingClient(client);
                if (task.IsFaulted) await task;
            }
            catch (OperationCanceledException exception)
            {
                break;
            }
        }


        ftpServer.Stop();
        Console.WriteLine("FTP Server stopped");
    }

    private async Task StartServingClient(TcpClient client)
    {
        var connectionTask = ServeClient(client);

        await connectionTask;
    }

    private Task ServeClient(TcpClient client)
    {
        return Task.Run(
            async () =>
            {
                var address = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                Console.WriteLine($"Connected to {address}");

                try
                {
                    await using var stream = client.GetStream();

                    // Since UTF-8 assumed everywhere and default value of TcpClient.ReceiveBufferSize is 8192 bytes,
                    // we may not think of the length of incoming message as it can be up to 2048 chars,
                    // even if if consists of 4 byte UTF-8 chars.
                    var bytes = new byte[client.ReceiveBufferSize];

                    int i;
                    while ((i = await stream.ReadAsync(bytes, _token)) != 0)
                    {
                        var data = Encoding.UTF8.GetString(bytes, 0, i);

                        var commands = data.Trim().Split(" ");
                        switch (commands[0])
                        {
                            case "1":
                                Console.WriteLine($"Got List command for {commands[1]}");
                                await ListAsync(commands[1], stream);
                                break;
                            case "2":
                                Console.WriteLine($"Got Get command for {commands[1]}");
                                await GetAsync(commands[1], stream);
                                break;
                            default:
                                Console.WriteLine($"Got unknown command: {commands[0]}");
                                break;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.Write("Shutting down client. ");
                }
                finally
                {
                    Console.WriteLine($"Disconnected from {address}");
                    client.Close();
                }
            });
    }
}