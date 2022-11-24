using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Popups
{
    public class NumericEditorPopupVM : ViewModelBase
    {
        [ObservableProperty]
        private string _value;

        [RelayCommand]
        private void EnterNumber(string number)
        {
            Value += number;
        }

        [RelayCommand]
        private void Backspace()
        {
            if (!string.IsNullOrEmpty(_value))
            {
                Value = _value.Remove(_value.Length - 1);
            }
        }

        [RelayCommand]
        private void Clear()
        {
            Value = string.Empty;
        }
    }
}
