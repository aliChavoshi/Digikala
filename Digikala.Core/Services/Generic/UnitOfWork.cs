using System;
using System.Collections;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Context;

namespace Digikala.Core.Services.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DigikalaContext _context;
        private Hashtable _repositories;

        public UnitOfWork(DigikalaContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            //اگر مقداری در جدول وجود نداشت ، یدونه واسم بساز که میخواهم چیزی داخلش بریزم
            if (_repositories == null) _repositories = new Hashtable();

            //اینجا از جنس جنریک استفاده کردم و نامش را به عنوان کلید استفاده میکنم
            var type = typeof(TEntity).Name;

            //اگر چنین کلیدی داشتی برو اون را پیدا کن
            //و مقدارش را از هش تیبل پیدا کن و بهم بده
            //دقت کنید که برحسب کلید مقدارش را برمیگردانم
            if (_repositories.ContainsKey(type)) return (IGenericRepository<TEntity>)_repositories[type];

            //اگر داخل جدول چیزی پیدا نکرد باید به جدول خودم اضافه کنم 
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

            //برحسب مقدار و کلید به جدول مورد نظر اضافه کردم
            _repositories.Add(type, repositoryInstance);

            //میگم حالا که اضافه کردی مثل مورد قبلی بر حسب کلید اون را به من بده 
            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
    }
}