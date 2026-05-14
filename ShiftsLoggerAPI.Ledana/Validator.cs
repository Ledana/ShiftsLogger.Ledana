namespace ShiftsLoggerAPI.Ledana
{
    public static class Validator
    {
        public static bool ValidateEndTime(DateTime start, DateTime end)
        {
            return end > start;
        }
    }
}
