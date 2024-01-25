using BRIX.Lexis;
using BRIX.Library.DiceValue;
using System.Globalization;

namespace BRIX.Lexica
{
    public class LexisFormatter : ICustomFormatter, IFormatProvider
    {
        private readonly CultureInfo _culture;

        private const string PropertyFormatType = "prop";
        private const string ComplexFormatType = "comp";

        public LexisFormatter(CultureInfo cultureInfo)
        {
            _culture = cultureInfo;
        }

        public object? GetFormat(Type? formatType) => this;

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (!Equals(formatProvider))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format));
            }

            if(arg == null)
            {
                return string.Empty;
            }

            string[] splittedFormat = format.Split('.');
            
            switch(splittedFormat.First())
            {
                case PropertyFormatType:
                    return FormatProperty(splittedFormat[1], arg);
                case ComplexFormatType:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException($"Неизвестный формат {splittedFormat.First()}");
            }
        }

        private string FormatProperty(string propertyFormat, object arg)
        {
            string[] splittedPropertyFormat = propertyFormat.Split("--");
            object? propertyValue = arg.GetType().GetProperty(splittedPropertyFormat[0])?.GetValue(arg);

            //TODO: switch(propertyValue) case Type1: case Type2: ...

            bool haveDeclension = splittedPropertyFormat.Count() > 0;

            switch (_culture.Name)
            {
                case "en-US":
                    return FormatPropertyEng(splittedPropertyFormat, propertyValue, haveDeclension);
                case "ru-RU":
                    return FormatPropertyRus(splittedPropertyFormat, propertyValue, haveDeclension);
                default:
                    throw new NotImplementedException($"Для культуры {_culture.Name} не предоставлена реализация.");
            }
        }

        private string FormatPropertyEng(string[] splittedPropertyFormat, object? propertyValue, bool haveDeclension)
        {
            string nominative = splittedPropertyFormat[1];

            if(haveDeclension)
            {
                if (propertyValue is int number)
                {
                    return Numbers.ENGDeclension(number, nominative, true);
                }
                else if(propertyValue is DicePool dicePool)
                {
                    return Numbers.ENGDeclension(dicePool, nominative);
                }
                else
                {
                    throw new ArgumentException(
                        "Склонения от числительных могут быть применены только к числам или костям."
                    );
                }
            }
            else
            {
                return propertyValue?.ToString() ?? string.Empty;
            }
        }

        private string FormatPropertyRus(string[] splittedPropertyFormat, object? propertyValue, bool haveDeclension)
        {
            if (haveDeclension)
            {
                string[] declensions = splittedPropertyFormat[1].Split(',');

                if (propertyValue is int number)
                {
                    return Numbers.RUSDeclension(number, declensions[0], declensions[1], declensions[2], true);
                }
                else if (propertyValue is DicePool dicePool)
                {
                    return Numbers.RUSDeclension(dicePool, declensions[0], declensions[1], declensions[2]);
                }
                else
                {
                    throw new ArgumentException(
                        "Склонения от числительных могут быть применены только к числам или костям."
                    );
                }
            }
            else
            {
                return propertyValue?.ToString() ?? string.Empty;
            }
        }
    }
}
