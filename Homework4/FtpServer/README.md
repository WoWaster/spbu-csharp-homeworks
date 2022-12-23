# Simple FTP Server

## Configuration

By default settings are read from `appsettings.json` file, which MUST be present.
Three fields are required:

* `Directory` -- root directory to serve
* `ServepIp` -- IP address or domain to bind
* `Port` -- Port to bind

You can override values using command line arguments of the same name.
For example:

```
dotnet run --Directory=/srv --ServerIp="example.com" --Port=3000
```