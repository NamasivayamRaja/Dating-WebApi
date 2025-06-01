namespace API.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }
    }
}
