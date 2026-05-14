using ShiftsLoggerAPI.Ledana.DTOs;
using ShiftsLoggerAPI.Ledana.Models;

namespace ShiftsLoggerAPI.Ledana.Services
{
    public interface IShiftService
    {
        public Task<ApiResponseDto<Shift>> CreateShift(ShiftDto shift);
        public Task<ApiResponseDto<string?>> DeleteShift(int id);
        public Task<ApiResponseDto<Shift?>> GetShiftById(int id);
        public Task<ApiResponseDto<List<Shift>>> GetShifts(ShiftOptions shiftOptions);
        public Task<ApiResponseDto<Shift?>> UpdatePartialShift(int id, PartialShiftDto shift);
        public Task<ApiResponseDto<Shift?>> UpdateShift(int id, ShiftDto shift);
        public Task<List<Shift>> GetJustShifts();
    }
}
