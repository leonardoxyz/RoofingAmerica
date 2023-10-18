using Microsoft.EntityFrameworkCore;
using RoofingAmerica.Domain.Core.Data;
using RoofingAmerica.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoofingAmerica.Infrastructure.Repository
{
    public class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DataContext  _dataContext;
        protected readonly DbSet<TEntity> _entity;

        public RepositoryBase(DataContext dataContext)
        {
            _dataContext = dataContext;
            _entity = _dataContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _entity.Remove(entity);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _entity.FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _entity.Update(entity);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
