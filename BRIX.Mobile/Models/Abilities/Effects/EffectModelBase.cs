using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public abstract partial class EffectModelBase : ObservableObject
    {
        public EffectBase? InternalModel { get; set; }

        public T GetSpecificEffect<T>() where T : EffectBase
        {
            return InternalModel != null
                ? (T)InternalModel
                : throw new Exception("InternalModel не инициализирован.");
        }

        public string Name => EffectsDictionary.Collection[GetType()].Name;

        public string Description 
        {
            get
            {
                return InternalModel != null
                    ? InternalModel.ToLexis()
                    : string.Empty;
            }
        }

        public List<AspectModelBase> Aspects { get; protected set; } = [];

        public AspectModelBase GetAspect(Type aspectType)
        {
            return Aspects.Single(x => x.InternalBase.GetType() == aspectType);
        }

        public void UpdateAspect(AspectModelBase aspect)
        {
            InternalModel?.SetAspect(aspect.InternalBase);
            UpdateAspects();
        }

        public void UpdateAspects()
        {
            if(InternalModel == null)
            {
                return;
            }

            Aspects = InternalModel.Aspects
                .Select(AspectModelFactory.GetAspectModel)
                .Where(x => x != null)
                .ToList();
        }
    }
}
