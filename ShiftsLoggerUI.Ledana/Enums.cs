
namespace ShiftsLoggerUI.Ledana
{
    internal class Enums
    {
        internal enum MainMenuOptions
        {
            AddANewShift,
            AddNewNightShift,
            DeleteAShift,
            UpdateAWholeShift,
            UpdatePartOfAShift,
            ViewingShiftsMenu,
            Quit
        }

        internal enum ViewingShiftsOptions
        {
            ViewAShift,
            ViewAllShifts,
            ViewTodaysShifts,
            ViewShiftsPerFilter,
            ViewShiftsSorted,
            GoBack
        }

        internal enum ViewShiftsPerFilter
        {
            ViewShiftsPerDate,
            ViewShiftsPerEmployeeId,
            ViewShiftsPerDuration,
            GoBack
        }

        internal enum ViewShiftsSorted
        {
            ViewShiftsSortedByDate,
            ViewShiftsSortedByEmployeeId,
            ViewShiftsSortedByDuration,
            GoBack
        }

        internal enum ShiftsPerDurationOptions
        {
            ViewShiftsOnThatDuration,
            ViewShiftsBelowThatDuration,
            ViewShiftsAboveThatDuration,
            GoBack
        }
    }
}
