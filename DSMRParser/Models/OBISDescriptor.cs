using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace DSMRParser.Models
{
    [DebuggerDisplay("{Id} ({Description,nq})")]
    public record OBISDescriptor
    {
        public static readonly IReadOnlyDictionary<OBISId, OBISDescriptor> KNOWNDESCRIPTORS = new ReadOnlyDictionary<OBISId, OBISDescriptor>(
            typeof(OBISRegistry).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(t => t.FieldType == typeof(OBISDescriptor))
                .Select(p => p.GetValue(null))
                .Cast<OBISDescriptor>()
                .ToDictionary(d => d.Id)
        );

        public OBISId Id { get; init; } = OBISId.NONE;
        public string Description { get; init; } = "UNKNOWN";
        public OBISUnit Unit { get; init; } = OBISUnit.NONE;
        public decimal Factor { get; init; } = 1;

        public static OBISDescriptor? GetKnownDescriptor(OBISId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return TryGetKnownDescriptor(id, out var result) ? result : throw new KeyNotFoundException(id.ToString());
        }

        public static bool TryGetKnownDescriptor(OBISId id, [NotNullWhen(true)] out OBISDescriptor? result)
        {
            if (KNOWNDESCRIPTORS.TryGetValue(id, out var t))
            {
                result = t;
                return true;
            }
            result = null;
            return false;
        }
    }
}