using Microsoft.EntityFrameworkCore;
using ShiftsLoggerAPI.Ledana.Models;

namespace ShiftsLoggerAPI.Ledana.Data
{
    public class ShiftContext : DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public ShiftContext(DbContextOptions options)
            : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new() { Id = 1, FirstName = "Amelia", LastName = "Aster" },
                new() { Id = 2, FirstName = "Lory", LastName = "Marti" },
                new() { Id = 3, FirstName = "Laila", LastName = "Martini" },
                new() { Id = 4, FirstName = "Vivian", LastName = "Scott" },
                new() { Id = 5, FirstName = "Luiza", LastName = "Griffin" },
                new() { Id = 6, FirstName = "Violet", LastName = "Jinx" }
                );

            modelBuilder.Entity<Shift>()
    .Property(s => s.Duration)
    .HasComputedColumnSql(
        "CONVERT(varchar(5), DATEADD(MINUTE, DATEDIFF(minute, [StartTime], [EndTime]), 0),108)",
        stored: true);


            modelBuilder.Entity<Shift>().HasData(
                new()
                {
                    Id = 1,
                    StartTime = new(2026, 05, 01, 09, 30, 00),
                    EndTime = new(2026, 05, 01, 14, 35, 00),
                    EmployeeId = 2
                },
                new()
                {
                    Id = 2,
                    StartTime = new(2026, 05, 01, 10, 30, 00),
                    EndTime = new(2026, 05, 01, 16, 00, 00),
                    EmployeeId = 3
                },
                new()
                {
                    Id = 3,
                    StartTime = new(2026, 05, 01, 09, 00, 00),
                    EndTime = new(2026, 05, 01, 14, 55, 00),
                    EmployeeId = 4
                },
                new()
                {
                    Id = 4,
                    StartTime = new(2026, 05, 01, 09, 30, 00),
                    EndTime = new(2026, 05, 01, 17, 30, 00),
                    EmployeeId = 5
                },
                new()
                {
                    Id = 5,
                    StartTime = new(2026, 05, 01, 23, 30, 00),
                    EndTime = new(2026, 06, 02, 05, 35, 00),
                    EmployeeId = 2
                },
                new()
                {
                    Id = 6,
                    StartTime = new(2026, 05, 02, 06, 30, 00),
                    EndTime = new(2026, 05, 02, 16, 00, 00),
                    EmployeeId = 3
                },
                new()
                {
                    Id = 7,
                    StartTime = new(2026, 05, 02, 09, 00, 00),
                    EndTime = new(2026, 05, 02, 14, 55, 00),
                    EmployeeId = 4
                },
                new()
                {
                    Id = 8,
                    StartTime = new(2026, 05, 02, 12, 30, 00),
                    EndTime = new(2026, 05, 02, 17, 30, 00),
                    EmployeeId = 5
                },
                new()
                {
                    Id = 9,
                    StartTime = new(2026, 05, 02, 22, 30, 00),
                    EndTime = new(2026, 06, 03, 06, 00, 00),
                    EmployeeId = 6
                },
                new()
                {
                    Id = 10,
                    StartTime = new(2026, 05, 03, 12, 30, 00),
                    EndTime = new(2026, 05, 03, 19, 00, 00),
                    EmployeeId = 1
                },
                new()
                {
                    Id = 11,
                    StartTime = new(2026, 05, 03, 19, 00, 00),
                    EndTime = new(2026, 05, 03, 23, 55, 00),
                    EmployeeId = 2
                },
                new()
                {
                    Id = 12,
                    StartTime = new(2026, 05, 04, 09, 30, 00),
                    EndTime = new(2026, 05, 04, 17, 30, 00),
                    EmployeeId = 5
                },
                new()
                {
                    Id = 13,
                    StartTime = new(2026, 05, 06, 09, 30, 00),
                    EndTime = new(2026, 05, 06, 14, 35, 00),
                    EmployeeId = 2
                },
                new()
                {
                    Id = 14,
                    StartTime = new(2026, 05, 06, 10, 30, 00),
                    EndTime = new(2026, 05, 06, 16, 00, 00),
                    EmployeeId = 3
                },
                new()
                {
                    Id = 15,
                    StartTime = new(2026, 05, 07, 09, 00, 00),
                    EndTime = new(2026, 05, 07, 14, 55, 00),
                    EmployeeId = 4
                },
                new()
                {
                    Id = 16,
                    StartTime = new(2026, 05, 08, 09, 30, 00),
                    EndTime = new(2026, 05, 08, 17, 30, 00),
                    EmployeeId = 5
                },
                new()
                {
                    Id = 17,
                    StartTime = new(2026, 05, 08, 23, 30, 00),
                    EndTime = new(2026, 06, 09, 05, 35, 00),
                    EmployeeId = 2
                },
                new()
                {
                    Id = 18,
                    StartTime = new(2026, 05, 08, 06, 30, 00),
                    EndTime = new(2026, 05, 08, 16, 00, 00),
                    EmployeeId = 3
                },
                new()
                {
                    Id = 19,
                    StartTime = new(2026, 05, 08, 09, 00, 00),
                    EndTime = new(2026, 05, 09, 14, 55, 00),
                    EmployeeId = 4
                },
                new()
                {
                    Id = 20,
                    StartTime = new(2026, 05, 09, 12, 30, 00),
                    EndTime = new(2026, 05, 09, 17, 30, 00),
                    EmployeeId = 3
                },
                new()
                {
                    Id = 21,
                    StartTime = new(2026, 05, 09, 22, 30, 00),
                    EndTime = new(2026, 06, 10, 06, 00, 00),
                    EmployeeId = 6
                },
                new()
                {
                    Id = 22,
                    StartTime = new(2026, 05, 09, 12, 30, 00),
                    EndTime = new(2026, 05, 09, 19, 00, 00),
                    EmployeeId = 1
                },
                new()
                {
                    Id = 23,
                    StartTime = new(2026, 05, 09, 17, 00, 00),
                    EndTime = new(2026, 05, 09, 23, 25, 00),
                    EmployeeId = 2
                },
                new()
                {
                    Id = 24,
                    StartTime = new(2026, 05, 09, 21, 30, 00),
                    EndTime = new(2026, 05, 10, 05, 30, 00),
                    EmployeeId = 5
                }
                );
        }
    }
}
