using CommunityToolkit.Maui.Views;
using System.Collections;

namespace BRIX.Mobile.View.Popups;

public partial class PickerPopup : Popup
{
	public PickerPopup(IEnumerable itemSource, DataTemplate itemTemplate, string Title, object selectedItem = null)
	{
		InitializeComponent();
		cvOptions.ItemsSource = itemSource;
		cvOptions.ItemTemplate = itemTemplate;
		lblTitle.Text = Title;

		if(selectedItem != null)
		{
			_closeAfterSelectionChanged = false;
			cvOptions.SelectedItem = selectedItem;
			_closeAfterSelectionChanged = true;
		}
	}

	private bool _closeAfterSelectionChanged = true;

    private void cvOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (_closeAfterSelectionChanged)
		{
			object currentItem = e.CurrentSelection.FirstOrDefault();
			Close(currentItem);
		}
    }
}