using BRIX.Library.Effects;
using BRIX.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public abstract partial class EffectModel : ObservableObject
    {
        public EffectBase InternalModel { get; set; }

        public T GetSpecificEffect<T>() where T : EffectBase
        {
            return InternalModel as T;
        }

        public virtual string Name => LibraryTextHelper.GetEffectName(this);

        public abstract string EffectString { get; }

        public abstract List<string> Aspects { get; }
    }
}
