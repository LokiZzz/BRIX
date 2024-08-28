using BRIX.Library.Enums;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class TargetSizeAspectPageVM(ILocalizationResourceManager localization) 
        : AspectPageVMBase<TargetSizeAspectModel>
    {
        public ILocalizationResourceManager Localization { get; } = localization;

        public override void Initialize()
        {
            if(Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

            Aspect.Sizes = new(Aspect.Internal.AllowedTargetSizes.Select(x => 
                new TargetSizeVM { Size = x, Text = Localization[x.ToString("G")].ToString() ?? string.Empty }
            ));
            OnPropertyChanged(nameof(ShowSizesCollection));
        }

        public bool ShowSizesCollection => Aspect?.Sizes?.Any() == true;

        [RelayCommand]
        public async Task AddSize()
        {
            if (Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

            List<object> allSizes = Enum.GetValues<ETargetSize>()
                .Select(x => new TargetSizeVM
                {
                    Size = x,
                    Text = Localization[x.ToString("G")].ToString() ?? string.Empty
                })
                .Where(x => !Aspect.Sizes.Any(y => y.Size == x.Size))
                .Select(x => x as object)
                .ToList();

            PickerPopupResult? result = await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(
                new()
                {
                    Title = Resources.Localizations.Localization.TargetsSizes,
                    Items = allSizes,
                    SelectedItems = [allSizes.First()],
                    SelectMultiple = true
                }
            );

            if (result != null)
            {
                List<TargetSizeVM> concreteResult = result.SelectedItems.Select(x => (TargetSizeVM)x).ToList();

                if (Aspect.Sizes.Any(x => concreteResult.Any(y => y.Size == x.Size)))
                {
                    return;
                }

                foreach(TargetSizeVM sizeVM in concreteResult)
                {
                    Aspect.Sizes.Add(sizeVM); 
                    Aspect.Internal.AddSize(sizeVM.Size);
                }

                Aspect.Sizes = new(Aspect.Sizes.OrderBy(x => x.Size));
            }

            CostMonitor?.UpdateCost();

            OnPropertyChanged(nameof(ShowSizesCollection));
        }

        [RelayCommand]
        public void DeleteSize(TargetSizeVM property)
        {
            if (Aspect == null)
            {
                throw new Exception("Не инициализирована модель" + nameof(Aspect));
            }

            Aspect.Sizes.Remove(property);
            Aspect.Internal.RemoveSize(property.Size);

            CostMonitor?.UpdateCost();

            OnPropertyChanged(nameof(ShowSizesCollection));
        }
    }

    public class TargetSizeVM
    {
        public ETargetSize Size { get; set; }
        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }
}
