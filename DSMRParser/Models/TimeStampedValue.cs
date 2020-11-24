using System;

namespace DSMRParser.Models
{
    public record TimeStampedValue<T>
    {
        public DateTimeOffset? DateTime { get; init; }
        public T? Value { get; init; }

        public TimeStampedValue(DateTimeOffset? dateTimeOffset, T? value)
        {
            DateTime = dateTimeOffset;
            Value = value;
        }
        public override string ToString() => $"{DateTime:O}: {Value}";
    }
}