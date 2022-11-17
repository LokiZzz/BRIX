using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
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

        [ObservableProperty]
        private string _simpleText = "Not changed";

        public virtual Task OnNavigatedAsync() => Task.CompletedTask;
    }
}
