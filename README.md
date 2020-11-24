# ![Logo](https://raw.githubusercontent.com/RobThree/DSMR.Net/main/DSMRParser/dsmr_logo.png) DSMR.Net
DSMR Parser for .Net

## Usage

```c#
var parser = new DSMRTelegramParser();
var telegram = parser.Parse(File.ReadAllText(@"mytelegram.txt"));
```
