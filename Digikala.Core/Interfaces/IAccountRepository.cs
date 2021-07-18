using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Interfaces
{
    public interface IAccountRepository : IGenericRepository<User>
    {
        Task<bool> IsExistMobileNumber(string mobileNumber);
        Task<bool> IsExistMail(string mail);
        Task<User> GetUserByMobile(string mobile);
        Task<User> GetUser(string mobile, string password);
        Task<User> GetUser(string mobile, string email, string password);
        Task<int> GetUserRole(int userId);
        Task<User> GetUserIncludeStore(int userId);
        Task<User> IsExistUserByMobileEmailPass(string mobile, string email, string password);
        //with update & save
        Task UpdateSaveUserRoleId(User user, int newRoleId);
        Task UpdateSaveUser(User user);
        Task<User> ChangeMobileNumberOfUserUpdateSaveUser(User user, string newMobile);
        Task ConfirmMobileAndActiveUserUpdateSaveUser(User user);
        Task<User> ResetActiveCodeUpdateSaveUser(User user);
        Task<bool> ConfirmEmailWithActiveCodeUpdateUser(string activeCode);
    }
}