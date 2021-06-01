using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digikala.Core.Interfaces
{

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region CRUD
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void DeleteRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        #endregion

        #region Get

        Task<TEntity> GetById(int id);
        //یه دونه با شرط و با اینکلود هاش واسم میاره
        Task<IEnumerable<TEntity>> WhereByIncludes(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> ToListAsync();
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where);

        #endregion

        #region Count

        Task<int> Count(Expression<Func<TEntity, bool>> where);
        Task<int> Count();

        #endregion

        #region Where&IsExist

        Task<bool> IsExist(Expression<Func<TEntity, bool>> whereConditions);
        Task<bool> WhereIsExist(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, bool>> anyWhere);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region SelectListItem

        IEnumerable<SelectListItem> ToSelectList(string text, string value, string selected, string defaultOption);

        #endregion

        #region Save

        Task Save();

        #endregion
    }
}
