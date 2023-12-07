namespace BRIX.Mobile.ViewModel.Abilities
{
    public class EffectTypeVM
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public Type? EditPage { get; set; }
        public bool ForStatus { get; set; }
    }
}
