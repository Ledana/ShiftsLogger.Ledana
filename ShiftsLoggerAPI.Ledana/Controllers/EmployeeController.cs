using Microsoft.AspNetCore.Mvc;
using ShiftsLoggerAPI.Ledana.Models;
using ShiftsLoggerAPI.Ledana.Services;

namespace ShiftsLoggerAPI.Ledana.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmplyeeService _workerService;
        public EmployeeController(IEmplyeeService workerService)
        {
            _workerService = workerService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllWorkers()
        {
            var workers = await _workerService.GetAllWorkers();

            return Ok(workers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetWorkerById(int id)
        {
            Employee? worker = await _workerService.GetWorkerById(id);

            if (worker is null) return NotFound();

            return Ok(worker);
        }
    }
}
