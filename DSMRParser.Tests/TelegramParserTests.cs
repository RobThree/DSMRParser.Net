using DSMRParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace DSMRParser.Tests;

[TestClass]
public class TelegramParserTests
{
    [TestMethod]
    public void DSMRTelegramParser_Reads_V2_2_Telegrams()
    {
        var target = new DSMRTelegramParser();
        var telegram = target.Parse(File.ReadAllBytes(@"testdata/v2_2_ok.txt"));

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
    public void DSMRTelegramParser_Reads_V2_2_Mangled_Telegrams()
    {
        var target = new DSMRTelegramParser(true);
        var telegram = target.Parse(File.ReadAllBytes(@"testdata/v2_2_mangled.txt"));

        Assert.AreEqual(@"Test\V2_2-Telegram_Mangled", telegram.Identification);

        Assert.AreEqual(999m, telegram.ElectricityThreshold!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.ElectricityThreshold!.Unit);

        Assert.AreEqual(22806.137m, telegram.GasDeliveredOld!.Value!.Value);
        Assert.AreEqual(OBISUnit.m3, telegram.GasDeliveredOld!.Value!.Unit);
    }

    [TestMethod]
    public void DSMRTelegramParser_Reads_V3_Telegrams()
    {
        var target = new DSMRTelegramParser();
        var telegram = target.Parse(File.ReadAllBytes(@"testdata/v3_ok.txt"));

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

    [TestMethod]
    public void DSMRTelegramParser_Reads_V4_2_Telegrams()
    {
        var target = new DSMRTelegramParser();
        var telegram = target.Parse(File.ReadAllBytes(@"testdata/v4_2_ok.txt"));
        Assert.AreEqual(@"Test\V4_2-Telegram", telegram.Identification);
        Assert.AreEqual(42, telegram.DSMRVersion);
        Assert.AreEqual(new DateTimeOffset(2010, 12, 9, 11, 30, 20, TimeSpan.FromHours(1)), telegram.TimeStamp);
        Assert.AreEqual("K8EG004046395507", telegram.EquipmentId);
        Assert.AreEqual(123456.789m, telegram.EnergyDeliveredTariff1!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff1!.Unit);
        Assert.AreEqual(987654.321m, telegram.EnergyDeliveredTariff2!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff2!.Unit);
        Assert.AreEqual(101010.010m, telegram.EnergyReturnedTariff1!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff1!.Unit);
        Assert.AreEqual(020202.202m, telegram.EnergyReturnedTariff2!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff2!.Unit);
        Assert.AreEqual(2, telegram.ElectricityTariff);
        Assert.AreEqual(11.111m, telegram.PowerDelivered!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDelivered!.Unit);
        Assert.AreEqual(22.222m, telegram.PowerReturned!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturned!.Unit);
        Assert.AreEqual(15, telegram.ElectricityFailures);
        Assert.AreEqual(7, telegram.ElectricityLongFailures);

        var fl = telegram.ElectricityFailureLog.ToArray();
        Assert.AreEqual(3, fl.Length);
        Assert.AreEqual(TimeSpan.FromSeconds(237126), fl[0].Value);
        Assert.AreEqual(new DateTimeOffset(2000, 1, 4, 18, 03, 20, TimeSpan.FromHours(1)), fl[0].DateTime);
        Assert.AreEqual(TimeSpan.FromSeconds(2147583646), fl[1].Value);
        Assert.AreEqual(new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.FromHours(1)), fl[1].DateTime);
        Assert.AreEqual(TimeSpan.FromSeconds(2317482647), fl[2].Value);
        Assert.AreEqual(new DateTimeOffset(2020, 1, 2, 1, 2, 3, TimeSpan.FromHours(1)), fl[2].DateTime);

        Assert.AreEqual(1, telegram.ElectricitySagsL1);
        Assert.AreEqual(10, telegram.ElectricitySagsL2);
        Assert.AreEqual(100, telegram.ElectricitySagsL3);
        Assert.AreEqual(2, telegram.ElectricitySwellsL1);
        Assert.AreEqual(20, telegram.ElectricitySwellsL2);
        Assert.AreEqual(200, telegram.ElectricitySwellsL3);
        Assert.AreEqual("Life, the Universe and Everything", telegram.MessageShort);
        Assert.AreEqual("0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?", telegram.MessageLong);
        Assert.AreEqual(1, telegram.CurrentL1!.Value);
        Assert.AreEqual(OBISUnit.A, telegram.CurrentL1!.Unit);
        Assert.AreEqual(2, telegram.CurrentL2!.Value);
        Assert.AreEqual(OBISUnit.A, telegram.CurrentL2!.Unit);
        Assert.AreEqual(3, telegram.CurrentL3!.Value);
        Assert.AreEqual(OBISUnit.A, telegram.CurrentL3!.Unit);
        Assert.AreEqual(1.111m, telegram.PowerDeliveredL1!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredL1!.Unit);
        Assert.AreEqual(2.222m, telegram.PowerDeliveredL2!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredL2!.Unit);
        Assert.AreEqual(3.333m, telegram.PowerDeliveredL3!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredL3!.Unit);
        Assert.AreEqual(4.444m, telegram.PowerReturnedL1!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturnedL1!.Unit);
        Assert.AreEqual(5.555m, telegram.PowerReturnedL2!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturnedL2!.Unit);
        Assert.AreEqual(6.666m, telegram.PowerReturnedL3!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturnedL3!.Unit);
        Assert.AreEqual(3, telegram.GasDeviceType);
        Assert.AreEqual("9876ABCD123456789", telegram.GasEquipmentId);
        Assert.AreEqual(new DateTimeOffset(2010, 12, 9, 11, 0, 0, TimeSpan.FromHours(1)), telegram.GasDelivered!.DateTime);
        Assert.AreEqual(12321.232m, telegram.GasDelivered!.Value!.Value);
        Assert.AreEqual(OBISUnit.m3, telegram.GasDelivered!.Value!.Unit);
    }

    [TestMethod]
    public void DSMRTelegramParser_Reads_V5_Telegrams()
    {
        var target = new DSMRTelegramParser();
        var telegram = target.Parse(File.ReadAllBytes(@"testdata/v5_ok.txt"));
        Assert.AreEqual(@"Test\V5-Telegram", telegram.Identification);
        Assert.AreEqual(50, telegram.DSMRVersion);
        Assert.AreEqual(new DateTimeOffset(2010, 12, 9, 11, 30, 20, TimeSpan.FromHours(1)), telegram.TimeStamp);
        Assert.AreEqual(new DateTimeOffset(2010, 12, 9, 11, 30, 20, TimeSpan.FromHours(1)), telegram.TimeStamp);
        Assert.AreEqual("K8EG004046395507", telegram.EquipmentId);
        Assert.AreEqual(123456.789m, telegram.EnergyDeliveredTariff1!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff1!.Unit);
        Assert.AreEqual(987654.321m, telegram.EnergyDeliveredTariff2!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff2!.Unit);
        Assert.AreEqual(101010.010m, telegram.EnergyReturnedTariff1!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff1!.Unit);
        Assert.AreEqual(020202.202m, telegram.EnergyReturnedTariff2!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff2!.Unit);
        Assert.AreEqual(2, telegram.ElectricityTariff);
        Assert.AreEqual(11.111m, telegram.PowerDelivered!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDelivered!.Unit);
        Assert.AreEqual(22.222m, telegram.PowerReturned!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturned!.Unit);
        Assert.AreEqual(15, telegram.ElectricityFailures);
        Assert.AreEqual(7, telegram.ElectricityLongFailures);

        var fl = telegram.ElectricityFailureLog.ToArray();
        Assert.AreEqual(3, fl.Length);
        Assert.AreEqual(TimeSpan.FromSeconds(237126), fl[0].Value);
        Assert.AreEqual(new DateTimeOffset(2000, 1, 4, 18, 03, 20, TimeSpan.FromHours(1)), fl[0].DateTime);
        Assert.AreEqual(TimeSpan.FromSeconds(2147583646), fl[1].Value);
        Assert.AreEqual(new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.FromHours(1)), fl[1].DateTime);
        Assert.AreEqual(TimeSpan.FromSeconds(2317482647), fl[2].Value);
        Assert.AreEqual(new DateTimeOffset(2020, 1, 2, 1, 2, 3, TimeSpan.FromHours(1)), fl[2].DateTime);
        Assert.AreEqual(1, telegram.ElectricitySagsL1);
        Assert.AreEqual(10, telegram.ElectricitySagsL2);
        Assert.AreEqual(100, telegram.ElectricitySagsL3);
        Assert.AreEqual(2, telegram.ElectricitySwellsL1);
        Assert.AreEqual(20, telegram.ElectricitySwellsL2);
        Assert.AreEqual(200, telegram.ElectricitySwellsL3);
        Assert.AreEqual("I like big butts and I can not lie", telegram.MessageShort);
        Assert.AreEqual("0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?0123456789:;<=>?", telegram.MessageLong);
        Assert.AreEqual(230.1m, telegram.VoltageL1!.Value);
        Assert.AreEqual(OBISUnit.V, telegram.VoltageL1!.Unit);
        Assert.AreEqual(230.0m, telegram.VoltageL2!.Value);
        Assert.AreEqual(OBISUnit.V, telegram.VoltageL2!.Unit);
        Assert.AreEqual(229.9m, telegram.VoltageL3!.Value);
        Assert.AreEqual(OBISUnit.V, telegram.VoltageL3!.Unit);
        Assert.AreEqual(1, telegram.CurrentL1!.Value);
        Assert.AreEqual(OBISUnit.A, telegram.CurrentL1!.Unit);
        Assert.AreEqual(2, telegram.CurrentL2!.Value);
        Assert.AreEqual(OBISUnit.A, telegram.CurrentL2!.Unit);
        Assert.AreEqual(3, telegram.CurrentL3!.Value);
        Assert.AreEqual(OBISUnit.A, telegram.CurrentL3!.Unit);
        Assert.AreEqual(1.111m, telegram.PowerDeliveredL1!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredL1!.Unit);
        Assert.AreEqual(2.222m, telegram.PowerDeliveredL2!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredL2!.Unit);
        Assert.AreEqual(3.333m, telegram.PowerDeliveredL3!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredL3!.Unit);
        Assert.AreEqual(4.444m, telegram.PowerReturnedL1!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturnedL1!.Unit);
        Assert.AreEqual(5.555m, telegram.PowerReturnedL2!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturnedL2!.Unit);
        Assert.AreEqual(6.666m, telegram.PowerReturnedL3!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturnedL3!.Unit);

        Assert.AreEqual(303, telegram.GasDeviceType);
        Assert.AreEqual("9876ABCD123456789", telegram.GasEquipmentId);
        Assert.AreEqual(new DateTimeOffset(2020, 12, 11, 10, 9, 8, TimeSpan.FromHours(1)), telegram.GasDelivered!.DateTime);
        Assert.AreEqual(12321.232m, telegram.GasDelivered!.Value!.Value);
        Assert.AreEqual(OBISUnit.m3, telegram.GasDelivered!.Value!.Unit);

        Assert.AreEqual(909, telegram.WaterDeviceType);
        Assert.AreEqual("0123ABCD123456789", telegram.WaterEquipmentId);
        Assert.AreEqual(new DateTimeOffset(2020, 12, 11, 10, 9, 7, TimeSpan.FromHours(1)), telegram.WaterDelivered!.DateTime);
        Assert.AreEqual(32123.212m, telegram.WaterDelivered!.Value!.Value);
        Assert.AreEqual(OBISUnit.m3, telegram.WaterDelivered!.Value!.Unit);

        Assert.AreEqual(808, telegram.ThermalDeviceType);
        Assert.AreEqual("9999ABCD123456789", telegram.ThermalEquipmentId);
        Assert.AreEqual(new DateTimeOffset(2020, 12, 11, 10, 9, 6, TimeSpan.FromHours(1)), telegram.ThermalDelivered!.DateTime);
        Assert.AreEqual(33221.122m, telegram.ThermalDelivered!.Value!.Value);
        Assert.AreEqual(OBISUnit.MJ, telegram.ThermalDelivered!.Value!.Unit);

        Assert.AreEqual(101, telegram.SlaveDeviceType);
        Assert.AreEqual("0000ABCD123456789", telegram.SlaveEquipmentId);
        Assert.AreEqual(new DateTimeOffset(2020, 12, 11, 10, 9, 5, TimeSpan.FromHours(1)), telegram.SlaveDelivered!.DateTime);
        Assert.AreEqual(11223.322m, telegram.SlaveDelivered!.Value!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.SlaveDelivered!.Value!.Unit);
    }

    [TestMethod]
    public void DSMRTelegramParser_Reads_V5_0_2_Flu_Telegrams()
    {
        var target = new DSMRTelegramParser();
        var telegram = target.Parse(File.ReadAllBytes(@"testdata\v5_02_flu_ok.txt"));
        Assert.AreEqual(@"FLU5\253770234_A", telegram.Identification);
        Assert.AreEqual(50217, telegram.DSMRVersion);
        Assert.AreEqual("K8EG004046395507", telegram.EquipmentId);
        Assert.AreEqual(new DateTimeOffset(2023, 01, 8, 22, 29, 30, TimeSpan.FromHours(1)), telegram.TimeStamp);

        Assert.AreEqual(3540.994m, telegram.EnergyDeliveredTariff1!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff1!.Unit);
        Assert.AreEqual(4343.832m, telegram.EnergyDeliveredTariff2!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyDeliveredTariff2!.Unit);
        Assert.AreEqual(447.900m, telegram.EnergyReturnedTariff1!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff1!.Unit);
        Assert.AreEqual(203.828m, telegram.EnergyReturnedTariff2!.Value);
        Assert.AreEqual(OBISUnit.kWh, telegram.EnergyReturnedTariff2!.Unit);

        Assert.AreEqual(2, telegram.ElectricityTariff);

        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredCurrentAvg!.Unit);
        Assert.AreEqual(0.554m, telegram.PowerDeliveredCurrentAvg!.Value);

        Assert.AreEqual(new DateTimeOffset(2023, 01, 7, 17, 15, 00, TimeSpan.FromHours(1)), telegram.EnergyDeliveredMaxRunningMonth!.DateTime);
        Assert.AreEqual(OBISUnit.kW, telegram.EnergyDeliveredMaxRunningMonth!.Value!.Unit);
        Assert.AreEqual(2.572m, telegram.EnergyDeliveredMaxRunningMonth!.Value!.Value);

        // TODO: Maximum demand – Active energy import of the last 13 months

        Assert.AreEqual(0.544m, telegram.PowerDelivered!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDelivered!.Unit);
        Assert.AreEqual(0.0m, telegram.PowerReturned!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDelivered!.Unit);

        Assert.AreEqual(0.544m, telegram.PowerDeliveredL1!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerDeliveredL1!.Unit);
        Assert.AreEqual(0.0m, telegram.PowerReturnedL1!.Value);
        Assert.AreEqual(OBISUnit.kW, telegram.PowerReturnedL1!.Unit);

        Assert.AreEqual(231.5m, telegram.VoltageL1!.Value);
        Assert.AreEqual(OBISUnit.V, telegram.VoltageL1!.Unit);

        Assert.AreEqual(2.85m, telegram.CurrentL1!.Value);
        Assert.AreEqual(OBISUnit.A, telegram.CurrentL1!.Unit);

        // TODO: add breaker state
        // TODO: add limiter threshold
        // TODO: add Fuse supervision threshold

        Assert.AreEqual(string.Empty, telegram.MessageLong);
        Assert.AreEqual(3, telegram.GasDeviceType);
        Assert.AreEqual("9999ABCD123456789", telegram.GasEquipmentId);
        Assert.AreEqual(1, telegram.GasValvePosition);
        Assert.AreEqual(new DateTimeOffset(2023, 01, 8, 22, 25, 01, TimeSpan.FromHours(1)), telegram.GasDelivered!.DateTime!.Value);
        Assert.AreEqual(4856.664m, telegram.GasDelivered!.Value!.Value);
        Assert.AreEqual(OBISUnit.m3, telegram.GasDelivered!.Value!.Unit);
    }
}