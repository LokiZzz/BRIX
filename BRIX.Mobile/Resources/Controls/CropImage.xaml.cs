using Microsoft.Maui.Controls.Shapes;

namespace BRIX.Mobile.Resources.Controls;

public partial class CropImage : Grid
{
    public CropImage()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CropBoundsProperty = BindableProperty.Create(nameof(CropBounds), typeof(Shape), typeof(CropImage), null, BindingMode.TwoWay);
    public Shape CropBounds
    {
        get => (Shape)GetValue(CropBoundsProperty);
        set => SetValue(CropBoundsProperty, value);
    }

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(CropImage));
    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty ClipGeometryProperty = BindableProperty.Create(nameof(ClipGeometry), typeof(Geometry), typeof(CropImage));
    public Geometry ClipGeometry
    {
        get => (Geometry)GetValue(ClipGeometryProperty);
        set => SetValue(ClipGeometryProperty, value);
    }

    private void OnCropBoundsUpdated(object sender, Shape cropBounds)
    {
        ClipGeometry = Map(cropBounds);
    }

    Geometry Map(Shape shape)
    {
        switch (shape)
        {
            case Rectangle rectangle:
                Rect rect = new Rect(
                    rectangle.Frame.X + rectangle.TranslationX,
                    rectangle.Frame.Y + rectangle.TranslationY,
                    rectangle.Frame.Width * rectangle.Scale,
                    rectangle.Frame.Height * rectangle.Scale);
                return new RectangleGeometry(rect);

            default:
                return null;
        }
    }
}