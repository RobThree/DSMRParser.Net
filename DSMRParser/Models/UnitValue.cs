namespace DSMRParser.Models
{
    public record UnitValue<T>
    {
        public T? Value { get; init; }
        public OBISUnit Unit { get; init; }

        public UnitValue(T? value, OBISUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public override string ToString() => $"{Value}{Unit}";
    }
}