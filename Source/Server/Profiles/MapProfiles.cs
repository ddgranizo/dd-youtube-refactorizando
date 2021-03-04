using AutoMapper;
using Refactorizando.Shared.Data.Models;
using Refactorizando.Shared.Data.Models.Dtos;

namespace Refactorizando.Server.Profiles
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            CreateMap<Request, RequestDto>();
            CreateMap<RequestDto, Request>();

            CreateMap<SystemUser, SystemUserDto>();
            CreateMap<SystemUserDto, SystemUser>();

            CreateMap<LikeRequest, LikeRequestDto>();
            CreateMap<LikeRequestDto, LikeRequest>();
        }
    }
}