namespace BRIX.Library.Items
{
    /// <summary>
    /// Предмет из инвентаря типа «Вещь»
    /// </summary>
    public class Item
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Count { get; set; } = 1;

        public override string ToString() => Name;

        public override bool Equals(object? otherObject)
        {
            if (otherObject is not Item other)
            {
                return false;
            }

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
