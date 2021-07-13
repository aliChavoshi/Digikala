using System;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.Utility.Generator;
using System.Threading.Tasks;
using Digikala.Core.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace Digikala.Core.Services
{
    public class AccountRepository : GenericRepository<User>, IAccountRepository
    {
        private readonly DigikalaContext _context;
        public AccountRepository(DigikalaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsExistMobileNumber(string mobileNumber)
        {
            return await IsExist(x => x.Mobile == mobileNumber);
        }

        public async Task<bool> IsExistMail(string mail)
        {
            return await IsExist(x => x.Email == mail);
        }

        public async Task ConfirmMobileAndActiveUserUpdateSaveUser(User user)
        {
            user.IsActive = true;
            user.ConfirmMobile = true;
            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            await UpdateSaveUser(user);
        }

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await FirstOrDefaultAsync(x => x.Mobile == mobile);
        }

        public async Task<User> GetUser(string mobile, string password)
        {
            var hashPassword = HashGenerators.Encrypt(password);
            return await FirstOrDefaultAsync(x => x.Mobile == mobile && x.Password == hashPassword);
        }

        public async Task<User> GetUser(string mobile, string email, string password)
        {
            var hashPassword = HashGenerators.Encrypt(password);
            return await FirstOrDefaultAsync(x => x.Mobile == mobile && x.Email.Trim() == email.Trim() &&
                                           x.Password == hashPassword);
        }

        public async Task<User> GetUserByEmail(string email, string password)
        {
            return await FirstOrDefaultAsync(x => x.Email.Trim() == email.Trim() && x.Password == HashGenerators.Encrypt(password));
        }

        public async Task UpdateSaveUser(User user)
        {
            Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> ResetActiveCodeUpdateSaveUser(User user)
        {
            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            await UpdateSaveUser(user);
            return user;
        }

        public async Task<User> ChangeMobileNumberOfUserUpdateSaveUser(User user, string newMobile)
        {
            user.Mobile = newMobile;
            await UpdateSaveUser(user);
            return user;
        }
        public async Task<int> GetUserRole(int userId)
        {
            var user = await FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new Exception("user not fount ");
            }
            return user.RoleId;
        }

        public async Task<User> GetUserIncludeStore(int userId)
        {
            return await Context.User.Include(x=>x.Store)
                .SingleOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<bool> ConfirmEmailWithActiveCodeUpdateUser(string activeCode)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.ActiveCodeEmail == activeCode);
            if (user == null || user.ConfirmEmail)
                return false;
            //به این دلیل مجددا آی دی را عوض کردیم چون اگر کاربری را غیر فعال کردیم نتواند مجددا با ایمیل بازیابی به سایت برگردد
            user.IsActive = true;
            user.ActiveCodeEmail = CodeGenerators.GuidId();
            user.ConfirmEmail = true;
            await UpdateSaveUser(user);

            return true;
        }

        public async Task UpdateSaveUserRoleId(User user, int newRoleId)
        {
            user.RoleId = newRoleId;
            await UpdateSaveUser(user);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}