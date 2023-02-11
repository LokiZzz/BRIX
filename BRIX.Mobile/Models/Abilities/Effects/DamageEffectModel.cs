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
    public partial class DamageEffectModel : EffectModel
    {
        public DamageEffectModel() : this(new DamageEffect()) { }

        public DamageEffectModel(DamageEffect model) => InternalModel = model;

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
            }
        }

        [ObservableProperty]
        private ObservableCollection<DiceFormulaChunkVM> _diceChunks = new();

        public string SpreadText => $"{Impact.Min()} — {Impact.Max()}";
        public int Average => Impact.Average();

        public override string EffectString => DiceChunks.GetChunkCollectionText();

        public override List<string> Aspects => new List<string>();
    }
}
