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

            CreateMap<TeamMember,TeamMemberDto>().ReverseMap();
            CreateMap<Service,ServiceDto>().ReverseMap();
            
            CreateMap<Complaint,ComplaintDto>().ReverseMap();
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Message,MessageDto>().ReverseMap();
            CreateMap<Project,ProjectDto>().ReverseMap();
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Service_ID,
                        opt => opt.MapFrom(src => src.ProjectServices.FirstOrDefault().Service_ID));

        }
    }
}
