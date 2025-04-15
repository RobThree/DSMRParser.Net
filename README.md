# ![Logo](https://raw.githubusercontent.com/RobThree/DSMRParser.Net/main/DSMRParser/dsmr_logo_24x24.png) DSMRParser.Net

![Build Status](https://img.shields.io/github/actions/workflow/status/RobThree/DSMRParser.Net/test.yml?branch=main&style=flat-square) [![Nuget version](https://img.shields.io/nuget/v/DSMRParser.Net.svg?style=flat-square)](https://www.nuget.org/packages/DSMRParser.Net/)


DSMR Parser for .Net. Available as [NuGet package](https://www.nuget.org/packages/DSMRParser.Net).

## Quickstart

```c#
var parser = new DSMRTelegramParser();
var telegram = parser.Parse(File.ReadAllText(@"mytelegram.txt"));

// Do stuff with the telegram
Console.WriteLine(telegram.VoltageL1?.ToString());
Console.WriteLine(telegram.VoltageL1?.Value);
Console.WriteLine(telegram.VoltageL1?.Unit);
```

Output:

```cmd
230.1V
230.1
V
```

## API

The [`IDSMRTelegramParser`](DSMRParser/IDSMRTelegramParser.cs) interface has the following methods:
```c#
Telegram Parse(Span<byte> telegram);
```
However, the [default implementation](DSMRParser/DSMRTelegramParser.cs) provides a few useful additional methods, show (simplified) below:
```c#
public Telegram Parse(Span<byte> telegram, bool ignoreCrc = false)
public Telegram Parse(string telegram, bool ignoreCrc = false)
public bool TryParse(Span<byte> telegram, bool ignoreCrc = false, out Telegram? result) 
public bool TryParse(string telegram, bool ignoreCrc = false, out Telegram? result)
```

Since this API follows .Net conventions, usage shouldn't be a surprise. The `Parse` method will throw an exception if the telegram is invalid, while `TryParse` will return a boolean indicating success or failure and provide the result in the `out` argument. The `telegram` can be provided as string or as `Span<byte>` and the `ignoreCrc` does what it says on the tin: it ignores issues with the CRC.