using Advertisement.Common;
using AdvertisementApp.Business.Extensions;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.DTOs.Interfaces;
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
    public class Service<CreateDto, UpdateDto, ListDto, T> : IService<CreateDto, UpdateDto, ListDto, T>
        where CreateDto : class, IDto, new()
        where UpdateDto : class, IUpdateDto, new()
        where ListDto : class, IDto, new()
        where T : BaseEntity, new()
    {
        private readonly IMapper mapper;
        private readonly IValidator<CreateDto> createDtoValidator;
        private readonly IValidator<UpdateDto> updateDtoValidator;
        private readonly IUow uow;

        public Service(IMapper mapper, IValidator<CreateDto> createDtoValidator, IValidator<UpdateDto> updateDtoValidator, IUow uow)
        {
            this.mapper = mapper;
            this.createDtoValidator = createDtoValidator;
            this.updateDtoValidator = updateDtoValidator;
            this.uow = uow;
        }

        public async Task<IResponse<CreateDto>> CreateAsync(CreateDto dto)
        {
            var result = createDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                var createdEntity = mapper.Map<T>(dto);
                await uow.GetRepository<T>().CreateAsync(createdEntity);
                return new Response<CreateDto>(ResponseType.Success, dto);
            }
            return new Response<CreateDto>(dto, result.ConvertToCustomValidationError());
        }

        public async Task<IResponse<List<ListDto>>> GetAllAsync()
        {
            var data = await uow.GetRepository<T>().GetAllAsync();
            var dto = mapper.Map<List<ListDto>>(data);
            return new Response<List<ListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<IDto>> GetByIdAsync<IDto>(int id)
        {
            var data = await uow.GetRepository<T>().GetByFilterAsync(x => x.Id.Equals(id));

            if (data is null)
                return new Response<IDto>(ResponseType.NotFound, $"{id}ye sahip data bulunamadı");

            var dto = mapper.Map<IDto>(data);
            return new Response<IDto>(ResponseType.Success, dto);
        }

        public async Task<IResponse> RemoveAsync(int id)
        {
            var data = await uow.GetRepository<T>().FindAsync(id);

            if (data is null)
                return new Response<IDto>(ResponseType.NotFound, $"{id} idsine sahip data bulunamadı");

            uow.GetRepository<T>().Remove(data);
            return new Response(ResponseType.Success);
        }

        public async Task<IResponse<UpdateDto>> UpdateAsync(UpdateDto dto)
        {
            var result = updateDtoValidator.Validate(dto);
            
            if (result.IsValid)
            {
                var unchangedData = await uow.GetRepository<T>().FindAsync(dto.Id);
                
                if (unchangedData is null)
                    return new Response<UpdateDto>(ResponseType.NotFound, $"{dto.Id} idsine sahip data bulunamadı");
                
                var entity = mapper.Map<T>(dto);
                uow.GetRepository<T>().Update(entity, unchangedData);
                return new Response<UpdateDto>(ResponseType.Success, dto);
            }
            return new Response<UpdateDto>(dto, result.ConvertToCustomValidationError());
        }
    }
}
