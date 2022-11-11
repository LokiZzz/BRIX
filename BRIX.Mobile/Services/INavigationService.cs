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
    }

    public class NavigationService : INavigationService
    {
        public Task NavigateAsync(string route, IDictionary<string, object> parameters = null)
        {
            return parameters != null
                ? Shell.Current.GoToAsync(route, parameters)
                : Shell.Current.GoToAsync(route);
        }
    }
}
