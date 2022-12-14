using BRIX.Mobile.Services;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Account
{
    public partial class SignInPageVM : ViewModelBase
    {
        private readonly IAccountService _accountService;

        public SignInPageVM(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [ObservableProperty]
        private string _login;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _rememberMe;

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task SignIn()
        {
            IsBusy = true;

            bool successfullSignIn = await _accountService.SignInAsync(_login, Password);

            if (successfullSignIn)
            {
                if (RememberMe)
                {
                    Preferences.Set(Mobile.Settings.Account.RememberMe, true);
                    Preferences.Set(Mobile.Settings.Account.Login, _login);
                    Preferences.Set(Mobile.Settings.Account.Password, _password);
                }

                await Navigation.NavigateAsync<CurrentCharacterPage>(ENavigationMode.Absolute);
            }

            IsBusy = false;
        }

        public override async Task OnNavigatedAsync()
        {
            RememberMe = Preferences.Get(Mobile.Settings.Account.RememberMe, false);

            // Set properties & automatically go through this page to the character page
            if (RememberMe)
            {
                Login = Preferences.Get(Mobile.Settings.Account.Login, string.Empty);
                Password = Preferences.Get(Mobile.Settings.Account.Password, string.Empty);

                await Navigation.NavigateAsync<CurrentCharacterPage>(ENavigationMode.Absolute);
            }
        }
    }
}
