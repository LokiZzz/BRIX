using Android.Telephony.Euicc;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Popups;
using CommunityToolkit.Maui.Views;
using System.Collections;
using System.Reflection;
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

    public DataTemplate ItemTemplate { get; set; }

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

    public event EventHandler<EventArgs> OpenPickerEvent;

    public static readonly BindableProperty OpenPickerCommandProperty = BindableProperty.Create(
       propertyName: nameof(OpenPickerCommand),
       returnType: typeof(ICommand),
       declaringType: typeof(PickerButton),
       defaultBindingMode: BindingMode.TwoWay);

    public ICommand OpenPickerCommand
    {
        get => (ICommand)GetValue(OpenPickerCommandProperty);
        set => SetValue(OpenPickerCommandProperty, value);
    }

    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
      propertyName: nameof(IsOpen),
      returnType: typeof(bool),
      declaringType: typeof(PickerButton),
      propertyChanged: IsDisplayPickerControlPropertyChanged,
      defaultBindingMode: BindingMode.TwoWay);

    private async static void IsDisplayPickerControlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        PickerButton control = (PickerButton)bindable;

        if (newValue != null)
        {
            if ((bool)newValue)
            {
                object response = await Application.Current.MainPage.ShowPopupAsync(
                    new PickerPopup(control.ItemSource, control.ItemTemplate, control.Placeholder)
                );

                if (response != null)
                {
                    control.SelectedItem = response;
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
        OpenPickerCommand?.Execute(null);
        OpenPickerEvent?.Invoke(sender, e);
    }
}