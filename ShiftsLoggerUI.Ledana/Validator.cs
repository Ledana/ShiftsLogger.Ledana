using ShiftsLoggerAPI.Ledana.Models;
using System.Globalization;

namespace ShiftsLoggerUI.Ledana
{
    internal static class Validator
    {
        internal static bool ValidateDate(string? date, out DateTime start)
        {
            return DateTime.TryParseExact(date,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start);
        }

        internal static bool ValidateTimeSpan(string? time, out TimeSpan end)
        {
            return TimeSpan.TryParseExact(time,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                TimeSpanStyles.None,
                out end);
        }

        internal static bool ShiftOverLaps(DateTime start, DateTime end, Shift existingShift)
        {
            return (end >= existingShift.StartTime && end <= existingShift.EndTime
                ||
                start >= existingShift.StartTime && start <= existingShift.EndTime
                ||
                start <= existingShift.StartTime && end >= existingShift.EndTime
                );
        }

        internal static bool ValidateDateTime(string? timeString, out DateTime time)
        {
            return DateTime.TryParseExact(timeString,
                "yyyy-MM-ddTHH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out time);
        }
    }
}
