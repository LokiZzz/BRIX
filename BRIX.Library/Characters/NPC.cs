namespace BRIX.Library.Characters
{
    public class NPC : CharacterBase
    {
        public int Health { get; set; }

        public override int RawMaxHealth => Health;

        public string Description { get; set; } = string.Empty;
    }
}