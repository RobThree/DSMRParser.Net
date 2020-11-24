# <img src="https://raw.githubusercontent.com/RobThree/DSMR.Net/main/DSMRParser/dsmr_logo.png" alt="Logo" width="100" height="100"> DSMR.Net
DSMR Parser for .Net. Available as [NuGet package](https://www.nuget.org/packages/DSMRParser.Net).

## TODO:

* [ ] Complete README
* [ ] Complete Documentation
* [ ] Complete Unittests

## Usage

```c#
var parser = new DSMRTelegramParser();
var telegram = parser.Parse(File.ReadAllText(@"mytelegram.txt"));
```
