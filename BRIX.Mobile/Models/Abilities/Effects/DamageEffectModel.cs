using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.ViewModel.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public partial class DamageEffectModel : EffectModelBase
    {
        public DamageEffectModel() : this(new DamageEffect()) { }

        public DamageEffectModel(DamageEffect model)
        {
            InternalModel = model;
            model.ForceAspectInitialize();
            UpdateAspects();
        }

        public DamageEffect Internal => GetSpecificEffect<DamageEffect>();
    }
}
