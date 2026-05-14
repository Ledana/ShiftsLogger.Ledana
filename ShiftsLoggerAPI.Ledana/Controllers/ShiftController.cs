using Microsoft.AspNetCore.Mvc;
using ShiftsLoggerAPI.Ledana.DTOs;
using ShiftsLoggerAPI.Ledana.Models;
using ShiftsLoggerAPI.Ledana.Services;

namespace ShiftsLoggerAPI.Ledana.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<Shift>>>> GetAllShifts(ShiftOptions shiftOptions)
        {
            var shifts = await _shiftService.GetShifts(shiftOptions);
            return Ok(shifts);
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<Shift>>> GetJustShifts()
        {
            var shifts = await _shiftService.GetJustShifts();

            if (shifts is null) return BadRequest();

            return Ok(shifts);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<Shift>>> GetShift(int id)
        {
            var shift = await _shiftService.GetShiftById(id);
            if (shift is null) return NotFound();

            return Ok(shift);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<Shift>>> Post(ShiftDto shift)
        {
            
            if (!ModelState.IsValid)
                return BadRequest();

            var newShift = await _shiftService.CreateShift(shift);

            if (newShift.Data is null) return BadRequest(newShift);

            return new ObjectResult(newShift) { StatusCode = 201 };
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<Shift>>> Delete(int id)
        {
            var result = await _shiftService.DeleteShift(id);

            if (result is null) return NotFound(result);
            if (result.RequestFailed == true) return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<Shift>>> UpdateShift(int id, [FromBody] ShiftDto shift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newShift = await _shiftService.UpdateShift(id, shift);

            if (newShift.Data is null) return NotFound(newShift);

            return Ok(newShift);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponseDto<Shift>>> UpdatePartialShift(int id, [FromBody] PartialShiftDto shift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newShift = await _shiftService.UpdatePartialShift(id, shift);

            if (newShift.RequestFailed == true) return NotFound(newShift);

            return Ok(newShift);
        }

    }
}
