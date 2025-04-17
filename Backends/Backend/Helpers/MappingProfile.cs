using AutoMapper;
using Backend.Models;
using Backend.DTOs;

public class MappingProfile : Profile
{
          public MappingProfile()
          {
                    CreateMap<User, UserDto>().ReverseMap();
          }
}