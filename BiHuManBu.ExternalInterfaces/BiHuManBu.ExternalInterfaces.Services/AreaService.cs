using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class AreaService:IAreaService
    {
        private IAreaRepository _areaRepository;
        public AreaService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }
        public List<bx_area> Find()
        {
            var key = "ExternalApi_Area_Find";
            lock (key)
            {
                var cachelst = CacheProvider.Get<List<bx_area>>(key);
                if (cachelst == null)
                {
                    var lst = _areaRepository.Find();
                    CacheProvider.Set(key, lst);
                    return lst;
                }
                return cachelst;
            }
        }

        public List<bx_area> FindByPid(int pid)
        {
            var key = string.Format("ExternalApi_Area_Find_{0}", pid);
            lock (key)
            {
                var cachelst = CacheProvider.Get<List<bx_area>>(key);
                if (cachelst == null)
                {
                    var lst = _areaRepository.FindByPid(pid);
                    CacheProvider.Set(key, lst);
                    return lst;
                }
                return cachelst;
            }
        }

    }
}
