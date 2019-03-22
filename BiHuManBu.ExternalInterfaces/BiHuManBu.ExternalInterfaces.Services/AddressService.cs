using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using bx_area = BiHuManBu.ExternalInterfaces.Models.bx_area;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class AddressService : IAddressService
    {
        public IAddressRepository _AddressRepository;
        private IUserService _userService;
        private ICityService _cityService;
        private IAreaService _areaService;

        public AddressService(IAddressRepository addressRepository, IUserService userService, ICityService cityService, IAreaService areaService)
        {
            _AddressRepository = addressRepository;
            _userService = userService;
            _cityService = cityService;
            _areaService = areaService;
        }

        public int Add(AddressRequest request)
        {
            //根据openid，mobile，获取userid
            //var user = _userService.AddUser(request.openId, request.phone);
            //if (user == null || user.UserId <= 0)
            //{
            //    return 0;
            //}
            var bxAddress = new bx_address()
            {
                userid = request.ChildAgent,//user.UserId,
                Name = request.Name,
                phone = request.phone,
                address = request.address,
                provinceId = request.provinceId,
                cityId = request.cityId,
                areaId = request.areaId,
                agentId = request.agentId,
                Status = 1,
                createtime = DateTime.Now
            };
            return _AddressRepository.Add(bxAddress);
        }

        public AddressModel Find(int addressId, int childAgent)
        {
            var response = new AddressModel();
            var address = _AddressRepository.Find(addressId, childAgent);
            if (address == null)
            {
                return null;
            }
            //解析address对象
            response = Helper.CopySameFieldsObject<AddressModel>(address);
            //城市名称
            var areas = _areaService.Find();
            //省
            var province = areas.FirstOrDefault(x => x.Id == response.provinceId);
            if (province != null)
            {
                response.provinceName = province.Name;
                //市
                var city = _areaService.FindByPid(province.Id).FirstOrDefault(x => x.Id == response.cityId);
                if (city != null)
                {
                    response.cityName = city.Name;
                    //区
                    var area = _areaService.FindByPid(city.Id).FirstOrDefault(x => x.Id == response.areaId);
                    if (area != null)
                    {
                        response.areaName = area.Name;
                    }
                }
            }
            return response;

        }
        public int Delete(int addressId, int childAgent)
        {
            return _AddressRepository.Delete(addressId, childAgent);
        }

        public int Update(AddressRequest request)
        {
            //根据openid，mobile，获取userid
            //var user = _userService.FindUserByOpenId(request.openId);
            //if (user == null || user.UserId <= 0)
            //{
            //    return 0;
            //}
            var bxAddress = new bx_address()
            {
                id = request.id,
                userid = request.ChildAgent,//user.UserId,
                Name = request.Name,
                phone = request.phone,
                address = request.address,
                provinceId = request.provinceId,
                cityId = request.cityId,
                areaId = request.areaId,
                agentId = request.agentId,
                updatetime = DateTime.Now
            };
            return _AddressRepository.Update(bxAddress);
        }

        /// <summary>
        /// 查询代理人下地址列表
        /// 以前的FindByBuidAndAgentId(string openid, int agentId)改的，去掉openid字段和user表
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public List<AddressModel> FindByBuidAndAgentId(int agentId)
        {
            var addresses = _AddressRepository.FindByBuidAndAgentId(agentId);//, agentId
            List<AddressModel> response = new List<AddressModel>();
            //城市名称
            //var citys = _cityService.FindList();
            //地区
            var areas = _areaService.Find();
            foreach (var item in addresses)
            {
                var addressmodel = Helper.CopySameFieldsObject<AddressModel>(item);
                //省
                var province = areas.FirstOrDefault(x => x.Id == addressmodel.provinceId);
                if (province != null)
                {
                    addressmodel.provinceName = province.Name;
                    //市
                    var city = _areaService.FindByPid(province.Id).FirstOrDefault(x => x.Id == addressmodel.cityId);
                    if (city != null)
                    {
                        addressmodel.cityName = city.Name;
                        //区
                        var area = _areaService.FindByPid(city.Id).FirstOrDefault(x => x.Id == addressmodel.areaId);
                        if (area != null)
                        {
                            addressmodel.areaName = area.Name;
                        }
                    }
                }
                response.Add(addressmodel);
            }
            return response;
        }

        #region 20170228统一账号时注释掉的方法
        //public AddressModel Find(int addressId, string openid)
        //{
        //    //根据openid，mobile，获取userid
        //    var user = _userService.FindUserByOpenId(openid);
        //    if (user == null || user.UserId <= 0)
        //    {
        //        return null;
        //    }
        //    var address= _AddressRepository.Find(addressId, user.UserId);
        //    var response = Helper.CopySameFieldsObject<AddressModel>(address);
        //    //城市名称
        //    var areas = _areaService.Find();
        //    //省
        //    var province = areas.FirstOrDefault(x => x.Id == response.provinceId);
        //    if (province != null)
        //    {
        //        response.provinceName = province.Name;
        //        //市
        //        var city = _areaService.FindByPid(province.Id).FirstOrDefault(x => x.Id == response.cityId);
        //        if (city != null)
        //        {
        //            response.cityName = city.Name;
        //            //区
        //            var area = _areaService.FindByPid(city.Id).FirstOrDefault(x => x.Id == response.areaId);
        //            if (area != null)
        //            {
        //                response.areaName = area.Name;
        //            }
        //        }
        //    }

        //    return response;
        //}

        //public int Delete(int addressId, string openid)
        //{
        //    //根据openid，mobile，获取userid
        //    var user = _userService.FindUserByOpenId(openid);
        //    if (user == null || user.UserId <= 0)
        //    {
        //        return 0;
        //    }
        //    return _AddressRepository.Delete(addressId, user.UserId);
        //}

        //public List<AddressModel> FindByBuidAndAgentId(string openid, int agentId)
        //{
        //    //根据openid，mobile，获取userid
        //    var user = _userService.FindUserByOpenId(openid);
        //    if (user == null || user.UserId <= 0)
        //    {
        //        return new List<AddressModel>();
        //    }
        //    var addresses = _AddressRepository.FindByBuidAndAgentId(user.UserId);//, agentId
        //    List<AddressModel> response=new List<AddressModel>();
        //    //城市名称
        //    //var citys = _cityService.FindList();
        //    //地区
        //    var areas = _areaService.Find();
        //    foreach (var item in addresses)
        //    {
        //        var addressmodel = Helper.CopySameFieldsObject<AddressModel>(item);
        //        //省
        //        var province = areas.FirstOrDefault(x => x.Id == addressmodel.provinceId);
        //        if (province != null)
        //        {
        //            addressmodel.provinceName= province.Name;
        //            //市
        //            var city = _areaService.FindByPid(province.Id).FirstOrDefault(x => x.Id == addressmodel.cityId);
        //            if (city != null)
        //            {
        //                addressmodel.cityName = city.Name;
        //                //区
        //                var area = _areaService.FindByPid(city.Id).FirstOrDefault(x => x.Id == addressmodel.areaId);
        //                if (area != null)
        //                {
        //                    addressmodel.areaName = area.Name;
        //                }
        //            }
        //        }
        //        response.Add(addressmodel);
        //    }
        //    return response;
        //}
        #endregion
    }
}
