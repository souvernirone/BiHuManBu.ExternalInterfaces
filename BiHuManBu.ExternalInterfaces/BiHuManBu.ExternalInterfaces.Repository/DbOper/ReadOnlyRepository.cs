using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BiHuManBu.ExternalInterfaces.Repository.DbOper
{
    public class ReadOnlyRepository<T>:IReadOnlyRepository<T> where T:class
    {
        private DbContext _dbContext;

        public ReadOnlyRepository(DbContext context)
        {
            _dbContext = context;
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
    }
}
