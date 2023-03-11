using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.ViewModel.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Aspects = model.Aspects
                .Select(AspectModelFactory.GetAspectModel)
                .Where(x => x != null)
                .ToList();
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

        public override string EffectString => DiceChunks.GetChunkCollectionText();
    }
}
