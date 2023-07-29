namespace BRIX.Mobile.Resources.Controls;

public partial class CircularProgressBar : ContentView
{
	public CircularProgressBar()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty ProgressProperty = 
        BindableProperty.Create(nameof(Progress), typeof(int), typeof(CircularProgressBar), 
            propertyChanged:OnProgressChanged);

    private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CircularProgressBar progressBar = bindable as CircularProgressBar;
        progressBar.graphicsView.Invalidate();
    }

    public static readonly BindableProperty InnerTextProperty =
            BindableProperty.Create(nameof(InnerText), typeof(string), typeof(CircularProgressBarDrawable));

    public static readonly BindableProperty SizeProperty = 
        BindableProperty.Create(nameof(Size), typeof(int), typeof(CircularProgressBar));

    public static readonly BindableProperty ThicknessProperty = 
        BindableProperty.Create(nameof(Thickness), typeof(int), typeof(CircularProgressBar));

    public static readonly BindableProperty ProgressColorProperty = 
        BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircularProgressBar));

    public static readonly BindableProperty ProgressLeftColorProperty = 
        BindableProperty.Create(nameof(ProgressLeftColor), typeof(Color), typeof(CircularProgressBar));

    public static readonly BindableProperty TextColorProperty = 
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CircularProgressBar));

    public int Progress
    {
        get { return (int)GetValue(ProgressProperty); }
        set => SetValue(ProgressProperty, value);
    }

    public string InnerText
    {
        get => (string)GetValue(InnerTextProperty);
        set => SetValue(InnerTextProperty, value);
    }

    public int Size
    {
        get { return (int)GetValue(SizeProperty); }
        set { SetValue(SizeProperty, value); }
    }

    public int Thickness
    {
        get { return (int)GetValue(ThicknessProperty); }
        set { SetValue(ThicknessProperty, value); }
    }

    public Color ProgressColor
    {
        get { return (Color)GetValue(ProgressColorProperty); }
        set { SetValue(ProgressColorProperty, value); }
    }

    public Color ProgressLeftColor
    {
        get { return (Color)GetValue(ProgressLeftColorProperty); }
        set { SetValue(ProgressLeftColorProperty, value); }
    }

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == SizeProperty.PropertyName)
        {
            HeightRequest = Size;
            WidthRequest = Size;
        }
    }
}