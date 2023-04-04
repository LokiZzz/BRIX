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

        [ObservableProperty]
        private AbilityModel _ability;

        [ObservableProperty]
        private int _availiableExp;

        [ObservableProperty]
        private int _exp;

        [ObservableProperty]
        private int _expSumWithoutEditingAbility;

        [ObservableProperty]
        public double _percentWithoutEditingAbility;

        [ObservableProperty]
        private double _percentWithEditingAbility;

        public Guid CommandChangedGuid { get; set; }

        private IAsyncRelayCommand _saveCommand;
        public IAsyncRelayCommand SaveCommand 
        {
            get => _saveCommand;
            set
            {
                SetProperty(ref _saveCommand, value);
                CommandChangedGuid = Guid.NewGuid();
            }
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
        }
    }
}
