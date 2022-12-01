using Microsoft.Maui.Controls.Shapes;

namespace BRIX.Mobile.Resources.Controls;

public partial class CropImage : Grid
{
    public CropImage()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ZoomProperty = BindableProperty.Create(nameof(Zoom), typeof(Shape), typeof(CropImage));
    public Shape Zoom
    {
        get => (Shape)GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(CropImage));
    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty SelectionProperty = BindableProperty.Create(nameof(Selection), typeof(Geometry), typeof(CropImage));
    public Geometry Selection
    {
        get => (Geometry)GetValue(SelectionProperty);
        set => SetValue(SelectionProperty, value);
    }

    private Geometry Map(Shape shape)
    {
        switch (shape)
        {
            case Rectangle rectangle:
                Rect rect = new Rect(
                    rectangle.X + rectangle.TranslationX,
                    rectangle.Y + rectangle.TranslationY,
                    rectangle.Width * rectangle.Scale,
                    rectangle.Height * rectangle.Scale);
                return new RectangleGeometry(rect);

            default: 
                return null;
        }
    }

    private void OnZoomUpdated(object sender, Shape zoom)
    {
        Selection = Map(zoom);
    }
}