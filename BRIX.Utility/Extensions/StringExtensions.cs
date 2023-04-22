namespace BRIX.Utility.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            return string.IsNullOrEmpty(input)
                ? string.Empty
                : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
        }

        /// <summary>
        /// Add a compound string format with singular and plural variants of the word.
        /// <para>
        /// <example>
        /// value format like {"index":"singular variant";"plural variant"}
        /// <para>
        /// How to use
        /// </para>
        /// <code>
        /// Resource.StringValue.PluralFormat(valAtIndex1, valAtIndex2...);
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        public static string PluralFormat(this string format, params object[] values) => string.Format(new FormatProviders.PluralFormatProvider(), format, values);
    }
}
