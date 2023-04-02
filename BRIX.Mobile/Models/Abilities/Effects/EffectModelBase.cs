using BRIX.Lexica;
using BRIX.Lexis;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
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

        public string Name => EffectsDictionary.Collection[GetType()].Name;

        public string Description 
        {
            get
            {
                ILocalizationResourceManager localization = ServicePool.GetService<ILocalizationResourceManager>();
                ELexisLanguage language = localization.LexisLanguage;

                return EffectLexis.GetEffectDescription(InternalModel, language);
            }
        }

        public List<AspectModelBase> Aspects { get; protected set; }

        public AspectModelBase GetAspect(Type aspectType)
        {
            return Aspects.FirstOrDefault(x => x.InternalModel.GetType() == aspectType);
        }
    }
}
