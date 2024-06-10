using BRIX.Library.DiceValue;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class DiceValuePopupVM : ParametrizedPopupVMBase<DiceValuePopupParameters>
    {
        [ObservableProperty]
        private string _formula = string.Empty;

        [RelayCommand]
        private void Set()
        {
            if (DicePool.TryParse(Formula, out DicePool? parsed) && parsed != null)
            {
                View?.Close(new DiceValuePopupResult { DicePool = parsed });
            }
            else
            {
                OnInvalidFormulaEntered?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? OnInvalidFormulaEntered;

        protected override void HandleParameters()
        {
            if (Parameters == null)
            {
                return;
            }

            Formula = Parameters.Formula;
        }
    }

    public class DiceValuePopupParameters
    {
        public string Formula { get; init; } = string.Empty;
    }
    
    public class DiceValuePopupResult
    {
        public DicePool DicePool { get; set; } = new DicePool();
    }
}
