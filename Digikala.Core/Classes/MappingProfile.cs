using AutoMapper;
using Digikala.Core.Utility;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.AccountDtos;

namespace Digikala.Core.Classes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Register

            CreateMap<RegisterDto, User>()
                .ForMember(x => x.ActiveCode,
                    c => c.MapFrom(v => CodeGenerators.ActiveCodeFiveNumbers()))
                .ForMember(x => x.Password,
                    c => c.MapFrom(v => HashGenerators.Encrypt(v.Password)))
                .ForMember(x => x.RoleId,
                    c => c.MapFrom(v => 1));

            #endregion

        }
    }
}