

using System;
namespace SQLite_database
{
    public class Utils
    {


        public static DateTime FromUnixTime(long unixTimeMillis)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTimeMillis);
        }

        public static long DateTimeToUnixMillis(DateTime original)
        {
            original = original.ToUniversalTime();

            var dateTimeOffset = new DateTimeOffset(original);
            long unixDateTime = dateTimeOffset.ToUnixTimeMilliseconds();

            return unixDateTime;
        }
    }
}