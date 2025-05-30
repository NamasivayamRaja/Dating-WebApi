namespace API.Extensions
{
    public static class EnumExtension
    {
        public static int ToInt<TEnum>(this TEnum enumValue) where TEnum : struct, Enum
        {
            return Convert.ToInt32(enumValue);
        }

        public static TEnum? FromString<TEnum>(this string? value) where TEnum : struct, Enum
        {
            if(Enum.TryParse<TEnum>(value, true, out var parsedEnum))
            {
                return parsedEnum;
            }

            return null;
        }
    }
}
