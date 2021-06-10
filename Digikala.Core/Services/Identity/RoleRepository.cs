using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Services.Identity
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(DigikalaContext context) : base(context)
        {
        }
    }
}