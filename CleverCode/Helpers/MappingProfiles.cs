using AutoMapper;
using CleverCode.DTO;
using CleverCode.Models;

namespace CleverCode.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CompanyInformation, CompanyInformationDto>();
            CreateMap<CompanyInformationDto, CompanyInformation>()
                .ForMember(dest => dest.Company_ID, opt => opt.Ignore());

            CreateMap<ContactInfo, ContactInfoDto>().ReverseMap();
            CreateMap<CompanyValues, CompanyValuesDto>().ReverseMap();
        }
    }
}
