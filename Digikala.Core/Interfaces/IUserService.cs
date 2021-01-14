using System.Threading.Tasks;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Interfaces
{
    public interface IUserService 
    {
        Task<int> GetUserRole(int userId);
        Task<User> GetUserById(int userId);
    }
}