using BRIX.Library.Characters;
using BRIX.Mobile.View.Popups;
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
    public partial class NumericEditorPopupVM : ViewModelBase
    {
        public NumericEditorPopup View;

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

        [RelayCommand]
        private void Add()
        {
            View.Close(new NumericEditorResult(ENumericEditorResult.Add, int.Parse(Value)));
        }

        [RelayCommand]
        private void Set()
        {
            View.Close(new NumericEditorResult(ENumericEditorResult.Set, int.Parse(Value)));
        }

        [RelayCommand]
        private void Substract()
        {
            View.Close(new NumericEditorResult(ENumericEditorResult.Substract, int.Parse(Value)));
        }
    }

    public class NumericEditorResult
    {
        public NumericEditorResult(ENumericEditorResult action, int enteredValue)
        {
            Action = action;
            EnteredValue = enteredValue;
        }

        public ENumericEditorResult Action { get; set; }
        public int EnteredValue { get; set; }

        public int ToValue(int oldValue)
        {
            switch (Action)
            {
                case ENumericEditorResult.Add:
                    oldValue += EnteredValue;
                    break;
                case ENumericEditorResult.Set:
                    oldValue = EnteredValue;
                    break;
                case ENumericEditorResult.Substract:
                    oldValue -= EnteredValue;
                    break;
            }

            return oldValue;
        }
    }

    public enum ENumericEditorResult
    {
        None = 0,
        Add = 1,
        Set = 2,
        Substract = 3
    }
}
