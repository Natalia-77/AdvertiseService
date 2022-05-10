using AdvertisePublish.Models;
using AdvertisePudlish.Models;
using AutoMapper;
using Domain.Entities;

namespace AdvertisePublish.Mapper
{
    public class AdvertiseProfile : Profile
    {
        public AdvertiseProfile()
        {
            CreateMap<CreateAdvertiseViewModel, Advertise>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(x => x.Price))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(x => x.Images));

            CreateMap<ImageViewModel, Image>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name));

            CreateMap<Advertise, AdvertiseViewModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(x => x.Price))
                .ForMember(dest => dest.ImageList, opt => opt.MapFrom(x => x.Images));          

            CreateMap< Image, ImageViewModel>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(x => "/images/" + x.Name));
        }
    }
}
