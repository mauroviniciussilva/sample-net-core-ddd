using AutoMapper;
using Sample.Domain.Entities;

namespace Sample.Api.ViewModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserEditViewModel>();
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserListViewModel>();

            CreateMap<SampleEntity, SampleEditViewModel>();
            CreateMap<SampleEditViewModel, SampleEntity>();
            CreateMap<SampleEntity, SampleListViewModel>();
        }
    }
}