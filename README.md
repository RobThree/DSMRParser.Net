# ![Logo](https://raw.githubusercontent.com/RobThree/DSMR.Net/main/DSMRParser/dsmr_logo_24x24.png) DSMRParser.Net

![Build Status](https://img.shields.io/github/actions/workflow/status/RobThree/DSMR.Net/test.yml?branch=main&style=flat-square) [![Nuget version](https://img.shields.io/nuget/v/DSMRParser.Net.svg?style=flat-square)](https://www.nuget.org/packages/DSMRParser.Net/)


DSMR Parser for .Net. Available as [NuGet package](https://www.nuget.org/packages/DSMRParser.Net).

## TODO:

* [ ] Complete README
* [X] Complete Documentation
* [ ] Complete Unittests

## Usage

```c#
var parser = new DSMRTelegramParser();
var telegram = parser.Parse(File.ReadAllText(@"mytelegram.txt"));
```
