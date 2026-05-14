using ShiftsLoggerUI.Ledana.Services;
using Spectre.Console;
using static ShiftsLoggerUI.Ledana.Enums;

namespace ShiftsLoggerUI.Ledana.UI
{
    internal class UserInterface
    {
        static ShiftService shiftService = new();
        internal async Task MainMenu()
        {
            bool flag = true;
            Console.WriteLine("Welcome to our app");
            while (flag)
            {
                Console.WriteLine("Press any key to view the main menu");
                Console.ReadKey();
                Console.Clear();

                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(
                        MainMenuOptions.AddANewShift,
                        MainMenuOptions.DeleteAShift,
                        MainMenuOptions.UpdateAWholeShift,
                        MainMenuOptions.UpdatePartOfAShift,
                        MainMenuOptions.ViewingShiftsMenu,
                        MainMenuOptions.Quit
                        ));

                switch (option)
                {
                    case MainMenuOptions.AddANewShift:
                        await shiftService.CreateNewShift();
                        break;
                    case MainMenuOptions.DeleteAShift:
                        await shiftService.DeleteShift();
                        break;
                    case MainMenuOptions.UpdateAWholeShift:
                        await shiftService.UpdateWholeShift();
                        break;
                    case MainMenuOptions.UpdatePartOfAShift:
                        await shiftService.UpdatePartialShift();
                        break;
                    case MainMenuOptions.ViewingShiftsMenu:
                        await ViewingShiftsMenu();
                        break;

                    case MainMenuOptions.Quit:
                        Console.WriteLine("Good bye!");
                        Console.ReadKey();
                        flag = false;
                        break;
                }
            }
        }

        private async Task ViewingShiftsMenu()
        {
            bool flag = true;

            while (flag)
            {
                Console.Clear();
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ViewingShiftsOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(ViewingShiftsOptions.ViewAShift,
                    ViewingShiftsOptions.ViewAllShifts,
                    ViewingShiftsOptions.ViewTodaysShifts,
                    ViewingShiftsOptions.ViewShiftsPerFilter,
                    ViewingShiftsOptions.ViewShiftsSorted,
                    ViewingShiftsOptions.GoBack
                        ));
                switch (option)
                {
                    case ViewingShiftsOptions.ViewAShift:
                        await shiftService.ViewShift();
                        break;
                    case ViewingShiftsOptions.ViewAllShifts:
                        await shiftService.GetAllShifts();
                        break;
                    case ViewingShiftsOptions.ViewTodaysShifts:
                        await shiftService.GetTodaysShifts();
                        break;
                    case ViewingShiftsOptions.ViewShiftsPerFilter:
                        await ShiftsPerFilterMenu();
                        break;
                    case ViewingShiftsOptions.ViewShiftsSorted:
                        await ShiftsSortedMenu();
                        break;
                    case ViewingShiftsOptions.GoBack:
                        flag = false;
                        break;
                }
            }
        }


        private async Task ShiftsSortedMenu()
        {
            bool flag = true;

            while (flag)
            {
                Console.Clear();
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ViewShiftsSorted>()
                    .Title("What do you want to do?")
                    .AddChoices(ViewShiftsSorted.ViewShiftsSortedByDate,
                    ViewShiftsSorted.ViewShiftsSortedByDuration,
                    ViewShiftsSorted.ViewShiftsSortedByEmployeeId,
                    ViewShiftsSorted.GoBack
                        ));
                switch (option)
                {
                    case ViewShiftsSorted.ViewShiftsSortedByDate:
                        await shiftService.ViewShiftsSortedByDate();
                        break;
                    case ViewShiftsSorted.ViewShiftsSortedByDuration:
                        await shiftService.ViewShiftsSortedByDuration();
                        break;
                    case ViewShiftsSorted.ViewShiftsSortedByEmployeeId:
                        await shiftService.ViewShiftsSortedByEmployeeId();
                        break;
                    case ViewShiftsSorted.GoBack:
                        flag = false;
                        break;
                }
            }
        }

        private async Task ShiftsPerFilterMenu()
        {
            bool flag = true;

            while (flag)
            {
                Console.Clear();
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ViewShiftsPerFilter>()
                    .Title("What do you want to do?")
                    .AddChoices(ViewShiftsPerFilter.ViewShiftsPerDate,
                    ViewShiftsPerFilter.ViewShiftsPerDuration,
                    ViewShiftsPerFilter.ViewShiftsPerEmployeeId,
                    ViewShiftsPerFilter.GoBack
                        ));
                switch (option)
                {
                    case ViewShiftsPerFilter.ViewShiftsPerDate:
                        await shiftService.ViewShiftsPerDate();
                        break;
                    case ViewShiftsPerFilter.ViewShiftsPerEmployeeId:
                        await shiftService.ViewShiftsPerEmployeeId();
                        break;
                    case ViewShiftsPerFilter.ViewShiftsPerDuration:
                        await ShiftsPerDurationMenu();
                        break;
                    case ViewShiftsPerFilter.GoBack:
                        flag = false;
                        break;
                }
            }
        }

        private async Task ShiftsPerDurationMenu()
        {
            bool flag = true;

            while (flag)
            {
                Console.Clear();
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ShiftsPerDurationOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(ShiftsPerDurationOptions.ViewShiftsOnThatDuration,
                    ShiftsPerDurationOptions.ViewShiftsBelowThatDuration,
                    ShiftsPerDurationOptions.ViewShiftsAboveThatDuration,
                    ShiftsPerDurationOptions.GoBack
                        ));
                switch (option)
                {
                    case ShiftsPerDurationOptions.ViewShiftsOnThatDuration:
                        await shiftService.ViewShiftsPerDuration();
                        break;
                    case ShiftsPerDurationOptions.ViewShiftsBelowThatDuration:
                        await shiftService.ViewShiftsBelowDuration();
                        break;
                    case ShiftsPerDurationOptions.ViewShiftsAboveThatDuration:
                        await shiftService.ViewShiftsAboveDuration();
                        break;
                    case ShiftsPerDurationOptions.GoBack:
                        flag = false;
                        break;
                }
            }
        }
    }
}
