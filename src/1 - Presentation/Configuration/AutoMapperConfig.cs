using AutoMapper;
using MyApiV8.Application.DTOs;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Models;

namespace MyApiV8.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Supplier, SupplierDTO>()
            .ForMember(
                dest => dest.Address,
                src => src.MapFrom(s => s.Address))
            .ReverseMap();

        CreateMap<Address, AddressDTO>()
            .ForMember(
                dest => dest.SupplierId,
                src => src.MapFrom(a => a.Supplier.Id))
            .ReverseMap();

        CreateMap<ProductDTO, Product>();

        CreateMap<Product, ProductDTO>()
            .ForMember(
                dest => dest.SupplierName,
                src => src.MapFrom(p => p.Supplier.Name));

        CreateMap<LoginResponse, LoginResponseDTO>()
            .ForMember(
                dest => dest.UserToken,
                src => src.MapFrom(l => l.UserToken))
            .ReverseMap();

        CreateMap<ClaimModel, ClaimDTO>().ReverseMap();

        CreateMap<UserToken, UserTokenDTO>()
            .ForMember(
                dest => dest.Claims,
                src => src.MapFrom(ut => ut.Claims))
            .ForMember(
                dest => dest.Roles,
                src => src.MapFrom(ut => ut.Roles))
            .ReverseMap();

        CreateMap<ViaCep, ViaCepDTO>().ReverseMap();
    }
}