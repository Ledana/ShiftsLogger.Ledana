using ShiftsLoggerAPI.Ledana.Models;

namespace ShiftsLoggerAPI.Ledana.Services
{
    public interface IEmplyeeService
    {
        public Task<List<Employee>> GetAllWorkers();
        public Task<Employee?> GetWorkerById(int id);
    }
}
