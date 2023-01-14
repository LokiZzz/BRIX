using BRIX.Mobile.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Services.Navigation
{
    public interface INavigationService
    {
        Task Back(int stepsBack = 1);
        Task Back(int stepsBack = 1, params (string, object)[] parameters);
        Task NavigateAsync<T>(params (string, object)[] parameters) where T : Page;
        Task NavigateAsync<T>(ENavigationMode mode = ENavigationMode.Push, params (string, object)[] parameters) where T : Page;
        Task NavigateAsync(string route, ENavigationMode mode = ENavigationMode.Push, params (string, object)[] parameters);
        Task FireOnNavigatedAsync();
    }
}
