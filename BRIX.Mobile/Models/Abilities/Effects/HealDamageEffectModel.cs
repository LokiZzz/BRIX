using BRIX.Library;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public partial class HealDamageEffectModel : ObservableObject
    {
        public HealDamageEffectModel() : this(new HealDamageEffect()) { }

        public HealDamageEffectModel(HealDamageEffect character) => InternalModel = character;

        public HealDamageEffect InternalModel { get; }

        public DicePool Impact
        {
            get => InternalModel.Impact;
            set
            {
                SetProperty(InternalModel.Impact, value, InternalModel, (model, prop) => model.Impact = prop);
                DiceChunks = new(DiceFormulaChunkVM.GetChunks(value));
                OnPropertyChanged(nameof(SpreadText));
                OnPropertyChanged(nameof(Average));
            }
        }

        [ObservableProperty]
        private ObservableCollection<DiceFormulaChunkVM> _diceChunks = new();

        public string SpreadText => $"{Impact.Min()} — {Impact.Max()}";
        public int Average => Impact.Average();
    }
}
