using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Store;

namespace Digikala.Core.Services
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        private readonly DigikalaContext _context;

        public StoreRepository(DigikalaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsExistUser(int userId)
        {
            return await IsExist(x => x.UserId == userId);
        }

        public async Task<bool> IsActiveStore(int userId)
        {
            return await IsExist(x => x.UserId == userId && x.IsActive);
        }
    }
}