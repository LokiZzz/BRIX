using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AbilityCostMonitorPanelVM : ViewModelBase
    {
        private CharacterModel _character;

        public AbilityCostMonitorPanelVM(AbilityModel ability, IAsyncRelayCommand saveCommand, CharacterModel character)
        {
            Ability = ability;
            SaveCommand = saveCommand;
            _character = character;
            UpdatePercents();
        }

        private AbilityModel _ability;
        public AbilityModel Ability
        {
            get => _ability;
            set => SetProperty(ref _ability, value);
        }

        private int _availiableExp;
        public int AvailiableExp
        {
            get => _availiableExp;
            set => SetProperty(ref _availiableExp, value);
        }

        private int _exp;
        public int Exp
        {
            get => _exp;
            set => SetProperty(ref _exp, value);
        }

        private int _expSumWithoutEditingAbility;
        public int ExpSumWithoutEditingAbility
        {
            get => _expSumWithoutEditingAbility;
            set => SetProperty(ref _expSumWithoutEditingAbility, value);
        }

        private double _percentWithoutEditingAbility;
        public double PercentWithoutEditingAbility
        {
            get => _percentWithoutEditingAbility;
            set => SetProperty(ref _percentWithoutEditingAbility, value);
        }

        private double _percentWithEditingAbility;
        public double PercentWithEditingAbility
        {
            get => _percentWithEditingAbility;
            set => SetProperty(ref _percentWithEditingAbility, value);
        }

        public bool EXPOverflow => AvailiableExp < 0;

        private IAsyncRelayCommand _saveCommand;
        public IAsyncRelayCommand SaveCommand 
        {
            get => _saveCommand;
            set => SetProperty(ref _saveCommand, value);
        }

        public void UpdateCost()
        {
            Ability.UpdateCost();
            UpdatePercents();
        }

        private void UpdatePercents()
        {
            Exp = _character.Experience;
            ExpSumWithoutEditingAbility = _character.Abilities
                .Where(x => x.InternalModel.Guid != Ability.InternalModel.Guid)
                .Sum(x => x.Cost);
            int expSumWithEditingAbility = ExpSumWithoutEditingAbility + Ability.Cost;
            PercentWithoutEditingAbility = (double)ExpSumWithoutEditingAbility / _character.Experience;
            PercentWithEditingAbility = (double)expSumWithEditingAbility / _character.Experience;
            AvailiableExp = Exp - expSumWithEditingAbility;

            OnPropertyChanged(nameof(EXPOverflow));
        }
    }
}
