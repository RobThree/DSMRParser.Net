namespace DSMRParser.Models;

/// <summary>
/// Specifies the OBIS units encountered in DSMR telegrams.
/// </summary>
public enum OBISUnit
{
    /// <summary>Unit-less</summary>
    NONE,
    /// <summary>Watts</summary>
    W,
    /// <summary>KiloWatts</summary>
    kW,
    /// <summary>WattHours</summary>
    Wh,
    /// <summary>KiloWattHours</summary>
    kWh,
    /// <summary>Volts</summary>
    V,
    /// <summary>MilliVolts</summary>
    mV,
    /// <summary>Ampéres</summary>
    A,
    /// <summary>MilliAmpéres</summary>
    mA,
    /// <summary>Cubic meters (m³)</summary>
    m3,
    /// <summary>Cubic decimeters (dm³)</summary>
    dm3,
    /// <summary>GigaJoules</summary>
    GJ,
    /// <summary>MegaJoules</summary>
    MJ,
    /// <summary>Seconds</summary>
    s
}