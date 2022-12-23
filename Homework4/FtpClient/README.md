# Simple FTP Client

## Configuration

By default settings are read from `appsettings.json` file, which MUST be present.
Three fields are required:

* `Directory` -- directory where files will be saved
* `ServepIp` -- IP address or domain of the server
* `Port` -- Port to connect to

You can override values using command line arguments of the same name.
For example:

```
dotnet run --Directory=Downloads --ServerIp="example.com" --Port=3000
```