using System;
using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.Core.Utility;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace Digikala.Core.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsExistMobileNumber(string mobileNumber)
        {
            return await _unitOfWork.Repository<User>().IsExist(x => x.Mobile == mobileNumber);
        }

        public async Task<bool> IsExistMail(string mail)
        {
            return await _unitOfWork.Repository<User>().IsExist(x => x.Email == mail);
        }

        public async Task AddUser(User user)
        {
            await _unitOfWork.Repository<User>().Add(user);
            await _unitOfWork.Complete();
        }

        public async Task<bool> ActivateUser(string activeCode, string mobile)
        {
            var user = await GetUserByMobile(mobile);

            user.IsActive = true;
            user.ConfirmMobile = true;
            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            await UpdateUser(user);
            return true;
        }

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _unitOfWork.Repository<User>().SingleOrDefaultAsync(x => x.Mobile == mobile);
        }

        public async Task<User> GetUser(string mobile, string password)
        {
            return await _unitOfWork.Repository<User>()
                .SingleOrDefaultAsync(x => x.Mobile == mobile && x.Password == password);
        }

        public async Task UpdateUser(User user)
        {
            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.Complete();
        }

        public async Task<User> ResetActiveCode(User user)
        {
            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            await UpdateUser(user);
            return user;
        }

        public async Task<User> ChangeMobileNumberOfUser(User user, string newMobile)
        {
            user.Mobile = newMobile;
            await UpdateUser(user);
            return user;
        }
        public async Task<int> GetUserRole(int userId)
        {
            return (await GetUserById(userId)).RoleId;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _unitOfWork.Repository<User>().GetById(userId);
        }
    }
}