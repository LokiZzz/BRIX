﻿using BRIX.Mobile.ViewModel.Base;
using System.Linq;

namespace BRIX.Mobile.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task Back(int stepsBack = 1)
        {
            await NavigateAsync(GetStepBackPath(stepsBack), ENavigationMode.None);
        }

        public async Task Back(int stepsBack = 1, params (string, object?)[] parameters)
        {
            await NavigateAsync(GetStepBackPath(stepsBack), ENavigationMode.None, parameters);
        }

        public async Task NavigateAsync<T>(params (string, object?)[] parameters) where T : Page
        {
            await NavigateAsync(typeof(T).Name, ENavigationMode.Push, parameters);
        }

        public async Task NavigateAsync<T>(ENavigationMode mode, params (string, object?)[] parameters) where T : Page
        {
            await NavigateAsync(typeof(T).Name, mode, parameters);
        }

        public async Task NavigateAsync(
            string route, 
            ENavigationMode mode = ENavigationMode.Push, 
            params (string, object?)[] parameters)
        {
            // Для отладки:
            string pathBefore = $"{Shell.Current.CurrentItem} :: {Shell.Current.CurrentPage} :: {Shell.Current.CurrentState}";

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

            string pathAfter = $"{Shell.Current.CurrentItem} :: {Shell.Current.CurrentPage} :: {Shell.Current.CurrentState}";
        }

        public async Task FireOnNavigatedAsync()
        {
            if (Shell.Current.CurrentPage?.BindingContext is ViewModelBase currentPageVM)
            {
                await currentPageVM.OnNavigatedAsync();
            }
        }

        private static string GetStepBackPath(int stepsBack)
        {
            string path = "..";

            for (int steps = stepsBack - 1; steps > 0; steps--)
            {
                path += "/..";
            }

            return path;
        }
    }

    public static class NavigationParametersExtension
    {
        public static Dictionary<string, object> ToParametersDictionary(this (string, object?)[] parameters)
        {
            Dictionary<string, object> parametersDictionary = [];

            foreach ((string Key, object? Value) parameter in parameters)
            {
                if (parameter.Value != null)
                {
                    parametersDictionary.Add(parameter.Key, parameter.Value);
                }
            }

            return parametersDictionary;
        }
    }
}
