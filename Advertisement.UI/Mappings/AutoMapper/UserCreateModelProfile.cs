using Advertisement.UI.Models;
using AdvertisementApp.DTOs.AppUserDtos;
using AutoMapper;

namespace Advertisement.UI.Mappings.AutoMapper
{
    public class UserCreateModelProfile : Profile
    {
        public UserCreateModelProfile()
        {
            CreateMap<UserCreateModel,AppUserCreateDto>();
        }
    }
}
