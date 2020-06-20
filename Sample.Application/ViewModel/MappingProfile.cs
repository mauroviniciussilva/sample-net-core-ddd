using AutoMapper;
using Sample.Domain.Entities;

namespace Sample.Application.ViewModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserEditViewModel>();
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserListViewModel>();
        }
    }
}