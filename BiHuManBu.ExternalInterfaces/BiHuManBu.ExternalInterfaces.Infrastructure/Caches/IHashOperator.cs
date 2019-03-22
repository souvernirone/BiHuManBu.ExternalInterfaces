using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Caches
{
    public interface IHashOperator
    {
        bool Exist<T>(string hashId, string key);
        bool Set<T>(string hashId, string key, T t);
        bool Remove(string hashId, string key);
        bool Remove(string key);
        T Get<T>(string hashId, string key);
        List<T> GetAll<T>(string hashId);
        void SetExpire(string key, DateTime datetime);
    }
}
