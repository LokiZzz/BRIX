namespace BRIX.Utility.FormatProviders
{
    public class PluralFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object? GetFormat(Type? formatType)
        {
            return this;
        }


        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(format))
            {
                return arg?.ToString() ?? string.Empty;
            }

            string[] forms = format.Split(';');
            int value = (int)arg;
            int form = value == 1 ? 0 : 1;
            return value.ToString() + " " + forms[form];
        }
    }
}
