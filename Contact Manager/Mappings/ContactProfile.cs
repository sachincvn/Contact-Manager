using AutoMapper;
using Contact_Manager.Models;
using Contact_Manager.Realm;
using Contact_Manager.ApiObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_Manager.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile() : base()
        {
            CreateMap<ContactModel, ContactObject>()
                .ReverseMap();
        }
    }

    public class ContactObjectProfile : Profile {
        public ContactObjectProfile()
        {
            CreateMap<Contact, ContactObject>()
               .ForMember(dest => dest.Id,
               opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.Name,
               opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dest => dest.SurName,
               opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dest => dest.Number,
               opt => opt.MapFrom(src => src.Phones.FirstOrDefault().PhoneNumber??string.Empty))
               .ForMember(dest => dest.FullName,
               opt => opt.MapFrom(src => src.DisplayName));
        }
    }

    public class ContactDtoProfile : Profile
    {
        public ContactDtoProfile()
        {
            CreateMap<ContactData, ContactModel>();
        }
    }
    public class ContactDtoObjectProfile : Profile
    {
        public ContactDtoObjectProfile()
        {
            CreateMap<ContactData, ContactObject>();
        }
    }

}
