using BRIX.Mobile.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Services
{
    public interface INavigationService
    {
        Task NavigateAsync(string route, IDictionary<string, object> parameters = null);
        Task FireOnNavigatedAsync();
    }

    public class NavigationService : INavigationService
    {
        public async Task NavigateAsync(string route, IDictionary<string, object> parameters = null)
        {
            if (parameters != null)
            {
                await Shell.Current.GoToAsync(route, parameters);
            }
            else
            {
                await Shell.Current.GoToAsync(route);
            }
        }

        public async Task FireOnNavigatedAsync()
        {
            ViewModelBase currentPageVM = Shell.Current.CurrentPage?.BindingContext as ViewModelBase;

            if (currentPageVM != null)
            {
                await currentPageVM.OnNavigatedAsync();
            }
        }
    }

    public static class NavigationParameters
    {
        public const string Character = nameof(Character);
    }
}
