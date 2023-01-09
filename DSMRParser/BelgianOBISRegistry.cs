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
}