using BRIX.Mobile.Services;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;
using System.Collections;
using System.Windows.Input;

namespace BRIX.Mobile.Resources.Controls;

public partial class PickerButton : Grid
{
    public PickerButton()
    {
        InitializeComponent();

        Application.Current.Resources.TryGetValue("BRIXLight", out object colorResource);

        if (colorResource != null && colorResource is Color entryColor)
        {
            EntryColor = entryColor;
        }
    }

    //public DataTemplate ItemTemplate { get; set; }

    public string DisplayMember { get; set; }


    public static readonly BindableProperty EntryColorProperty = BindableProperty.Create(
        propertyName: nameof(EntryColor),
        returnType: typeof(Color),
        declaringType: typeof(PickerButton),
        defaultValue: Colors.White,
        defaultBindingMode: BindingMode.TwoWay
    );

    public Color EntryColor
    {
        get => (Color)GetValue(EntryColorProperty);
        set
        {
            SetValue(EntryColorProperty, value);
            entryBorder.Stroke = new SolidColorBrush(value);
        }
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(PickerButton),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay
    );

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        propertyName: nameof(Title),
        returnType: typeof(string),
        declaringType: typeof(PickerButton),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay
    );

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(
        propertyName: nameof(ItemSource),
        returnType: typeof(IEnumerable),
        declaringType: typeof(PickerButton),
        defaultBindingMode: BindingMode.OneWay
    );

    public IEnumerable ItemSource
    {
        get => (IEnumerable)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        propertyName: nameof(SelectedItem),
        returnType: typeof(object),
        declaringType: typeof(PickerButton),
        propertyChanged: SelectedItemPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private static void SelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        PickerButton control = (PickerButton)bindable;

        if (newValue != null)
        {
            if (!string.IsNullOrWhiteSpace(control.DisplayMember))
            {
                control.lblSelectedItemText.Text = newValue
                    .GetType()
                    .GetProperty(control.DisplayMember)
                    .GetValue(newValue, null)
                    .ToString();
            }

            control.Up();
        }
        else
        {
            control.Down();
        }
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set
        {
            SetValue(SelectedItemProperty, value);

            if (value == null)
            {
                Down();
            }
            else
            {
                Up();
            }
        }
    }

    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
        propertyName: nameof(IsOpen),
        returnType: typeof(bool),
        declaringType: typeof(PickerButton),
        propertyChanged: IsOpenPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private async static void IsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        PickerButton control = (PickerButton)bindable;

        if (newValue != null)
        {
            if ((bool)newValue)
            {
                PickerPopup popupToShow = Resolver.Resolve<PickerPopup>();
                ParametrizedPopupVMBase<PickerPopupParameters> viewModel =
                    popupToShow.BindingContext as ParametrizedPopupVMBase<PickerPopupParameters>;
                viewModel.Parameters = new PickerPopupParameters
                {
                    Items = control.ItemSource.Cast<object>().ToList(),
                    SelectedItems = new() { control.SelectedItem },
                    Title = control.Title,
                };
                PickerPopupResult result = await Application.Current.MainPage.ShowPopupAsync(popupToShow) 
                    as PickerPopupResult;

                if (result != null)
                {
                    control.SelectedItem = result.SelectedItem;
                }

                control.IsOpen = false;
            }

        }
    }

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    private void Down()
    {
        lblPlaceholder.FontSize = 15;
        lblPlaceholder.TranslateTo(0, 0, 100, Easing.BounceIn);
    }

    private void Up()
    {
        lblPlaceholder.FontSize = 13;
        lblPlaceholder.TranslateTo(0, -25, 100, Easing.BounceIn);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        IsOpen = true;
    }
}