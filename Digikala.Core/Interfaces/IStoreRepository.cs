using Digikala.DataAccessLayer.Entities.Store;
using System.Threading.Tasks;

namespace Digikala.Core.Interfaces
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<bool> IsExistUser(int userId);
        Task<bool> IsActiveStore(int userId);
    }
}