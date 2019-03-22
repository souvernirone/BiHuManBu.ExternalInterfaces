using BiHuManBu.ExternalInterfaces.Models.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityState = System.Data.Entity.EntityState;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class EFRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private DbContext _context;

        public EFRepositoryBase()
        {
            _context = DataContextFactory.GetDataContext();
        }

        public virtual DbSet<TEntity> Table { get { return _context.Set<TEntity>(); } }
        public int Count()
        {
            return GetAll().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public void Delete<TEntity1>(TEntity1 entity) where TEntity1 : class
        {
            AttachIfNot(entity);
            _context.Set<TEntity1>().Remove(entity);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public IQueryable<TEntity1> GetAll<TEntity1>() where TEntity1 : class
        {
            return _context.Set<TEntity1>();
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public DbContext GetDbContext()
        {
            return _context;
        }

        public void Insert(TEntity entity)
        {
            Table.Add(entity);
        }

        public void Insert<TEntity1>(TEntity1 entity) where TEntity1 : class
        {
            _context.Set<TEntity1>().Add(entity);
        }

        public void InsertAsync(TEntity entity)
        {
            Task.FromResult(Table.Add(entity));
        }

        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Any(predicate);
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(_context.SaveChanges());
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public void Update(TEntity entity)
        {
            AttachIfNot(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Update<TEntity1>(TEntity1 entity) where TEntity1 : class
        {
            AttachIfNot(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        protected virtual void AttachIfNot<TEntity1>(TEntity1 entity) where TEntity1 : class
        {
            if (!_context.Set<TEntity1>().Local.Contains(entity))
            {
                _context.Set<TEntity1>().Attach(entity);
            }
        }
    }
}
