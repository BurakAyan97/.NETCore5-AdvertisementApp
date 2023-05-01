using Advertisement.Common.Enums;
using AdvertisementApp.DataAccess.Context;
using AdvertisementApp.DataAccess.Interfaces;
using AdvertisementApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly AdvertisementContext context;

        public Repository(AdvertisementContext context)
        {
            this.context = context;
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await context.Set<T>().Where(filter).AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync<TKey>(Expression<Func<T, TKey>> selector, OrderByType orderByType = OrderByType.DESC)
        {
            return orderByType is OrderByType.ASC
                ? await context.Set<T>().AsNoTracking().OrderBy(selector).ToListAsync()
                : await context.Set<T>().AsNoTracking().OrderByDescending(selector).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> selector, OrderByType orderByType = OrderByType.DESC)
        {
            return orderByType is OrderByType.ASC
                ? await context.Set<T>().Where(filter).AsNoTracking().OrderBy(selector).ToListAsync()
                : await context.Set<T>().Where(filter).AsNoTracking().OrderByDescending(selector).ToListAsync();
        }

        public async Task<T> FindAsync(object id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            return asNoTracking is false
                ? await context.Set<T>().AsNoTracking().SingleOrDefaultAsync(filter)
                : await context.Set<T>().SingleOrDefaultAsync(filter);
        }

        public  IQueryable<T> GetQuery()
        {
            return context.Set<T>().AsQueryable();
        }

        public void Remove ( T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task CreateAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity,T unchanged)
        {
            context.Entry(unchanged).CurrentValues.SetValues(entity);
        }


    }
}
