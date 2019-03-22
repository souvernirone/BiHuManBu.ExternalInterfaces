using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BiHuManBu.ExternalInterfaces.Repository.DbOper
{
    public class Repository<T>:IRepository<T> where T:class
    {
        private DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }  
        public List<T> SqlQuery(string strSql, params object[] paramObjects)
        {
            return _dbContext.Database.SqlQuery<T>(strSql, paramObjects).ToList();
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public IQueryable<T> FindAll()
        {
            return _dbContext.Set<T>();
        }

        public void Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Insert(List<T> entitys)
        {
            foreach (var item in entitys)
            {
                _dbContext.Set<T>().Add(item);
            }
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbContext.Entry<T>(entity).State=EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Update(List<T> entities)
        {
            foreach (var item in entities)
            {
                _dbContext.Entry<T>(item).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Entry<T>(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }
    }
}
