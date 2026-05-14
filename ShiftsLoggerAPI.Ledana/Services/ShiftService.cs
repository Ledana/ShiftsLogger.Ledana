using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftsLoggerAPI.Ledana.Data;
using ShiftsLoggerAPI.Ledana.DTOs;
using ShiftsLoggerAPI.Ledana.Models;
using System.Net;

namespace ShiftsLoggerAPI.Ledana.Services
{
    public class ShiftService : IShiftService
    {
        private readonly ShiftContext _dbContext;
        private readonly IMapper _mapper;

        public ShiftService(ShiftContext shiftContext, IMapper mapper)
        {
            _dbContext = shiftContext;
            _mapper = mapper;
        }
        public async Task<ApiResponseDto<Shift>> CreateShift(ShiftDto shiftDto)
        {
            Shift shift = _mapper.Map<Shift>(shiftDto);

            if (!Validator.ValidateEndTime(shift.StartTime, shift.EndTime))
            {
                return new ApiResponseDto<Shift>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "End Time of the shift should be after the Start Time"
                };
            }

            //shift.Duration = shift.EndTime - shift.StartTime;

            var savedShift = await _dbContext.Shifts.AddAsync(shift);
            await _dbContext.SaveChangesAsync();

            return new ApiResponseDto<Shift>
            {
                Data = savedShift.Entity,
                ResponseCode = HttpStatusCode.Created
            };
        }

        public async Task<ApiResponseDto<string?>> DeleteShift(int id)
        {
            var shift = await _dbContext.Shifts.Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shift is null)
                return new ApiResponseDto<string?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Resource with id {id} was not found"
                };

            _dbContext.Remove(shift);

            await _dbContext.SaveChangesAsync();

            return new ApiResponseDto<string?>
            {
                Data = $"Shift with id {id} deleted successfully!",
                ResponseCode = HttpStatusCode.NoContent
            };
        }

        public async Task<ApiResponseDto<Shift?>> GetShiftById(int id)
        {
            var shift = await _dbContext.Shifts.Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shift is null)
            {
                return new ApiResponseDto<Shift?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Resource with id {id} was not found"
                };
            }

            return new ApiResponseDto<Shift?>
            {
                Data = shift,
                ResponseCode = HttpStatusCode.OK
            };
        }

        public async Task<List<Shift>> GetJustShifts()
        {
            return await _dbContext.Shifts.Include(s => s.Employee).ToListAsync();
        }

        public async Task<ApiResponseDto<List<Shift>>> GetShifts(ShiftOptions shiftOptions)
        {
            var query = _dbContext.Shifts.Include(s => s.Employee).AsQueryable();

            var totalShifts = await query.CountAsync();
            List<Shift>? shifts;

            //when user wants to view shifts per date, it will be shifts that started or ended on that date
            if (shiftOptions.Date.HasValue)
            {
                var start = shiftOptions.Date.Value.Date;
                var end = start.AddDays(1);
                query = query
                    .Where(s => 
                    s.EndTime >= shiftOptions.Date && s.EndTime < end
                    || s.StartTime >= shiftOptions.Date && s.StartTime < end);
            }
            if (shiftOptions.Duration is not null)
            {
                query = query.Where(s => s.Duration == shiftOptions.Duration);
            }
            if (shiftOptions.EmployeeId.HasValue)
            {
                query = query.Where(s => s.EmployeeId == shiftOptions.EmployeeId);
            }

            if (shiftOptions.SortBy == "id" || !string.IsNullOrEmpty(shiftOptions.SortBy))
            {
                switch (shiftOptions.SortBy)
                {
                    case "date":
                        query = shiftOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.StartTime)
                            : query.OrderByDescending(s => s.StartTime);
                        break;
                    case "duration":
                        query = shiftOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.Duration)
                            : query.OrderByDescending(s => s.Duration);
                        break;
                    case "employee_id":
                        query = shiftOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.EmployeeId)
                            : query.OrderByDescending(s => s.EmployeeId);
                        break;
                    default:
                        query = shiftOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.Id)
                            : query.OrderByDescending(s => s.Id);
                        break;
                }
            }
                query = query.Skip((shiftOptions.PageNumber - 1) * shiftOptions.PageSize)
                    .Take(shiftOptions.PageSize);
                shifts = await query.ToListAsync();

            bool hasPrevious = shiftOptions.PageNumber > 1;
            bool hasNext = (shiftOptions.PageNumber * shiftOptions.PageSize) < totalShifts;

            return new ApiResponseDto<List<Shift>>
            {
                Data = shifts,
                ResponseCode = HttpStatusCode.OK,
                TotalCount = totalShifts,
                CurrentPage = shiftOptions.PageNumber,
                PageSize = shiftOptions.PageSize,
                HasPrevious = hasPrevious,
                HasNext = hasNext
            };
        }

        public async Task<ApiResponseDto<Shift?>> UpdatePartialShift(int id, PartialShiftDto shift)
        {
            var shiftToUpdate = await _dbContext.Shifts.Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shiftToUpdate is null)
            {
                return new ApiResponseDto<Shift?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Resource with id {id} was not found"
                };
            }

            if (!ValidateEmployeeId(shiftToUpdate.EmployeeId))
            {
                return new ApiResponseDto<Shift?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Employee Id {shiftToUpdate.EmployeeId} is not valid!"
                };
            }

            if (shift.StartTime.HasValue)
                shiftToUpdate.StartTime = shift.StartTime.Value;
            if (shift.EndTime.HasValue)
                shiftToUpdate.EndTime = shift.EndTime.Value;
            if (shift.EmployeeId.HasValue)
                shiftToUpdate.EmployeeId = shift.EmployeeId.Value;

            if (!Validator.ValidateEndTime(shiftToUpdate.StartTime, shiftToUpdate.EndTime))
            {
                return new ApiResponseDto<Shift?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "End Time of the shift should be after the Start Time"
                };
            }

            //shiftToUpdate.Duration = shiftToUpdate.EndTime - shiftToUpdate.StartTime;

            await _dbContext.SaveChangesAsync();

            return new ApiResponseDto<Shift?>
            {
                Data = shiftToUpdate,
                ResponseCode = HttpStatusCode.OK
            };
        }

        public async Task<ApiResponseDto<Shift?>> UpdateShift(int id, ShiftDto shift)
        {
            var shiftToUpdate = await _dbContext.Shifts.Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shiftToUpdate is null)
            {
                return new ApiResponseDto<Shift?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Resource with id {id} was not found"
                };
            }

            if (!ValidateEmployeeId(shiftToUpdate.EmployeeId))
            {
                return new ApiResponseDto<Shift?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Employee Id {shiftToUpdate.EmployeeId} is not valid!"
                };
            }

            shiftToUpdate = _mapper.Map(shift, shiftToUpdate);


            if (!Validator.ValidateEndTime(shiftToUpdate.StartTime, shiftToUpdate.EndTime))
            {
                return new ApiResponseDto<Shift?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = "End Time of the shift should be after the Start Time"
                };
            }

            //shiftToUpdate.Duration = shiftToUpdate.EndTime - shiftToUpdate.StartTime;

            _dbContext.Shifts.Update(shiftToUpdate);
            await _dbContext.SaveChangesAsync();

            return new ApiResponseDto<Shift?>
            {
                Data = shiftToUpdate,
                ResponseCode = HttpStatusCode.OK
            };
        }
        private bool ValidateEmployeeId(int EmployeeId)
        {
            return _dbContext.Employees.Any(w => w.Id == EmployeeId);
        }
    }
}
