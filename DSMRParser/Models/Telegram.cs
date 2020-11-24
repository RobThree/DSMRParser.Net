using DSMRParser.CRCHandling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DSMRParser.Models
{
    [DebuggerDisplay("{Identification,nq}@{TimeStamp,nq}")]
    public class Telegram
    {
        private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
        protected static readonly IReadOnlyDictionary<OBISId, IEnumerable<string?>> EMPTY = new ReadOnlyDictionary<OBISId, IEnumerable<string?>>(Array.Empty<OBISDescriptor>().ToDictionary(i => i.Id, i => Enumerable.Empty<string?>()));
        public string? Identification { get; init; }
        public IReadOnlyDictionary<OBISId, IEnumerable<string?>> Values { get; init; } = EMPTY;

        public Telegram(string? identification, IEnumerable<(OBISId obisid, IEnumerable<string?> values)> values)
        {
            Identification = identification ?? throw new ArgumentNullException(nameof(identification));
            Values = new ReadOnlyDictionary<OBISId, IEnumerable<string?>>(values.ToDictionary(i => i.obisid, i => i.values));
        }

        public string? GetByDescriptor(OBISDescriptor descriptor) => GetMultiByDescriptor(descriptor)?.FirstOrDefault();
        public IEnumerable<string?> GetMultiByDescriptor(OBISDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            return Values.TryGetValue(descriptor.Id, out var value) ? value : Array.Empty<string?>();
        }

        public int? DSMRVersion => ParseInt(OBISRegistry.DSMRVersion);
        public DateTimeOffset? TimeStamp => ParseTimeStamp(OBISRegistry.TimeStamp);
        public string? EquipmentId => DecodeString(GetByDescriptor(OBISRegistry.EquipmentId));
        public UnitValue<decimal>? EnergyDeliveredTariff1 => ParseDecimalUnit(OBISRegistry.EnergyDeliveredTariff1);
        public UnitValue<decimal>? EnergyDeliveredTariff2 => ParseDecimalUnit(OBISRegistry.EnergyDeliveredTariff2);
        public UnitValue<decimal>? EnergyReturnedTariff1 => ParseDecimalUnit(OBISRegistry.EnergyReturnedTariff1);
        public UnitValue<decimal>? EnergyReturnedTariff2 => ParseDecimalUnit(OBISRegistry.EnergyReturnedTariff2);
        public int? ElectricityTariff => ParseInt(OBISRegistry.ElectricityTariff);
        public UnitValue<decimal>? PowerDelivered => ParseDecimalUnit(OBISRegistry.PowerDelivered);
        public UnitValue<decimal>? PowerReturned => ParseDecimalUnit(OBISRegistry.PowerReturned);
        public UnitValue<decimal>? ElectricityThreshold => ParseDecimalUnit(OBISRegistry.ElectricityThreshold);
        public int? ElectricitySwitchPosition => ParseInt(OBISRegistry.ElectricitySwitchPosition);
        public int? ElectricityFailures => ParseInt(OBISRegistry.ElectricityFailures);
        public int? ElectricityLongFailures => ParseInt(OBISRegistry.ElectricityLongFailures);
        public IEnumerable<TimeStampedValue<TimeSpan>> ElectricityFailureLog => ParseTimeStampedValues(OBISRegistry.ElectricityFailureLog, (d, v) => TimeSpan.FromSeconds(ParseIntUnit(d, v)?.Value ?? 0), 2);
        public int? ElectricitySagsL1 => ParseInt(OBISRegistry.ElectricitySagsL1);
        public int? ElectricitySagsL2 => ParseInt(OBISRegistry.ElectricitySagsL2);
        public int? ElectricitySagsL3 => ParseInt(OBISRegistry.ElectricitySagsL3);
        public int? ElectricitySwellsL1 => ParseInt(OBISRegistry.ElectricitySwellsL1);
        public int? ElectricitySwellsL2 => ParseInt(OBISRegistry.ElectricitySwellsL2);
        public int? ElectricitySwellsL3 => ParseInt(OBISRegistry.ElectricitySwellsL3);
        public string? MessageShort => DecodeString(GetByDescriptor(OBISRegistry.MessageShort));
        public string? MessageLong => DecodeString(GetByDescriptor(OBISRegistry.MessageLong));
        public UnitValue<decimal>? VoltageL1 => ParseDecimalUnit(OBISRegistry.VoltageL1);
        public UnitValue<decimal>? VoltageL2 => ParseDecimalUnit(OBISRegistry.VoltageL2);
        public UnitValue<decimal>? VoltageL3 => ParseDecimalUnit(OBISRegistry.VoltageL3);
        public UnitValue<int>? CurrentL1 => ParseIntUnit(OBISRegistry.CurrentL1);
        public UnitValue<int>? CurrentL2 => ParseIntUnit(OBISRegistry.CurrentL2);
        public UnitValue<int>? CurrentL3 => ParseIntUnit(OBISRegistry.CurrentL3);
        public UnitValue<decimal>? PowerDeliveredL1 => ParseDecimalUnit(OBISRegistry.PowerDeliveredL1);
        public UnitValue<decimal>? PowerDeliveredL2 => ParseDecimalUnit(OBISRegistry.PowerDeliveredL2);
        public UnitValue<decimal>? PowerDeliveredL3 => ParseDecimalUnit(OBISRegistry.PowerDeliveredL3);
        public UnitValue<decimal>? PowerReturnedL1 => ParseDecimalUnit(OBISRegistry.PowerReturnedL1);
        public UnitValue<decimal>? PowerReturnedL2 => ParseDecimalUnit(OBISRegistry.PowerReturnedL2);
        public UnitValue<decimal>? PowerReturnedL3 => ParseDecimalUnit(OBISRegistry.PowerReturnedL3);
        public int? GasDeviceType => ParseInt(OBISRegistry.GasDeviceType);
        public string? GasEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.GasEquipmentId));
        public int? GasValvePosition => ParseInt(OBISRegistry.GasValvePosition);
        //TODO: Check unit/factor
        public TimeStampedValue<UnitValue<decimal>>? GasDelivered => ParseTimeStampedValues(OBISRegistry.GasDelivered, (d, v) => ParseDecimalUnit(d, v)).FirstOrDefault();
        public int? ThermalDeviceType => ParseInt(OBISRegistry.ThermalDeviceType);
        public string? ThermalEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.ThermalEquipmentId));
        public int? ThermalValvePosition => ParseInt(OBISRegistry.ThermalValvePosition);
        //TODO: Check unit/factor
        public TimeStampedValue<UnitValue<decimal>>? ThermalDelivered => ParseTimeStampedValues(OBISRegistry.ThermalDelivered, (d, v) => ParseDecimalUnit(d, v)).FirstOrDefault();
        public int? WaterDeviceType => ParseInt(OBISRegistry.WaterDeviceType);
        public string? WaterEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.WaterEquipmentId));
        public int? WaterValvePosition => ParseInt(OBISRegistry.WaterValvePosition);
        //TODO: Check unit/factor
        public TimeStampedValue<UnitValue<decimal>>? WaterDelivered => ParseTimeStampedValues(OBISRegistry.WaterDelivered, (d, v) => ParseDecimalUnit(d, v)).FirstOrDefault();
        public int? SlaveDeviceType => ParseInt(OBISRegistry.SlaveDeviceType);
        public string? SlaveEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.SlaveEquipmentId));
        public int? SlaveValvePosition => ParseInt(OBISRegistry.SlaveValvePosition);
        //TODO: Check unit/factor
        public TimeStampedValue<UnitValue<decimal>>? SlaveDelivered => ParseTimeStampedValues(OBISRegistry.SlaveDelivered, (d, v) => ParseDecimalUnit(d, v)).FirstOrDefault();

        //TODO: Check if W/S (Winter/Summer 🤪) has any effect / needs to be corrected
        protected static DateTimeOffset? ParseTimeStamp(string? value) => DateTimeOffset.TryParseExact(value?.TrimEnd('W', 'S'), "yyMMddHHmmss", _culture, DateTimeStyles.AssumeLocal, out var result) ? result : null;
        protected DateTimeOffset? ParseTimeStamp(OBISDescriptor descriptor) => ParseTimeStamp(GetByDescriptor(descriptor));
        protected UnitValue<decimal>? ParseDecimalUnit(OBISDescriptor descriptor) => ParseDecimalUnit(descriptor, GetByDescriptor(descriptor));
        protected static UnitValue<decimal>? ParseDecimalUnit(OBISDescriptor descriptor, string? obisValue)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            var (value, unit) = SplitValues(obisValue);
            if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, _culture, out var parsed) && IsCorrectUnit(descriptor.Unit, unit))
                return new UnitValue<decimal>(parsed * descriptor.Factor, descriptor.Unit);
            return null;
        }
        protected int? ParseInt(OBISDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            if (!int.TryParse(GetByDescriptor(descriptor), NumberStyles.Integer, _culture, out var result))
                return null;
            return (int)(result * descriptor.Factor);
        }
        protected UnitValue<int>? ParseIntUnit(OBISDescriptor descriptor) => ParseIntUnit(descriptor, GetByDescriptor(descriptor));
        protected static UnitValue<int>? ParseIntUnit(OBISDescriptor descriptor, string? obisValue)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            var (value, unit) = SplitValues(obisValue);
            if (int.TryParse(value, NumberStyles.Integer, _culture, out var parsed) && IsCorrectUnit(descriptor.Unit, unit))
                return new UnitValue<int>((int)(parsed * descriptor.Factor), descriptor.Unit);
            return null;
        }
        protected static bool IsCorrectUnit(OBISUnit expected, string? actual) => Enum.TryParse<OBISUnit>(actual, out var result) && expected == result;
        protected IEnumerable<TimeStampedValue<T>> ParseTimeStampedValues<T>(OBISDescriptor descriptor, Func<OBISDescriptor, string?, T?> valueSelector, int skip = 0)
        {
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            var values = GetMultiByDescriptor(descriptor).Skip(skip).ToArray();
            if (values.Length % 2 == 0)  // We should have an even number of values
            {
                for (var i = 0; i < values.Length; i += 2)
                    yield return new TimeStampedValue<T>(ParseTimeStamp(values[i]), valueSelector(descriptor, values[i + 1]));
            }
        }
        protected static (string? value, string? unit) SplitValues(string? value, char unitSeparator = '*')
        {
            if (string.IsNullOrEmpty(value))
                return (null, null);

            var d = value.LastIndexOf(unitSeparator);
            if (d >= 0)
                return (value[0..d], value[(d + 1)..]);
            return (value, null);
        }
        protected static string? DecodeString(string? value) => (!string.IsNullOrEmpty(value) && value.Length % 2 == 0) ? Encoding.ASCII.GetString(DecodeHexString(value).ToArray()) : value;
        protected static IEnumerable<byte> DecodeHexString(string? value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length % 2 == 0)
            {
                for (var i = 0; i < value.Length; i += 2)
                    if (byte.TryParse(value.Substring(i, 2), NumberStyles.HexNumber, _culture, out var result))
                        yield return result;
            }
        }

        public string AsString(ICRCCalculator? crcCalculator = null)
        {
            var values = string.Join("\r\n", Values.Select(kv => $"{kv.Key}({string.Join(")(", kv.Value)})"));
            var telegram = $"/{Identification}\r\n\r\n{values}\r\n!";
            var crc = (crcCalculator ?? ICRCCalculator.Default).CalculateCRC(Encoding.ASCII.GetBytes(telegram));
            return $"{telegram}{crc:X4}\r\n";
        }
    }
}