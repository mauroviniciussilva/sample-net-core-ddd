using AutoMapper;
using Sample.Domain.Entities;

namespace Sample.Application.ViewModel
{
    public class MappingProfile : Profile
    {
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