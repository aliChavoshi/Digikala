using System.Threading.Tasks;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> IsExistMobileNumber(string mobileNumber);
        Task AddUser(User user);
        Task<bool> ActivateUser(string activeCode,string mobile);
        Task<User> GetUser(string mobile);
        Task<User> GetUser(string mobile, string password);
        Task UpdateUser(User user);
        Task<User> ResetActiveCode(User user);
        Task<User> ChangeMobileNumberOfUser(User user, string newMobile);
    }
}