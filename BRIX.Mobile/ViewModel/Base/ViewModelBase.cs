using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            where TPopup : ParametrizedPopup<TParams> where TResult : class where TParams : class
        {
            TPopup popupToShow = ServicePool.GetService<TPopup>();
            popupToShow.Parameters = parameters;
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
