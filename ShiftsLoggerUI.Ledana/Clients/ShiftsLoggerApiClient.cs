using ShiftsLoggerAPI.Ledana.DTOs;
using ShiftsLoggerAPI.Ledana.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

internal class ShiftsLoggerApiClient
{
    internal async Task<bool> IsEmployeeIdCorrect(int id)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var employees = await client.GetFromJsonAsync<List<Employee>>("https://localhost:7264/employee");

            if (employees is null) return false;

            return employees.Any(w => w.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting employees didn't work " + e.Message);
            return false;
        }
    }

    internal async Task<string?> CreateShift(ShiftDto shift)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync($"https://localhost:7264/shift/", shift);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto<Shift>>();

            if (response.IsSuccessStatusCode)
                return "Shift created successfully!";
            else
                return result?.ErrorMessage;
        }
        catch (Exception e)
        {
            return "Creating shift went wrong " + e.Message;
        }
    }

    internal async Task<ApiResponseDto<List<Shift>>?> GetAllShifts(int pageNumber, int pageSize)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetFromJsonAsync<ApiResponseDto<List<Shift>>>($"https://localhost:7264/shift?page_size={pageSize}&page_number={pageNumber}");

        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }

    //when the user wants to view a shift, they can choose from all the shifts available
    internal async Task<bool> IsShiftIdCorrectFromAllShifts(int id)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response =  await client.GetFromJsonAsync<List<Shift>>("https://localhost:7264/shift/all");
            if (response is null) return false;

            return response.Any(s => s.Id == id);

        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return false;
        }
    }

    //getting all shifts without pagination
    internal async Task<List<Shift>?> GetAllShifts()
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetFromJsonAsync<List<Shift>>("https://localhost:7264/shift/all");

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }

    //check if shift is correct against shifts tht ended today
    internal async Task<bool> IsShiftIdCorrectForToday(int id)
    {

            var shifts = await GetShiftsPerDate(DateTime.Today);

            if (shifts is null) return false;

            return shifts.Any(w => w.Id == id);       
    }


    internal async Task<string?> DeleteShift(int id)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.DeleteAsync($"https://localhost:7264/shift/{id}");
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto<string>>();

            if (response.IsSuccessStatusCode)
                return $"Shift with id {id} is deleted!";
            return result?.ErrorMessage;

        }
        catch (Exception e)
        {
            return "Deleting shift went wrong " + e.Message;
        }
    }

    internal async Task<string?> UpdateWholeShift(int id, ShiftDto shift)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PutAsJsonAsync($"https://localhost:7264/shift/{id}", shift);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto<Shift>>();

            if (response.IsSuccessStatusCode)
                return "Shift updated successfully!";
            else
                return result?.ErrorMessage;

        }
        catch (Exception e)
        {
            return "Updating shift went wrong " + e.Message;
        }
    }

    internal async Task<string?> UpdatePartialShift(int id, PartialShiftDto shift)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PatchAsJsonAsync($"https://localhost:7264/shift/{id}", shift);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto<Shift>>();

            if (response.IsSuccessStatusCode)
                return "Shift updated successfully!";
            else
                return result?.ErrorMessage;

        }
        catch (Exception e)
        {
            return "Updating shift went wrong " + e.Message;
        }
    }

    internal async Task<Shift?> GetShiftById(int id)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetFromJsonAsync<ApiResponseDto<Shift>>($"https://localhost:7264/shift/{id}");

            return response?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine("Updating shift went wrong " + e.Message);
            return null;
        }
    }

    //this method gets the shifts that ended on the date provided into it
    internal async Task<List<Shift>?> GetShiftsPerDate(DateTime date)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //convert date in the format the get is expecting
            var formattedDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var response = await client.GetFromJsonAsync<ApiResponseDto<List<Shift>>>($"https://localhost:7264/shift?date={formattedDate}&sort_by=date");

            return response?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }

    internal async Task<List<Shift>?> GetShiftsPerEmployeeId(int id)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetFromJsonAsync<ApiResponseDto<List<Shift>>>($"https://localhost:7264/shift?employee_id={id}");

            return response?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }

    internal async Task<List<Shift>?> GetShiftsPerDuration(string duration)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetFromJsonAsync<ApiResponseDto<List<Shift>>>($"https://localhost:7264/shift?duration={duration}");

            return response?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }

    internal async Task<ApiResponseDto<List<Shift>>?> GetShiftsSortedByDate(int pageNumber, int pageSize)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetFromJsonAsync<ApiResponseDto<List<Shift>>>($"https://localhost:7264/shift?sort_by=date&page_size={pageSize}&page_number={pageNumber}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }

    internal async Task<ApiResponseDto<List<Shift>>?> GetShiftsSortedByDuration(int pageNumber, int pageSize)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetFromJsonAsync<ApiResponseDto<List<Shift>>>($"https://localhost:7264/shift?sort_by=duration&page_size={pageSize}&page_number={pageNumber}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }

    internal async Task<ApiResponseDto<List<Shift>>?> GetShiftsSortedByEmployeeId(int pageNumber, int pageSize)
    {
        try
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetFromJsonAsync<ApiResponseDto<List<Shift>>>($"https://localhost:7264/shift?sort_by=employee_id&page_size={pageSize}&page_number={pageNumber}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Getting shifts went wrong " + e.Message);
            return null;
        }
    }
}