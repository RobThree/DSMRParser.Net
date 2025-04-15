using DSMRParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;

namespace DSMRParser.Tests;

[TestClass]
public class TelegramTests
{
    [TestMethod]
    public void ToString_Should_FormatCorrectly()
    {
        var telegram = new Telegram("foo", [
            (OBISRegistry.VoltageL1.Id, ["1234.56*V"]),
            (OBISRegistry.GasDelivered.Id, ["201211100907S", "6789.12*m3"]),
            (OBISRegistry.ElectricityFailures.Id, ["42"]),
            (OBISRegistry.ElectricityFailureLog.Id, ["000104180320W","0000237126*s","000101000001W","2147583646*s","200102010203W","2317482647*s"]),
        ]);

        Assert.AreEqual("1234.56V", telegram.VoltageL1!.ToString());
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789.12m³", telegram.GasDelivered!.ToString());
        Assert.AreEqual("2000-01-01T00:00:01.0000000+01:00: 24856.07:00:46", telegram.ElectricityFailureLog.ToArray()[0].ToString());

        var nl_culture = CultureInfo.GetCultureInfo(1043);
        Assert.AreEqual("1234,56V", telegram.VoltageL1!.ToString(nl_culture));
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789,12m³", telegram.GasDelivered!.ToString(nl_culture));
        Assert.AreEqual("11-12-20 10:09:07: 6789,12m³", telegram.GasDelivered!.ToString(null, "dd-MM-yy HH:mm:ss", nl_culture));

        var us_culture = CultureInfo.GetCultureInfo(1033);
        Assert.AreEqual("1234.56V", telegram.VoltageL1!.ToString(us_culture));
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789.12m³", telegram.GasDelivered!.ToString(us_culture));
        Assert.AreEqual("12/11/20 10:09:07: 6789.12m³", telegram.GasDelivered!.ToString(null, "MM/dd/yy HH:mm:ss", us_culture));
    }
}
