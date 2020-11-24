# <img src="https://raw.githubusercontent.com/RobThree/DSMR.Net/main/DSMRParser/dsmr_logo.png" alt="Logo" width="64" height="64"> DSMR.Net
DSMR Parser for .Net

## Usage

```c#
var parser = new DSMRTelegramParser();
var telegram = parser.Parse(File.ReadAllText(@"mytelegram.txt"));
```
