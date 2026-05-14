namespace ShiftsLoggerAPI.Ledana.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public List<Shift> Shifts { get; set; } = [];

    }
}
