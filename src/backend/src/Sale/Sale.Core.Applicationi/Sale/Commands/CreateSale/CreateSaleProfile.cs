﻿using AutoMapper;
using Shared.Dto.Command.Sale;

namespace Sale.Core.Application.Sales.Create
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
        
            CreateMap<CreateSaleCommand, SaleCoreDomainEntities.Sale>()
                   .ForMember(dto => dto.SaleItens, conf => conf.MapFrom(ol => ol.SaleItens));                    

            CreateMap<CreateSaleItemDto, SaleCoreDomainEntities.SaleItens>();

            CreateMap<SaleCoreDomainEntities.Sale, CreateSaleResult>();
        }
    }
}
