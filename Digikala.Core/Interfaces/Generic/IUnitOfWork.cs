using System;
using System.Threading.Tasks;

namespace Digikala.Core.Interfaces.Generic
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> Complete();
    }
}