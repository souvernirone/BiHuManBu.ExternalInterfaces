using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BiHuManBu.ExternalInterfaces.Repository.DbOper
{
    public interface IReadOnlyRepository<T> where T:class
    {
        List<T> SqlQuery(string strSql, params Object[] paramObjects);
        IQueryable<T> Search(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll();
      
    }
}
