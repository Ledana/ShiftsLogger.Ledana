using System.ComponentModel.DataAnnotations;

namespace ShiftsLoggerAPI.Ledana.DTOs
{
    public class ShiftDto
    {
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public int? EmployeeId { get; set; }
    }
}
