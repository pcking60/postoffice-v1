using AutoMapper;
using PostOffice.Model.Models;
using PostOffice.Web.Models;

namespace PostOffice.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
            Mapper.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<District, DistrictViewModel>();
            Mapper.CreateMap<PO, POViewModel>();
            Mapper.CreateMap<ServiceGroup, ServiceGroupViewModel>();
            Mapper.CreateMap<PostOffice.Model.Models.Service, ServiceViewModel>();
            Mapper.CreateMap<MainServiceGroup, MainServiceGroupViewModel>();
        }
    }
}