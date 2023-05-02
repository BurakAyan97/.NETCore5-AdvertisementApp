using Advertisement.Common;
using Advertisement.Common.Enums;
using AdvertisementApp.Business.Extensions;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.DTOs.AdvertisementAppUserCreateDtos;
using AdvertisementApp.DTOs.AdvertisementAppUserDtos;
using AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.Business.Services
{
    public class AdvertisementAppUserService : IAdvertisementAppUserService
    {
        private readonly IUow uow;
        private readonly IValidator<AdvertisementAppUserCreateDto> createDtoValidator;
        private readonly IMapper mapper;

        public AdvertisementAppUserService(IUow uow, IValidator<AdvertisementAppUserCreateDto> createDtoValidator, IMapper mapper)
        {
            this.uow = uow;
            this.createDtoValidator = createDtoValidator;
            this.mapper = mapper;
        }

        public async Task<IResponse<AdvertisementAppUserCreateDto>> CreateAsync(AdvertisementAppUserCreateDto dto)
        {
            var result = createDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                var control = await uow.GetRepository<AdvertisementAppUser>().GetByFilterAsync(x => x.AppUserId == dto.AppUserId && x.AdvertisementId == dto.AdvertisementId);
                if (control is null)
                {
                    var createdAdvertisementAppUser = mapper.Map<AdvertisementAppUser>(dto);
                    await uow.GetRepository<AdvertisementAppUser>().CreateAsync(createdAdvertisementAppUser);
                    await uow.SaveChangesAsync();
                    return new Response<AdvertisementAppUserCreateDto>(ResponseType.Success, dto);
                }
                List<CustomValidationError> errors = new List<CustomValidationError> { new CustomValidationError { ErrorMessage = "Daha önce başvurulan ilana başvurulamaz.", PropertyName = "" } };
                return new Response<AdvertisementAppUserCreateDto>(dto, errors);
            }
            return new Response<AdvertisementAppUserCreateDto>(dto, result.ConvertToCustomValidationError());
        }

        public async Task<List<AdvertisementAppUserListDto>> GetList(AdvertisementAppUserStatusType type)
        {
            var query = uow.GetRepository<AdvertisementAppUser>().GetQuery();

            var list = await query
                                 .Include(x => x.Advertisement)
                                 .Include(x => x.AdvertisementAppUserStatus)
                                 .Include(x => x.MilitaryStatus)
                                 .Include(x => x.AppUser).ThenInclude(x => x.Gender)
                                 .Where(x => x.AdvertisementAppUserStatusId == (int)type)
                                 .ToListAsync();
            return mapper.Map<List<AdvertisementAppUserListDto>>(list);
        }

        public async Task SetStatusAsync(int advertisementAppUserId, AdvertisementAppUserStatusType type)
        {
            var query = uow.GetRepository<AdvertisementAppUser>().GetQuery();

            var entity = await query.SingleOrDefaultAsync(x => x.Id == advertisementAppUserId);
            entity.AdvertisementAppUserStatusId = (int)type;
            await uow.SaveChangesAsync();
        }
    }
}
