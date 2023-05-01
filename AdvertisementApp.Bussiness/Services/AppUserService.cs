using Advertisement.Common;
using AdvertisementApp.Business.Extensions;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.DTOs.AdvertisementDtos;
using AdvertisementApp.DTOs.AppRoleDtos;
using AdvertisementApp.DTOs.AppUserDtos;
using AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.Business.Services
{
    public class AppUserService : Service<AppUserCreateDto, AppUserUpdateDto, AppUserListDto, AppUser>, IAppUserService
    {
        private readonly IUow uow;
        private readonly IMapper mapper;
        private readonly IValidator<AppUserCreateDto> createValidator;
        private readonly IValidator<AppUserLoginDto> loginValidator;
        public AppUserService(IMapper mapper, IValidator<AppUserCreateDto> createDtoValidator, IValidator<AppUserUpdateDto> updateDtoValidator, IUow uow, IValidator<AppUserLoginDto> loginValidator) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.createValidator = createDtoValidator;
            this.loginValidator = loginValidator;
        }

        public async Task<IResponse<AppUserCreateDto>> CreateWithRoleAsync(AppUserCreateDto dto, int roleId)
        {
            var validationResult = createValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = mapper.Map<AppUser>(dto);
                await uow.GetRepository<AppUser>().CreateAsync(user);
                await uow.GetRepository<AppUserRole>().CreateAsync(new AppUserRole
                {
                    AppUser = user,
                    AppRoleId = roleId
                });
                await uow.SaveChangesAsync();

                return new Response<AppUserCreateDto>(ResponseType.Success, dto);
            }
            return new Response<AppUserCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }

        public async Task<IResponse<AppUserListDto>> CheckUserAsync(AppUserLoginDto dto)
        {
            var validationResult = loginValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = await uow.GetRepository<AppUser>().GetByFilterAsync(x => x.Username.Equals(dto.Username) && x.Password.Equals(dto.Password));
                if (user is not null)
                {
                    var appUserDto = mapper.Map<AppUserListDto>(user);
                    return new Response<AppUserListDto>(ResponseType.Success, appUserDto);
                }
                return new Response<AppUserListDto>(ResponseType.NotFound, "Kullanıcı adı veya şifre hatalı");
            }
            return new Response<AppUserListDto>(ResponseType.ValidationError, "Kullanıcı adı veya şifre boş olamaz");
        }

        public async Task<IResponse<List<AppRoleListDto>>> GetRolesByUserIdAsync(int userId)
        {
            var roles = await uow.GetRepository<AppRole>().GetAllAsync(x => x.AppUserRoles.Any(x => x.AppUserId == userId));
            if (roles is null)
                return new Response<List<AppRoleListDto>>(ResponseType.NotFound, "İlgili rol bulunamadı");

            var dto = mapper.Map<List<AppRoleListDto>>(roles);
            return new Response<List<AppRoleListDto>>(ResponseType.Success, dto);

        }
    }
}
