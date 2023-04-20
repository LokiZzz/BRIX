namespace BRIX.Mobile.Resources.Controls;

public class ZoomContainer : ContentView
{
    public ZoomContainer()
    {
        PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += OnPinchUpdated;
        GestureRecognizers.Add(pinchGesture);

        // Set PanGestureRecognizer.TouchPoints to control the
        // number of touch points needed to pan
        PanGestureRecognizer panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);

        ChildAdded += OnChildAdded;
    }

    public static readonly BindableProperty ContentXProperty = BindableProperty.Create(
        nameof(ContentX), typeof(double), typeof(ZoomContainer), 0d
    );

    public double ContentX
    {
        get => (double)GetValue(ContentXProperty);
        set
        {
            SetValue(ContentXProperty, value);
            if (Content != null)
            {
                Content.TranslationX = value;
                OnPropertyChanged(nameof(Content.TranslationX));
            }
        }
    }

    public static readonly BindableProperty ContentYProperty = BindableProperty.Create(
        nameof(ContentY), typeof(double), typeof(ZoomContainer), 0d
    );

    public double ContentY
    {
        get => (double)GetValue(ContentYProperty);
        set
        {
            SetValue(ContentYProperty, value);
            if (Content != null)
            {
                Content.TranslationY = value;
                OnPropertyChanged(nameof(Content.TranslationY));
            }
        }
    }

    public static readonly BindableProperty ContentScaleProperty = BindableProperty.Create(
        nameof(ContentScale), typeof(double), typeof(ZoomContainer), 1d
    );

    public double ContentScale
    {
        get => (double)GetValue(ContentScaleProperty);
        set
        {
            SetValue(ContentScaleProperty, value);
            if (Content != null)
            {
                Content.Scale = value;
                OnPropertyChanged(nameof(Content.Scale));
            }
        }
    }

    double currentScale = 1;
    double startScale = 1;
    double xOffset = 0;
    double yOffset = 0;

    void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            // Store the current scale factor applied to the wrapped user interface element,
            // and zero the components for the center point of the translate transform.
            startScale = ContentScale;
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
            ContentX = Math.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
            ContentY = Math.Clamp(targetY, -Content.Height * (currentScale - 1), 0);

            // Apply scale factor
            ContentScale = currentScale;
        }
        if (e.Status == GestureStatus.Completed)
        {
            // Store the translation delta's of the wrapped user interface element.
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
                Content.TranslationX = ContentX + e.TotalX;
                Content.TranslationY = ContentY + e.TotalY;
                break;

            case GestureStatus.Completed:
                // Store the translation applied during the pan
                ContentX = Content.TranslationX;
                ContentY = Content.TranslationY;
                break;
        }
    }

    void OnChildAdded(object sender, ElementEventArgs e)
    {
        Content.AnchorX = 0;
        Content.AnchorY = 0;
        Content.TranslationX = ContentX;
        Content.TranslationY = ContentY;
        Content.Scale = ContentScale;
    }
}