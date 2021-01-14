using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Digikala.Core.Services
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        #region Ioc

        protected readonly DigikalaContext Context;
        protected readonly DbSet<TEntity> DbSet;
        public GenericRepository(DigikalaContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        #endregion

        #region Get

        public async Task<TEntity> GetById(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> ToListAsync()
        {
            return await DbSet.ToListAsync();
        }

        public IEnumerable<TEntity> ToList()
        {
            return DbSet.AsEnumerable();
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.SingleOrDefaultAsync(where);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.FirstOrDefaultAsync(where);
        }

        public async Task<IEnumerable<TEntity>> WhereByIncludes(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = includes.
                Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(DbSet, (current, include) => current.Include(include));

            return await query.Where(where).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> WhereByIncludes(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = includes.
                Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(DbSet, (current, include) => current.Include(include));

            return await query.ToListAsync();
        }


        #endregion

        #region CRUD

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }
        public void Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }
        public void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }
        public async Task Add(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }
        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Count

        public async Task<int> Count(Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).CountAsync();
        }
        public async Task<int> Count()
        {
            return await DbSet.CountAsync();
        }
        #endregion

        #region Where&IsExist

        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> whereConditions)
        {
            return await DbSet.AnyAsync(whereConditions);
        }

        public async Task<bool> WhereIsExist(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, bool>> anyWhere)
        {
            return await DbSet.Where(where).AnyAsync(anyWhere);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        #endregion

        #region SelectListItem

        public IEnumerable<SelectListItem> ToSelectList(string text, string value, string selected, string defaultOption)
        {
            IEnumerable<TEntity> result = DbSet;

            var query = from e in result
                        select new
                        {
                            Value = e.GetType().GetProperty(value)?.GetValue(e, null),
                            Text = e.GetType().GetProperty(text)?.GetValue(e, null)
                        };

            return query.AsEnumerable()
                .Select(s => new SelectListItem
                {
                    Value = s.Value.ToString(),
                    Text = s.Text.ToString(),
                    Selected = selected == s.Value.ToString()
                }).Prepend(new SelectListItem { Value = "0", Text = "لطفا انتخاب کنید" });
        }

        #endregion

        #region Save

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }

        #endregion
    }
}