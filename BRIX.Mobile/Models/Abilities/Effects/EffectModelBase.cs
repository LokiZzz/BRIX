using BRIX.Library.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public partial class EffectModelBase<TEffect> : ObservableObject where TEffect : EffectBase, new()
    {
        public EffectModelBase() : this(new TEffect()) { }

        public EffectModelBase(TEffect effect) => InternalModel = effect;

        public TEffect InternalModel { get; }
    }
}
