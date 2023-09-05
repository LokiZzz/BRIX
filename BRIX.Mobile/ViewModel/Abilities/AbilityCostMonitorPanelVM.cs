using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AbilityCostMonitorPanelVM : ViewModelBase
    {
        /// <summary>
        /// Если есть необходимость, то монитор можно заглушить, создав его через этот конструктор.
        /// При редактировании статусов в эффектах и аспектах нет стоимости способности и таким образом монитор 
        /// отключется.
        /// </summary>
        public AbilityCostMonitorPanelVM()
        {
            IsMock = true;
        }

        public AbilityCostMonitorPanelVM(CharacterAbilityModel ability, IAsyncRelayCommand saveCommand)
        {
            Ability = ability;
            SaveCommand = saveCommand;
            Character = new(ability.Character);
            UpdatePercents();
        }
        
        private CharacterModel Character;

        private CharacterAbilityModel _ability;
        public CharacterAbilityModel Ability
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

        private bool _isMock;
        public bool IsMock
        {
            get => _isMock;
            set => SetProperty(ref _isMock, value);
        }

        public void UpdateCost()
        {
            if(IsMock)
            {
                return;
            }

            Ability.UpdateCost();
            UpdatePercents();
        }

        private void UpdatePercents()
        {
            if (IsMock)
            {
                return;
            }

            Exp = Character.Experience;
            ExpSumWithoutEditingAbility = Character.Abilities
                .Where(x => x.InternalModel.Id != Ability.InternalModel.Id)
                .Sum(x => x.Cost);
            int expSumWithEditingAbility = ExpSumWithoutEditingAbility + Ability.Cost;
            PercentWithoutEditingAbility = (double)ExpSumWithoutEditingAbility / Character.Experience;
            PercentWithEditingAbility = (double)expSumWithEditingAbility / Character.Experience;
            AvailiableExp = Exp - expSumWithEditingAbility;

            OnPropertyChanged(nameof(EXPOverflow));
        }
    }
}
