namespace API.Extensions
{
    public static class StringExtensions
    {
        public static bool NotNullEmptyOrWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
