using Auth.Domain.Entities;
using AutoMapper;

namespace Auth.Application.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile() => CreateMap<User, Responses.UserResponse>().ReverseMap();
}
