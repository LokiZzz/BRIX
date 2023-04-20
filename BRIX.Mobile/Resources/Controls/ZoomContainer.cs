﻿namespace BRIX.Mobile.Resources.Controls
{
    public class ZoomContainer : ContentView
    {
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;

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
        }



        public static readonly BindableProperty ContentTranslationXProperty = BindableProperty.Create(
            propertyName: nameof(ContentTranslationX),
            returnType: typeof(double),
            declaringType: typeof(ZoomContainer),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay
        );

        public double ContentTranslationX
        {
            get => (double)GetValue(ContentTranslationXProperty);
            set
            {
                SetValue(ContentTranslationXProperty, value);
                if (Content != null) Content.TranslationX = value;
            }
        }

        public static readonly BindableProperty ContentTranslationYProperty = BindableProperty.Create(
            propertyName: nameof(ContentTranslationY),
            returnType: typeof(double),
            declaringType: typeof(ZoomContainer),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay
        );

        public double ContentTranslationY
        {
            get => (double)GetValue(ContentTranslationYProperty);
            set
            {
                SetValue(ContentTranslationYProperty, value);
                if (Content != null) Content.TranslationY = value;
            }
        }

        public static readonly BindableProperty ContentScaleProperty = BindableProperty.Create(
            propertyName: nameof(ContentScale),
            returnType: typeof(double),
            declaringType: typeof(ZoomContainer),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay
        );

        public double ContentScale
        {
            get => (double)GetValue(ContentScaleProperty);
            set
            {
                SetValue(ContentScaleProperty, value);
                if (Content!= null) Content.Scale = value;
            }
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

                // Apply scale factor
                Content.Scale = currentScale;
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
                    Content.TranslationX = Math.Max(Math.Min(0, xOffset + e.TotalX), -Math.Abs(Content.Width - DeviceDisplay.MainDisplayInfo.Width));
                    Content.TranslationY = Math.Max(Math.Min(0, yOffset + e.TotalY), -Math.Abs(Content.Height - DeviceDisplay.MainDisplayInfo.Height));
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    xOffset = Content.TranslationX;
                    yOffset = Content.TranslationY;
                    break;
            }
        }
    }
}
