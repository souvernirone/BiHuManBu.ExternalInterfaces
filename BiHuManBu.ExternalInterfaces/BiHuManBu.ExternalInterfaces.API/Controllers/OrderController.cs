using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class OrderController : ApiController
    {
        private ILog _logInfo;
        private ILog _logError;
        private IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            _logInfo = LogManager.GetLogger("INFO");
            _logError = LogManager.GetLogger("ERROR");
        }


        /// <summary>
        /// 创建订单接口-微信创建订单第一步用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> NewCreateOrder([FromBody]CreateOrderRequest request)
        {
            _logInfo.Info("创建订单请求" + request.ToJson());
            var viewModel = new CreateOrderViewMode();

            //Dictionary<String, Object> map = new Dictionary<string, object>();
            //Type t = request.GetType();
            //PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //foreach (PropertyInfo p in pi)
            //{
            //    MethodInfo mi = p.GetGetMethod();
            //    if (mi != null && mi.IsPublic)
            //    {
            //        map.Add(p.Name, mi.Invoke(request, new Object[] { }));
            //    }
            //}
            //_logInfo.Info("返回的map：" + map.ToJson());
            //return viewModel.ResponseToJson();

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = await _orderService.NewCreateOrder(request, Request.GetQueryNameValuePairs());
            _logInfo.Info("创建订单返回值" + response.ToJson());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "创建订单失败";
                return viewModel.ResponseToJson();
            }
            if (response.OrderId > 0)
            {
                viewModel.BusinessStatus = 1;
                viewModel.OrderId = response.OrderId;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "创建订单失败";
            }

            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 订单更新接口-微信创建订单第二步用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateOrder([FromBody]ModifyOrderRequest request)
        {
            _logInfo.Info("更新订单请求" + request.ToJson());
            var viewModel = new CreateOrderViewMode();

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return viewModel.ResponseToJson();
            }

            var response = _orderService.UpdateOrder(request);
            _logInfo.Info("更新订单返回值" + response.ToJson());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }

            if (response.Count > 0)
            {
                viewModel.OrderId = response.Count;
                viewModel.BusinessStatus = 1;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "更新订单失败";
            }

            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 微信用，保单已收到
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> CreateSureOrder([FromBody]CreateSureOrderRequest request)
        {
            _logInfo.Info("创建预约单请求" + request.ToJson());
            var viewModel = new CreateOrderViewMode();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = await _orderService.CreateSureOrder(request, Request.GetQueryNameValuePairs());
            _logInfo.Info("创建订单返回值" + response.ToJson());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "创建订单失败";
                return viewModel.ResponseToJson();
            }
            if (response.OrderId > 0)
            {
                viewModel.BusinessStatus = 1;
                viewModel.OrderId = response.OrderId;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "创建订单失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 订单状态更新接口-微信用,删除订单用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateSelf([FromBody]UpdateOrderRequest request)
        {
            _logInfo.Info("更新订单请求" + request.ToJson());
            var viewModel = new CreateOrderViewMode();

            try
            {
                if (!ModelState.IsValid)
                {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                    return viewModel.ResponseToJson();
                }

                var response = await _orderService.UpdateSelf(request, Request.GetQueryNameValuePairs());
                _logInfo.Info("更新订单返回值" + response.ToJson());
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }

                if (response.Count > 0)
                {
                    viewModel.OrderId = response.Count;
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "更新订单失败";
                }
            }
            catch (Exception ex)
            {
                _logInfo.Info("更新订单异常:" + ex.Message);

            }


            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 预约单详情，从缓存中取
        /// 商户丁丁单独取此接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage OrderDetail([FromUri]OrderDetailRequest request)
        {
            _logInfo.Info("获取订单详情接口请求" + request.ToJson());
            var viewModel = new OrderDetailViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return viewModel.ResponseToJson();
            }
            var response = _orderService.OrderDetail(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "获取订单异常";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.OK)
            {
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "获取信息成功";
                viewModel.CarOrder = response.CarOrder;
                viewModel.ClaimDetail = response.ClaimDetail;
                viewModel.PrecisePrice = response.PrecisePrice;
                viewModel.UserInfo = response.UserInfo;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "获取信息失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 订单状态更新接口-微信用,删除订单用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> ChangeOrderStatus([FromBody]UpdateOrderRequest request)
        {
            _logInfo.Info("更新订单请求" + request.ToJson());
            var viewModel = new CreateOrderViewMode();

            try
            {
                if (!ModelState.IsValid)
                {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                    return viewModel.ResponseToJson();
                }

                var response = await _orderService.ChangeOrderStatus(request, Request.GetQueryNameValuePairs());
                _logInfo.Info("更新订单返回值" + response.ToJson());
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }

                if (response.Count > 0)
                {
                    viewModel.OrderId = response.Count;
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "更新订单失败";
                }
            }
            catch (Exception ex)
            {
                _logInfo.Info("更新订单异常:" + ex.Message);

            }


            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="topAgentId"></param>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public HttpResponseMessage GetOrders([FromUri]GetOrdersRequest request)
        {
            _logInfo.Info("获取订单列表接口请求" + Request.RequestUri);
            var viewModel = new OrdersViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                int totalCount = 0;
                //获取所有大于0的订单，正常已下单的
                var carOrder = _orderService.GetOrders(request, 0, out totalCount).ConvertToViewModel();
                if (carOrder.Count == 0)
                {
                    viewModel.BusinessStatus = -1;
                    viewModel.StatusMessage = "无订单信息";
                }
                else
                {
                    viewModel.CarOrders = carOrder;
                    viewModel.TotalCount = totalCount;
                    viewModel.BusinessStatus = 1;
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询订单列表失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取已出保单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOutOrders([FromUri]GetOrdersRequest request)
        {
            _logInfo.Info("获取已出订单列表接口请求" + Request.RequestUri);
            var viewModel = new OrdersViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                int totalCount = 0;
                //获取所有=-3的订单，正常已收单的
                var carOrder = _orderService.GetOrders(request, -3, out totalCount).ConvertToViewModel();
                if (carOrder.Count == 0)
                {
                    viewModel.BusinessStatus = -1;
                    viewModel.StatusMessage = "无订单信息";
                }
                else
                {
                    viewModel.CarOrders = carOrder;
                    viewModel.TotalCount = totalCount;
                    viewModel.BusinessStatus = 1;
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询订单列表失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetOrder([FromUri]GetOrderRequest request)
        {
            _logInfo.Info("获取订单详情请求" + Request.RequestUri);
            var viewModel = new OrderViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                var carOrder = _orderService.FindCarOrderBy(request, Request.GetQueryNameValuePairs()).ConvertToViewModel();
                _logInfo.Info("获取订单详情返回值" + carOrder.ToJson());
                if (carOrder != null)
                {
                    viewModel.CarOrder = carOrder;
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -1;
                    viewModel.StatusMessage = "无此订单信息";
                }
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询订单列表失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 对外的查询订单详情接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetDetail")]
        public HttpResponseMessage GetOrderOutside([FromUri]GetOrderRequest request)
        {
            _logInfo.Info("对外获取订单详情请求" + Request.RequestUri);
            var viewModel = new GetOrderDetailViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                var carOrder = _orderService.GetOrderDetail(request, Request.GetQueryNameValuePairs());
                if (carOrder.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }
                if (carOrder.Status == HttpStatusCode.NoContent)
                {
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "查无此订单";
                    return viewModel.ResponseToJson();
                }
                viewModel.BusinessStatus = 1;
                viewModel.CarOrder = carOrder.CarOrder.ConverToViewModel(request.Agent);
                //_logInfo.Info("对外获取订单详情返回值" + carOrder.ToJson());
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "获取订单异常";
                _logError.Info("对外获取订单详情请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return viewModel.ResponseToJson();
        }

        #region 目前未使用的接口

        /// <summary>
        /// 创建订单接
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> PostOrder([FromUri]CreateOrderRequest request)
        {
            _logInfo.Info("创建订单请求" + Request.RequestUri);
            var viewModel = new CreateOrderViewMode();

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return viewModel.ResponseToJson();
            }

            var response = await _orderService.Create(request, Request.GetQueryNameValuePairs());
            _logInfo.Info("创建订单返回值" + response.ToJson());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.OrderId > 0)
            {
                viewModel.BusinessStatus = 1;
                viewModel.OrderId = response.OrderId;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "创建订单失败";
            }

            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 订单状态更新接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Update([FromUri]UpdateOrderRequest request)
        {
            _logInfo.Info("更新订单请求" + Request.RequestUri);
            var viewModel = new CreateOrderViewMode();

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return viewModel.ResponseToJson();
            }

            var response = await _orderService.Update(request, Request.GetQueryNameValuePairs());
            _logInfo.Info("更新订单返回值" + response.ToJson());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }

            if (response.Count > 0)
            {
                viewModel.OrderId = response.Count;
                viewModel.BusinessStatus = 1;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "更新订单失败";
            }

            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 创建订单接口-微信用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> PostOrderSelf([FromBody]CreateOrderRequest request)
        {
            _logInfo.Info("创建订单请求" + request.ToJson());
            var viewModel = new CreateOrderViewMode();

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }

            var response = await _orderService.CreateSelf(request, Request.GetQueryNameValuePairs());
            _logInfo.Info("创建订单返回值" + response.ToJson());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.OrderId > 0)
            {
                viewModel.BusinessStatus = 1;
                viewModel.OrderId = response.OrderId;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "创建订单失败";
            }

            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 上传身份证接口
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="openId"></param>
        /// <param name="idImgFirs"></param>
        /// <param name="idImgSecd"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateIdImg(long orderId, string openId, string idImgFirs, string idImgSecd)
        {
            _logInfo.Info("上传身份证请求" + Request.RequestUri);
            var viewModel = new CreateOrderViewMode();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _orderService.UpdateImg(orderId, openId, idImgFirs, idImgSecd);
            _logInfo.Info("上传身份证请求返回值" + response.ToJson());
            if (response.Count > 0)
            {
                viewModel.OrderId = response.Count;
                viewModel.BusinessStatus = 1;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "上传身份证失败";
            }
            return viewModel.ResponseToJson();
        }


        /// <summary>
        /// 判断当前报价的订单是否生成新的订单
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="topAgentId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOrderByBuid([FromUri]GetOrderByBuidRequest request)
        {
            _logInfo.Info("获取订单详情请求" + Request.RequestUri);
            var viewModel = new OrderViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                var carOrder = _orderService.GetOrderByBuid(request, Request.GetQueryNameValuePairs());
                _logInfo.Info("获取订单详情返回值" + carOrder.ToJson());
                if (carOrder != null)
                {
                    viewModel.CarOrder = carOrder;
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -1;
                    viewModel.StatusMessage = "无此订单信息";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询订单列表失败";
            }
            return viewModel.ResponseToJson();
        }

        #endregion
    }
}