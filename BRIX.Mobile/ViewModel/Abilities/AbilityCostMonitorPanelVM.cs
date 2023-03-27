using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Lang;

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
        public double _percentWithoutEditingAbility;

        [ObservableProperty]
        private double _percentWithEditingAbility;

        public IAsyncRelayCommand SaveCommand { get; set; }

        public void UpdateCost()
        {
            Ability.UpdateCost();
            UpdatePercents();
        }

        private void UpdatePercents()
        {
            int expSumWithoutEditingAbility = _character.Abilities
                .Where(x => x.InternalModel.Guid != Ability.InternalModel.Guid)
                .Sum(x => x.Cost);
            int expSumWithEditingAbility = expSumWithoutEditingAbility + Ability.Cost;
            PercentWithoutEditingAbility = (double)expSumWithoutEditingAbility / _character.Experience;
            PercentWithEditingAbility = (double)expSumWithEditingAbility / _character.Experience;
        }
    }
}
