using API.DTOs;
using API.Entities;
using API.Enums;
using API.Extensions;
using AutoMapper;

namespace API.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(d=> d.PhotoUrl, o=> o.MapFrom(s=> s.Photos.FirstOrDefault(x=>x.IsMain)!.Url));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberEditDTO, AppUser>();
            CreateMap<string, DateOnly>().ConstructUsing(s=> DateOnly.Parse(s));
            CreateMap<RegisterDTO, AppUser>()
                .ForMember(g=>g.Gender, o=> o.MapFrom(src=> src.Gender.FromString<Gender>() ?? 0));
        }
    }
}
