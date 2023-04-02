using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.ViewModel.Base
{
    public partial class ViewModelBase : ObservableObject 
    {
        protected INavigationService Navigation;

        public ViewModelBase()
        {
            Navigation = ServicePool.GetService<INavigationService>();
        }

        [ObservableProperty]
        private bool _isBusy;

        public virtual Task OnNavigatedAsync() => Task.CompletedTask;

        protected async Task<TResult> ShowPopupAsync<TPopup, TResult, TParams>(TParams parameters) 
            where TPopup : Popup where TResult : class where TParams : class
        {
            TPopup popupToShow = ServicePool.GetService<TPopup>();
            ParametrizedPopupVMBase<TParams> viewModel = 
                popupToShow.BindingContext as ParametrizedPopupVMBase<TParams>;
            viewModel.Parameters = parameters;
            object result = await Application.Current.MainPage.ShowPopupAsync(popupToShow);

            return (TResult)result;
        }

        protected async Task<TResult> ShowPopupAsync<TPopup, TResult>() where TPopup : Popup where TResult : class
        {
            Popup popupToShow = ServicePool.GetService<TPopup>();
            object result = await Application.Current.MainPage.ShowPopupAsync(popupToShow);

            return (TResult)result;
        }

        protected async Task ShowPopupAsync<TPopup>() where TPopup : Popup
        {
            Popup popupToShow = ServicePool.GetService<TPopup>();
            await Application.Current.MainPage.ShowPopupAsync(popupToShow);
        }
    }
}
