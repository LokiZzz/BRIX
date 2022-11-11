﻿using BRIX.Mobile.Services;
using BRIX.Mobile.View.Character;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Account
{
    public partial class SignInPageVM : ViewModelBase
    {
        private readonly IAccountService _accountService;

        public SignInPageVM(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private async Task InitializeAsync()
        {
            RememberMe = Preferences.Get(Settings.Account.RememberMe, false);

            // Set properties & automatically go through this page to the character page
            if (RememberMe)
            {
                Login = Preferences.Get(Settings.Account.Login, string.Empty);
                Password = Preferences.Get(Settings.Account.Password, string.Empty);

                await SignIn();
            }
        }

        [ObservableProperty]
        private string _login;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _rememberMe;

        [RelayCommand]
        private async Task SignIn()
        {
            if(await _accountService.SignInAsync(_login, Password))
            {
                if (RememberMe)
                {
                    Preferences.Set(Settings.Account.RememberMe, true);
                    Preferences.Set(Settings.Account.Login, _login);
                    Preferences.Set(Settings.Account.Password, _password);
                }

                await Navigation.NavigateAsync($"//{nameof(CurrentCharacterPage)}");
            }
        }

        public override async Task OnNavigatedAsync()
        {
            await InitializeAsync();
        }
    }
}
