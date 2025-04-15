using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DSMRParser.Tests;

[TestClass]
public class TelegramTests
{
    [TestMethod]
    public void ToString_Should_FormatCorrectly()
    {
        var parser = new DSMRTelegramParser();
        var target = parser.Parse(File.ReadAllText("testdata/formattest.txt"));

        // No specific culture is set, so the default culture is used.
        Assert.AreEqual("1234.56V", target.VoltageL1!.ToString());
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789.12m³", target.GasDelivered!.ToString());
        Assert.AreEqual("2000-01-04T18:03:20.0000000+01:00: 2.17:52:06", target.ElectricityFailureLog.ToArray()[0].ToString());

        // Use Dutch culture
        var nl_culture = CultureInfo.GetCultureInfo(1043);
        Assert.AreEqual("1234,56V", target.VoltageL1!.ToString(nl_culture));
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789,12m³", target.GasDelivered!.ToString(nl_culture));
        Assert.AreEqual("11-12-20 10:09:07: 6789,12m³", target.GasDelivered!.ToString(null, "dd-MM-yy HH:mm:ss", nl_culture));

        // Use US culture
        var us_culture = CultureInfo.GetCultureInfo(1033);
        Assert.AreEqual("1234.56V", target.VoltageL1!.ToString(us_culture));
        Assert.AreEqual("2020-12-11T10:09:07.0000000+01:00: 6789.12m³", target.GasDelivered!.ToString(us_culture));
        Assert.AreEqual("12/11/20 10:09:07: 6789.12m³", target.GasDelivered!.ToString(null, "MM/dd/yy HH:mm:ss", us_culture));
    }
}
