using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsBooking.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public void Update(T entity)
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync(id, ct);
            if (entity != null)
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
