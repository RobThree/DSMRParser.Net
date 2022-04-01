using DSMRParser.Models;

namespace DSMRParser;

/// <summary>
/// Default registry of known <see cref="OBISDescriptor"/>s found in DSMR <see cref="Telegram"/>s.
/// </summary>
public static class OBISRegistry
{
    /// <summary>The version of the DSMR telegram.</summary>
    public static readonly OBISDescriptor DSMRVersion = new() { Id = new OBISId(1, 3, 0, 2, 8), Description = "DSMR version" };
    /// <summary>The timestamp of the DSMR telegram.</summary>
    public static readonly OBISDescriptor TimeStamp = new() { Id = new OBISId(0, 0, 1, 0, 0), Description = "Timestamp" };
    /// <summary>Equipment identifier.</summary>
    public static readonly OBISDescriptor EquipmentId = new() { Id = new OBISId(0, 0, 96, 1, 1), Description = "Equipment ID" };
    /// <summary>Meter Reading electricity delivered to client (Tariff 1).</summary>
    public static readonly OBISDescriptor EnergyDeliveredTariff1 = new() { Id = new OBISId(1, 0, 1, 8, 1), Description = "Energy delivered (Tariff 1)", Unit = OBISUnit.kWh };
    /// <summary>Meter Reading electricity delivered to client (Tariff 2).</summary>
    public static readonly OBISDescriptor EnergyDeliveredTariff2 = new() { Id = new OBISId(1, 0, 1, 8, 2), Description = "Energy delivered (Tariff 2)", Unit = OBISUnit.kWh };
    /// <summary>Electricity returned (Tariff 1).</summary>
    public static readonly OBISDescriptor EnergyReturnedTariff1 = new() { Id = new OBISId(1, 0, 2, 8, 1), Description = "Energy returned (Tariff 1)", Unit = OBISUnit.kWh };
    /// <summary>Electricity returned (Tariff 2).</summary>
    public static readonly OBISDescriptor EnergyReturnedTariff2 = new() { Id = new OBISId(1, 0, 2, 8, 2), Description = "Energy returned (Tariff 2)", Unit = OBISUnit.kWh };
    /// <summary>Tariff indicator electricity.</summary>
    public static readonly OBISDescriptor ElectricityTariff = new() { Id = new OBISId(0, 0, 96, 14, 0), Description = "Electricity tariff" };
    /// <summary>Actual electricity power delivered.</summary>
    public static readonly OBISDescriptor PowerDelivered = new() { Id = new OBISId(1, 0, 1, 7, 0), Description = "Power delivered", Unit = OBISUnit.kW };
    /// <summary>Actual electricity power returned.</summary>
    public static readonly OBISDescriptor PowerReturned = new() { Id = new OBISId(1, 0, 2, 7, 0), Description = "Power returned", Unit = OBISUnit.kW };
    /// <summary>The actual threshold Electricity.</summary>
    public static readonly OBISDescriptor ElectricityThreshold = new() { Id = new OBISId(0, 0, 17, 0, 0), Description = "Electricity threshold", Unit = OBISUnit.kW };
    /// <summary>Switch position Electricity.</summary>
    public static readonly OBISDescriptor ElectricitySwitchPosition = new() { Id = new OBISId(0, 0, 96, 3, 10), Description = "Electricity switch position" };
    /// <summary>Number of power failures in any phase.</summary>
    public static readonly OBISDescriptor ElectricityFailures = new() { Id = new OBISId(0, 0, 96, 7, 21), Description = "Electricity failures" };
    /// <summary>Number of long power failures in any phase.</summary>
    public static readonly OBISDescriptor ElectricityLongFailures = new() { Id = new OBISId(0, 0, 96, 7, 9), Description = "Electricity long failures" };
    /// <summary>Power Failure Event Log.</summary>
    public static readonly OBISDescriptor ElectricityFailureLog = new() { Id = new OBISId(1, 0, 99, 97, 0), Description = "Electricity failure log", Unit = OBISUnit.s };
    /// <summary>Number of voltage sags in phase L1.</summary>
    public static readonly OBISDescriptor ElectricitySagsL1 = new() { Id = new OBISId(1, 0, 32, 32, 0), Description = "Electricity sags l1" };
    /// <summary>Number of voltage sags in phase L2.</summary>
    public static readonly OBISDescriptor ElectricitySagsL2 = new() { Id = new OBISId(1, 0, 52, 32, 0), Description = "Electricity sags l2" };
    /// <summary>Number of voltage sags in phase L3.</summary>
    public static readonly OBISDescriptor ElectricitySagsL3 = new() { Id = new OBISId(1, 0, 72, 32, 0), Description = "Electricity sags l3" };
    /// <summary>Number of voltage swells in phase L1.</summary>
    public static readonly OBISDescriptor ElectricitySwellsL1 = new() { Id = new OBISId(1, 0, 32, 36, 0), Description = "Electricity swells l1" };
    /// <summary>Number of voltage swells in phase L2.</summary>
    public static readonly OBISDescriptor ElectricitySwellsL2 = new() { Id = new OBISId(1, 0, 52, 36, 0), Description = "Electricity swells l2" };
    /// <summary>Number of voltage swells in phase L3.</summary>
    public static readonly OBISDescriptor ElectricitySwellsL3 = new() { Id = new OBISId(1, 0, 72, 36, 0), Description = "Electricity swells l3" };
    /// <summary>Short text message.</summary>
    public static readonly OBISDescriptor MessageShort = new() { Id = new OBISId(0, 0, 96, 13, 1), Description = "Message short" };
    /// <summary>Long text message.</summary>
    public static readonly OBISDescriptor MessageLong = new() { Id = new OBISId(0, 0, 96, 13, 0), Description = "Message long" };
    /// <summary>Instantaneous voltage L1.</summary>
    public static readonly OBISDescriptor VoltageL1 = new() { Id = new OBISId(1, 0, 32, 7, 0), Description = "Voltage l1", Unit = OBISUnit.V };
    /// <summary>Instantaneous voltage L2.</summary>
    public static readonly OBISDescriptor VoltageL2 = new() { Id = new OBISId(1, 0, 52, 7, 0), Description = "Voltage l2", Unit = OBISUnit.V };
    /// <summary>Instantaneous voltage L3.</summary>
    public static readonly OBISDescriptor VoltageL3 = new() { Id = new OBISId(1, 0, 72, 7, 0), Description = "Voltage l3", Unit = OBISUnit.V };
    /// <summary>Instantaneous current L1.</summary>
    public static readonly OBISDescriptor CurrentL1 = new() { Id = new OBISId(1, 0, 31, 7, 0), Description = "Current l1", Unit = OBISUnit.A };
    /// <summary>Instantaneous current L2.</summary>
    public static readonly OBISDescriptor CurrentL2 = new() { Id = new OBISId(1, 0, 51, 7, 0), Description = "Current l2", Unit = OBISUnit.A };
    /// <summary>Instantaneous current L3.</summary>
    public static readonly OBISDescriptor CurrentL3 = new() { Id = new OBISId(1, 0, 71, 7, 0), Description = "Current l3", Unit = OBISUnit.A };
    /// <summary>Instantaneous active power L1.</summary>
    public static readonly OBISDescriptor PowerDeliveredL1 = new() { Id = new OBISId(1, 0, 21, 7, 0), Description = "Power delivered l1", Unit = OBISUnit.kW };
    /// <summary>Instantaneous active power L2.</summary>
    public static readonly OBISDescriptor PowerDeliveredL2 = new() { Id = new OBISId(1, 0, 41, 7, 0), Description = "Power delivered l2", Unit = OBISUnit.kW };
    /// <summary>Instantaneous active power L3.</summary>
    public static readonly OBISDescriptor PowerDeliveredL3 = new() { Id = new OBISId(1, 0, 61, 7, 0), Description = "Power delivered l3", Unit = OBISUnit.kW };
    /// <summary>Instantaneous active power returned L1.</summary>
    public static readonly OBISDescriptor PowerReturnedL1 = new() { Id = new OBISId(1, 0, 22, 7, 0), Description = "Power returned l1", Unit = OBISUnit.kW };
    /// <summary>Instantaneous active power returned L2.</summary>
    public static readonly OBISDescriptor PowerReturnedL2 = new() { Id = new OBISId(1, 0, 42, 7, 0), Description = "Power returned l2", Unit = OBISUnit.kW };
    /// <summary>Instantaneous active power returned L3.</summary>
    public static readonly OBISDescriptor PowerReturnedL3 = new() { Id = new OBISId(1, 0, 62, 7, 0), Description = "Power returned l3", Unit = OBISUnit.kW };
    /// <summary>Gas devicetype.</summary>
    public static readonly OBISDescriptor GasDeviceType = new() { Id = new OBISId(0, GAS_MBUS_ID, 24, 1, 0), Description = "Gas device type" };
    /// <summary>Gas equipment identifier.</summary>
    public static readonly OBISDescriptor GasEquipmentId = new() { Id = new OBISId(0, GAS_MBUS_ID, 96, 1, 0), Description = "Gas equipment id" };
    /// <summary>Gas valve position.</summary>
    public static readonly OBISDescriptor GasValvePosition = new() { Id = new OBISId(0, GAS_MBUS_ID, 24, 4, 0), Description = "Gas valve position" };
    /// <summary>Gas delivered.</summary>
    public static readonly OBISDescriptor GasDelivered = new() { Id = new OBISId(0, GAS_MBUS_ID, 24, 2, 1), Description = "Gas delivered", Unit = OBISUnit.m3 };
    /// <summary>Gas delivered for V2.2 messages.</summary>
    public static readonly OBISDescriptor GasDeliveredOld = new() { Id = new OBISId(0, GAS_MBUS_ID, 24, 3, 0), Description = "Gas delivered", Unit = OBISUnit.m3 };
    /// <summary>Thermal devicetype.</summary>
    public static readonly OBISDescriptor ThermalDeviceType = new() { Id = new OBISId(0, THERMAL_MBUS_ID, 24, 1, 0), Description = "Thermal device type" };
    /// <summary>Thermal equipment identifier.</summary>
    public static readonly OBISDescriptor ThermalEquipmentId = new() { Id = new OBISId(0, THERMAL_MBUS_ID, 96, 1, 0), Description = "Thermal equipment id" };
    /// <summary>Thermal valve position.</summary>
    public static readonly OBISDescriptor ThermalValvePosition = new() { Id = new OBISId(0, THERMAL_MBUS_ID, 24, 4, 0), Description = "Thermal valve position" };
    /// <summary>Thermal energy delivered.</summary>
    public static readonly OBISDescriptor ThermalDelivered = new() { Id = new OBISId(0, THERMAL_MBUS_ID, 24, 2, 1), Description = "Thermal delivered", Unit = OBISUnit.MJ };
    /// <summary>Water devicetype.</summary>
    public static readonly OBISDescriptor WaterDeviceType = new() { Id = new OBISId(0, WATER_MBUS_ID, 24, 1, 0), Description = "Water device type" };
    /// <summary>Water equipment identifier.</summary>
    public static readonly OBISDescriptor WaterEquipmentId = new() { Id = new OBISId(0, WATER_MBUS_ID, 96, 1, 0), Description = "Water equipment id" };
    /// <summary>Water valve position.</summary>
    public static readonly OBISDescriptor WaterValvePosition = new() { Id = new OBISId(0, WATER_MBUS_ID, 24, 4, 0), Description = "Water valve position" };
    /// <summary>Water delivered.</summary>
    public static readonly OBISDescriptor WaterDelivered = new() { Id = new OBISId(0, WATER_MBUS_ID, 24, 2, 1), Description = "Water delivered", Unit = OBISUnit.m3 };
    /// <summary>Slave devicetype.</summary>
    public static readonly OBISDescriptor SlaveDeviceType = new() { Id = new OBISId(0, SLAVE_MBUS_ID, 24, 1, 0), Description = "Slave device type" };
    /// <summary>Slave equipment identifier.</summary>
    public static readonly OBISDescriptor SlaveEquipmentId = new() { Id = new OBISId(0, SLAVE_MBUS_ID, 96, 1, 0), Description = "Slave equipment id" };
    /// <summary>Slave valve position.</summary>
    public static readonly OBISDescriptor SlaveValvePosition = new() { Id = new OBISId(0, SLAVE_MBUS_ID, 24, 4, 0), Description = "Slave valve position" };
    /// <summary>Slave delivered value.</summary>
    public static readonly OBISDescriptor SlaveDelivered = new() { Id = new OBISId(0, SLAVE_MBUS_ID, 24, 2, 1), Description = "Slave delivered", Unit = OBISUnit.kWh };

    private const int GAS_MBUS_ID = 1;
    private const int WATER_MBUS_ID = 2;
    private const int THERMAL_MBUS_ID = 3;
    private const int SLAVE_MBUS_ID = 4;
}