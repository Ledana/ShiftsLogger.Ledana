namespace ShiftsLoggerAPI.Ledana.DTOs
{
    //this dto is only used to patch, when the user wants to only change some of the props
    public class PartialShiftDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? EmployeeId { get; set; }
    }
}
