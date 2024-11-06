using System.Text;

namespace CommonService.Extentions
{
    public static class StringExtentions
    {
        public static string? ToCamelCase(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        public static int? ToIntNullable(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return int.TryParse(str, out var val) ? val : null;
        }

        public static int ToInt(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default;
            }
            return int.TryParse(str, out var val) ? val : default;
        }

        public static byte[] ConvertStringToByteArray(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static bool IsNullOrEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
