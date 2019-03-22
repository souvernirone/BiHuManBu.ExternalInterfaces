using System;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class AddressController : ApiController
    {
        //
        // GET: /Address/

        public IAddressService _AddressService;

        public AddressController(IAddressService addressService)
        {
            _AddressService = addressService;
        }
        /// <summary>
        /// 新增收单地址,返回id
        /// </summary>
        /// <param name="bxAddress"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Add([FromBody]AddressRequest bxAddress)
        {
            var viewModel = new CreateAddressViewModel();
            try
            {
                int addressId = _AddressService.Add(bxAddress);
                if (addressId > 0)
                {
                    viewModel.BusinessStatus = 1;
                    viewModel.AddressId = addressId;
                }
                else
                {
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "创建地址失败";
                }
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "创建地址失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 按ID查询收单地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Find(int addressId,int childAgent)
        {
            var viewModel = new AddressViewModel();
            try
            {
                var address = _AddressService.Find(addressId, childAgent);
                if (address != null)
                {
                    viewModel.Address = address;
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "没有地址信息";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询地址失败";
            }
            return viewModel.ResponseToJson();
        }
        //public HttpResponseMessage Find(int addressId, string openid)
        //{
        //    var viewModel = new AddressViewModel();
        //    try
        //    {
        //        var address = _AddressService.Find(addressId, openid);
        //        if (address != null)
        //        {
        //            viewModel.Address = address;
        //            viewModel.BusinessStatus = 1;
        //        }
        //        else
        //        {
        //            viewModel.BusinessStatus = -10003;
        //            viewModel.StatusMessage = "没有地址信息";
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        viewModel.BusinessStatus = -10002;
        //        viewModel.StatusMessage = "查询地址失败";
        //    }
        //    return viewModel.ResponseToJson();
        //}


        /// <summary>
        /// 删除收单地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Delete(int addressId, int ChildAgent)
        {
            var viewModel = new BaseViewModel();
            try
            {
                var count = _AddressService.Delete(addressId, ChildAgent);
                if (count > 0)
                {
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "删除失败";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "删除失败";
            }
            return viewModel.ResponseToJson();
        }
        //public HttpResponseMessage Delete(int addressId, string openid)
        //{
        //    var viewModel = new BaseViewModel();
        //    try
        //    {
        //        var count= _AddressService.Delete(addressId, openid);
        //        if (count > 0)
        //        {
        //            viewModel.BusinessStatus = 1;
        //        }
        //        else
        //        {
        //            viewModel.BusinessStatus = -10002;
        //            viewModel.StatusMessage = "删除失败";
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        viewModel.BusinessStatus = -10002;
        //        viewModel.StatusMessage = "删除失败";
        //    }
        //    return viewModel.ResponseToJson();
        //}
        /// <summary>
        /// 更新收单地址
        /// </summary>
        /// <param name="bxAddress"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Update([FromBody]AddressRequest bxAddress)
        {
            var viewModel = new BaseViewModel();
            try
            {
                int count = _AddressService.Update(bxAddress);
                if (count > 0)
                {
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "更新失败";
                }
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "更新地址失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 按openid和agentid获取收单地址列表
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAddress(int agentId)
        {
            var viewModel = new AddressesViewModel();
            try
            {
                viewModel.Addresses = _AddressService.FindByBuidAndAgentId(agentId);
                viewModel.BusinessStatus = 1;
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询地址失败";
            }

            return viewModel.ResponseToJson();
        }
        //public HttpResponseMessage GetAddress(string openid, int agentId)
        //{
        //    var viewModel = new AddressesViewModel();
        //    try
        //    {
        //        viewModel.Addresses = _AddressService.FindByBuidAndAgentId(openid, agentId);
        //        viewModel.BusinessStatus = 1;
        //    }
        //    catch (Exception)
        //    {
        //        viewModel.BusinessStatus = -10002;
        //        viewModel.StatusMessage = "查询地址失败";
        //    }

        //    return viewModel.ResponseToJson();
        //}
    }
}
