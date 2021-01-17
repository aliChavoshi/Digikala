using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Store;

namespace Digikala.Core.Services
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Insert(Store store)
        {
            await _unitOfWork.Repository<Store>().Add(store);
            await _unitOfWork.Complete();
            return store.UserId;
        }

        public async Task<bool> IsExistUser(int userId)
        {
            return await _unitOfWork.Repository<Store>().IsExist(x => x.UserId == userId);
        }
    }
}