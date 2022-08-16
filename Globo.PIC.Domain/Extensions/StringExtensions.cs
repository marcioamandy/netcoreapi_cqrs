using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Globo.PIC.Domain.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Trims and Removes duplicated spaces between words
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimAndRemoveSpaces(this string value)
        {
            return string.Join(" ", value.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetEnumValueFromDescription<T>(this string description)
        {
            var type = typeof(T);

            if (!type.IsEnum) throw new ArgumentException();

            FieldInfo[] fields = type.GetFields();
            var field = fields
                        .SelectMany(f => f.GetCustomAttributes(
                            typeof(DescriptionAttribute), false), (
                                f, a) => new { Field = f, Att = a })
                        .Where(a => ((DescriptionAttribute)a.Att)
                            .Description == description).SingleOrDefault();

            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
    }
}
