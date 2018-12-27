namespace StackApp.Auth.Extensions
{
    using System;

    public static class DateExtensions
    {
        public static DateTime RandomDate(this DateTime date)
        {
            var startDate = new DateTime(1973, 1, 1);
            var endDate = new DateTime(2004, 12, 31);
            TimeSpan timeSpan = endDate - startDate;
            var randomTest = new Random();
            TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
            return startDate + newSpan;
        }
    }
}
