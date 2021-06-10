using Digikala.DataAccessLayer.Entities.Store;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;

namespace Digikala.Core.Interfaces
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<bool> IsExistUser(int userId);
        Task<bool> IsActiveStore(int userId);
    }
}