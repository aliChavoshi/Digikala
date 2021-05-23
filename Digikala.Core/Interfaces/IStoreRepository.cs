using Digikala.DataAccessLayer.Entities.Store;
using System.Threading.Tasks;

namespace Digikala.Core.Interfaces
{
    public interface IStoreRepository
    {
        Task<int> Insert(Store store);
        Task<bool> IsExistUser(int userId);
        Task<bool> IsActiveStore(int userId);
    }
}