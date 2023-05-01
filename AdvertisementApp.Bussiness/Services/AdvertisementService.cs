using Advertisement.Common;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.DTOs.AdvertisementDtos;
using AdvertisementApp.DTOs.ProvidedServiceDtos;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.Business.Services
{
    public class AdvertisementService : Service<AdvertisementCreateDto, AdvertisementUpdateDto, AdvertisementListDto, Entities.Advertisement>, IAdvertisementService
    {
        private readonly IUow uow;
        private readonly IMapper mapper;
        public AdvertisementService(IMapper mapper, IValidator<AdvertisementCreateDto> createDtoValidator, IValidator<AdvertisementUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IResponse<List<AdvertisementListDto>>> GetActivesAsync()
        {
            var data = await uow.GetRepository<Entities.Advertisement>().GetAllAsync(x => x.Status, x => x.CreatedDate, Advertisement.Common.Enums.OrderByType.DESC);
            var dto = mapper.Map<List<AdvertisementListDto>>(data);
            return new Response<List<AdvertisementListDto>>(ResponseType.Success, dto);
        }
    }
}
