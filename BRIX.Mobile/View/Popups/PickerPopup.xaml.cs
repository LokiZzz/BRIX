using CommunityToolkit.Maui.Views;
using System.Collections;

namespace BRIX.Mobile.View.Popups;

public partial class PickerPopup : Popup
{
	public PickerPopup(IEnumerable itemSource, DataTemplate itemTemplate, string Title)
	{
		InitializeComponent();
		cvOptions.ItemsSource = itemSource;
		cvOptions.ItemTemplate = itemTemplate;
		lblTitle.Text = Title;
	}

    private void cvOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        object currentItem = e.CurrentSelection.FirstOrDefault();
		Close(currentItem);
    }
}