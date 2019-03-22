using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Repository.DbOper
{
    public interface IRepository<T>:IReadOnlyRepository<T> where T:class 
    {
        void Insert(T entity);
        void Insert(List<T> entitys);
        void Update(T entity);
        void Update(List<T> entities);
        void Delete(T entity);
    }
}
