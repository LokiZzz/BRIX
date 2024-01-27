﻿using BRIX.Lexis;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using System.Globalization;
using System.Net;
using System.Reflection;

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

        public string Format(string? format, object? model, IFormatProvider? formatProvider)
        {
            if (!Equals(formatProvider))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format));
            }

            if(model == null)
            {
                return string.Empty;
            }

            string[] spliitedFormat = format.Split('.');
            string formatType = spliitedFormat[0];
            string formatValue = spliitedFormat[1];

            switch (formatType)
            {
                case PropertyFormatType:
                    return FormatProperty(formatValue, model);
                case ComplexFormatType:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException($"Неизвестный формат {formatType}");
            }
        }

        private string FormatProperty(string propertyFormat, object model)
        {
            string[] splittedPropertyFormat = propertyFormat.Split("--");
            object? propertyValue = model.GetType().GetProperty(splittedPropertyFormat[0])?.GetValue(model);
            string? propertyOptions = splittedPropertyFormat.Count() > 1 ? splittedPropertyFormat[1] : null;

            switch (propertyValue)
            {
                case int or DicePool:
                    return FormatNumericProperty(model, propertyValue, propertyOptions);
                case Enum enumValue:
                    return FormatEnumProperty(model, propertyFormat, enumValue);
                case List<(Enum, string)> multiCondition:
                    return FormatMultiCondition(model, propertyFormat, multiCondition);
                case string stringValue:
                    return stringValue;
                default:
                    throw new NotImplementedException($"Нет формата для свойства с типом {propertyValue?.GetType()}");
            }
        }

        private string FormatMultiCondition(object model, string propertyName, List<(Enum, string)> propertyValue)
        {
            string resultString = string.Empty;
            
            foreach((Enum Condition, string Comment) entry in propertyValue.Select(x => (x.Item1, x.Item2)))
            {
                string resourceName = $"{model.GetType().Name}_{propertyName}_{entry.Condition}";
                resultString += ResourceHelper.GetResourceString(resourceName);

                if (!string.IsNullOrEmpty(entry.Comment))
                {
                    resultString += $": «{entry.Comment}»";
                }

                resultString += "; ";
            }

            resultString = resultString.Trim();

            return resultString;
        }

        private string FormatEnumProperty(object model, string propertyName, Enum propertyValue)
        {
            string resourceName = $"{model.GetType().Name}_{propertyName}_{propertyValue}";

            return ResourceHelper.GetResourceString(resourceName);
        }

        private string FormatNumericProperty(object model, object? propertyValue, string? propertyOptions)
        {
            switch (_culture.Name)
            {
                case "en-US":
                    return FormatNumericPropertyEng(model, propertyValue, propertyOptions);
                case "ru-RU":
                    return FormatNumericPropertyRus(model, propertyValue, propertyOptions);
                default:
                    throw new NotImplementedException($"Для культуры {_culture.Name} не предоставлена реализация.");
            }
        }

        private string FormatNumericPropertyEng(object model, object? propertyValue, string? declensionString)
        {
            if(!string.IsNullOrEmpty(declensionString))
            {
                string nominative = declensionString;

                if (declensionString.Contains('@') == true)
                {
                    string declensionPropertyName = declensionString.Replace("@", "");
                    object? optionsPropertyValue = model.GetType().GetProperty(declensionPropertyName)?.GetValue(model);
                    string resourceName = $"{model.GetType().Name}_{declensionPropertyName}_{optionsPropertyValue}";
                    nominative = ResourceHelper.GetResourceString(resourceName);
                }

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

        private string FormatNumericPropertyRus(object model, object? propertyValue, string? declensionsString)
        {
            if (!string.IsNullOrEmpty(declensionsString))
            {
                string[] declensions;

                if (declensionsString.Contains('@') == true)
                {
                    string declensionPropertyName = declensionsString.Replace("@", "");
                    object? optionsPropertyValue = model.GetType().GetProperty(declensionPropertyName)?.GetValue(model);
                    string resourceName = $"{model.GetType().Name}_{declensionPropertyName}_{optionsPropertyValue}";
                    declensions = ResourceHelper.GetResourceString(resourceName).Split(',');
                }
                else
                {
                    declensions = declensionsString.Split(',');
                }

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
