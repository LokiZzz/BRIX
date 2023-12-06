using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class NumericEditorPopupVM : ParametrizedPopupVMBase<NumericEditorParameters>
    {
        protected override void HandleParameters()
        {
            EditorTitle = Parameters?.Title ?? string.Empty;
        }

        [ObservableProperty]
        private string _editorTitle = string.Empty;

        [ObservableProperty]
        private string _value = string.Empty;

        [RelayCommand]
        private void EnterNumber(string number)
        {
            Value += number;
        }

        [RelayCommand]
        private void Backspace()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                Value = Value.Remove(Value.Length - 1);
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
            if (!string.IsNullOrEmpty(Value))
            {
                View?.Close(new NumericEditorResult(ENumericEditorResult.Add, int.Parse(Value)));
            }
        }

        [RelayCommand]
        private void Set()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                View?.Close(new NumericEditorResult(ENumericEditorResult.Set, int.Parse(Value)));
            }
        }

        [RelayCommand]
        private void Substract()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                View?.Close(new NumericEditorResult(ENumericEditorResult.Substract, int.Parse(Value)));
            }
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

    public class NumericEditorParameters
    {
        public string Title { get; set; } = string.Empty;
    }
}
