using System.Threading.Tasks;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> IsExistMobileNumber(string mobileNumber);
        Task<bool> IsExistMail(string mail);
        Task AddUser(User user);
        Task ConfirmMobileAndActiveUser(User user);
        Task<User> GetUserByMobile(string mobile);
        Task<User> GetUser(string mobile, string password);
        Task<User> GetUserByEmail(string email, string password);
        Task<User> GetUserById(int userId);
        Task UpdateUser(User user);
        Task<User> ResetActiveCode(User user);
        Task<User> ChangeMobileNumberOfUser(User user, string newMobile);
        Task<int> GetUserRole(int userId);
    }
}