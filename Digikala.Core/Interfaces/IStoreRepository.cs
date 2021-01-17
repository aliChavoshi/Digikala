using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Digikala.DataAccessLayer.Entities.Store;

namespace Digikala.Core.Interfaces
{
    public interface IStoreRepository
    {
        Task<int> Insert(Store store);
        Task<bool> IsExistUser(int userId);
    }
}