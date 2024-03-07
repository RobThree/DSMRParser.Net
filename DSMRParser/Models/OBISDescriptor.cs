using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace DSMRParser.Models;

/// <summary>
/// Used to describe a value found in a <see cref="Telegram"/> based on an <see cref="OBISId"/>.
/// </summary>
[DebuggerDisplay("{Id} ({Description,nq})")]
public record OBISDescriptor
{
    /// <summary>
    /// Gets an <see cref="IReadOnlyDictionary{TKey, TValue}"/> of known DSMR <see cref="OBISDescriptor"/>s from a registry of known values.
    /// </summary>
    public static readonly IReadOnlyDictionary<OBISId, OBISDescriptor> KNOWNDESCRIPTORS = new ReadOnlyDictionary<OBISId, OBISDescriptor>(
        typeof(OBISRegistry)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(t => t.FieldType == typeof(OBISDescriptor))
            .Select(p => p.GetValue(null))
            .Cast<OBISDescriptor>()
            .ToDictionary(d => d.Id)
    );

    /// <summary>
    /// Gets the <see cref="OBISId"/> for the value.
    /// </summary>
    public OBISId Id { get; init; } = OBISId.NONE;
    /// <summary>
    /// Get the description of the value.
    /// </summary>
    public string Description { get; init; } = "UNKNOWN";
    /// <summary>
    /// Gets the <see cref="OBISUnit"/> of the value.
    /// </summary>
    public OBISUnit Unit { get; init; } = OBISUnit.NONE;
    /// <summary>
    /// Gets the scaling factor of the value.
    /// </summary>
    public decimal Factor { get; init; } = 1;

    /// <summary>
    /// Resolves a given <see cref="OBISId"/> to a known descriptor.
    /// </summary>
    /// <param name="id">An <see cref="OBISId"/> containing the OBIS ID to find the descriptor for.</param>
    /// <returns>An <see cref="OBISDescriptor"/> that represents the given <paramref name="id"/></returns>
    /// <exception cref="KeyNotFoundException">Thrown when the given <see cref="OBISId"/> could not be found.</exception>
    public static OBISDescriptor? GetKnownDescriptor(OBISId id) => id is null
            ? throw new ArgumentNullException(nameof(id))
            : TryGetKnownDescriptor(id, out var result) ? result : throw new KeyNotFoundException(id.ToString());

    /// <summary>
    /// Attempts to resolve a given <see cref="OBISId"/> to a known descriptor.
    /// </summary>
    /// <param name="id">An <see cref="OBISId"/> containing the OBIS ID to find the descriptor for.</param>
    /// <param name="result">
    /// An <see cref="OBISDescriptor"/> that represents the given <paramref name="id"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="OBISDescriptor"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
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

    /// <summary>
    /// Creates an OBISDescriptor from an <see cref="OBISId"/>.
    /// </summary>
    /// <param name="obisId">The OBIS ID to create from.</param>
    /// <returns>The created <see cref="OBISDescriptor"/>.</returns>
    public static OBISDescriptor FromOBISId(OBISId obisId) => CreateDescriptor(obisId);

    /// <summary>
    /// Creates an OBISDescriptor from an <see cref="OBISId"/>.
    /// </summary>
    /// <param name="obisId">The OBIS ID to create from.</param>
    public static implicit operator OBISDescriptor(OBISId obisId) => FromOBISId(obisId);

    /// <summary>
    /// Creates and returns an descriptor for a given ObisID.
    /// </summary>
    /// <param name="obisId">The OBIS ID of the descriptor.</param>
    /// <param name="description">The description of the value.</param>
    /// <param name="factor">The the scaling factor of the the value.</param>
    /// <param name="unit">Tthe <see cref="OBISUnit"/> of the value.</param>
    /// <returns>The created <see cref="OBISDescriptor"/>.</returns>
    public static OBISDescriptor CreateDescriptor(OBISId obisId, string description = "UNKNOWN", decimal factor = 1, OBISUnit unit = OBISUnit.NONE)
        => new()
        { Id = obisId, Description = description, Factor = factor, Unit = unit };
}