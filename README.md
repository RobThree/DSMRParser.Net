# ![Logo](https://raw.githubusercontent.com/RobThree/DSMRParser.Net/main/DSMRParser/dsmr_logo_24x24.png) DSMRParser.Net

![Build Status](https://img.shields.io/github/actions/workflow/status/RobThree/DSMRParser.Net/test.yml?branch=main&style=flat-square) [![Nuget version](https://img.shields.io/nuget/v/DSMRParser.Net.svg?style=flat-square)](https://www.nuget.org/packages/DSMRParser.Net/)


DSMR Parser for .Net. Available as [NuGet package](https://www.nuget.org/packages/DSMRParser.Net).

## Quickstart

```c#
var parser = new DSMRTelegramParser();
var telegram = parser.Parse(File.ReadAllText("path/to/mytelegram.txt"));
// You don't need to use a file, you can also use a string or span<byte> directly

// Do stuff with the telegram
Console.WriteLine(telegram.EnergyDeliveredTariff1.ToString());
Console.WriteLine(telegram.EnergyDeliveredTariff1.Value);
Console.WriteLine(telegram.EnergyDeliveredTariff1.Unit);

// Get original, raw string
Console.WriteLine(telegram.GetByObisID("1-0:1.8.1")?.ToString());
Console.WriteLine(telegram.GetByObisID(OBISRegistry.EnergyDeliveredTariff1.Id)?.ToString());
Console.WriteLine(telegram.GetByDescriptor(OBISRegistry.EnergyDeliveredTariff1)?.ToString());

// Timestamped value
Console.WriteLine(telegram.GasDelivered.DateTime);
Console.WriteLine(telegram.GasDelivered.Value.Value);
Console.WriteLine(telegram.GasDelivered.Value.Unit);
Console.WriteLine(telegram.GasDelivered.Value.Unit.ToUnitString());
Console.WriteLine(telegram.GasDelivered.ToString());
```

Output:

```cmd
123456.789kWh
123456,789
kWh
123456.789*kWh
123456.789*kWh
123456.789*kWh
11-12-2020 10:09:08 +01:00
12321,232
m3
m³
2020-12-11T10:09:08.0000000+01:00: 12321.232m³
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

As for a [Telegram](DSMRParser/Models/Telegram.cs) object, it has a number of properties that are mostly nullable. The reason for this is that the telegram may not contain all values, depending on the type of meter and the configuration. For example, a gas meter will not have any voltage values. The properties are all strongly typed, so you can use them directly in your code without having to parse them yourself.

A telegram may contain 'unknown' values. These are values that are not defined in the DSMR standard, but may be present in some meters. You can get to these values by using the `GetByDescriptor(...)`, `GetMultiByDescriptor(...)`, `GetByObisID(...)` and `GetMultiByObisID(...)` methods. You can use the [`OBISRegistry`](DSMRParser/OBISRegistry.cs) and/or [`OBISId`](DSMRParser/Models/OBISId.cs) classes to get the OBIS IDs and descriptors for documented values.

```c#
var value = telegram.GetByDescriptor(OBISRegistry.CurrentL1);
// or..
var value = telegram.GetByObisID(OBISRegistry.CurrentL1.Id);
// or..
var value = telegram.GetByObisID(OBISId.FromString("1-0:31.7.0"));
```

This library is locale aware (you can set a timezone for the [`DSMRTelegramParser`](DSMRParser/DSMRTelegramParser.cs) in it's constructor. All [`UnitValue<T>`](DSMRParser/Models/UnitValue.cs) values have `.ToString(...)` overrides that allow you to specify an `IFormatProvider` and `format`.

### CRCs

The library will automatically check the CRC of the telegram. If you want to ignore the CRC, you can set the `ignoreCrc` parameter to `true` in the `Parse` and `TryParse` methods. The library will still parse the telegram, but it will not check the CRC. This is useful for testing or if you are sure that the telegram is valid. You can also provide the `DSMRTelegramParser` with a custom [`ICRCVerifier`](DSMRParser/CRCHandling/ICRCVerifier.cs) implementation if you want to use a different CRC algorithm. This library provides a default [`CRC16Verifier`](DSMRParser/CRCHandling/CRC16Verifier.cs) implementation that uses the CRC16 algorithm.