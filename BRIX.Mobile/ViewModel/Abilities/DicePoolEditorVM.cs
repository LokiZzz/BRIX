using BRIX.Library.DiceValue;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class DicePoolEditorVM : ViewModelBase
    {
        public DicePoolEditorVM()
        {
            Dices = new DicePool((1, 4));
        }

        public event EventHandler? DicePoolUpdated;

        private DicePool _dices = new();
        public DicePool Dices
        {
            get => _dices;
            set
            {
                SetProperty(ref _dices, value);
                DiceChunks = new(DiceFormulaChunkVM.GetChunks(value));
                OnPropertyChanged(nameof(SpreadText));
                OnPropertyChanged(nameof(Average));
                OnPropertyChanged(nameof(DiceChunks));
                OnPropertyChanged(nameof(RollOptions));
                OnPropertyChanged(nameof(ShowNoDicesMessage));
                OnPropertyChanged(nameof(ShowRollOptions));
                FireDicePoolUpdated();
            }
        }

        private ObservableCollection<DiceFormulaChunkVM> _diceChunks = new();
        public ObservableCollection<DiceFormulaChunkVM> DiceChunks
        {
            get => _diceChunks;
            set
            {
                SetProperty(ref _diceChunks, value);
                OnPropertyChanged(nameof(ShowNoDicesMessage));
            }
        }

        public string SpreadText => $"{Dices.Min()} — {Dices.Max()}";

        public int Average => Dices.Average();

        public string RollOptions => Dices.ToString("R");

        public bool ShowRollOptions => !string.IsNullOrEmpty(RollOptions);

        public bool ShowNoDicesMessage => !DiceChunks.Any();

        [RelayCommand]
        private async Task EditFormula()
        {
            string formula = Dices.ToString() ?? string.Empty;
            DiceValuePopupResult? result = 
                await ShowPopupAsync<DiceValuePopup, DiceValuePopupResult, DiceValuePopupParameters>(
                new DiceValuePopupParameters { Formula = formula }
            );

            if (result != null)
            {
                ResetAdjustment();
                Dices = result.DicePool;
                _dicePoolToReset = null;
            }

            FireDicePoolUpdated();
        }


        private DicePool? _dicePoolToReset = null;

        private double _adjustment = 0;

        public double Adjustment
        {
            get => _adjustment;
            set
            {
                if (value < 1 && value > -1)
                {
                    if (_dicePoolToReset != null)
                    {
                        Dices = _dicePoolToReset;
                        _dicePoolToReset = null;
                        FireDicePoolUpdated();
                    }
                }
                else
                {
                    bool crossInteger = Math.Abs(Math.Floor(_adjustment) - Math.Floor(value)) >= 1;

                    if (crossInteger || value == -5 || value == 5)
                    {
                        int adjustmentPercent = (int)(value > 0 ? Math.Floor(value) : Math.Ceiling(value));
                        Adjust(adjustmentPercent * 10);
                    }
                }

                SetProperty(ref _adjustment, value);
            }
        }

        private void Adjust(int percent)
        {
            _dicePoolToReset = _dicePoolToReset == null ? Dices.Copy() : _dicePoolToReset;

            if (_dicePoolToReset != null)
            {
                Dices = DicePool.FromAdjusted(_dicePoolToReset, percent);
                FireDicePoolUpdated();
            }
        }

        [RelayCommand]
        public void ApplyAdjustment()
        {
            _dicePoolToReset = null;
            Adjustment = 0;
        }

        [RelayCommand]
        public void ResetAdjustment()
        {
            Adjustment = 0;
            FireDicePoolUpdated();
        }

        private void FireDicePoolUpdated()
        {
            if (DicePoolUpdated != null)
            {
                DicePoolUpdated(this, EventArgs.Empty);
            }
        }
    }
}