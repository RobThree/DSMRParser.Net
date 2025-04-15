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
        var parser = new DSMRTelegramParser();
        var target = parser.Parse(
            """
            /Test\V5-Telegram

            1-3:0.2.8(50)
            1-0:99.97.0(3)(0-0:96.7.19)(000104180320W)(0000237126*s)(000101000001W)(2147583646*s)(200102010203W)(2317482647*s)
            1-0:32.7.0(1234.56*V)
            0-1:24.2.1(201211100907S)(6789.12*m3)
            !FA4C
            
            """);   //Note: The string is a verbatim string literal, so the newlines are preserved. We NEED the extra empty line for a newline

        Assert.AreEqual("1234.56V", target.VoltageL1!.ToString());
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789.12m³", target.GasDelivered!.ToString());
        Assert.AreEqual("2000-01-04T18:03:20.0000000+01:00: 2.17:52:06", target.ElectricityFailureLog.ToArray()[0].ToString());

        var nl_culture = CultureInfo.GetCultureInfo(1043);
        Assert.AreEqual("1234,56V", target.VoltageL1!.ToString(nl_culture));
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789,12m³", target.GasDelivered!.ToString(nl_culture));
        Assert.AreEqual("11-12-20 10:09:07: 6789,12m³", target.GasDelivered!.ToString(null, "dd-MM-yy HH:mm:ss", nl_culture));

        var us_culture = CultureInfo.GetCultureInfo(1033);
        Assert.AreEqual("1234.56V", target.VoltageL1!.ToString(us_culture));
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789.12m³", target.GasDelivered!.ToString(us_culture));
        Assert.AreEqual("12/11/20 10:09:07: 6789.12m³", target.GasDelivered!.ToString(null, "MM/dd/yy HH:mm:ss", us_culture));
    }
}
