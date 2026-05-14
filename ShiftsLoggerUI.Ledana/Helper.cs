using ShiftsLoggerUI.Ledana.UI;

namespace ShiftsLoggerUI.Ledana
{
    internal class Helper
    {
        internal static readonly ShiftsLoggerApiClient shiftsLoggerApiClient = new();

        //getting employee id when creating a new shift
        internal async Task<int> GetEmployeeId()
        {
            Console.WriteLine("Please insert employee id '1 - 6'");

            string? input = Console.ReadLine();
            int id;

            while (!int.TryParse(input, out id) || !await shiftsLoggerApiClient.IsEmployeeIdCorrect(id))
            {
                Console.WriteLine("Please put a valid id or type 'x' to go back");
                input = Console.ReadLine();
                if (input is not null && input.ToLower() == "x") return 0;
            }

            return id;
        }

        //getting shift id when the user wants to view a shift
        //they can choose from all the shifts in the db
        internal async Task<int> GetShiftIdFromAllShifts(string message)
        {
            Console.WriteLine(message);

            string? input = Console.ReadLine();
            int id;

            while (!int.TryParse(input, out id) || !await shiftsLoggerApiClient.IsShiftIdCorrectFromAllShifts(id))
            {
                Console.WriteLine("Please put a valid id or type 'x' to go back");
                input = Console.ReadLine();
                if (input is not null && input.ToLower() == "x") return 0;
            }

            return id;
        }
        //getting shift id when user wants to delete or update a shift
        //they can choose only from shifts that end today
        internal async Task<int> GetShiftIdFromTodaysShifts(string message)
        {
            Console.WriteLine(message);

            string? input = Console.ReadLine();
            int id;

            while (!int.TryParse(input, out id) || !await shiftsLoggerApiClient.IsShiftIdCorrectForToday(id))
            {
                Console.WriteLine("Please put a valid id or type 'x' to go back");
                input = Console.ReadLine();
                if (input is not null && input.ToLower() == "x") return 0;
            }

            return id;
        }

        //getting date input for get shifts per date in the right format
        internal DateTime GetDate(string message)
        {
            Console.WriteLine(message);
            string? timeString = Console.ReadLine();
            DateTime time;

            while (!Validator.ValidateDate(timeString, out time))
            {
                Console.WriteLine("Date Time format is not correct. Try again or type 'x' to go back");
                timeString = Console.ReadLine();
                if (timeString is not null && timeString.ToLower() == "x") return DateTime.MinValue;
            }
            return time;
        }

        //ask the user for a duration in format "hh:mm" to filter shifts in that duration
        internal async Task<TimeSpan> GetDuration(string message)
        {
            Console.WriteLine(message);
            string? time = Console.ReadLine();
            TimeSpan duration;

            while (!Validator.ValidateTimeSpan(time, out duration))
            {
                Console.WriteLine("Duration format is not correct. Try again or type 'x' to go back");
                time = Console.ReadLine();
                if (time is not null && time.ToLower() == "x") return TimeSpan.Zero;
            }
            return duration;
        }

        //checking for over lapping shift when creating one
        internal async Task<bool> CheckForOverlappingShifts(int employeeId, DateTime startTime, DateTime endTime)
        {
            var shiftsPerDate = await shiftsLoggerApiClient.GetShiftsPerDate(DateTime.Today);
            if (shiftsPerDate is null) return true;

            //check if employee already has a shift for the date
            if (shiftsPerDate.Any(s => s.EmployeeId == employeeId))
            {
                var existingShifts = shiftsPerDate.Where(s => s.EmployeeId == employeeId).ToList();
                if (existingShifts.Count == 2)
                {
                    Console.WriteLine("Employee cannot have more then 2 shifts per day");
                    TableVisualisation.ShowShifts(existingShifts);
                    return true;
                }
                var existingShift = existingShifts.First(s => s.EmployeeId == employeeId);

                if (Validator.ShiftOverLaps(startTime, endTime, existingShift))
                {
                    Console.WriteLine("The new shift cannot over lap the existing one");
                    TableVisualisation.ShowShift(existingShift);
                    return true;
                }
            }
            return false;
        }

        //checking for over lapping shift when updating one
        internal async Task<bool> CheckForOverlappingShifts(int employeeId, DateTime start, DateTime end, int shiftIdToUpdate)
        {
            var shiftsPerDate = await shiftsLoggerApiClient.GetShiftsPerDate(DateTime.Today);
            if (shiftsPerDate is null) return true;

            //skip the one we are updating
            var updatingShift = shiftsPerDate.First(s => s.Id == shiftIdToUpdate);

            if (shiftsPerDate.Remove(updatingShift))
            {
                //check if employee already has a shift for the date
                if (shiftsPerDate.Any(s => s.EmployeeId == employeeId))
                {
                    var existingShifts = shiftsPerDate.Where(s => s.EmployeeId == employeeId).ToList();
                    if (existingShifts.Count == 2)
                    {
                        Console.WriteLine("Employee cannot have more then 2 shifts per day");
                        TableVisualisation.ShowShifts(existingShifts);
                        return true;
                    }
                    var existingShift = existingShifts.First(s => s.EmployeeId == employeeId);

                    if (Validator.ShiftOverLaps(start, end, existingShift))
                    {
                        Console.WriteLine("The new shift cannot over lap the existing one");
                        TableVisualisation.ShowShift(existingShift);
                        return true;
                    }
                }
            }

            return false;
        }

        internal DateTime GetDateTime(string message)
        {
            Console.WriteLine(message);
            string? timeString = Console.ReadLine();
            if (timeString is not null && timeString.ToLower() == "x") return DateTime.MinValue;
            DateTime time;

            while (!Validator.ValidateDateTime(timeString, out time))
            {
                Console.WriteLine("Date time format is not correct, try again or type 'x' to go back");
                timeString = Console.ReadLine();
                if (timeString is not null && timeString.ToLower() == "x") return DateTime.MinValue;
            }
            return time;
        }

        internal bool ValidateDateInput(DateTime startTime, DateTime endTime)
        {
            //end date should be today
            if (DateOnly.FromDateTime(endTime) != DateOnly.FromDateTime(DateTime.Today)) return false;

            //start day can be yesterday or today
            if (DateOnly.FromDateTime(startTime) != DateOnly.FromDateTime(DateTime.Today) && DateOnly.FromDateTime(startTime) != DateOnly.FromDateTime(DateTime.Today.AddDays(-1))) return false;

            //end day can be yesterday only if end time is between 00:00 and 11:59 
            //and start time is between 12:00 and 23:59
            if (TimeOnly.FromDateTime(startTime) >= new TimeOnly(12, 00, 00) 
                && TimeOnly.FromDateTime(startTime) <= new TimeOnly(23, 59, 59)
                && TimeOnly.FromDateTime(endTime) >= new TimeOnly(00, 00, 00)
                && TimeOnly.FromDateTime(endTime) <= new TimeOnly(11, 59, 59)
                && DateOnly.FromDateTime(startTime) == DateOnly.FromDateTime(DateTime.Today.AddDays(-1))
                )
            {
                return true;
            }

            return true;
        }
    }
}
