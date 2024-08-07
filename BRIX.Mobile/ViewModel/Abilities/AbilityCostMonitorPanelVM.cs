﻿using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AbilityCostMonitorPanelVM : ViewModelBase
    {
        /// <summary>
        /// Если есть необходимость, то монитор можно заглушить, создав его через этот конструктор.
        /// При редактировании статусов в эффектах и аспектах нет стоимости способности и таким образом монитор 
        /// отключится.
        /// </summary>
        public AbilityCostMonitorPanelVM()
        {
            ShowCost = false;
            ShowExpEconomics = false;
        }

        public AbilityCostMonitorPanelVM(CharacterAbilityModel ability, IAsyncRelayCommand saveCommand)
        {
            Ability = ability;
            SaveCommand = saveCommand;

            if(ability?.Character != null && ability.Character is Character playerCharacter)
            {
                Character = new(playerCharacter);
            }

            ShowExpEconomics = Character != null;

            UpdateCost();
        }
        
        public readonly CharacterModel? Character;

        private CharacterAbilityModel _ability = new();
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

        private int _spentEXP;
        /// <summary>
        /// Опыт, уже занятый другими способнностями и увеличенным здоровьем,
        /// но не включающий текущую редактируемую способность.
        /// </summary>
        public int SpentEXP
        {
            get => _spentEXP;
            set => SetProperty(ref _spentEXP, value);
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

        private IAsyncRelayCommand? _saveCommand;
        public IAsyncRelayCommand? SaveCommand 
        {
            get => _saveCommand;
            set => SetProperty(ref _saveCommand, value);
        }

        private bool _showCost = true;
        public bool ShowCost
        {
            get => _showCost;
            set => SetProperty(ref _showCost, value);
        }

        private bool _showExpEconomics = true;
        public bool ShowExpEconomics
        {
            get => _showExpEconomics;
            set => SetProperty(ref _showExpEconomics, value);
        }

        public void UpdateCost()
        {
            if(Ability == null)
            {
                return;
            }

            if(Character == null)
            {
                Ability.UpdateCost();

                return;
            }

            Exp = Character.Experience;
            SpentEXP = Character.InternalModel.GetSpentExp(Ability.Internal.Id);

            int abilityCost = Ability.Cost;
            int expSumWithEditingAbility = SpentEXP + abilityCost;

            PercentWithoutEditingAbility = (double)SpentEXP / Character.Experience;
            PercentWithEditingAbility = (double)expSumWithEditingAbility / Character.Experience;

            AvailiableExp = Exp - expSumWithEditingAbility;

            OnPropertyChanged(nameof(EXPOverflow));
            Ability.UpdateCost();
        }
    }
}
