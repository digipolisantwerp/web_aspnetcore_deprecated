using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Digipolis.Web
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the given string, starting with a lowercase letter.
        /// </summary>
        /// <param name="input">The string that needs to be converted to camel case.</param>
        /// <returns>The given string, starting with a lowercase letter.</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            if (input.Length < 2)
            {
                return char.IsLower(input[0]) ? input : FirstLetterLowerCase(input);
            }

            var clean = Regex.Replace(input, @"[\W]", " ");

            var words = clean.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            var result = "";
            for (int i = 0; i < words.Length; i++)
            {
                var currentWord = words[i];
                if (i == 0)
                    result += FirstLetterLowerCase(currentWord);
                else
                    result += FirstLetterUpperCase(currentWord);

                if (currentWord.Length > 1) result += currentWord.Substring(1);
            }

            return result;
        }

        private static string FirstLetterLowerCase(string input)
        {
            return char.ToLowerInvariant(input[0]).ToString();
        }

        private static string FirstLetterUpperCase(string input)
        {
            return char.ToUpperInvariant(input[0]).ToString();
        }
    }
}
