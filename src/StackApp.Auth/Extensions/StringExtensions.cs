namespace StackApp.Auth.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string ToEmail(this string fullName)
        {
            return String.Join(".", fullName.Split(' '));
        }

        public static string ToUserName(this string fullName)
        {
            return fullName.Remove(1, fullName.IndexOf(' ')).ToLowerInvariant();
        }
    }
}
