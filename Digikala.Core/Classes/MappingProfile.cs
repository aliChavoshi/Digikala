using AutoMapper;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.AccountDtos;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Category;
using Digikala.DTOs.Store;
using Digikala.Utility.Generator;

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

            #region Store

            CreateMap<StoreRegisterDto, Store>()
                .ForMember(x => x.IsActive,
                    c => c.MapFrom(v => false));
            //post create Store
            CreateMap<StoreRegisterDto, User>()
                .ForMember(x => x.ActiveCodeEmail,
                    c =>
                        c.MapFrom(v => CodeGenerators.GuidId()))
                .ForMember(x => x.Password,
                    c =>
                        c.MapFrom(v => HashGenerators.Encrypt(v.Password)))
                .ForMember(x => x.ActiveCode,
                    c =>
                        c.MapFrom(v => CodeGenerators.ActiveCodeFiveNumbers()))
                .ForMember(x => x.RoleId,
                    c =>
                        c.MapFrom(v => 2));

            #endregion

            #region AdminPanel

            #region Category

            CreateMap<CreateCategoryViewModel, Category>()
                .ForMember(x => x.Icon,
                    c => c.Ignore());
            CreateMap<Category, EditCategoryViewModel>()
                .ForMember(x=>x.Icon,
                    c=>c.Ignore())
                .ForMember(x => x.OldIconPath,
                    c => c.MapFrom(v => v.Icon));

            #endregion

            #endregion
        }
    }
}