using System;
using AutoMapper;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.AccountDtos;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Category;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Home;
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

            //get properties Store
            CreateMap<Store, PropertiesStoreDto>()
                .ForMember(x => x.OldLogo,
                    c => c.MapFrom(v => v.Logo))
                .ForMember(x => x.Logo,
                    c => c.Ignore());
            //post properties store 
            CreateMap<PropertiesStoreDto, Store>()
                .ForMember(x => x.Version,
                    c =>
                        c.MapFrom(v => v.Version+1))
                .ForMember(x => x.ModificationDate,
                    c =>
                        c.MapFrom(v => DateTime.Now))
                .ForMember(x => x.Logo,
                    x => x.Ignore());
            #endregion

            #region AdminPanel

            #region Permission

            //Get Sub Create sub Permission
            CreateMap<Permission, CreateSubPermissionDto>()
                .ForMember(x => x.Name,
                    c => c.Ignore())
                .ForMember(x => x.ParentName,
                    c => c.MapFrom(v => v.Name))
                .ForMember(x => x.ParentId,
                    c => c.MapFrom(v => v.Id));
            //postSub Create sub Permission
            CreateMap<CreateSubPermissionDto, Permission>();

            //get edit sub permission
            CreateMap<Permission, EditSubPermissionDto>().ReverseMap();
            #endregion

            #region Category
            //Create Parent post
            CreateMap<CreateCategoryViewModel, Category>();

            //Get Edit Parent
            CreateMap<Category, EditCategoryViewModel>();

            //post Edit Parent
            CreateMap<EditCategoryViewModel, Category>()
                .ForMember(x => x.Version,
                    c =>
                        c.MapFrom(v => v.Version + 1))
                .ForMember(x => x.ModificationDate,
                    c =>
                        c.MapFrom(v => DateTime.Now));

            //Get Sub Create sub category
            CreateMap<Category, CreateSubCategoryDto>()
                .ForMember(x => x.Name,
                    c => c.Ignore())
                .ForMember(x => x.ParentName,
                    c => c.MapFrom(v => v.Name))
                .ForMember(x => x.ParentId,
                    c => c.MapFrom(v => v.Id));

            //Post Sub Create sub category
            CreateMap<CreateSubCategoryDto, Category>();

            //get edit sub category
            CreateMap<Category, EditSubCategoryDto>();

            //post edit sub category
            CreateMap<EditSubCategoryDto, Category>()
                .ForMember(x => x.ModificationDate,
                    c => c.MapFrom(v => DateTime.Now))
                .ForMember(x => x.Version,
                    c => c.MapFrom(v => v.Version + 1));

            #endregion

            #endregion
        }
    }
}