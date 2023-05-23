using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

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
        CollectionView collectionView = sender as CollectionView;

        if(collectionView != null)
        {
            if(collectionView.SelectionMode == SelectionMode.Multiple)
            {
                _context.SelectedItems = new(e.CurrentSelection.Select(x => x as PickerItemVM));
            }
        }
    }
}