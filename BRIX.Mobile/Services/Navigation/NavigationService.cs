using BRIX.Mobile.ViewModel.Base;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace BRIX.Mobile.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task Back(int stepsBack = 1)
        {
            await NavigateAsync(GetStepBackPath(stepsBack), ENavigationMode.None);
        }

        public async Task Back(int stepsBack = 1, params (string, object)[] parameters)
        {
            await NavigateAsync(GetStepBackPath(stepsBack), ENavigationMode.None, parameters);
        }

        public async Task NavigateAsync<T>(params (string, object)[] parameters) where T : Page
        {
            await NavigateAsync(typeof(T).Name, ENavigationMode.Push, parameters);
        }

        public async Task NavigateAsync<T>(ENavigationMode mode, params (string, object)[] parameters) where T : Page
        {
            await NavigateAsync(typeof(T).Name, mode, parameters);
        }

        public async Task NavigateAsync(
            string route, 
            ENavigationMode mode = ENavigationMode.Push, 
            params (string, object)[] parameters)
        {
            switch(mode)
            {
                case ENavigationMode.None:
                    break;
                case ENavigationMode.Push:
                    route = $"/{route}";
                    break;
                case ENavigationMode.Absolute:
                    route = $"//{route}";
                    break;
            }

            if (parameters.Any())
            {
                await Shell.Current.GoToAsync(route, parameters.ToParametersDictionary());
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

        private static string GetStepBackPath(int stepsBack)
        {
            string path = "..";

            for (int steps = stepsBack - 1; stepsBack == 0; steps--)
            {
                path += "/..";
            }

            return path;
        }
    }

    public static class NavigationParametersExtension
    {
        public static Dictionary<string, object> ToParametersDictionary(this (string, object)[] parameters)
        {
            Dictionary<string, object> parametersDictionary = new();

            foreach ((string Key, object Value) parameter in parameters)
            {
                parametersDictionary.Add(parameter.Key, parameter.Value);
            }

            return parametersDictionary;
        }
    }
}
