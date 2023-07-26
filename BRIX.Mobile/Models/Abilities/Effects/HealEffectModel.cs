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
            DiceChunks = new(DiceFormulaChunkVM.GetChunks(Impact));
            model.ForceAspectInitialize();
            UpdateAspects();
        }

        public HealEffect Internal => GetSpecificEffect<HealEffect>();

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
    }
}
