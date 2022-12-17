using BRIX.Library.DiceValue;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class DamageEffectPageVM : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<DamageChunkVM> _damage;

        [RelayCommand]
        private async Task EditFormula()
        {
            DiceValuePopupResult result = await ShowPopupAsync<DiceValuePopup, DiceValuePopupResult, DiceValuePopupParameters>(
                new DiceValuePopupParameters { Formula = "" }
            );
        }

        public override Task OnNavigatedAsync()
        {
            Damage = new()
            {
                new() { Dice = new Dice(EStandartDice.D6, 2) },
                new() { Dice = new Dice(EStandartDice.D4) },
                new() { Modifier = 5 },
            };

            return base.OnNavigatedAsync();
        }

        private List<DamageChunkVM> GetChunks(DicePool dicePool)
        {
            List<DamageChunkVM> chunks = new();

            if(dicePool != null)
            {
                if(dicePool.Dice.Any())
                {
                    chunks.AddRange(dicePool.Dice.Select(x => new DamageChunkVM { Dice = x }));
                }

                if(dicePool.Modifier > 0)
                {
                    chunks.Add(new DamageChunkVM { Modifier = dicePool.Modifier });
                }
            }

            return chunks;
        }
    }

    /// <summary>
    /// Позже можно украсить эти кусочки формулы иконками разных типов дайсов
    /// </summary>
    public class DamageChunkVM
    {
        public Dice Dice { get; set; }

        public int Modifier { get; set; }

        public string ChunkText => Dice != null && Dice.Count > 0
            ? $"{Dice.Count}d{Dice.NumberOfFaces}"
            : $"+{Modifier}";
    }
}
