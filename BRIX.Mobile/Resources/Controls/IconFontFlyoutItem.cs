namespace BRIX.Mobile.Resources.Controls
{
    public class IconFontFlyoutItem : FlyoutItem
    {
        public static readonly BindableProperty GlyphProperty = BindableProperty.Create(
            nameof(GlyphProperty), 
            typeof(string), 
            typeof(IconFontFlyoutItem), 
            string.Empty
        );

        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public static readonly BindableProperty GlyphFontProperty = BindableProperty.Create(
            nameof(GlyphFontProperty),
            typeof(string),
            typeof(IconFontFlyoutItem),
            string.Empty
        );

        public string GlyphFont
        {
            get { return (string)GetValue(GlyphFontProperty); }
            set { SetValue(GlyphFontProperty, value); }
        }
    }
}
