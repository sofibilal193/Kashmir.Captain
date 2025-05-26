
using AutoMapper;
using Kashmir.Captain.Common.Extensions;
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
