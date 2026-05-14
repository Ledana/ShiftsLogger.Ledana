using ShiftsLoggerAPI.Ledana.Models;
using Spectre.Console;

namespace ShiftsLoggerUI.Ledana.UI
{
    internal class TableVisualisation
    {
        internal static void ShowShifts(List<Shift>? shifts)
        {
            if(shifts is null)
            {
                Console.WriteLine("Couldn't fetch shifts");
                return;
            }

            var table = new Table();
            table.ShowRowSeparators();

            table.AddColumn("Shift Id");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Duration");
            table.AddColumn("Employee Id");
            table.AddColumn("First Name");
            table.AddColumn("Last Name");

            foreach (var item in shifts)
            {
                table.AddRow(item.Id.ToString(),
                    item.StartTime.ToString(),
                    item.EndTime.ToString(),
                    item.Duration.ToString(),
                    item.EmployeeId.ToString(),
                    item.Employee.FirstName,
                    item.Employee.LastName);
            }
            AnsiConsole.Write(table);    
        }

        internal static void ShowShift(Shift shift)
        {
            var panel = new Panel($@"Id: {shift.Id}
Start time: {shift.StartTime}
End time: {shift.EndTime}
Duration: {shift.Duration}
Employee Id: {shift.EmployeeId}
First Name: {shift.Employee.FirstName}
Last Name: {shift.Employee.LastName}")
            {
                Header = new PanelHeader("Shift's info"),
                Padding = new Padding(2, 2, 2, 2)
            };

            AnsiConsole.Write(panel);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        internal static void ShowEmployees(List<Employee>? employees)
        {

            if (employees is null)
            {
                Console.WriteLine("Employees could not be loaded");
                return;
            }
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("First Name");
            table.AddColumn("Last Name");

            foreach (var item in employees)
            {
                table.AddRow(item.Id.ToString(), item.FirstName, item.LastName);
            }
            AnsiConsole.Write(table);
        }
    }
}
