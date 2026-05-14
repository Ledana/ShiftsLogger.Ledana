using Microsoft.AspNetCore.Mvc;

namespace ShiftsLoggerAPI.Ledana.DTOs
{
    public class ShiftOptions
    {
        [FromQuery(Name = "date")]
        public DateTime? Date { get; set; }
        [FromQuery(Name = "duration")]
        public string? Duration { get; set; }
        [FromQuery(Name = "employee_id")]
        public int? EmployeeId { get; set; }

        [FromQuery(Name = "sort_by")]
        public string SortBy { get; set; } = "id";
        [FromQuery(Name = "sort_order")]
        public string SortOrder { get; set; } = "ASC";
        [FromQuery(Name = "search")]
        public string Search { get; set; } = string.Empty;
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; } = 10;
        [FromQuery(Name = "page_number")]
        public int PageNumber { get; set; } = 1;
        
    }
}
