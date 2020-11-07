using System.Text.RegularExpressions;

namespace Pas.Common.Extensions
{
    /// <summary>
    ///     Extension methods for enumerarions
    /// </summary>
    /// <remarks>
    ///     Provide access to attributes or store logic relevant to enumeration item display.
    /// </remarks>
    public static class StringExtensions
    {
        /// <summary>
        ///     Converts an 'stringValue' to 'String Value'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="searchTerm"></param>
        /// <returns>string</returns>
        public static string HighlightTerm(this string value, string searchTerm, string tag = "em")
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(searchTerm)) return value;

            var rxNF = new Regex($@"(?si)(?<!\S).{{0,0}}(?<!\S)\S*{Regex.Escape(searchTerm)}\S*(?!\S).{{0,0}}(?!\S)",
                RegexOptions.Compiled);

            string Evaluator(Match match)
            {
                return $@"<{tag}>{match}</{tag}>";
            }

            return rxNF.Replace(value, Evaluator);
        }

        /// <summary>
        ///     Return the first item in the supplied list that is not null empty or whitespace
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValues"></param>
        /// <returns></returns>
        public static string DefaultTo(this string value, params string[] defaultValues)
        {
            if (!string.IsNullOrWhiteSpace(value)) return value;
            {
                foreach (var defaultValue in defaultValues)
                    if (!string.IsNullOrWhiteSpace(defaultValue))
                        return defaultValue;
            }

            return string.Empty;
        }


        /// <summary>
        ///     Converts an 'stringValue' to 'String Value'
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static string ToNormalForm(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            var rxNF =
                new Regex(@"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))",
                    RegexOptions.Compiled);

            return rxNF.Replace(value, @" ${a}");
        }


        /// <summary>
        ///     Converts an 'string_value' to 'String_Value'
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUnderScoreLowercase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            var rxNF =
                new Regex(@"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))",
                    RegexOptions.Compiled);

            return rxNF.Replace(value, @"_${a}").ToLower();
        }

        public static string StripNonAlpha(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            var rxNF =
                new Regex(@"[^a-zA-Z0-9]", RegexOptions.Compiled);

            return rxNF.Replace(value, @"");
        }


        public static string StripNumeric(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            var rxNF =
                new Regex(@"[\d\.]", RegexOptions.Compiled);

            return rxNF.Replace(value, @"");
        }

        public static string StripNonNumeric(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            var rxNF =
                new Regex(@"[^\d\.]", RegexOptions.Compiled);

            return rxNF.Replace(value, @"");
        }

        public static bool IsValidEmail(this string value)
        {
            var rxNF = new Regex(@".+@.+\..+", RegexOptions.Compiled);

            return rxNF.IsMatch(value);
        }

    }
}