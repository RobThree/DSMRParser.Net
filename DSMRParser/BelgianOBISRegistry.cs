using DSMRParser.Models;

namespace DSMRParser;

/// <summary>
/// OBIS registry of known <see cref="OBISDescriptor"/>s found in belgian DSMR <see cref="Telegram"/>s.
/// </summary>
public static class BelgianOBISRegistry
{
    /// <summary>The version of the DSMR telegram.</summary>
    public static readonly OBISDescriptor DSMRVersion = new() { Id = new OBISId(0, 0, 96, 1, 4), Description = "DSMR version" };
    /// <summary>Gas equipment identifier.</summary>
    public static readonly OBISDescriptor GasEquipmentId = new() { Id = new OBISId(0, 1, 96, 1, 1), Description = "Gas equipment id" };
    /// <summary>Gas delivered.</summary>
    public static readonly OBISDescriptor GasDelivered = new() { Id = new OBISId(0, 1, 24, 2, 3), Description = "Gas delivered", Unit = OBISUnit.m3 };
    /// <summary>Peak power current month.</summary>
    public static readonly OBISDescriptor PowerMaxCurrentAverage = new() { Id = new OBISId(1, 0, 1, 4, 0), Description = "Current average demand - active energy import", Unit = OBISUnit.kW };
    /// <summary>Peak power current month.</summary>
    public static readonly OBISDescriptor PowerDeliveredMaxRunningMonth = new() { Id = new OBISId(1, 0, 1, 6, 0), Description = "Peak power running Month", Unit = OBISUnit.kW};
}