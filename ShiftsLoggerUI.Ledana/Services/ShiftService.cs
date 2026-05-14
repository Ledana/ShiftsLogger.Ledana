using ShiftsLoggerAPI.Ledana.DTOs;
using ShiftsLoggerAPI.Ledana.Models;
using ShiftsLoggerUI.Ledana.UI;
using Spectre.Console;
using System.Globalization;
using System.Timers;

namespace ShiftsLoggerUI.Ledana.Services
{
    internal class ShiftService
    {
        internal static readonly ShiftsLoggerApiClient shiftsLoggerApiClient = new();
        internal static readonly Helper helper = new();
        internal static Random random = new();

        //user can add a new shift with an employee id. start of shift will be now
        //and end randomly chosen from a 6 hour to a 12 hour shift
        internal async Task CreateNewShift()
        {
            int employeeId = await helper.GetEmployeeId();
            if (employeeId == 0) return;

            DateTime startTime = DateTime.Now;
            Console.WriteLine($"Shift start at {startTime:HH:mm}");

            Thread.Sleep(2000);

            DateTime endTime = DateTime.Now.AddHours(random.Next(4, 12)).AddMinutes(random.Next(1, 60));
            Console.WriteLine($"Shift end at {endTime:HH:mm}\n");
            
            if (await helper.CheckForOverlappingShifts(employeeId, startTime, endTime))
                return;

            ShiftDto shift = new()
            {
                EmployeeId = employeeId,
                StartTime = startTime,
                EndTime = endTime
            };

            Console.WriteLine();
            Console.WriteLine(await shiftsLoggerApiClient.CreateShift(shift));
            Console.WriteLine();
        }

        //user can delete or update shifts only from shifts that ended today
        internal async Task DeleteShift()
        {
            var shifts = await shiftsLoggerApiClient.GetShiftsPerDate(DateTime.Today);
            if (shifts is not null && shifts.Count != 0) TableVisualisation.ShowShifts(shifts);
            else
            {
                Console.WriteLine("No shifts for today yet");
                return;
            }

            int id = await helper.GetShiftIdFromTodaysShifts("Please put the id of the shift you want to delete");
            if (id == 0) return;

            Console.WriteLine(await shiftsLoggerApiClient.DeleteShift(id));
        }

        //user can update a shift and if the shift goes on over night the start should be
        //the day before and end today
        internal async Task UpdateWholeShift()
        {
            Console.WriteLine("If you're trying to update into a night shift, please have in mind that the start time should be on the day before and end time today");

            var shifts = await shiftsLoggerApiClient.GetShiftsPerDate(DateTime.Today);
            if (shifts is not null && shifts.Count != 0) TableVisualisation.ShowShifts(shifts);
            else
            {
                Console.WriteLine("No shifts for today yet");
                return;
            }

            int shiftIdToUpdate = await helper.GetShiftIdFromTodaysShifts("Please put the id of the shift you want to update");
            if (shiftIdToUpdate == 0) return;

            bool isDateRight = false;
            DateTime startTime = new();
            DateTime endTime = new();
            int employeeId = 0;

            while (!isDateRight)
            {
                startTime = helper.GetDateTime("Please put the new Start Time (yyyy-MM-ddTHH:mm:ss)");
                endTime = helper.GetDateTime("Please put the new End Time (yyyy-MM-ddTHH:mm:ss)");
                employeeId = await helper.GetEmployeeId();

                isDateRight = helper.ValidateDateInput(startTime, endTime);
                if (!isDateRight) Console.WriteLine("Incorrect input in dates. Try again or type 'x' to exit");
            }
            Console.WriteLine($"Shift start at {startTime:HH:mm}");
            Console.WriteLine($"Shift end at {endTime:HH:mm}\n");

            if (await helper.CheckForOverlappingShifts(employeeId, startTime, endTime, shiftIdToUpdate))
                return;

            if (endTime - startTime > new TimeSpan(12, 00, 00))
            {
                Console.WriteLine("Shift can not be longer then 12 hours");
                return;
            }

            ShiftDto shift = new()
            {
                EmployeeId = employeeId,
                StartTime = startTime,
                EndTime = endTime
            };

            Console.WriteLine("\n" + await shiftsLoggerApiClient.UpdateWholeShift(shiftIdToUpdate, shift) + "\n");
        }

        internal async Task UpdatePartialShift()
        {
            Console.WriteLine("If you're trying to update into a night shift, please have in mind that the start time should be on the day before and end time today");

            var shifts = await shiftsLoggerApiClient.GetShiftsPerDate(DateTime.Today);
            if (shifts is not null && shifts.Count != 0) TableVisualisation.ShowShifts(shifts);
            else
            {
                Console.WriteLine("No shifts for today yet");
                return;
            }

            int shiftIdToUpdate = await helper.GetShiftIdFromTodaysShifts("Please put the id of the shift you want to update");
            if (shiftIdToUpdate == 0) return;

            var existingShift = await shiftsLoggerApiClient.GetShiftById(shiftIdToUpdate);

            if (existingShift is null)
            {
                Console.WriteLine("Couldn't find shift");
                return;
            }
            //keeping existing values to validate over lapping
            DateTime existingStart = existingShift.StartTime;
            DateTime existingEnd = existingShift.EndTime;
            int existingEmployeeId = existingShift.EmployeeId;

            //passing null if the prop is not changed
            DateTime? startTime = null;
            DateTime? endTime = null;
            int? employeeId = null;

            bool isDateRight = false;
            bool shiftIsChanging = false;

            while (!isDateRight)
            {
                if (AnsiConsole.Confirm("Do you want to update start time?"))
                    {
                        startTime = helper.GetDateTime("Please put the new Start Time (yyyy-MM-ddTHH:mm:ss)");
                        if (startTime == DateTime.MinValue) return;
                        existingStart = (DateTime)startTime;
                        shiftIsChanging = true;
                    }

                if (AnsiConsole.Confirm("Do you want to update end time?"))
                    {
                        endTime = helper.GetDateTime("Please put the new End Time (yyyy-MM-ddTHH:mm:ss)");
                        if (endTime == DateTime.MinValue) return;
                        existingEnd = (DateTime)endTime;
                        shiftIsChanging = true;
                }

                if (AnsiConsole.Confirm("Do you want to update employee id?"))
                    {
                        employeeId = await helper.GetEmployeeId();
                        if (employeeId == 0) return;
                        existingEmployeeId = (int)employeeId;
                        shiftIsChanging = true;
                }

                isDateRight = helper.ValidateDateInput(existingStart, existingEnd);
                if (!isDateRight) Console.WriteLine("Incorrect input in dates. Try again or type 'x' to exit");
            }

            Console.WriteLine($"Shift start at {existingStart:HH:mm}");
            Console.WriteLine($"Shift end at {existingEnd:HH:mm}\n");

            if (await helper.CheckForOverlappingShifts(existingEmployeeId, existingStart, existingEnd, shiftIdToUpdate))
                return;


            if (existingEnd - existingStart > new TimeSpan(12, 00, 00))
            {
                Console.WriteLine("Shift can not be longer then 12 hours");
                return;
            }

            if (shiftIsChanging)
            {
                PartialShiftDto shift = new()
                {
                    EmployeeId = employeeId,
                    StartTime = startTime,
                    EndTime = endTime
                };

                Console.WriteLine();
                Console.WriteLine(await shiftsLoggerApiClient.UpdatePartialShift(shiftIdToUpdate, shift));
                Console.WriteLine();
            }
            else
                Console.WriteLine("Shift stayed the same!");
        }

        internal async Task ViewShift()
        {
            await GetAllShifts();
            int id = await helper.GetShiftIdFromAllShifts("Please put the id of the shift you want to see");
            if (id == 0) return;

            var shift = await shiftsLoggerApiClient.GetShiftById(id);

            if (shift is null)
            {
                Console.WriteLine("Not found");
                return;
            }

            TableVisualisation.ShowShift(shift);
        }
        internal async Task GetAllShifts()
        {
            int pageNumber = 1;
            int pageSize = 5;
            bool keepRunning = true;

            while (keepRunning)
            {
                var response = await shiftsLoggerApiClient.GetAllShifts(pageNumber, pageSize);

                ViewAllShifts(ref response, ref pageNumber, ref pageSize, ref keepRunning);   
            }
        }

        private void ViewAllShifts(ref ApiResponseDto<List<Shift>>? response, ref int pageNumber, ref int pageSize, ref bool keepRunning)
        {
            if (response is null) { Console.WriteLine("No shifts found!"); return; }

            Console.Clear();
            TableVisualisation.ShowShifts(response.Data);
            Console.WriteLine($"Total count: {response.TotalCount}");
            Console.WriteLine($"Current page: {response.CurrentPage}");
            Console.WriteLine($"Page size: {response.PageSize}");
            Console.WriteLine($"Has next: {response.HasNext}");
            Console.WriteLine($"Has previous: {response.HasPrevious}");

            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .AddChoices("Next", "Previous", "First", "Last", "Go Back"));

            switch (choice)
            {
                case "Next":
                    if (!response.HasNext) return;
                    pageNumber++;
                    break;
                case "Previous":
                    if (!response.HasPrevious) return;
                    if (pageNumber > 1) pageNumber--;
                    break;
                case "First":
                    pageNumber = 1;
                    break;
                case "Last":
                    pageNumber = response.TotalCount % pageSize == 0 ? response.TotalCount / pageSize : response.TotalCount / pageSize + 1;
                    break;
                case "Go Back":
                    keepRunning = false;
                    break;
            }
        }

        internal async Task ViewShiftsPerDate()
        {
            var date = helper.GetDate("Please put the date in format: \"yyyy-mm-dd\"");
            if (date == DateTime.MinValue) return;

            var shifts = await shiftsLoggerApiClient.GetShiftsPerDate(date);
            if (shifts is null || shifts.Count == 0) Console.WriteLine("No shifts found!");

            else
                TableVisualisation.ShowShifts(shifts);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        internal async Task ViewShiftsPerEmployeeId()
        {
            var id = await helper.GetEmployeeId();
            if (id == 0) return;

            var shifts = await shiftsLoggerApiClient.GetShiftsPerEmployeeId(id);

            if (shifts is null || shifts.Count == 0) Console.WriteLine("No shifts found!");

            else
                TableVisualisation.ShowShifts(shifts);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        internal async Task ViewShiftsPerDuration()
        {
            TimeSpan duration = await helper.GetDuration("Please put the duration in format: \"HH:mm\"");
            if (duration == TimeSpan.Zero) return;

            var shifts = await shiftsLoggerApiClient.GetShiftsPerDuration(duration.ToString(@"hh\:mm"));
            if (shifts is null || shifts.Count == 0) Console.WriteLine("No shifts found!");

            else
                TableVisualisation.ShowShifts(shifts);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        internal async Task ViewShiftsSortedByDate()
        {
            int pageNumber = 1;
            int pageSize = 5;
            bool keepRunning = true;

            while (keepRunning)
            {
                var response = await shiftsLoggerApiClient.GetShiftsSortedByDate(pageNumber, pageSize);

                ViewAllShifts(ref response, ref pageNumber, ref pageSize, ref keepRunning);

            }
        }

        internal async Task ViewShiftsSortedByDuration()
        {
            int pageNumber = 1;
            int pageSize = 5;
            bool keepRunning = true;

            while (keepRunning)
            {
                var response = await shiftsLoggerApiClient.GetShiftsSortedByDuration(pageNumber, pageSize);

                ViewAllShifts(ref response, ref pageNumber, ref pageSize, ref keepRunning);

            }
        }

        internal async Task ViewShiftsSortedByEmployeeId()
        {
            int pageNumber = 1;
            int pageSize = 5;
            bool keepRunning = true;

            while (keepRunning)
            {
                var response = await shiftsLoggerApiClient.GetShiftsSortedByEmployeeId(pageNumber, pageSize);

                ViewAllShifts(ref response, ref pageNumber, ref pageSize, ref keepRunning);

            }
        }

        internal async Task GetTodaysShifts()
        {
            var shifts = await shiftsLoggerApiClient.GetShiftsPerDate(DateTime.Today);
            if (shifts is null || shifts.Count == 0) Console.WriteLine("No shifts found!");

            else
                TableVisualisation.ShowShifts(shifts);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        internal async Task ViewShiftsBelowDuration()
        {
            TimeSpan duration = await helper.GetDuration("Please put the duration in format: \"HH:mm\"");
            if (duration == TimeSpan.Zero) return;

            var shifts = await shiftsLoggerApiClient.GetAllShifts();
            if (shifts is null || shifts.Count == 0)
            {
                Console.WriteLine("No shifts found!");
                return;
            }

            var shiftsBelowDuration =
                 shifts
                .Where(s =>
                    StringToTimespan(s.Duration)
                    <
                    duration).ToList();
            

                TableVisualisation.ShowShifts(shiftsBelowDuration);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
        }

        internal TimeSpan StringToTimespan(string timeSpan)
        {
            if (TimeSpan.TryParseExact(timeSpan,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                TimeSpanStyles.None,
                out TimeSpan time))
                return time;

            return new TimeSpan(0, 0, 0);
        }

        internal async Task ViewShiftsAboveDuration()
        {
            TimeSpan duration = await helper.GetDuration("Please put the duration in format: \"HH:mm\"");
            if (duration == TimeSpan.Zero) return;

            var shifts = await shiftsLoggerApiClient.GetAllShifts();
            if (shifts is null || shifts.Count == 0)
            {
                Console.WriteLine("No shifts found!");
                return;
            }

            var shiftsAboveDuration =
                 shifts
                .Where(s =>
                    StringToTimespan(s.Duration)
                    >
                    duration).ToList();


            TableVisualisation.ShowShifts(shiftsAboveDuration);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
