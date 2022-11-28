using CommunityToolkit.Maui.Views;
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

    public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(Image), typeof(ZoomContainer));
    public Image Image
    {
        get => (Image)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            // Store the current scale factor applied to the wrapped user interface element,
            // and zero the components for the center point of the translate transform.
            startScale = Content.Scale;
            Content.AnchorX = 0;
            Content.AnchorY = 0;
        }
        if (e.Status == GestureStatus.Running)
        {
            // Calculate the scale factor to be applied.
            currentScale += (e.Scale - 1) * startScale;
            currentScale = Math.Max(1, currentScale);

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the X pixel coordinate.
            double renderedX = Content.X + xOffset;
            double deltaX = renderedX / Width;
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
            Content.TranslationX = Math.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
            Content.TranslationY = Math.Clamp(targetY, -Content.Height * (currentScale - 1), 0);

            Content.Scale = currentScale;
        }
        if (e.Status == GestureStatus.Completed)
        {
            xOffset = Content.TranslationX;
            yOffset = Content.TranslationY;
        }
    }

    void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:


                // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                var newXPosition = Math.Clamp(Content.X + xOffset + e.TotalX, Image.Frame.Left, Image.Frame.Right - Content.Width * Content.Scale);
                Content.TranslationX = newXPosition - Content.X;

                var newYPosition = Math.Clamp(Content.Y + yOffset + e.TotalY, Image.Frame.Top, Image.Frame.Bottom - Content.Height * Content.Scale);
                Content.TranslationY = newYPosition - Content.Y;


                var path = Content as Microsoft.Maui.Controls.Shapes.Path;
                path.TranslationX = newXPosition - Content.X;
                path.TranslationY = newYPosition - Content.Y;
                break;

            case GestureStatus.Completed:
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
                break;
        }
    }
}