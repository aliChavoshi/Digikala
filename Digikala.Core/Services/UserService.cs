using System;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Digikala.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetUserRole(int userId)
        {
            return (await GetUserById(userId)).RoleId;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _unitOfWork.Repository<User>().GetById(userId);
        }
    }
}