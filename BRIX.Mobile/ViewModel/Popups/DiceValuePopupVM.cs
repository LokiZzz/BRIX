using BRIX.Library.DiceValue;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class DiceValuePopupVM : ParametrizedPopupVMBase<DiceValuePopupParameters>
    {
        [ObservableProperty]
        private string _formula;

        [RelayCommand]
        private void Set()
        {
            if (DicePool.TryParse(Formula, out DicePool? parsed) && parsed != null)
            {
                View.Close(new DiceValuePopupResult { DicePool = parsed });
            }
            else
            {
                if (OnInvalidFormulaEntered != null)
                {
                    OnInvalidFormulaEntered(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? OnInvalidFormulaEntered;

        protected override void HandleParameters()
        {
            Formula = Parameters.Formula;
        }
    }

    public class DiceValuePopupParameters
    {
        public string Formula { get; init; }
    }
    
    public class DiceValuePopupResult
    {
        public DicePool DicePool { get; set; }
    }
}
