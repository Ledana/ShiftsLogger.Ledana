using Microsoft.EntityFrameworkCore;
using ShiftsLoggerAPI.Ledana.Data;
using ShiftsLoggerAPI.Ledana.Models;

namespace ShiftsLoggerAPI.Ledana.Services
{
    public class EmployeeService : IEmplyeeService
    {
        private readonly ShiftContext _dbContext;

        public EmployeeService(ShiftContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllWorkers()
        {
            var workers = await _dbContext.Employees.ToListAsync();
            return workers;
        }

        public async Task<Employee?> GetWorkerById(int id)
        {
            var worker = await _dbContext.Employees.FindAsync(id);
            return worker;
        }
    }
}
