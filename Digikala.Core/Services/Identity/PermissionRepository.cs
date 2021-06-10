using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Services.Identity
{
    public class PermissionRepository:GenericRepository<Permission>,IPermissionRepository
    {
        public PermissionRepository(DigikalaContext context) : base(context)
        {
        }
    }
}