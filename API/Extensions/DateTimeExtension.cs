namespace API.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge(this DateOnly date)
        {
            var todayDate = DateOnly.FromDateTime(DateTime.Now);
            int age = todayDate.Year - date.Year;

            if (date > todayDate.AddYears(-age))
                age--;

            return age;
        }
    }
}
