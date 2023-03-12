using AutoMapper;
using Core.Application.Dto.auth;
using Core.Domain.Entities;

namespace Core.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ItemsCount, opt => opt.MapFrom(src => src.Items.Any() ? src.Items.Count : 0))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap()
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.CreatedOn, opt => opt.Ignore())
                .ForMember(d => d.ModifiedOn, opt => opt.Ignore())
                .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
                .ForMember(d => d.DeletedOn, opt => opt.Ignore())
                .ForMember(d => d.DeletedBy, opt => opt.Ignore());

            CreateMap<Item, ItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.CreatedOn, opt => opt.Ignore())
                .ForMember(d => d.ModifiedOn, opt => opt.Ignore())
                .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
                .ForMember(d => d.DeletedOn, opt => opt.Ignore())
                .ForMember(d => d.DeletedBy, opt => opt.Ignore());

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}