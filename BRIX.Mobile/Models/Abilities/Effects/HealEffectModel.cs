using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public partial class HealEffectModel : EffectModelBase
    {
        public HealEffectModel() : this(new HealEffect()) { }

        public HealEffectModel(HealEffect model)
        {
            InternalModel = model;
            model.ForceAspectInitialize();
            UpdateAspects();
        }

        public HealEffect Internal => GetSpecificEffect<HealEffect>();
    }
}
