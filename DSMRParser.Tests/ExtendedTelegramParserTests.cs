using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSMRParser.Tests;

[TestClass]
public class ExtendedTelegramParserTests
{
    [TestMethod]
    public void DSMRTelegramParser_Reads_MinimalVersionedTelegram()
    {
        var target = new DSMRTelegramParser();
        var result = target.Parse("/Foo\r\n\r\n1-3:0.2.8(50)\r\n!7773\r\n");
        Assert.AreEqual("Foo", result.Identification);
        Assert.AreEqual(50, result.DSMRVersion);
        Assert.IsNull(result.TimeStamp);
    }

    [TestMethod]
    public void DSMRTelegramParser_Reads_MinimalTelegram()
    {
        var target = new DSMRTelegramParser();
        var result = target.Parse("/Foo");
        Assert.AreEqual("Foo", result.Identification);
        Assert.IsNull(result.TimeStamp);
    }

    [TestMethod]
    public void DSMRTelegramParser_Reads_Telegram()
    {
        var target = new DSMRTelegramParser();
        var result = target.Parse("/Foo\r\nThisLineIsIgnored\r\nSoIsThisOne\r\n0.1.2.3(ThisLineIsNotIgnored)");
        Assert.AreEqual(1, result.Values.Count);
        Assert.AreEqual("ThisLineIsNotIgnored", result.GetByObisID("0-1:2.3"));
    }
}