using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.ConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using bx_car_order = BiHuManBu.ExternalInterfaces.Models.bx_car_order;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class OrderService : CommonBehaviorService, IOrderService
    {
        #region 声明
        private IOrderRepository _orderRepository;
        private ILog _logInfo = LogManager.GetLogger("INFO");
        private ILog _logError = LogManager.GetLogger("ERROR");
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        private IUserService _userService;
        private ILastInfoRepository _lastInfoRepository;
        private IUserInfoRepository _userInfoRepository;
        private IQuoteResultRepository _quoteResultRepository;
        private ISaveQuoteRepository _saveQuoteRepository;
        private ISubmitInfoRepository _submitInfoRepository;
        private IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;
        private IAddressRepository _addressRepository;
        private IUserClaimRepository _userClaimRepository;
        private IQuoteResultCarinfoRepository _carinfoRepository;
        private readonly IGetConfigValueService _getConfigValueService;

        public OrderService(IOrderRepository orderRepository,
            IAgentRepository agentRepository, ICacheHelper cacheHelper, IUserService userService,
            IUserInfoRepository userInfoRepository, ILastInfoRepository lastInfoRepository,
            IQuoteResultRepository quoteResultRepository, ISaveQuoteRepository saveQuoteRepository,
            ISubmitInfoRepository submitInfoRepository, IQuoteReqCarinfoRepository quoteReqCarinfoRepository,
            IAddressRepository addressRepository, IUserClaimRepository userClaimRepository,
            IQuoteResultCarinfoRepository carinfoRepository, IGetConfigValueService getConfigValueService)
            : base(agentRepository, cacheHelper)
        {
            _orderRepository = orderRepository;
            _agentRepository = agentRepository;
            _userService = userService;
            _lastInfoRepository = lastInfoRepository;
            _userInfoRepository = userInfoRepository;
            _quoteResultRepository = quoteResultRepository;
            _saveQuoteRepository = saveQuoteRepository;
            _submitInfoRepository = submitInfoRepository;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
            _addressRepository = addressRepository;
            _userClaimRepository = userClaimRepository;
            _carinfoRepository = carinfoRepository;
            _getConfigValueService = getConfigValueService;
        }
        #endregion

        /// <summary>
        /// 创建订单（同时复制历时的人员基础信息、险种信息、报价结果、核保状态）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<CreateOrderResponse> NewCreateOrder(CreateOrderRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            CreateOrderResponse response = new CreateOrderResponse();
            int childagent = request.ChildAgent == 0 ? request.Agent : request.ChildAgent;
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.NotFound;
                return response;
            }
            //if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}
            try
            {
                int sourceValue = 0;
                if (request.IsNewSource.HasValue)
                {
                    sourceValue = (int)(request.IsNewSource == 1 ? SourceGroupAlgorithm.GetOldSource(request.Source) : request.Source);
                }
                else
                {
                    sourceValue = (int)request.Source;
                }

                bx_userinfo userinfo = _userInfoRepository.FindByBuid(request.Buid);
                if (userinfo == null)
                {
                    response.Status = HttpStatusCode.ExpectationFailed;
                    return response;
                }
                bx_lastinfo lastinfo = _lastInfoRepository.GetByBuid(request.Buid);
                bx_savequote savequote = _saveQuoteRepository.GetSavequoteByBuid(request.Buid);
                bx_submit_info submitInfo = _submitInfoRepository.GetSubmitInfo(request.Buid, sourceValue);
                bx_quoteresult quoteresult = _quoteResultRepository.GetQuoteResultByBuid(request.Buid, sourceValue);
                bx_quoteresult_carinfo carInfo = _carinfoRepository.Find(request.Buid, sourceValue);
                List<bx_claim_detail> claimDetails = _userClaimRepository.FindList(request.Buid);
                //取商业险和交强险开始时间
                var qrStartDate = userinfo.QuoteStatus > 0 ? _quoteResultRepository.GetStartDate(userinfo.Id) : new InsuranceStartDate();
                bx_car_order exitCarOrder = _orderRepository.FindOrder(userinfo.LicenseNo, childagent);
                if (exitCarOrder != null)
                {
                    exitCarOrder.order_status = -1; //删除订单
                    _orderRepository.Update(exitCarOrder);
                }

                //根据openid，mobile，获取userid
                //var user = _userService.AddUser(request.OpenId, request.Mobile);

                #region 地址

                int addressid = request.AddressId.HasValue ? request.AddressId.Value : 0;

                var address = new bx_address();
                if (addressid == 0)
                {
                    if (!string.IsNullOrEmpty(request.DistributionAddress) &&
                        !string.IsNullOrEmpty(request.DistributionName) &&
                        !string.IsNullOrEmpty(request.DistributionPhone))
                    {
                        address.address = request.DistributionAddress;
                        if (request.ProvinceId.HasValue) address.provinceId = request.ProvinceId.Value;
                        if (request.CityId.HasValue) address.cityId = request.CityId.Value;
                        if (request.AreaId.HasValue) address.areaId = request.AreaId.Value;
                        address.phone = request.DistributionPhone;
                        address.Name = request.DistributionName;
                        address.agentId = request.Agent;
                        address.createtime = DateTime.Now;
                        address.userid = childagent;//request.ChildAgent;
                        address.Status = 1;
                        addressid = _addressRepository.Add(address);
                    }
                }

                #endregion

                #region 订单实例

                var order = new bx_car_order
                {
                    //**为必填项
                    buid = request.Buid, //**
                    //order_num = request.OrderNum,
                    source = sourceValue, //**
                    insured_name = request.InsuredName, //被保险人
                    contacts_name = request.ContactsName, //**联系人
                    mobile = request.Mobile, //**联系电话
                    receipt_head = request.ReceiptHead, //发票类型
                    receipt_title = request.Receipt, //发票内容
                    create_time = DateTime.Now,
                    user_id = childagent,//request.ChildAgent,//user_id = user != null ? user.UserId : 0,
                    openid = request.OpenId, //**
                    total_price = (decimal?)request.TotalPrice, //**
                    carriage_price = (decimal?)request.CarriagePrice, //**
                    insurance_price = (decimal?)request.InsurancePrice, //**
                    id_type = request.IdType, //证件类型
                    id_num = request.IdNum, //证件号
                    addressid = addressid, //配送关联id
                    distribution_type = request.DistributionType, //配送类型
                    distribution_address = request.DistributionAddress, //收件地址
                    distribution_name = request.DistributionName, //收件人
                    distribution_phone = request.DistributionPhone, //收件手机
                    distribution_time = request.DistributionTime, //配送时间
                    pay_type = request.PayType, //支付方式
                    id_img_firs = request.IdImgFirst,
                    id_img_secd = request.IdImgSecond,
                    top_agent = request.Topagent, //**顶级代理人id
                    cur_agent = request.Agent, //**当前代理人id
                    bizRate = (decimal?)request.BizRate, //商业险费率
                    LicenseNo = userinfo != null ? userinfo.LicenseNo : string.Empty, //车牌号
                    order_status = 1,
                    pay_status = 0,
                    imageUrls = request.ImageUrls
                };

                #endregion

                //创建订单
                long orderid = _orderRepository.CreateOrder(order, address, lastinfo, userinfo, savequote,
                    submitInfo, quoteresult, carInfo, claimDetails);
                if (orderid > 0)
                {
                    //生成订单号
                    var orderNum = Guid.NewGuid().ToString().Replace("-", "");//GenerateOrderNum(orderid, request.Fountain);//EnumOrderPlatform.WeChat
                    order.order_num = orderNum;
                    _orderRepository.Update(order);

                    //将对象添加到缓存start
                    var orderCache = new OrderCacheResponse
                    {
                        BxCarOrder = order,
                        BxLastInfo = lastinfo,
                        BxQuoteResult = quoteresult,
                        BxSaveQuote = savequote,
                        BxSubmitInfo = submitInfo,
                        BxUserInfo = userinfo,
                        BxCarInfo = carInfo,
                        BxClaimDetails = claimDetails,
                        QrStartDate = qrStartDate
                    };
                    var orderKey = string.Format("OrderDetail_{0}", orderid);
                    CacheProvider.Remove(orderKey);
                    CacheProvider.Set(orderKey, orderCache, 86400);
                    //将对象添加到缓存end

                    long newSourceValue = 0;
                    if (request.IsNewSource.HasValue)
                    {
                        newSourceValue = request.IsNewSource == 1 ? request.Source
                            : SourceGroupAlgorithm.GetNewSource((int)request.Source);
                    }
                    else
                    {
                        newSourceValue = SourceGroupAlgorithm.GetNewSource((int)request.Source);
                    }
                    //添加crm时间轴
                    PostCrmStep(request.InsurancePrice, request.Agent, orderid, request.Buid, newSourceValue);

                    //更新userinfo的回访信息
                    userinfo.IsReView = 6;
                    string strStatus = _userInfoRepository.Update(userinfo) > 0 ? "成功" : "失败";

                    //调用第三方接口传数据
                    bool thirdPart = false;
                    if (request.Topagent == 11899)
                    {
                        thirdPart = PostDataDingDing(orderCache, request.Topagent, "成功");
                    }
                    else
                    {
                        thirdPart = PostDataThirdPart(orderCache, request.Agent, request.Topagent, "success");
                    }

                    response.Status = HttpStatusCode.OK;
                    response.OrderId = orderid;
                    _logInfo.Info("创建订单成功，更新bx_userinfo记录" + strStatus + "，/n代理人" + request.Agent + "，调用第三方返回：" + thirdPart + "，/n订单信息:" + request.ToJson());
                }
                else
                {
                    response.Status = HttpStatusCode.ExpectationFailed;
                    response.OrderId = 0;
                    _logError.Info("创建订单失败，订单信息：" + request.ToJson());
                }
            }
            catch (Exception ex)
            {
                _logError.Info("创建订单异常，创建订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }
            return response;
        }

        public UpdateOrderResponse UpdateOrder(ModifyOrderRequest request)
        {
            var response = new UpdateOrderResponse();
            try
            {
                bx_car_order orderItem = _orderRepository.FindBy(request.OrderID);//, request.OpenId
                if (orderItem != null)
                {
                    //orderItem.source = request.Source;
                    orderItem.id_num = request.IdNum;
                    orderItem.id_type = request.IdType;

                    orderItem.insured_name = request.InsuredName;
                    orderItem.mobile = request.Mobile;
                    orderItem.contacts_name = request.ContactsName;

                    orderItem.receipt_title = request.Receipt;
                    orderItem.receipt_head = request.ReceiptHead;

                    orderItem.pay_type = request.PayType;
                    //orderItem.insurance_price = (decimal?)request.InsurancePrice;
                    //orderItem.carriage_price = (decimal?) request.CarriagePrice;
                    //orderItem.total_price = (decimal?)request.TotalPrice;

                    orderItem.id_img_firs = request.IdImgFirst;
                    orderItem.id_img_secd = request.IdImgSecond;

                    orderItem.distribution_type = request.DistributionType;
                    orderItem.addressid = request.AddressId;
                    orderItem.distribution_address = request.DistributionAddress;
                    orderItem.distribution_name = request.DistributionName;
                    orderItem.distribution_phone = request.DistributionPhone;
                    orderItem.distribution_time = (DateTime?)request.DistributionTime;

                    orderItem.imageUrls = request.ImageUrls;//新增的图片地址
                    _orderRepository.Update(orderItem);
                    response.Count = orderItem.id;

                    //20170209修改缓存对象
                    var orderKey = string.Format("OrderDetail_{0}", request.OrderID);
                    var cacheObject = CacheProvider.Get<OrderCacheResponse>(orderKey);
                    if (cacheObject != null)
                    {
                        //将对象添加到缓存start
                        var orderCache = new OrderCacheResponse
                        {
                            BxCarOrder = orderItem,
                            BxLastInfo = cacheObject.BxLastInfo,
                            BxQuoteResult = cacheObject.BxQuoteResult,
                            BxSaveQuote = cacheObject.BxSaveQuote,
                            BxSubmitInfo = cacheObject.BxSubmitInfo,
                            BxUserInfo = cacheObject.BxUserInfo,
                            BxCarInfo = cacheObject.BxCarInfo,
                            BxClaimDetails = cacheObject.BxClaimDetails,
                            QrStartDate = cacheObject.QrStartDate
                        };
                        CacheProvider.Remove(orderKey);
                        CacheProvider.Set(orderKey, orderCache, 86400);
                        //将对象添加到缓存end
                    }
                }
                else
                {
                    response.Count = 0;
                }
            }
            catch (Exception ex)
            {

                _logError.Info("更新订单异常，更新订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }

            return response;
        }

        /// <summary>
        /// 创建订单（同时复制历时的人员基础信息、险种信息、报价结果、核保状态）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<CreateOrderResponse> CreateSureOrder(CreateSureOrderRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new CreateOrderResponse();

            #region 验证信息
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.NotFound;
                return response;
            }
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            bx_agent agent = _agentRepository.GetAgent(request.CurAgent);
            if (agent == null)
            {
                response.Status = HttpStatusCode.NotFound;
                return response;
            }
            #endregion

            int sourceValue = SourceGroupAlgorithm.GetOldSource(request.Source);

            #region 更新预约单状态
            bx_car_order bxCarOrder = _orderRepository.FindBy(request.OrderId);
            if (bxCarOrder != null)
            {
                //-3微信端的订单终极状态：已收单。-2为整体流程的终极状态，-2由pc端审核 出单之后来改。
                //流程：报价->预约->收单->出单
                bxCarOrder.order_status = request.OrderStatus;
                //bxCarOrder.source = sourceValue;
                if (request.OrderStatus == -2)
                {//已出单增加出单时间
                    bxCarOrder.single_time = !string.IsNullOrEmpty(request.SingleTime) ? DateTime.Parse(request.SingleTime) : DateTime.Now;
                }
                else
                {
                    bxCarOrder.GetOrderTime = DateTime.Now; //20161011新增收到保单时间
                }
                bxCarOrder.bizCost = request.BizCost;
                bxCarOrder.forceCost = request.ForceCost;
                bxCarOrder.CategoryInfoId = request.CategoryInfoId;
                bxCarOrder.zs_zuoxi_id = request.ZuoxiId;
                if (_orderRepository.Update(bxCarOrder) > 0)
                {
                    response.Status = HttpStatusCode.OK;
                    response.OrderId = request.OrderId;
                    _logInfo.Info("更新订单成功,订单信息:" + request.ToJson());
                }
                else
                {
                    response.Status = HttpStatusCode.ExpectationFailed;
                    response.OrderId = 0;
                    _logError.Info("更新订单失败，订单信息：" + request.ToJson());
                }
                return response;
            }
            #endregion

            #region 创建预约单
            bx_lastinfo lastinfo = _lastInfoRepository.GetByBuid(request.Buid);
            bx_userinfo userinfo = _userInfoRepository.FindByBuid(request.Buid);
            bx_savequote savequote = _saveQuoteRepository.GetSavequoteByBuid(request.Buid);
            bx_submit_info submitInfo = _submitInfoRepository.GetSubmitInfo(request.Buid, sourceValue);
            bx_quoteresult quoteresult = _quoteResultRepository.GetQuoteResultByBuid(request.Buid, sourceValue);
            bx_quotereq_carinfo quotereqCarinfo = _quoteReqCarinfoRepository.Find(request.Buid);
            bx_quoteresult_carinfo quoteresultCarinfo = _carinfoRepository.Find(request.Buid, sourceValue);
            List<bx_claim_detail> claimDetails = _userClaimRepository.FindList(request.Buid);

            bx_car_order exitCarOrder = _orderRepository.FindBy(userinfo != null ? userinfo.LicenseNo : string.Empty, agent.OpenId, request.Agent);
            if (exitCarOrder != null)
            {
                exitCarOrder.order_status = -1;//删除订单
                _orderRepository.Update(exitCarOrder);
            }

            //根据openid，mobile，获取userid
            //var user = _userService.AddUser(agent.OpenId, agent.Mobile);

            int addressid = 0;

            //订单实例
            //start
            int idType = 2;
            double insurancePrice = 0;
            string insuredName = string.Empty;
            string idNum = string.Empty;
            if (quoteresult != null)
            {
                if (quoteresult.InsuredIdType.HasValue)
                {
                    if (quoteresult.InsuredIdType.Value == 1)
                        idType = 0;
                    else if (quoteresult.InsuredIdType.Value == 2)
                        idType = 1;
                }
                insuredName = quoteresult.InsuredName;
                idNum = quoteresult.InsuredIdCard;
                insurancePrice = (quoteresult.BizTotal.HasValue ? quoteresult.BizTotal.Value : 0) +
                    (quoteresult.ForceTotal.HasValue ? quoteresult.ForceTotal.Value : 0) +
                    (quoteresult.TaxTotal.HasValue ? quoteresult.TaxTotal.Value : 0);
            }

            var order = new bx_car_order
            {//**为必填项
                buid = request.Buid,//**
                //order_num = request.OrderNum,
                source = sourceValue,//**
                insured_name = insuredName,//被保险人
                contacts_name = agent.AgentName,//**联系人
                mobile = agent.Mobile,//**联系电话
                create_time = DateTime.Now,
                user_id = request.ChildAgent,//user_id = user != null ? user.UserId : 0,
                openid = agent.OpenId,//**
                total_price = (decimal?)insurancePrice,//**
                carriage_price = 0,//**
                insurance_price = (decimal?)insurancePrice,//**
                id_type = idType,//证件类型
                id_num = idNum,//证件号
                addressid = addressid,//配送关联id
                top_agent = request.Agent,//**顶级代理人id
                cur_agent = request.CurAgent,//**当前代理人id
                bizRate = request.BizRate,//商业险费率
                LicenseNo = userinfo != null ? userinfo.LicenseNo : string.Empty,//车牌号
                order_status = request.OrderStatus, //-3,//出单
                pay_status = 0,
                bizCost = request.BizCost,
                forceCost = request.ForceCost,
                CategoryInfoId = request.CategoryInfoId,
                zs_zuoxi_id = request.ZuoxiId
            };
            if (request.OrderStatus == -2)
                order.single_time = !string.IsNullOrEmpty(request.SingleTime)
                    ? DateTime.Parse(request.SingleTime)
                    : DateTime.Now;
            else
            {
                order.GetOrderTime = DateTime.Now; //20161011新增收到保单时间
            }
            //订单实例
            //end


            var address = new bx_address();

            //创建订单
            long orderid = _orderRepository.CreateOrder(order, address, lastinfo, userinfo, savequote, submitInfo, quoteresult, quoteresultCarinfo, claimDetails);
            if (orderid > 0)
            {
                //生成订单号
                var orderNum = GenerateOrderNum(orderid, request.Fountain);//EnumOrderPlatform.WeChat
                order.order_num = orderNum;
                _orderRepository.Update(order);

                response.Status = HttpStatusCode.OK;
                response.OrderId = orderid;
                _logInfo.Info("创建订单成功,订单信息:" + request.ToJson());
            }
            else
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                response.OrderId = 0;
                _logError.Info("创建订单失败，订单信息：" + request.ToJson());
            }
            return response;

            #endregion
        }

        public async Task<UpdateOrderResponse> UpdateSelf(UpdateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new UpdateOrderResponse();
            //根据经纪人获取手机号 
            //var agentModel = GetAgent(request.AgentId);

            try
            {
                var orderItem = _orderRepository.FindBy(request.OrderId);
                if (orderItem != null)
                {
                    orderItem.order_status = request.OrderStatus;
                    orderItem.pay_status = request.PayStatus;
                    _orderRepository.Update(orderItem);
                    response.Count = orderItem.id;
                }
                else
                {
                    response.Count = 0;
                }
            }
            catch (Exception ex)
            {
                _logError.Info("更新订单异常，更新订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }

            return response;
        }

        public async Task<UpdateOrderResponse> ChangeOrderStatus(UpdateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new UpdateOrderResponse();
            //根据经纪人获取手机号 
            //var agentModel = GetAgent(request.AgentId);

            try
            {
                var orderItem = _orderRepository.FindBy(request.OrderId.ToString());
                if (orderItem != null)
                {
                    orderItem.order_status = request.OrderStatus;
                    orderItem.pay_status = request.PayStatus;
                    _orderRepository.Update(orderItem);
                    response.Count = orderItem.id;
                }
                else
                {
                    response.Count = 0;
                }
            }
            catch (Exception ex)
            {
                _logError.Info("更新订单异常，更新订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }

            return response;
        }

        /// <summary>
        /// 预约单详情接口，值从缓存中取
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public OrderDetailResponse OrderDetail(OrderDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new OrderDetailResponse();
            IBxAgent agentModel = GetAgentModelFactory(request.Topagent);
            //参数校验
            if (agentModel == null)
            {
                response.Status = HttpStatusCode.NotFound;
                return response;
            }
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            try
            {
                var orderKey = string.Format("OrderDetail_{0}", request.OrderId);
                var orderModel = CacheProvider.Get<OrderCacheResponse>(orderKey);
                if (orderModel != null)
                {
                    #region 实例化

                    //新实例start
                    var carOrder = new CarOrder();
                    var userInfo = new UserInfo();
                    var claimDetails = new List<ClaimDetail>();
                    var precisePrice = new PrecisePrice();
                    //新实例end
                    //原有实例start
                    bx_car_order bxCarOrder = orderModel.BxCarOrder;
                    bx_userinfo bxUserInfo = orderModel.BxUserInfo;
                    bx_quoteresult bxQuoteResult = orderModel.BxQuoteResult;
                    bx_savequote bxSaveQuote = orderModel.BxSaveQuote;
                    bx_submit_info bxSubmitInfo = orderModel.BxSubmitInfo;
                    bx_lastinfo bxLastInfo = orderModel.BxLastInfo;
                    bx_quoteresult_carinfo bxCarInfo = orderModel.BxCarInfo;
                    List<bx_claim_detail> bxClaimDetails = orderModel.BxClaimDetails;
                    InsuranceStartDate qrStartDate = orderModel.QrStartDate;
                    //原有实例end

                    #endregion

                    #region CarOrder转换

                    if (bxCarOrder != null)
                    {
                        carOrder.AgentName = string.Empty;
                        carOrder.AgentMobile = string.Empty;
                        if (bxCarOrder.cur_agent.HasValue)
                        {
                            carOrder.Agent = bxCarOrder.cur_agent.Value;
                            var curAgent = _agentRepository.GetAgent(bxCarOrder.cur_agent.Value);
                            if (curAgent != null)
                            {
                                carOrder.AgentName = curAgent.AgentName;
                                carOrder.AgentMobile = curAgent.Mobile;
                            }
                        }
                        carOrder.OpenId = bxCarOrder.openid;
                        carOrder.Mobile = bxCarOrder.mobile;
                        carOrder.ContactsName = bxCarOrder.contacts_name;
                        carOrder.OrderStatus = bxCarOrder.order_status.HasValue ? bxCarOrder.order_status.Value : 0;
                        carOrder.GetOrderTime = bxCarOrder.GetOrderTime.ToString();
                        carOrder.CreateTime = bxCarOrder.create_time.ToString();
                        carOrder.OrderId = bxCarOrder.id;
                        carOrder.Buid = bxCarOrder.buid.HasValue ? bxCarOrder.buid.Value : 0;
                        carOrder.Source = bxCarOrder.source.HasValue ? bxCarOrder.source.Value : 0;
                        carOrder.Source = SourceGroupAlgorithm.GetNewSource((int)carOrder.Source);
                        carOrder.InsuredName = bxCarOrder.insured_name;
                        carOrder.IdType = bxCarOrder.id_type.HasValue ? bxCarOrder.id_type.Value : 2; //默认空时，赋值个“其他”
                        carOrder.IdNum = bxCarOrder.id_num;
                        carOrder.IdImgFirst = bxCarOrder.id_img_firs;
                        carOrder.IdImgSecond = bxCarOrder.id_img_secd;
                        carOrder.ImageUrls = bxCarOrder.imageUrls;
                        carOrder.ProvinceName = string.Empty;
                        carOrder.CityName = string.Empty;
                        carOrder.AreaName = string.Empty;
                        carOrder.DistributionAddress = bxCarOrder.distribution_address;
                        carOrder.DistributionName = bxCarOrder.distribution_name;
                        carOrder.DistributionPhone = bxCarOrder.distribution_phone;
                        carOrder.DistributionTime = bxCarOrder.distribution_time.HasValue
                            ? bxCarOrder.distribution_time.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            : string.Empty;
                        carOrder.InsurancePrice = bxCarOrder.insurance_price.HasValue
                            ? (double)bxCarOrder.insurance_price.Value
                            : 0;
                        carOrder.CarriagePrice = bxCarOrder.carriage_price.HasValue
                            ? (double)bxCarOrder.carriage_price.Value
                            : 0;
                        carOrder.TotalPrice = bxCarOrder.total_price.HasValue
                            ? (double)bxCarOrder.total_price.Value
                            : 0;
                        carOrder.Receipt = bxCarOrder.receipt_title;
                        carOrder.ReceiptHead = bxCarOrder.receipt_head.HasValue ? bxCarOrder.receipt_head.Value : 0;
                        carOrder.PayType = bxCarOrder.pay_type.HasValue ? bxCarOrder.pay_type.Value : 0;
                        carOrder.DistributionType = bxCarOrder.distribution_type.HasValue
                            ? bxCarOrder.distribution_type.Value
                            : 0;
                    }

                    #endregion

                    #region UserInfo转换

                    if (bxUserInfo != null)
                    {
                        userInfo.LicenseNo = bxUserInfo.LicenseNo;
                        userInfo.LicenseOwner = bxUserInfo.LicenseOwner;
                        userInfo.IdType = bxUserInfo.OwnerIdCardType; //默认空时，赋值个“其他”
                        userInfo.InsuredName = bxUserInfo.InsuredName;
                        userInfo.InsuredMobile = bxUserInfo.InsuredMobile;
                        userInfo.InsuredAddress = bxUserInfo.InsuredAddress;
                        userInfo.InsuredIdType = bxUserInfo.InsuredIdType.HasValue ? bxUserInfo.InsuredIdType.Value : 6;
                        //默认空时，赋值个“其他”
                        userInfo.CredentislasNum = bxUserInfo.InsuredIdCard;
                        userInfo.Email = bxUserInfo.Email;
                        userInfo.CityCode = !string.IsNullOrEmpty(bxUserInfo.CityCode)
                            ? int.Parse(bxUserInfo.CityCode)
                            : 0;
                        //CarUsedType如下
                        userInfo.EngineNo = bxUserInfo.EngineNo;
                        userInfo.CarVin = bxUserInfo.CarVIN;
                        //PurchasePrice如下
                        //SeatCount如下
                        userInfo.ModleName = bxUserInfo.MoldName;
                        userInfo.RegisterDate = bxUserInfo.RegisterDate;
                        //LastEndDate如下
                        //LastBusinessEndDdate如下
                        //ForceStartDate如下
                        //BizStartDate如下
                        //ClaimCount如下
                    }

                    #endregion

                    #region QuoteResultCarInfo转换

                    if (bxCarInfo != null)
                    {
                        userInfo.CarUsedType = bxCarInfo.car_used_type.HasValue ? bxCarInfo.car_used_type.Value : 0;
                        userInfo.PurchasePrice = bxCarInfo.purchase_price.HasValue ? (double)bxCarInfo.purchase_price.Value : 0;
                        userInfo.SeatCount = bxCarInfo.seat_count.HasValue ? bxCarInfo.seat_count.Value : 0;
                    }

                    #endregion

                    #region ClaimDetails转换

                    if (bxClaimDetails != null)
                    {
                        userInfo.ClaimCount = bxClaimDetails.Count;
                        var claim = new ClaimDetail();
                        var claimDetail = new bx_claim_detail();
                        for (int i = 0; i < userInfo.ClaimCount; i++)
                        {
                            claim = new ClaimDetail();
                            claimDetail = new bx_claim_detail();
                            claimDetail = bxClaimDetails[i];
                            claim.EndcaseTime = claimDetail.endcase_time.HasValue
                                ? claimDetail.endcase_time.Value.ToString()
                                : string.Empty;
                            claim.LossTime = claimDetail.loss_time.HasValue
                                ? claimDetail.loss_time.Value.ToString()
                                : string.Empty;
                            claim.PayAmount = claimDetail.pay_amount.HasValue ? claimDetail.pay_amount.Value : 0;
                            claim.PayCompanyName = claimDetail.pay_company_name;
                            claimDetails.Add(claim);
                        }
                    }

                    #endregion

                    #region SaveQuote、QuoteResult、SubmitInfo转换

                    if (bxSaveQuote != null)
                    {
                        bool isquoteresult = true;
                        if (bxQuoteResult == null)
                        {
                            isquoteresult = false;
                            bxQuoteResult = new bx_quoteresult();
                        }
                        precisePrice.BizTotal = bxQuoteResult.BizTotal.HasValue ? bxQuoteResult.BizTotal.Value : 0;
                        precisePrice.ForceTotal = bxQuoteResult.ForceTotal ?? 0;
                        precisePrice.TaxTotal = bxQuoteResult.TaxTotal ?? 0;
                        precisePrice.Source = bxQuoteResult.Source ?? 0;
                        precisePrice.Source = SourceGroupAlgorithm.GetNewSource((int)precisePrice.Source);
                        precisePrice.JiaoQiang = bxSaveQuote.JiaoQiang.HasValue ? bxSaveQuote.JiaoQiang.Value : 1;
                        if (bxSubmitInfo != null)
                        {
                            precisePrice.QuoteStatus = bxSubmitInfo.quote_status.HasValue
                                ? bxSubmitInfo.quote_status.Value
                                : 0;
                            precisePrice.QuoteResult = bxSubmitInfo.quote_result;
                            precisePrice.SubmitStatus = bxSubmitInfo.submit_status.HasValue
                                ? bxSubmitInfo.submit_status.Value
                                : 0;
                            precisePrice.SubmitResult = bxSubmitInfo.submit_result;
                            precisePrice.BizTno = bxSubmitInfo.biz_tno;
                            precisePrice.ForceTno = bxSubmitInfo.force_pno;
                            precisePrice.BizSysRate = bxSubmitInfo.biz_rate.HasValue ? (double)bxSubmitInfo.biz_rate.Value : 0;
                            precisePrice.ForceSysRate = bxSubmitInfo.force_rate.HasValue ? (double)bxSubmitInfo.force_rate.Value : 0;
                            //BenefitRate
                        }
                        precisePrice.CheSun = new XianZhongUnit
                        {
                            BaoE =
                                isquoteresult
                                    ? (bxQuoteResult.CheSunBE.HasValue ? bxQuoteResult.CheSunBE.Value : 0)
                                    : (bxSaveQuote.CheSun.HasValue ? bxSaveQuote.CheSun.Value : 0),
                            BaoFei = bxQuoteResult.CheSun.HasValue ? bxQuoteResult.CheSun.Value : 0
                        };
                        precisePrice.SanZhe = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.SanZhe.HasValue ? bxSaveQuote.SanZhe.Value : 0,
                            BaoFei = bxQuoteResult.SanZhe.HasValue ? bxQuoteResult.SanZhe.Value : 0
                        };
                        precisePrice.DaoQiang = new XianZhongUnit
                        {
                            BaoE =
                                isquoteresult
                                    ? (bxQuoteResult.DaoQiangBE.HasValue ? bxQuoteResult.DaoQiangBE.Value : 0)
                                    : (bxSaveQuote.DaoQiang.HasValue ? bxSaveQuote.DaoQiang.Value : 0),
                            BaoFei = bxQuoteResult.DaoQiang.HasValue ? bxQuoteResult.DaoQiang.Value : 0
                        };
                        precisePrice.SiJi = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.SiJi.HasValue ? bxSaveQuote.SiJi.Value : 0,
                            BaoFei = bxQuoteResult.SiJi.HasValue ? bxQuoteResult.SiJi.Value : 0
                        };
                        precisePrice.ChengKe = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.ChengKe.HasValue ? bxSaveQuote.ChengKe.Value : 0,
                            BaoFei = bxQuoteResult.ChengKe.HasValue ? bxQuoteResult.ChengKe.Value : 0
                        };
                        precisePrice.BoLi = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.BoLi.HasValue ? bxSaveQuote.BoLi.Value : 0,
                            BaoFei = bxQuoteResult.BoLi.HasValue ? bxQuoteResult.BoLi.Value : 0
                        };
                        precisePrice.HuaHen = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.HuaHen.HasValue ? bxSaveQuote.HuaHen.Value : 0,
                            BaoFei = bxQuoteResult.HuaHen.HasValue ? bxQuoteResult.HuaHen.Value : 0
                        };
                        precisePrice.BuJiMianCheSun = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.BuJiMianCheSun.HasValue ? bxSaveQuote.BuJiMianCheSun.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianCheSun.HasValue ? bxQuoteResult.BuJiMianCheSun.Value : 0
                        };
                        precisePrice.BuJiMianSanZhe = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.BuJiMianSanZhe.HasValue ? bxSaveQuote.BuJiMianSanZhe.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianSanZhe.HasValue ? bxQuoteResult.BuJiMianSanZhe.Value : 0
                        };
                        precisePrice.BuJiMianDaoQiang = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.BuJiMianDaoQiang.HasValue ? bxSaveQuote.BuJiMianDaoQiang.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianDaoQiang.HasValue ? bxQuoteResult.BuJiMianDaoQiang.Value : 0
                        };
                        precisePrice.BuJiMianChengKe = new XianZhongUnit()
                        {
                            BaoE = bxSaveQuote.BuJiMianChengKe.HasValue ? bxSaveQuote.BuJiMianChengKe.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianChengKe.HasValue ? bxQuoteResult.BuJiMianChengKe.Value : 0
                        };
                        precisePrice.BuJiMianSiJi = new XianZhongUnit()
                        {
                            BaoE = bxSaveQuote.BuJiMianSiJi.HasValue ? bxSaveQuote.BuJiMianSiJi.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianSiJi.HasValue ? bxQuoteResult.BuJiMianSiJi.Value : 0
                        };
                        precisePrice.BuJiMianHuaHen = new XianZhongUnit()
                        {
                            BaoE = bxSaveQuote.BuJiMianHuaHen.HasValue ? bxSaveQuote.BuJiMianHuaHen.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianHuaHen.HasValue ? bxQuoteResult.BuJiMianHuaHen.Value : 0
                        };
                        precisePrice.BuJiMianSheShui = new XianZhongUnit()
                        {
                            BaoE = bxSaveQuote.BuJiMianSheShui.HasValue ? bxSaveQuote.BuJiMianSheShui.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianSheShui.HasValue ? bxQuoteResult.BuJiMianSheShui.Value : 0
                        };
                        precisePrice.BuJiMianZiRan = new XianZhongUnit()
                        {
                            BaoE = bxSaveQuote.BuJiMianZiRan.HasValue ? bxSaveQuote.BuJiMianZiRan.Value : 0,
                            BaoFei = bxQuoteResult.BuJiMianZiRan.HasValue ? bxQuoteResult.BuJiMianZiRan.Value : 0
                        };
                        precisePrice.BuJiMianJingShenSunShi = new XianZhongUnit()
                        {
                            BaoE =
                                bxSaveQuote.BuJiMianJingShenSunShi.HasValue
                                    ? bxSaveQuote.BuJiMianJingShenSunShi.Value
                                    : 0,
                            BaoFei =
                                bxQuoteResult.BuJiMianJingShenSunShi.HasValue
                                    ? bxQuoteResult.BuJiMianJingShenSunShi.Value
                                    : 0
                        };
                        precisePrice.SheShui = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.SheShui.HasValue ? bxSaveQuote.SheShui.Value : 0,
                            BaoFei = bxQuoteResult.SheShui.HasValue ? bxQuoteResult.SheShui.Value : 0
                        };
                        precisePrice.ZiRan = new XianZhongUnit
                        {
                            BaoE =
                                isquoteresult
                                    ? (bxQuoteResult.ZiRanBE.HasValue ? bxQuoteResult.ZiRanBE.Value : 0)
                                    : (bxSaveQuote.ZiRan.HasValue ? bxSaveQuote.ZiRan.Value : 0),
                            BaoFei = bxQuoteResult.ZiRan.HasValue ? bxQuoteResult.ZiRan.Value : 0
                        };
                        precisePrice.HcSheBeiSunshi = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.HcSheBeiSunshi.HasValue ? bxSaveQuote.HcSheBeiSunshi.Value : 0,
                            BaoFei = bxQuoteResult.HcSheBeiSunshi.HasValue ? bxQuoteResult.HcSheBeiSunshi.Value : 0
                        };
                        precisePrice.HcHuoWuZeRen = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.HcHuoWuZeRen.HasValue ? bxSaveQuote.HcHuoWuZeRen.Value : 0,
                            BaoFei = bxQuoteResult.HcHuoWuZeRen.HasValue ? bxQuoteResult.HcHuoWuZeRen.Value : 0
                        };
                        precisePrice.HcFeiYongBuChang = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.HcFeiYongBuChang.HasValue ? bxSaveQuote.HcFeiYongBuChang.Value : 0,
                            BaoFei = bxQuoteResult.HcFeiYongBuChang.HasValue ? bxQuoteResult.HcFeiYongBuChang.Value : 0
                        };
                        precisePrice.HcJingShenSunShi = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.HcJingShenSunShi.HasValue ? bxSaveQuote.HcJingShenSunShi.Value : 0,
                            BaoFei = bxQuoteResult.HcJingShenSunShi.HasValue ? bxQuoteResult.HcJingShenSunShi.Value : 0
                        };
                        precisePrice.HcSanFangTeYue = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.HcSanFangTeYue.HasValue ? bxSaveQuote.HcSanFangTeYue.Value : 0,
                            BaoFei = bxQuoteResult.HcSanFangTeYue.HasValue ? bxQuoteResult.HcSanFangTeYue.Value : 0
                        };
                        precisePrice.HcXiuLiChang = new XianZhongUnit
                        {
                            BaoE = bxSaveQuote.HcXiuLiChang.HasValue ? bxSaveQuote.HcXiuLiChang.Value : 0,
                            BaoFei = bxQuoteResult.HcXiuLiChang.HasValue ? bxQuoteResult.HcXiuLiChang.Value : 0
                        };
                        precisePrice.HcXiuLiChangType = bxSaveQuote.HcXiuLiChangType.HasValue
                            ? bxSaveQuote.HcXiuLiChangType.Value.ToString()
                            : "-1";
                        precisePrice.RateFactor1 = bxQuoteResult.RateFactor1.HasValue
                            ? (double)bxQuoteResult.RateFactor1.Value
                            : 0;
                        precisePrice.RateFactor2 = bxQuoteResult.RateFactor2.HasValue
                            ? (double)bxQuoteResult.RateFactor2.Value
                            : 0;
                        precisePrice.RateFactor3 = bxQuoteResult.RateFactor3.HasValue
                            ? (double)bxQuoteResult.RateFactor3.Value
                            : 0;
                        precisePrice.RateFactor4 = bxQuoteResult.RateFactor4.HasValue
                            ? (double)bxQuoteResult.RateFactor4.Value
                            : 0;
                    }

                    #endregion

                    #region LastInfo转换

                    if (bxLastInfo != null)
                    {
                        userInfo.LastEndDate = bxLastInfo.last_end_date;
                        userInfo.LastBusinessEndDdate = bxLastInfo.last_business_end_date;
                    }

                    #endregion

                    #region 保险起始时间

                    if (qrStartDate != null)
                    {
                        userInfo.ForceStartDate = qrStartDate.ForceStartDate.HasValue
                            ? qrStartDate.ForceStartDate.Value.ToString()
                            : string.Empty;
                        userInfo.BizStartDate = qrStartDate.BizStartDate.HasValue
                            ? qrStartDate.BizStartDate.Value.ToString()
                            : string.Empty;
                    }

                    #endregion

                    response.Status = HttpStatusCode.OK;
                    response.CarOrder = carOrder;
                    response.ClaimDetail = claimDetails;
                    response.PrecisePrice = precisePrice;
                    response.UserInfo = userInfo;
                }
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                _logError.Info("获取订单异常，创建订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }
            return response;
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="topAgentId"></param>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="request"></param>
        /// <param name="status"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CarOrderModel> GetOrders(GetOrdersRequest request, int status, out int totalCount)
        {
            bool isAgent = false;
            var sonself = new List<bx_agent>();
            //当前根据openid获取当前经纪人 
            //var curAgent = _agentRepository.GetAgentByTopParentAgent(request.OpenId, request.TopAgentId);
            var curAgent = _agentRepository.GetAgent(request.ChildAgent);
            if (curAgent != null)
            {//代理人
                if (request.IsOnlyMine.HasValue)
                {
                    if (request.IsOnlyMine.Value == 0)
                    {
                        sonself = _agentRepository.GetSonsAgent(curAgent.Id).ToList();
                    }
                }
                isAgent = true;
                sonself.Add(curAgent);
            }

            return _orderRepository.FindListBy(status, isAgent, sonself, request.OpenId, request.TopAgentId, request.Search, request.PageIndex, request.PageSize, out totalCount);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="openId"></param>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public CarOrderModel FindCarOrderBy(GetOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)//, string openId
        {
            return _orderRepository.FindCarOrderBy(request.OrderId, request.Agent);//, openId
        }

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public GetOrderDetailResponse GetOrderDetail(GetOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)//, string openId
        {
            GetOrderDetailResponse response = new GetOrderDetailResponse();
            IBxAgent agentModel = GetCommonAgentModelFactory(request.Agent, _agentRepository);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //请求获取订单
            var carorder = _orderRepository.FindBy(request.OrderId, request.Agent);
            if (carorder == null)
            {
                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            else
            {
                int topagent = carorder.top_agent.HasValue ? carorder.top_agent.Value : 0;
                if (topagent != request.Agent)
                {
                    response.Status = HttpStatusCode.NoContent;
                    return response;
                }
                else
                {
                    response.Status = HttpStatusCode.OK;
                    response.CarOrder = carorder;
                }
            }
            return response;
        }


        /// <summary>
        ///     生成订单号
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <param name="sorcePlatform">平台</param>
        /// <returns>订单号</returns>
        private string GenerateOrderNum(long id, int fountain)//EnumOrderPlatform sorcePlatform
        {
            var sb = new StringBuilder();
            sb.Append(fountain);//sb.Append((int)sorcePlatform);
            sb.Append(DateTime.Now.ConvertToTimeStmap().ToString());
            sb.Append((100000 + id).ToString());
            return sb.ToString();
        }

        #region crm时间轴接口调用
        /// <summary>
        /// CRM时间线步骤记录请求串
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="orderId"></param>
        /// <param name="buid"></param>
        /// <param name="source"></param>
        public void PostCrmStep(double insurancePrice, int agentId, long orderId, long buid, long source)
        {

            string _url = ConfigurationManager.AppSettings["SystemCrmUrl"];
            _url = string.Format("{0}/api/ConsumerDetail/AddCrmSteps", _url);
            var obj = new JsonContain()
            {
                source = source,
                jingfei = insurancePrice,
                isEdit = "true",
                orderId = orderId,
                buid = buid
            };
            var datas = new JsonPost()
            {
                JsonContent = obj.ToJson(),
                AgentId = agentId,
                BUid = buid,
                Type = 3
            };
            _logInfo.Info(string.Format("CRM时间线步骤记录请求串: url:{0}; data:{1}", _url, datas.ToJson()));
            string result = HttpWebAsk.HttpClientPostAsync(datas.ToJson(), _url);
            _logInfo.Info(string.Format("CRM时间线步骤记录返回值:{0}", result));
        }

        public class JsonContain
        {//{"source":0,"jinfei":102,"isEdit":"true","orderId":1404,"buid":600962}
            public long source { get; set; }
            public double jingfei { get; set; }//净费
            public string isEdit { get; set; }
            public long orderId { get; set; }
            public long buid { get; set; }
        }

        public class JsonPost
        {
            public string JsonContent { get; set; }
            public int AgentId { get; set; }
            public int Type { get; set; }
            public long BUid { get; set; }
        }
        #endregion

        #region 预约单给第三方传数据
        /// <summary>
        /// 给丁丁车险回传数据
        /// </summary>
        /// <returns></returns>
        private bool PostDataDingDing(OrderCacheResponse response, int agent, string backSuccessKeyWord)
        {
            string strUrl = "http://www.dingdingcx.com/index.php?g=Api&m=order&a=orderInfo";
            return PostData(strUrl, 0, agent, response, backSuccessKeyWord);
        }
        /// <summary>
        /// 给第三方公司回传数据
        /// </summary>
        /// <param name="response"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        private bool PostDataThirdPart(OrderCacheResponse response, int childagent, int agent, string backSuccessKeyWord)
        {
            string strUrl = string.Empty;
            strUrl = _getConfigValueService.GetConfigValue(agent.ToString(), 3, "carorder_url_" + agent);
            if (string.IsNullOrEmpty(strUrl))
            {
                return false;
            }
            return PostData(strUrl, childagent, agent, response, backSuccessKeyWord);
        }
        /// <summary>
        /// 给第三方传数据通用方法
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="agent"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool PostData(string strUrl, int childagent, int agent, OrderCacheResponse response, string backSuccessKeyWord)
        {
            try
            {
                var WebClientObj = new WebClient();
                OrderDetailResponse orderDetail = response.ConverToViewModel();
                var PostVars = new System.Collections.Specialized.NameValueCollection();
                //{"Agent":4877,"AgentName":"丁丁","AgentMobile":"15478462479","OpenId":"oPb0Jj-JEhJfbQRYLXvDPexoO8PU","Mobile":"18600475651","ContactsName":"光鹏洁","OrderStatus":1,"GetOrderTime":"","CreateTime":"2017/2/14 11:32:39","OrderId":1520,"Buid":654912,"Source":1,"InsuredName":"徐连栋","IdType":1,"IdNum":"371121198108261519","ProvinceName":"","CityName":"","AreaName":"","DistributionTime":"","InsurancePrice":3064.81,"CarriagePrice":0,"TotalPrice":3064.81,"Receipt":"徐连栋","ReceiptHead":0,"PayType":0,"DistributionType":0}
                PostVars.Add("CarOrder", orderDetail.CarOrder.ToJson());
                //{"LicenseNo":"京QGY237","LicenseOwner":"徐连栋","IdType":1,"InsuredName":"徐连栋","InsuredMobile":"13121596391","InsuredIdType":1,"CredentislasNum":"371121198108261519","Email":"guabgxf@sina.com","CityCode":1,"CarUsedType":1,"EngineNo":"894770","CarVin":"LFV2A21K4D4014282","PurchasePrice":125800,"SeatCount":5,"ModleName":"大众FV7166FAAWG轿车","RegisterDate":"2013-02-26","LastEndDate":"2017-02-25","LastBusinessEndDdate":"2018-02-25","ForceStartDate":"2017/2/26 0:00:00","BizStartDate":"2017/2/26 0:00:00","ClaimCount":0}
                PostVars.Add("UserInfo", orderDetail.UserInfo.ToJson());
                //{"BizTotal":2138.95,"ForceTotal":575.86,"TaxTotal":350,"Source":1,"JiaoQiang":1,"RateFactor1":0.7,"RateFactor2":0.85,"RateFactor3":0.85,"RateFactor4":1,"BizTno":"ABEJ050Y1417A006828L","BizSysRate":0,"ForceSysRate":0,"SubmitStatus":0,"SubmitResult":"已暂存未核保","QuoteStatus":1,"QuoteResult":"成功","CheSun":{"BaoE":89570,"BaoFei":1042.16},"SanZhe":{"BaoE":500000,"BaoFei":744.46},"DaoQiang":{"BaoE":0,"BaoFei":0},"SiJi":{"BaoE":10000,"BaoFei":20.74},"ChengKe":{"BaoE":10000,"BaoFei":52.6},"BoLi":{"BaoE":0,"BaoFei":0},"HuaHen":{"BaoE":0,"BaoFei":0},"SheShui":{"BaoE":0,"BaoFei":0},"ZiRan":{"BaoE":0,"BaoFei":0},"BuJiMianCheSun":{"BaoE":1,"BaoFei":156.32},"BuJiMianSanZhe":{"BaoE":1,"BaoFei":111.67},"BuJiMianDaoQiang":{"BaoE":0,"BaoFei":0},"BuJiMianChengKe":{"BaoE":1,"BaoFei":7.89},"BuJiMianSiJi":{"BaoE":1,"BaoFei":3.11},"BuJiMianHuaHen":{"BaoE":0,"BaoFei":0},"HcXiuLiChangType":"-1","BuJiMianSheShui":{"BaoE":0,"BaoFei":0},"BuJiMianZiRan":{"BaoE":0,"BaoFei":0},"BuJiMianJingShenSunShi":{"BaoE":0,"BaoFei":0},"HcSheBeiSunshi":{"BaoE":0,"BaoFei":0},"HcHuoWuZeRen":{"BaoE":0,"BaoFei":0},"HcFeiYongBuChang":{"BaoE":0,"BaoFei":0},"HcJingShenSunShi":{"BaoE":0,"BaoFei":0},"HcSanFangTeYue":{"BaoE":0,"BaoFei":0},"HcXiuLiChang":{"BaoE":0,"BaoFei":0}}
                PostVars.Add("PrecisePrice", orderDetail.PrecisePrice.ToJson());
                var thirdpath = GetSecCode(childagent, agent);
                //第三方用户名
                PostVars.Add("UserName", thirdpath.Item1);
                //校验
                PostVars.Add("SecCode", thirdpath.Item2.ToUpper());
                byte[] byRemoteInfo = WebClientObj.UploadValues(strUrl, "POST", PostVars);
                string sRemoteInfo = Encoding.UTF8.GetString(byRemoteInfo);
                //保存请求日志记录
                StringBuilder sbb = new StringBuilder();
                sbb.Append("预约单回传请求OrderDetail为：").Append(orderDetail.ToJson())
                    .Append(";UserName为：").Append(thirdpath.Item1)
                    .Append(";SecCode为：").Append(thirdpath.Item2.ToUpper())
                    .Append("；第三方接口返回消息为：").Append(sRemoteInfo);
                _logInfo.Info(sbb.ToString());
                //这是获取返回信息
                if (sRemoteInfo.Contains(backSuccessKeyWord))
                    return true;
            }
            catch (Exception ex)
            {
                _logError.Info("创建订单时调用第三方代理人：" + agent + "的接口异常，信息为：" + "" + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 返回第三方用户名和校验串
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private Tuple<string, string> GetSecCode(int childagent, int agent)
        {
            IBxAgent agentModel = GetAgentModelFactory(agent);//顶级
            IBxAgent childAgentModel = GetAgentModelFactory(childagent);//子集
            if (agentModel.Id > 0)
            {
                string username = !string.IsNullOrEmpty(childAgentModel.AgentName) ? childAgentModel.AgentName : agentModel.AgentName;//以前是agentaccount，因为联合登陆多出来一个代理人id，现改直接取agentname即可。
                string secretkey = agentModel.SecretKey;
                StringBuilder sb = new StringBuilder();
                sb.Append("Agent=").Append(agent).Append("&")
                .Append("UserName=").Append(username).Append("&")
                .Append("SecretKey=").Append(secretkey);
                return Tuple.Create<string, string>(username, sb.ToString().GetMd5());
            }
            return Tuple.Create<string, string>(null, null);
        }
        #endregion

        #region 未使用的方法

        public CarOrderModel GetOrderByBuid(GetOrderByBuidRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            return _orderRepository.GetOrderByBuid(request.Buid, request.TopAgentId);
        }
        public bx_car_order GetOrder(GetOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            return _orderRepository.FindBy(request.OrderId, request.OpenId);
        }

        public async Task<CreateOrderResponse> Create(CreateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            CreateOrderResponse response = new CreateOrderResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);

            //登录信息
            //var account = await _loginService.LoginAccount(agentModel.Mobile);
            //if (account == null)
            //{
            //    response.Status = HttpStatusCode.BadRequest;
            //    return response;
            //}
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            bx_car_order order = new bx_car_order
            {
                buid = request.Buid,
                source = SourceGroupAlgorithm.GetOldSource(request.Source),
                carriage_price = (decimal?)request.CarriagePrice,
                create_time = DateTime.Now,
                distribution_type = request.DistributionType,
                id_num = request.IdNum,
                id_type = request.IdType,
                insurance_price = (decimal?)request.InsurancePrice,
                insured_name = request.InsuredName,
                //order_num = request.OrderNum,
                pay_type = request.PayType,
                receipt_title = request.Receipt,
                total_price = (decimal?)request.TotalPrice,
                user_id = request.UserId,//
                order_status = 1,
                pay_status = 1,
                distribution_address = request.DistributionAddress,
                id_img_firs = request.IdImgFirst,
                id_img_secd = request.IdImgSecond,
                top_agent = request.Topagent
            };
            try
            {
                var result = _orderRepository.Add(order);
                if (result > 0)
                {
                    //生成订单号
                    var orderNum = GenerateOrderNum(result, request.Fountain);//EnumOrderPlatform.WeChat
                    order.order_num = orderNum;
                    _orderRepository.Update(order);

                    response.Status = HttpStatusCode.OK;
                    response.OrderId = result;
                    _logInfo.Info("创建订单成功,订单信息:" + request.ToJson());
                }
                else
                {
                    response.Status = HttpStatusCode.Forbidden;
                    response.OrderId = 0;
                    _logError.Info("创建订单失败，订单信息：" + request.ToJson());
                }
            }
            catch (Exception ex)
            {
                _logError.Info("创建订单异常，订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }

            return response;
        }

        public async Task<UpdateOrderResponse> Update(UpdateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            UpdateOrderResponse response = new UpdateOrderResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.AgentId);

            //登录信息
            //var account = await _loginService.LoginAccount(agentModel.Mobile);
            //if (account == null)
            //{
            //    response.Status = HttpStatusCode.BadRequest;
            //    return response;
            //}
            //参数校验

            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            try
            {
                var orderItem = _orderRepository.FindBy(request.OrderId);
                if (orderItem != null)
                {
                    orderItem.order_status = request.OrderStatus;
                    orderItem.pay_status = request.PayStatus;
                    _orderRepository.Update(orderItem);
                    response.Count = orderItem.id;
                }
                else
                {
                    response.Count = 0;
                }
            }
            catch (Exception ex)
            {

                _logError.Info("更新订单异常，更新订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }

            return response;
        }

        public async Task<CreateOrderResponse> CreateSelf(CreateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            CreateOrderResponse response = new CreateOrderResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);

            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            //登录信息
            //var account = await _loginService.LoginAccount(agentModel.Mobile);
            //if (account == null)
            //{
            //    response.Status = HttpStatusCode.BadRequest;
            //    return response;
            //}
            //根据openid，mobile，获取userid
            var user = _userService.AddUser(request.OpenId, request.Mobile);

            bx_car_order order = new bx_car_order
            {
                buid = request.Buid,
                source = SourceGroupAlgorithm.GetOldSource(request.Source),
                carriage_price = (decimal?)request.CarriagePrice,
                create_time = DateTime.Now,
                distribution_type = request.DistributionType,
                id_num = request.IdNum,
                id_type = request.IdType,
                insurance_price = (decimal?)request.InsurancePrice,
                insured_name = request.InsuredName,
                //order_num = request.OrderNum,
                pay_type = request.PayType,
                receipt_title = request.Receipt,
                total_price = (decimal?)request.TotalPrice,
                user_id = user.UserId,//
                order_status = 1,
                pay_status = 0,
                distribution_address = request.DistributionAddress,
                id_img_firs = request.IdImgFirst,
                id_img_secd = request.IdImgSecond,
                top_agent = request.Topagent,
                cur_agent = request.Agent,
                openid = request.OpenId,
                mobile = request.Mobile,
                addressid = request.AddressId.HasValue ? request.AddressId.Value : 0,
                contacts_name = request.ContactsName
            };
            try
            {
                var result = _orderRepository.Add(order);
                if (result > 0)
                {
                    //生成订单号
                    var orderNum = GenerateOrderNum(result, request.Fountain);//EnumOrderPlatform.WeChat
                    order.order_num = orderNum;
                    _orderRepository.Update(order);

                    response.Status = HttpStatusCode.OK;
                    response.OrderId = result;
                    _logInfo.Info("创建订单成功,订单信息:" + request.ToJson());
                }
                else
                {
                    response.Status = HttpStatusCode.Forbidden;
                    response.OrderId = 0;
                    _logError.Info("创建订单失败，订单信息：" + request.ToJson());
                }
            }
            catch (Exception ex)
            {
                _logError.Info("创建订单异常，订单信息：" + request.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }

            return response;
        }

        public UpdateOrderResponse UpdateImg(long orderId, string openId, string idImgFirs, string idImgSecd)
        {
            UpdateOrderResponse response = new UpdateOrderResponse();

            try
            {
                bx_car_order orderItem = _orderRepository.FindBy(orderId, openId);
                if (orderItem != null)
                {
                    orderItem.id_img_firs = idImgFirs;
                    orderItem.id_img_secd = idImgSecd;
                    _orderRepository.Update(orderItem);
                    response.Count = orderItem.id;
                }
                else
                {
                    response.Count = 0;
                }
            }
            catch (Exception ex)
            {

                _logError.Info("上传身份证异常，更新订单信息：OrderID" + orderId + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }
            return response;

        }

        #endregion

    }
}
