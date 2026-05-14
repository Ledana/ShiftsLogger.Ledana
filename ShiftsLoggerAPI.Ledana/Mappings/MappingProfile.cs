using AutoMapper;
using ShiftsLoggerAPI.Ledana.DTOs;
using ShiftsLoggerAPI.Ledana.Models;

namespace ShiftsLoggerAPI.Ledana.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShiftDto, Shift>();
        }
    }
}
