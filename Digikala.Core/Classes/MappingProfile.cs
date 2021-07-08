using System;
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
            //Get Parent
            CreateMap<Category, EditCategoryViewModel>()
                .ForMember(x => x.Icon,
                    c => c.Ignore())
                .ForMember(x => x.OldIconPath,
                    c => c.MapFrom(v => v.Icon));
            //post Parent
            CreateMap<EditCategoryViewModel, Category>()
                .ForMember(x => x.Version,
                    c => c.MapFrom(v => v.Version + 1))
                .ForMember(x => x.Icon, c => c.Ignore())
                .ForMember(x => x.ModificationDate,
                    c => c.MapFrom(v => DateTime.Now));
            //Get Sub
            CreateMap<Category, CreateSubCategoryDto>()
                .ForMember(x=>x.Name,c=>c.Ignore())
                .ForMember(x=>x.ParentName,
                    c=>c.MapFrom(v=>v.Name))
                .ForMember(x => x.ParentId,
                    c => c.MapFrom(v => v.Id));
            //post sub
            CreateMap<CreateSubCategoryDto, Category>()
                .ForMember(x => x.Id,
                    c => c.Ignore());

            #endregion

            #endregion
        }
    }
}