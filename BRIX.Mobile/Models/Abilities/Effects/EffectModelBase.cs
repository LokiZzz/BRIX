using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public abstract partial class EffectModelBase : ObservableObject
    {
        public EffectBase InternalModel { get; set; }

        public T GetSpecificEffect<T>() where T : EffectBase
        {
            return InternalModel as T;
        }

        public virtual string Name => EffectsDictionary.Collection[GetType()].Name;

        public abstract string EffectString { get; }

        public List<AspectModelBase> Aspects { get; protected set; }

        public AspectModelBase GetAspect(Type aspectType)
        {
            return Aspects.FirstOrDefault(x => x.InternalModel.GetType() == aspectType);
        }
    }
}
