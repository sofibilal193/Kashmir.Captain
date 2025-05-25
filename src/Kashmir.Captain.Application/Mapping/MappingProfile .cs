
using AutoMapper;
using Kashmir.Captain.Application.Extensions;
using Kashmir.Captain.Application.Identity;
using Kashmir.Captain.Domain.Entities;

namespace KC.Common
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, UserDto>();
			//  .ForMember(dest => dest.DestinationProperty, opt => opt.MapFrom(src => src.SourceProperty));
			CreateMap<PagedList<User>, PagedList<UserDto>>();
			CreateMap<Tool, ToolDto>();
		}
	}

}
