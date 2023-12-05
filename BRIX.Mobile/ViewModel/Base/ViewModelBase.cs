using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.ViewModel.Base
{
    public partial class ViewModelBase : ObservableObject 
    {
        protected INavigationService Navigation;

        public ViewModelBase()
        {
            Navigation = Resolver.Resolve<INavigationService>();
        }

        [ObservableProperty]
        private bool _isBusy;

        public virtual Task OnNavigatedAsync() => Task.CompletedTask;

        protected async Task<TResult?> ShowPopupAsync<TPopup, TResult, TParams>(TParams parameters) 
            where TPopup : Popup where TResult : class where TParams : class
        {
            TPopup popupToShow = Resolver.Resolve<TPopup>();
            ParametrizedPopupVMBase<TParams>? viewModel = 
                popupToShow.BindingContext as ParametrizedPopupVMBase<TParams>;

            if (viewModel != null)
            {
                viewModel.Parameters = parameters;
            }

            Page? mainPage = Application.Current?.MainPage;
            object? result;

            if (mainPage != null)
            {
                result = await mainPage.ShowPopupAsync(popupToShow);
            }
            else
            {
                throw new Exception($"Application.Current?.MainPage is null.");
            }

            return result as TResult;
        }

        protected async Task<TResult?> ShowPopupAsync<TPopup, TResult>() where TPopup : Popup where TResult : class
        {
            Popup popupToShow = Resolver.Resolve<TPopup>();
            Page? mainPage = Application.Current?.MainPage;
            object? result;

            if (mainPage != null)
            {
                result = await mainPage.ShowPopupAsync(popupToShow);
            }
            else
            {
                throw new Exception($"Application.Current?.MainPage is null.");
            }

            return result as TResult;
        }

        protected async Task ShowPopupAsync<TPopup>() where TPopup : Popup
        {
            Popup popupToShow = Resolver.Resolve<TPopup>();
            Page? mainPage = Application.Current?.MainPage;

            if (mainPage != null)
            {
                await mainPage.ShowPopupAsync(popupToShow);
            }
            else
            {
                throw new Exception($"Application.Current?.MainPage is null.");
            }
        }

        protected async Task<AlertPopupResult?> Alert(AlertPopupParameters parameters)
        {
            return await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                parameters
            );
        }

        protected async Task<AlertPopupResult?> Alert(string message)
        {
            return await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                new AlertPopupParameters { Message = message }
            );
        }

        protected async Task<AlertPopupResult?> Ask(string message)
        {
            return await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                new AlertPopupParameters 
                {
                    Mode = EAlertMode.AskYesOrNo,
                    Message = message 
                }
            );
        }
    }
}
