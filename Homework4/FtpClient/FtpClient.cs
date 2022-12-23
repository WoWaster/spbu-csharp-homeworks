using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

namespace FtpClient;

public class FtpClient
{
    private readonly FtpClientSettings _settings;
    private readonly CancellationTokenSource _tokenSource;
    private readonly CancellationToken _token;
    
    private bool _keepRunning = true;
    private NetworkStream _stream = null!;

    public FtpClient(FtpClientSettings settings, CancellationTokenSource cancellationTokenSource)
    {
        _settings = settings;
        _tokenSource = cancellationTokenSource;
        _token = _tokenSource.Token;
    }

    private static void PrintHelp()
    {
        var commands = new Dictionary<string, string>
        {
            { "help", "Print help" },
            { "ls PATH", "List all entries in the given path" },
            { "get FILE", "Get file by name" },
            { "quit", "Exit the program" }
        };
        var sb = new StringBuilder();
        sb.Append("Available commands:\n");
        foreach (var (command, description) in commands) sb.Append($"* {command}\t{description}\n");
        Console.WriteLine(sb.ToString());
    }

    private static async Task WriteStringToStreamAsync(string str, Stream stream)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        await stream.WriteAsync(bytes);
    }


    private async Task<List<(string name, bool isDir)>> GetList(string path)
    {
        if (string.IsNullOrEmpty(path)) path = ".";
        await WriteStringToStreamAsync($"1 {path}", _stream);

        var bytes = new byte[65535];
        var size = await _stream.ReadAsync(bytes, _token);
        var str = Encoding.UTF8.GetString(bytes, 0, size).Split(" ");

        var result = new List<(string name, bool isDir)>();
        for (var i = 0; i < int.Parse(str[0]); i++) result.Add((str[2 * i + 1], bool.Parse(str[2 * i + 2])));

        return result;
    }

    private async Task PrintContents(string path)
    {
        var contents = await GetList(path);

        var sb = new StringBuilder();
        foreach (var (name, isDir) in contents)
        {
            sb.Append(name);
            if (isDir) sb.Append('/');
            sb.Append(' ');
        }

        Console.WriteLine(sb.ToString());
    }

    private async Task GetFile(string path)
    {
        if (string.IsNullOrEmpty(path)) path = ".";
        await WriteStringToStreamAsync($"2 {path}", _stream);

        var lengthStr = "";
        int nextByte;
        
        // 32 is a space in UTF-8
        while ((nextByte = _stream.ReadByte()) != 32)
        {
            lengthStr += Encoding.UTF8.GetString(new[]{Convert.ToByte(nextByte)});
        }

        var length = long.Parse(lengthStr);
        if (length == -1)
        {
            Console.WriteLine("File wasn't found on server.");
            return;
        }

        var filename = Path.GetFileName(path);
        
        await using var fileStream = File.Create(filename);
        while (length > 0)
        {
            var buffer = new byte[_stream.Socket.ReceiveBufferSize];
            var receivedLength = await _stream.ReadAsync(buffer, _token);
            await fileStream.WriteAsync(buffer.AsMemory(0, receivedLength), _token);
            length -= receivedLength;
        }
        Console.WriteLine($"Downloaded {filename} successfully");
    }
    
    public async Task RunClient()
    {
        if (!Directory.Exists(_settings.Directory))
            Directory.CreateDirectory(_settings.Directory);
        Directory.SetCurrentDirectory(_settings.Directory);

        var serverIps = await Dns.GetHostEntryAsync(_settings.ServerIp);
        var endPoint = new IPEndPoint(serverIps.AddressList[0], _settings.Port);
        using var client = new TcpClient();
        await client.ConnectAsync(endPoint, _token);
        _stream = client.GetStream();

        Console.WriteLine("Enter \"help\" for help.");
        while (_keepRunning)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            var splitInput = input.Trim().Split(" ");
            switch (splitInput[0])
            {
                case "quit":
                    _tokenSource.Cancel();
                    _tokenSource.Dispose();
                    _keepRunning = false;
                    break;
                case "help":
                    PrintHelp();
                    break;
                case "ls":
                    await PrintContents(splitInput.ElementAtOrDefault(1) ?? ".");
                    break;
                case "get":
                    if (splitInput.Length < 2)
                    {
                        Console.WriteLine("Specify filename");
                        break;
                    }

                    await GetFile(splitInput[1]);
                    break;
            }
        }
    }
}