using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using bx_area = BiHuManBu.ExternalInterfaces.Models.bx_area;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class AreaController : ApiController
    {
        //
        // GET: /Area/

        private IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }
        /// <summary>
        /// 获取省级列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Getprovince()
        {
            List<bx_area> bxAreas=new List<bx_area>();
            bxAreas = _areaService.Find();
            AreaViewModel viewModel=new AreaViewModel();
            viewModel.Areas=new List<Services.ViewModels.bx_area>();
            foreach (var item in bxAreas)
            {
                Services.ViewModels.bx_area area =
                    Helper.CopySameFieldsObject<Services.ViewModels.bx_area>(item);
                viewModel.Areas.Add(area);
            }
            return viewModel.ResponseToJson();
        }
        /// <summary>
        /// 按Pid获取市或者区列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public HttpResponseMessage GetArea(int pid)
        {
            List<bx_area> bxAreas = new List<bx_area>();
            bxAreas = _areaService.FindByPid(pid);

            AreaViewModel viewModel = new AreaViewModel();
            viewModel.Areas=new List<Services.ViewModels.bx_area>();
            foreach (var item in bxAreas)
            {
                Services.ViewModels.bx_area area =
                    Helper.CopySameFieldsObject<Services.ViewModels.bx_area>(item);
                viewModel.Areas.Add(area);
            }
            return bxAreas.ResponseToJson();
        }

    }
}
