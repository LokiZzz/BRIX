﻿using BRIX.Library;
using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AddOrEditAbilityPageVM : ViewModelBase, IQueryAttributable
    {
        [ObservableProperty]
        private AbilityModel _ability;

        [ObservableProperty]
        private EAbilityEditMode _mode;

        [RelayCommand]
        public void Save()
        {
            Navigation.Back((NavigationParameters.EditMode, Mode), (NavigationParameters.Ability, Ability));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability)
                ?? new AbilityModel(new Ability());

            Mode = query.GetParameterOrDefault<EAbilityEditMode>(NavigationParameters.EditMode);
            query.Clear();
        }
    }

    public enum EAbilityEditMode
    {
        None = 0,
        Add = 1,
        Edit = 2,
        Upgrade = 3
    }
}
