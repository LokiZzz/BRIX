using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Linq;

namespace BRIX.Mobile.View.Popups;

public partial class PickerPopup : Popup
{
    PickerPopupVM _context;

    public PickerPopup(PickerPopupVM context)
    {
        InitializeComponent();
        context.View = this;
        BindingContext = context;
        _context = context;
    }

    private void pickerCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        CollectionView? collectionView = sender as CollectionView;

        if(collectionView != null)
        {
            if(collectionView.SelectionMode == SelectionMode.Multiple)
            {
                IEnumerable<PickerItemVM> selected = e.CurrentSelection
                    .Select(x => x as PickerItemVM)
                    .Where(x => x != null)
                    .Cast<PickerItemVM>();
                _context.SelectedItems = new(selected);
            }
        }
    }
}