using BRIX.Mobile.Services;
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
    public partial class SignInPageVM : BusyVMBase
    {
        private readonly IAccountService _accountService;

        public SignInPageVM(IAccountService accountService)
        {
            _accountService = accountService;
            Initialize();
        }

        private void Initialize()
        {
            if(Preferences.Get(Settings.Account.RememberMe, false))
            {
                Login = Preferences.Get(Settings.Account.RememberMe, "Login");
                Password = Preferences.Get(Settings.Account.RememberMe, "Password");
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
                if (Preferences.Get(Settings.Account.RememberMe, false))
                {
                    Preferences.Set(Settings.Account.RememberMe, _login);
                    Preferences.Set(Settings.Account.RememberMe, _password);
                }

                // Navigate to main page!
                await AppConstant
            }
        }
    }
}
