using System;
using System.Globalization;

namespace _Game.Development.Static
{
    public static class DateExtension
    {
        public static string DateTimeToString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        public static DateTime StringToDateTime(this string dateTimeString)
        {
            var success = DateTime.TryParseExact(dateTimeString, "yyyy-MM-ddTHH:mm:ssZ",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime);

            if (success)
                return dateTime;
            throw new FormatException("Geçersiz tarih formatı");
        }
    }
}