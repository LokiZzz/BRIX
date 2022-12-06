using Microsoft.Maui.Controls.Shapes;

namespace BRIX.Mobile.Resources.Controls;

public class ZoomContainer : ContentView
{
    double currentScale = 1;
    double startScale = 1;
    double xOffset = 0;
    double yOffset = 0;

    public ZoomContainer()
    {
        var pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += OnPinchUpdated;
        GestureRecognizers.Add(pinchGesture);

        PanGestureRecognizer panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);
    }

    public static readonly BindableProperty AreaProperty = BindableProperty.Create(nameof(Area), typeof(VisualElement), typeof(ZoomContainer));
    public VisualElement Area
    {
        get => (VisualElement)GetValue(AreaProperty);
        set => SetValue(AreaProperty, value);
    }

    public event EventHandler<Shape> CropBoundsUpdated;

    void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        switch (e.Status)
        {
            case GestureStatus.Started:
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
                break;

            case GestureStatus.Running:
                // Calculate the scale factor to be applied.
                currentScale = Math.Max(1, currentScale + (e.Scale - 1) * startScale);
                // Ensure we don't pinch beyond the border
                currentScale = Math.Min(currentScale, Math.Min(Area.Height / Content.Height, Area.Width / Content.Width));

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + xOffset; // где сейчас находится фигура
                double deltaX = renderedX / Width; // что за ебанина?
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                UpdateTranslation(xOffset + targetX, yOffset + targetY);
                
                // Apply scale factor
                Content.Scale = currentScale;
                CropBoundsUpdated?.Invoke(this, Content as Shape);
                break;

            case GestureStatus.Completed:
                // Store the translation delta's of the wrapped user interface element.
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
                break;
        }
    }

    void UpdateTranslation(double xOffset, double yOffset)
    {
        // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
        var newXPosition = Math.Clamp(Content.X + xOffset, Area.Frame.Left, Math.Max(0, Area.Frame.Right - Content.Width * Content.Scale));
        var newYPosition = Math.Clamp(Content.Y + yOffset, Area.Frame.Top, Math.Max(0, Area.Frame.Bottom - Content.Height * Content.Scale));
        Content.TranslationX = newXPosition - Content.X;
        Content.TranslationY = newYPosition - Content.Y;
    }

    void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:

                UpdateTranslation(xOffset + e.TotalX, yOffset + e.TotalY);
                CropBoundsUpdated?.Invoke(this, Content as Shape);
                break;

            case GestureStatus.Completed:
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
                break;
        }
    }
}