using DSMRParser.Models;

namespace DSMRParser
{
    public static class OBISRegistry
    {
        public static readonly OBISDescriptor DSMRVersion = new OBISDescriptor { Id = new OBISId(1, 3, 0, 2, 8), Description = "DSMR version" };
        public static readonly OBISDescriptor TimeStamp = new OBISDescriptor { Id = new OBISId(0, 0, 1, 0, 0), Description = "Timestamp" };
        public static readonly OBISDescriptor EquipmentId = new OBISDescriptor { Id = new OBISId(0, 0, 96, 1, 1), Description = "Equipment ID" };
        public static readonly OBISDescriptor EnergyDeliveredTariff1 = new OBISDescriptor { Id = new OBISId(1, 0, 1, 8, 1), Description = "Energy delivered (Tariff 1)", Unit = OBISUnit.kWh };
        public static readonly OBISDescriptor EnergyDeliveredTariff2 = new OBISDescriptor { Id = new OBISId(1, 0, 1, 8, 2), Description = "Energy delivered (Tariff 2)", Unit = OBISUnit.kWh };
        public static readonly OBISDescriptor EnergyReturnedTariff1 = new OBISDescriptor { Id = new OBISId(1, 0, 2, 8, 1), Description = "Energy returned (Tariff 1)", Unit = OBISUnit.kWh };
        public static readonly OBISDescriptor EnergyReturnedTariff2 = new OBISDescriptor { Id = new OBISId(1, 0, 2, 8, 2), Description = "Energy returned (Tariff 2)", Unit = OBISUnit.kWh };
        public static readonly OBISDescriptor ElectricityTariff = new OBISDescriptor { Id = new OBISId(0, 0, 96, 14, 0), Description = "Electricity tariff" };
        public static readonly OBISDescriptor PowerDelivered = new OBISDescriptor { Id = new OBISId(1, 0, 1, 7, 0), Description = "Power delivered", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor PowerReturned = new OBISDescriptor { Id = new OBISId(1, 0, 2, 7, 0), Description = "Power returned", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor ElectricityThreshold = new OBISDescriptor { Id = new OBISId(0, 0, 17, 0, 0), Description = "Electricity threshold", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor ElectricitySwitchPosition = new OBISDescriptor { Id = new OBISId(0, 0, 96, 3, 10), Description = "Electricity switch position" };
        public static readonly OBISDescriptor ElectricityFailures = new OBISDescriptor { Id = new OBISId(0, 0, 96, 7, 21), Description = "Electricity failures" };
        public static readonly OBISDescriptor ElectricityLongFailures = new OBISDescriptor { Id = new OBISId(0, 0, 96, 7, 9), Description = "Electricity long failures" };
        public static readonly OBISDescriptor ElectricityFailureLog = new OBISDescriptor { Id = new OBISId(1, 0, 99, 97, 0), Description = "Electricity failure log", Unit = OBISUnit.s };
        public static readonly OBISDescriptor ElectricitySagsL1 = new OBISDescriptor { Id = new OBISId(1, 0, 32, 32, 0), Description = "Electricity sags l1" };
        public static readonly OBISDescriptor ElectricitySagsL2 = new OBISDescriptor { Id = new OBISId(1, 0, 52, 32, 0), Description = "Electricity sags l2" };
        public static readonly OBISDescriptor ElectricitySagsL3 = new OBISDescriptor { Id = new OBISId(1, 0, 72, 32, 0), Description = "Electricity sags l3" };
        public static readonly OBISDescriptor ElectricitySwellsL1 = new OBISDescriptor { Id = new OBISId(1, 0, 32, 36, 0), Description = "Electricity swells l1" };
        public static readonly OBISDescriptor ElectricitySwellsL2 = new OBISDescriptor { Id = new OBISId(1, 0, 52, 36, 0), Description = "Electricity swells l2" };
        public static readonly OBISDescriptor ElectricitySwellsL3 = new OBISDescriptor { Id = new OBISId(1, 0, 72, 36, 0), Description = "Electricity swells l3" };
        public static readonly OBISDescriptor MessageShort = new OBISDescriptor { Id = new OBISId(0, 0, 96, 13, 1), Description = "Message short" };
        public static readonly OBISDescriptor MessageLong = new OBISDescriptor { Id = new OBISId(0, 0, 96, 13, 0), Description = "Message long" };
        public static readonly OBISDescriptor VoltageL1 = new OBISDescriptor { Id = new OBISId(1, 0, 32, 7, 0), Description = "Voltage l1", Unit = OBISUnit.V };
        public static readonly OBISDescriptor VoltageL2 = new OBISDescriptor { Id = new OBISId(1, 0, 52, 7, 0), Description = "Voltage l2", Unit = OBISUnit.V };
        public static readonly OBISDescriptor VoltageL3 = new OBISDescriptor { Id = new OBISId(1, 0, 72, 7, 0), Description = "Voltage l3", Unit = OBISUnit.V };
        public static readonly OBISDescriptor CurrentL1 = new OBISDescriptor { Id = new OBISId(1, 0, 31, 7, 0), Description = "Current l1", Unit = OBISUnit.A };
        public static readonly OBISDescriptor CurrentL2 = new OBISDescriptor { Id = new OBISId(1, 0, 51, 7, 0), Description = "Current l2", Unit = OBISUnit.A };
        public static readonly OBISDescriptor CurrentL3 = new OBISDescriptor { Id = new OBISId(1, 0, 71, 7, 0), Description = "Current l3", Unit = OBISUnit.A };
        public static readonly OBISDescriptor PowerDeliveredL1 = new OBISDescriptor { Id = new OBISId(1, 0, 21, 7, 0), Description = "Power delivered l1", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor PowerDeliveredL2 = new OBISDescriptor { Id = new OBISId(1, 0, 41, 7, 0), Description = "Power delivered l2", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor PowerDeliveredL3 = new OBISDescriptor { Id = new OBISId(1, 0, 61, 7, 0), Description = "Power delivered l3", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor PowerReturnedL1 = new OBISDescriptor { Id = new OBISId(1, 0, 22, 7, 0), Description = "Power returned l1", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor PowerReturnedL2 = new OBISDescriptor { Id = new OBISId(1, 0, 42, 7, 0), Description = "Power returned l2", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor PowerReturnedL3 = new OBISDescriptor { Id = new OBISId(1, 0, 62, 7, 0), Description = "Power returned l3", Unit = OBISUnit.kW };
        public static readonly OBISDescriptor GasDeviceType = new OBISDescriptor { Id = new OBISId(0, GAS_MBUS_ID, 24, 1, 0), Description = "Gas device type" };
        public static readonly OBISDescriptor GasEquipmentId = new OBISDescriptor { Id = new OBISId(0, GAS_MBUS_ID, 96, 1, 0), Description = "Gas equipment id" };
        public static readonly OBISDescriptor GasValvePosition = new OBISDescriptor { Id = new OBISId(0, GAS_MBUS_ID, 24, 4, 0), Description = "Gas valve position" };
        public static readonly OBISDescriptor GasDelivered = new OBISDescriptor { Id = new OBISId(0, GAS_MBUS_ID, 24, 2, 1), Description = "Gas delivered", Unit = OBISUnit.m3 };
        public static readonly OBISDescriptor ThermalDeviceType = new OBISDescriptor { Id = new OBISId(0, THERMAL_MBUS_ID, 24, 1, 0), Description = "Thermal device type" };
        public static readonly OBISDescriptor ThermalEquipmentId = new OBISDescriptor { Id = new OBISId(0, THERMAL_MBUS_ID, 96, 1, 0), Description = "Thermal equipment id" };
        public static readonly OBISDescriptor ThermalValvePosition = new OBISDescriptor { Id = new OBISId(0, THERMAL_MBUS_ID, 24, 4, 0), Description = "Thermal valve position" };
        public static readonly OBISDescriptor ThermalDelivered = new OBISDescriptor { Id = new OBISId(0, THERMAL_MBUS_ID, 24, 2, 1), Description = "Thermal delivered", Unit = OBISUnit.MJ };
        public static readonly OBISDescriptor WaterDeviceType = new OBISDescriptor { Id = new OBISId(0, WATER_MBUS_ID, 24, 1, 0), Description = "Water device type" };
        public static readonly OBISDescriptor WaterEquipmentId = new OBISDescriptor { Id = new OBISId(0, WATER_MBUS_ID, 96, 1, 0), Description = "Water equipment id" };
        public static readonly OBISDescriptor WaterValvePosition = new OBISDescriptor { Id = new OBISId(0, WATER_MBUS_ID, 24, 4, 0), Description = "Water valve position" };
        public static readonly OBISDescriptor WaterDelivered = new OBISDescriptor { Id = new OBISId(0, WATER_MBUS_ID, 24, 2, 1), Description = "Water delivered", Unit = OBISUnit.m3 };
        public static readonly OBISDescriptor SlaveDeviceType = new OBISDescriptor { Id = new OBISId(0, SLAVE_MBUS_ID, 24, 1, 0), Description = "Slave device type" };
        public static readonly OBISDescriptor SlaveEquipmentId = new OBISDescriptor { Id = new OBISId(0, SLAVE_MBUS_ID, 96, 1, 0), Description = "Slave equipment id" };
        public static readonly OBISDescriptor SlaveValvePosition = new OBISDescriptor { Id = new OBISId(0, SLAVE_MBUS_ID, 24, 4, 0), Description = "Slave valve position" };
        public static readonly OBISDescriptor SlaveDelivered = new OBISDescriptor { Id = new OBISId(0, SLAVE_MBUS_ID, 24, 2, 1), Description = "Slave delivered", Unit = OBISUnit.m3 };

        private const int GAS_MBUS_ID = 1;
        private const int WATER_MBUS_ID = 2;
        private const int THERMAL_MBUS_ID = 3;
        private const int SLAVE_MBUS_ID = 4;
    }
}
