using AutoMapper;
using Sample.Domain.Entities;

namespace Sample.Application.ViewModel
{
    /// <summary>
    /// Gather mapping configuration before initialization
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor to declare the maps between entities and view models
        /// </summary>
        public MappingProfile()
        {
            CreateMap<UserEditViewModel, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Prevents id change
                .ForMember(dest => dest.Active, opt => opt.Ignore()) // Prevents unwanted entity status change
                .ReverseMap();
            CreateMap<User, UserListViewModel>();
        }
    }
}