using AutoMapper;
using DatingApp.API.DTO.Auth;
using DatingApp.API.DTO.Message;
using DatingApp.API.DTO.Photo;
using DatingApp.API.DTO.Users;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDetailDto>()
                .ForMember(x => x.photo_url, y => y.MapFrom(z => z.Photos.FirstOrDefault(p => p.is_main).url))
                .ForMember(x => x.age, y => y.MapFrom(z => z.date_of_birth.CalculateAge()));
            CreateMap<User, UserListDto>().ForMember(x => x.photo_url, y => y.MapFrom(z => z.Photos.FirstOrDefault(p => p.is_main).url))
                .ForMember(x => x.age, y => y.MapFrom(z => z.date_of_birth.CalculateAge()));
            CreateMap<Photo, PhotoDetailDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<Photo, PhotoCreateDto>();
            CreateMap<PhotoCreateDto, Photo>();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<MessageCreateDTO, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDTO>()
                .ForMember(m => m.sender_photo_url, p => p
                    .MapFrom(s => s.sender.Photos.FirstOrDefault(x => x.is_main).url))
                .ForMember(m => m.recipient_photo_url, p => p
                    .MapFrom(s => s.recipient.Photos.FirstOrDefault(x => x.is_main).url))
                .ForMember(m => m.sender_known_as, p => p.MapFrom(x => x.sender.known_as.ToString()))
                .ForMember(m => m.recipient_known_as, p => p.MapFrom(x => x.recipient.known_as.ToString()));
        }
    }
}
