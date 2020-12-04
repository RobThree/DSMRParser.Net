using DSMRParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DSMRParser.Tests
{
    [TestClass]
    public class TelegramParserTests
    {
        [TestMethod]
        public void DSMRTelegramParser_Reads_V2_2_Telegrams()
        {
            var target = new DSMRTelegramParser();
            var telegram = target.Parse(File.ReadAllBytes(@"testdata\v2_2_ok.txt"));
            
            Assert.AreEqual(@"Test\V2_2-Telegram", telegram.Identification);
            Assert.AreEqual("0123456", telegram.EquipmentId);
            Assert.AreEqual(1.234m, telegram.EnergyDeliveredTariff1!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff1!.Unit);
            Assert.AreEqual(2.345m, telegram.EnergyDeliveredTariff2!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff2!.Unit);
            Assert.AreEqual(3.456m, telegram.EnergyReturnedTariff1!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff1!.Unit);
            Assert.AreEqual(4.567m, telegram.EnergyReturnedTariff2!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff2!.Unit);
            Assert.AreEqual(1, telegram.ElectricityTariff);
            Assert.AreEqual(888.88m, telegram.PowerDelivered!.Value);
            Assert.AreEqual(OBISUnit.kW, telegram.PowerDelivered!.Unit);
            Assert.AreEqual(999.99m, telegram.PowerReturned!.Value);
            Assert.AreEqual(OBISUnit.kW, telegram.PowerReturned!.Unit);
            Assert.AreEqual(1, telegram.ElectricitySwitchPosition);
            Assert.AreEqual(string.Empty, telegram.MessageLong);
            Assert.AreEqual(string.Empty, telegram.MessageShort);
            Assert.AreEqual(3, telegram.GasDeviceType);
            Assert.AreEqual("6543210", telegram.GasEquipmentId);
            Assert.AreEqual(1, telegram.GasValvePosition);
        }

        [TestMethod]
        public void DSMRTelegramParser_Reads_V3_Telegrams()
        {
            var target = new DSMRTelegramParser();
            var telegram = target.Parse(File.ReadAllBytes(@"testdata\v3_ok.txt"));

            Assert.AreEqual(@"Test\V3-Telegram", telegram.Identification);
            Assert.AreEqual("FooBarBaz123", telegram.EquipmentId);
            Assert.AreEqual(12345.678m, telegram.EnergyDeliveredTariff1!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff1!.Unit);
            Assert.AreEqual(23456.789m, telegram.EnergyDeliveredTariff2!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff2!.Unit);
            Assert.AreEqual(34567.890m, telegram.EnergyReturnedTariff1!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff1!.Unit);
            Assert.AreEqual(45678.901m, telegram.EnergyReturnedTariff2!.Value);
            Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff2!.Unit);
            Assert.AreEqual(2, telegram.ElectricityTariff);
            Assert.AreEqual(3.14m, telegram.PowerDelivered!.Value);
            Assert.AreEqual(OBISUnit.kW, telegram.PowerDelivered!.Unit);
            Assert.AreEqual(420.69m, telegram.PowerReturned!.Value);
            Assert.AreEqual(OBISUnit.kW, telegram.PowerReturned!.Unit);
            Assert.AreEqual(12.3m, telegram.ElectricityThreshold!.Value);
            Assert.AreEqual(OBISUnit.kW, telegram.ElectricityThreshold!.Unit);
            Assert.AreEqual(1, telegram.ElectricitySwitchPosition);
            Assert.AreEqual("0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?", telegram.MessageLong);
            Assert.AreEqual("012345678", telegram.MessageShort);
            Assert.AreEqual(3, telegram.GasDeviceType);
            Assert.AreEqual("0123ABCD123456789", telegram.GasEquipmentId);
            Assert.AreEqual(1, telegram.GasValvePosition);
        }


    }
}