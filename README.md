# <img src="https://raw.githubusercontent.com/RobThree/DSMR.Net/main/DSMRParser/dsmr_logo.png" alt="Logo" width="100" height="100">.Net
DSMR Parser for .Net

## Usage

```c#
var parser = new DSMRTelegramParser();
var telegram = parser.Parse(File.ReadAllText(@"mytelegram.txt"));
```
