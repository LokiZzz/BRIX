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
            DiceChunks = new(DiceFormulaChunkVM.GetChunks(Impact));
            model.ForceAspectInitialize();
            UpdateAspects();
        }

        public DamageEffect Internal => GetSpecificEffect<DamageEffect>();

        public DicePool Impact
        {
            get => Internal.Impact;
            set
            {
                SetProperty(Internal.Impact, value, Internal, (model, prop) => model.Impact = prop);
                DiceChunks = new(DiceFormulaChunkVM.GetChunks(value));
                OnPropertyChanged(nameof(SpreadText));
                OnPropertyChanged(nameof(Average));
                OnPropertyChanged(nameof(DiceChunks));
            }
        }

        [ObservableProperty]
        private ObservableCollection<DiceFormulaChunkVM> _diceChunks = new();

        public string SpreadText => $"{Impact?.Min()} — {Impact?.Max()}";
        public int Average => Impact?.Average() ?? 0;

        public void UpdateAspect(AspectModelBase aspect)
        {
            InternalModel.SetAspect(aspect.InternalModel);
            UpdateAspects();
        }

        public void UpdateAspects()
        {
            Aspects = InternalModel.Aspects
                .Select(AspectModelFactory.GetAspectModel)
                .Where(x => x != null)
                .ToList();
        }
    }
}
