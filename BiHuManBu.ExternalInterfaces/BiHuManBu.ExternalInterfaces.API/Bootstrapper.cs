using Autofac;
using Autofac.Integration.WebApi;
using BiHuManBu.ExternalInterfaces.API.Controllers;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Repository.DbOper;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.CacheServices;
using BiHuManBu.ExternalInterfaces.Services.ChannelService;
using BiHuManBu.ExternalInterfaces.Services.CommonService.implementations;
using BiHuManBu.ExternalInterfaces.Services.CommonService.interfaces;
using BiHuManBu.ExternalInterfaces.Services.ConfigService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.ConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Distribute.Implements;
using BiHuManBu.ExternalInterfaces.Services.Distribute.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.DistributeService.Implements;
using BiHuManBu.ExternalInterfaces.Services.DistributeService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetCenterValueService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.GetCenterValueService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetPrecisePriceService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.GetPrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetSubmitInfoService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.GetSubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.IndependentService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.IndependentService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.QuotedChannelsService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.QuotedChannelsService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.UploadImgService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

namespace BiHuManBu.ExternalInterfaces.API
{
    public static class Bootstrapper
    {
        public static void Run(HttpConfiguration configuration)
        {

            SetAutofacWebAPIServices(configuration);
        }
        private static void SetAutofacWebAPIServices(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            #region reposities
            builder.RegisterType<EntityContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<AgentRepository>().As<IAgentRepository>();
            builder.RegisterType<LastInfoRepository>().As<ILastInfoRepository>();
            builder.RegisterType<QuoteResultRepository>().As<IQuoteResultRepository>();
            builder.RegisterType<SaveQuoteRepository>().As<ISaveQuoteRepository>();
            builder.RegisterType<SubmitInfoRepository>().As<ISubmitInfoRepository>();
            builder.RegisterType<UserClaimRepository>().As<IUserClaimRepository>();
            builder.RegisterType<UserInfoRepository>().As<IUserInfoRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
            builder.RegisterType<CarOrderQuoteResultRepository>().As<ICarOrderQuoteResultRepository>();
            builder.RegisterType<CarOrderSaveQuoteRepository>().As<ICarOrderSaveQuoteRepository>();
            builder.RegisterType<CarOrderSubmitInfoRepository>().As<ICarOrderSubmitInfoRepository>();
            builder.RegisterType<CarOrderUserInfoRepository>().As<ICarOrderUserInfoRepository>();
            builder.RegisterType<CarInfoRepository>().As<ICarInfoRepository>();
            builder.RegisterType<RenewalQuoteRepository>().As<IRenewalQuoteRepository>();
            builder.RegisterType<GscRepostory>().As<IGscRepository>();
            builder.RegisterType<CarRenewalRepository>().As<ICarRenewalRepository>();
            builder.RegisterType<ChargeRepository>().As<IChargeRepository>();
            builder.RegisterType<ChargeHistoryRepository>().As<IChargeHistoryRepository>();
            builder.RegisterType<CityRepository>().As<ICityRepository>();
            builder.RegisterType<AgentRateRepository>().As<IAgentRateRepository>();
            builder.RegisterType<AgentSpecialRateRepository>().As<IAgentSpecialRateRepository>();
            builder.RegisterType<ImagesRepository>().As<IImagesRepository>();
            builder.RegisterType<AreaRepository>().As<IAreaRepository>();
            builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            builder.RegisterType<AgentConfigRepository>().As<IAgentConfigRepository>();
            builder.RegisterType<AgentPointRepository>().As<IAgentPointRepository>();
            builder.RegisterType<HebaoDianweiRepository>().As<IHebaoDianweiRepository>();
            builder.RegisterType<BxBjUnionRepository>().As<IBxBjUnionRepository>();
            builder.RegisterType<BaodanXianZhongRepository>().As<IBaodanXianZhongRepository>();
            builder.RegisterType<BaodanxinxiRepository>().As<IBaodanxinxiRepository>();
            builder.RegisterType<DeviceDetailRepository>().As<IDeviceDetailRepository>();
            builder.RegisterType<QuoteReqCarinfoRepository>().As<IQuoteReqCarinfoRepository>();
            builder.RegisterType<QuoteResultCarinfoRepository>().As<IQuoteResultCarinfoRepository>();
            builder.RegisterType<SmsContentRepository>().As<ISmsContentRepository>();
            builder.RegisterType<SmsOrderRepository>().As<ISmsOrderRepository>();
            builder.RegisterType<UserinfoRenewalInfoRepository>().As<IUserinfoRenewalInfoRepository>();
            builder.RegisterType<ConsumerReviewRepository>().As<IConsumerReviewRepository>();
            builder.RegisterType<AgentDistributedRepository>().As<IAgentDistributedRepository>();
            builder.RegisterType<TransferRecordRepository>().As<ITransferRecordRepository>();
            builder.RegisterType<NoticexbRepository>().As<INoticexbRepository>();
            builder.RegisterType<ReportClaimRepository>().As<IReportClaimRepository>();
            builder.RegisterType<HistoryContractRepository>().As<IHistoryClaimRepository>();
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<ParaBhTypeRepository>().As<IParaBhTypeRepository>();
            builder.RegisterType<AppSettingRepository>().As<IAppSettingRepository>();
            builder.RegisterType<PreferentialActivityRepository>().As<IPreferentialActivityRepository>();
            builder.RegisterType<GscRenewalRepository>().As<IGscRenewalRepository>();
            builder.RegisterType<CityQuoteDayRepository>().As<ICityQuoteDayRepository>();
            builder.RegisterType<DriverLicenseTypeRepository>().As<IDriverLicenseTypeRepository>();
            builder.RegisterType<CarModelRepository>().As<ICarModelRepository>();

            builder.RegisterType<AgentUKeyRepository>().As<IAgentUKeyRepository>();

            builder.RegisterType<PictureRepository>().As<IPictureRepository>();
            builder.RegisterType<ConfigRepository>().As<IConfigRepository>();
            builder.RegisterType<Repository<bx_lastinfo>>().As<IRepository<bx_lastinfo>>();
            builder.RegisterType<Repository<bx_submit_info>>().As<IRepository<bx_submit_info>>();
            builder.RegisterType<Repository<bx_userinfo>>().As<IRepository<bx_userinfo>>();
            builder.RegisterType<Repository<bx_agent_ukey>>().As<IRepository<bx_agent_ukey>>();

            builder.RegisterType<AgentSelectRepository>().As<IAgentSelectRepository>();
            builder.RegisterType<QuoteManySourceRepository>().As<IQuoteManySourceRepository>();
            builder.RegisterType<Repository<bx_anxin_delivery>>().As<IRepository<bx_anxin_delivery>>();
            builder.RegisterType<RenewalStatusRepository>().As<IRenewalStatusRepository>();

            builder.RegisterType<SpecialOptionRepository>().As<ISpecialOptionRepository>();
            builder.RegisterType<QuoteHistoryRepository>().As<IQuoteHistoryRepository>();
            builder.RegisterType<BatchRenewalRepository>().As<IBatchRenewalRepository>();

            builder.RegisterType<CarRecordRepository>().As<ICarRecordRepository>();
            builder.RegisterType<CameraDistributeRepository>().As<ICameraDistributeRepository>();
            builder.RegisterType<ManagerRoleRepository>().As<IManagerRoleRepository>();
            builder.RegisterType<CameraConfigRepository>().As<ICameraConfigRepository>();
            builder.RegisterType<UserinfoExpandRepository>().As<IUserinfoExpandRepository>();
            builder.RegisterType<ReviewDetailRecordReponsitory>().As<IReviewDetailRecordReponsitory>();

            builder.RegisterType<QuotehistoryRelatedRepository>().As<IQuotehistoryRelatedRepository>();
            builder.RegisterType<YwxdetailRepository>().As<IYwxdetailRepository>();
            builder.RegisterType<CustomerCategoriesRepository>().As<ICustomerCategoriesRepository>();
            #endregion

            #region controllers

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            #endregion

            #region Service

            #region 大方法里的service 未归类
            builder.RegisterType<AgentService>().As<IAgentService>();
            builder.RegisterType<CarInsuranceService>().As<ICarInsuranceService>();
            builder.RegisterType<LoginService>().As<ILoginService>();
            builder.RegisterType<UserClaimService>().As<IUserClaimService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<GscService>().As<IGscService>();
            builder.RegisterType<ChargeService>().As<IChargeService>();
            builder.RegisterType<CityService>().As<ICityService>();
            builder.RegisterType<ImagesService>().As<IImagesService>();
            builder.RegisterType<AreaService>().As<IAreaService>();
            builder.RegisterType<AddressService>().As<IAddressService>();
            builder.RegisterType<AgentPointService>().As<IAgentPointService>();
            builder.RegisterType<BjdService>().As<IBjdService>();
            builder.RegisterType<SmsService>().As<ISmsService>();
            builder.RegisterType<WorkOrderService>().As<IWorkOrderService>();
            builder.RegisterType<NoticexbService>().As<INoticexbService>();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<EnumService>().As<IEnumService>();
            builder.RegisterType<AppSettingService>().As<IAppSettingService>();
            builder.RegisterType<ChargingSystemService>().As<IChargingSystemService>();
            builder.RegisterType<AppAchieveService>().As<IAppAchieveService>();
            builder.RegisterType<AppVerifyService>().As<IAppVerifyService>();
            builder.RegisterType<DriverLicenseTypeService>().As<IDriverLicenseTypeService>();
            builder.RegisterType<CacheManagerService>().As<ICacheManagerService>();
            builder.RegisterType<QuoteSpecialService>().As<IQuoteSpecialService>();

            builder.RegisterType<CreateBjdInfoService>().As<ICreateBjdInfoService>();
            builder.RegisterType<CreateActivity>().As<ICreateActivity>();
            builder.RegisterType<MapBaoDanXinXiRecord>().As<IMapBaoDanXinXiRecord>();
            builder.RegisterType<MapBaoDanXianZhongRecord>().As<IMapBaoDanXianZhongRecord>();
            builder.RegisterType<UpdateBjdCheck>().As<IUpdateBjdCheck>();
            builder.RegisterType<GetBjdInfoService>().As<IGetBjdInfoService>();
            builder.RegisterType<MapBjdInfoRecord>().As<IMapBjdInfoRecord>();
            builder.RegisterType<GetBjdCheck>().As<IGetBjdCheck>();

            builder.RegisterType<AgentUKeyService>().As<IAgentUKeyService>();
            builder.RegisterType<PictureService>().As<IPictureService>();
            builder.RegisterType<AddCrmStepsService>().As<IAddCrmStepsService>();

            builder.RegisterType<PostSubmitInfoService>().As<IPostSubmitInfoService>();
            builder.RegisterType<PostValidate>().As<IPostValidate>();
            builder.RegisterType<RemoveHeBaoKey>().As<IRemoveHeBaoKey>();
            builder.RegisterType<CityChannelService>().As<ICityChannelService>();
            builder.RegisterType<GetAgentConfigByCitysService>().As<IGetAgentConfigByCitysService>();
            builder.RegisterType<RenewalStatusService>().As<IRenewalStatusService>();

            builder.RegisterType<GetMyBjdDetailService>().As<IGetMyBjdDetailService>();
            builder.RegisterType<SetPrecisePriceItem>().As<ISetPrecisePriceItem>();
            builder.RegisterType<CheckRequestService>().As<ICheckRequestService>();
            builder.RegisterType<GetDateService>().As<IGetDateService>();
            builder.RegisterType<SetActivitiesService>().As<ISetActivitiesService>();
            builder.RegisterType<SetAgentService>().As<ISetAgentService>();
            builder.RegisterType<SetBaseInfoService>().As<ISetBaseInfoService>();
            builder.RegisterType<SetCarInfoService>().As<ISetCarInfoService>();
            builder.RegisterType<SetClaimsService>().As<ISetClaimsService>();
            builder.RegisterType<SetDateService>().As<ISetDateService>();
            builder.RegisterType<SetOrderService>().As<ISetOrderService>();
            builder.RegisterType<SetPrecisePriceItemService>().As<ISetPrecisePriceItemService>();
            builder.RegisterType<SetQuoteReqService>().As<ISetQuoteReqService>();

            builder.RegisterType<CacheGetExpireDate>().Named<IGetExpireDate>("defaultExpiredata");
            builder.RegisterType<DbGetExpireDate>().Named<IGetExpireDate>("dbExpireddata");

            builder.RegisterType<CacheRepeatInfoFormat>().Named<IRepeatInfoFormat>("defaultRepeatinfo");
            builder.RegisterType<DbRepeatInfoFormat>().Named<IRepeatInfoFormat>("dbRepeatinfo");

            builder.Register(
              x =>
                  new RepeatSubmitInnerService(x.Resolve<IAgentPrivilegeService>(), x.Resolve<IRequestValidService>(),
                      x.Resolve<IRepository<bx_userinfo>>(), x.ResolveNamed<IGetExpireDate>("dbExpireddata"),
                      x.ResolveNamed<IRepeatInfoFormat>("dbRepeatinfo"))).Named<IRepeatSubmitService>("dbrepeat");

            builder.Register(
                x =>
                    new RepeatSubmitService(x.Resolve<IAgentPrivilegeService>(), x.Resolve<IRequestValidService>(),
                        x.Resolve<IRepository<bx_userinfo>>(), x.ResolveNamed<IGetExpireDate>("defaultExpiredata"),
                        x.ResolveNamed<IRepeatInfoFormat>("defaultRepeatinfo"))).Named<IRepeatSubmitService>("defaultrepeat");

            builder.Register(x => new RepeatSubmitInnerController(x.ResolveNamed<IRepeatSubmitService>("dbrepeat")));
            builder.Register(x => new RepeatSubmitController(x.ResolveNamed<IRepeatSubmitService>("defaultrepeat")));

            builder.RegisterType<CityChannelService>().Named<ICityChannelService>("defaultCityChannelService");

            builder.Register(
                x =>
                    new CacheCityChannelService(x.ResolveNamed<ICityChannelService>("defaultCityChannelService"),
                        x.Resolve<ICacheService>())
                ).Named<ICityChannelService>("cacheCityChannelService");
            builder.Register(x => new ChannelController((x.ResolveNamed<ICityChannelService>("cacheCityChannelService"))));

            builder.RegisterType<GetAccidentListService>().As<IGetAccidentListService>();
            #endregion

            #region 平安上传图片
            builder.RegisterType<CheckUploadImgTimes>().As<ICheckUploadImgTimes>();
            builder.RegisterType<UpdateImgTimes>().As<IUpdateImgTimes>();
            builder.RegisterType<UploadImgValidate>().As<IUploadImgValidate>();
            builder.RegisterType<UploadMultipleImgService>().As<IUploadMultipleImgService>();
            #endregion

            #region 获取渠道
            builder.RegisterType<ChannelService>().As<IChannelService>();
            builder.RegisterType<ChannelModelMapRedisService>().As<IChannelModelMapRedisService>();
            builder.RegisterType<AgentConfigByCityService>().As<IAgentConfigByCityService>();
            builder.RegisterType<GetMultiQuotedChannelsService>().As<IGetMultiQuotedChannelsService>();
            builder.RegisterType<GetConfigValueService>().As<IGetConfigValueService>();
            builder.RegisterType<ChannelStatusByUkeyIdService>().As<IChannelStatusByUkeyIdService>();
            #endregion

            #region 续保
            builder.RegisterType<GetReInfoMainService>().As<IGetReInfoMainService>();
            builder.RegisterType<IsFalseReInfoService>().As<IIsFalseReInfoService>();
            builder.RegisterType<CheckRequestReInfo>().As<ICheckRequestReInfo>();
            builder.RegisterType<ReWriteUserInfo>().As<IReWriteUserInfo>();

            builder.RegisterType<GetCenterValueService>().As<IGetCenterValueService>();
            builder.RegisterType<GetIntelligentReInfoService>().As<IGetIntelligentReInfoService>();
            builder.RegisterType<CheckReInfoService>().As<ICheckReInfoService>();
            builder.RegisterType<CheckCarNeedDrivingInfoService>().As<ICheckCarNeedDrivingInfoService>();
            builder.RegisterType<ToCenterFromReInfoService>().As<IToCenterFromReInfoService>();
            builder.RegisterType<SentDistributedService>().As<ISentDistributedService>();
            builder.RegisterType<GetReInfoState>().As<IGetReInfoState>();
            builder.RegisterType<PackagingResponseService>().As<IPackagingResponseService>();
            builder.RegisterType<PostThirdPartService>().As<IPostThirdPartService>();
            #endregion

            #region 请求报价接口
            builder.RegisterType<CheckReqBaseInfo>().As<ICheckReqBaseInfo>();
            builder.RegisterType<CheckReqInsurance>().As<ICheckReqInsurance>();
            builder.RegisterType<CheckRequestPostPrecisePrice>().As<ICheckRequestPostPrecisePrice>();
            #region 多渠道报价
            builder.RegisterType<CheckMultiChannels>().As<ICheckMultiChannels>();
            builder.RegisterType<CheckShebei>().As<ICheckShebei>();
            builder.RegisterType<CheckXianZhong>().As<ICheckXianZhong>();
            builder.RegisterType<GetMonth>().As<IGetMonth>();
            builder.RegisterType<UpdateAgentSelect>().As<IUpdateAgentSelect>();
            builder.RegisterType<UpdateQuoteManySource>().As<IUpdateQuoteManySource>();
            builder.RegisterType<GetMultiChannels>().As<IGetMultiChannels>();
            builder.RegisterType<MultiChannelsService>().As<IMultiChannelsService>();
            #region 安心报价核保相关
            builder.RegisterType<GetFloatingInfoService>().As<IGetFloatingInfoService>();
            builder.RegisterType<GetSpecialAssumpsitService>().As<IGetSpecialAssumpsitService>();
            builder.RegisterType<PostIndependentSubmitService>().As<IPostIndependentSubmitService>();
            #endregion
            #endregion
            builder.RegisterType<UserInfoValidateService>().As<IUserInfoValidateService>();
            #region 特约
            builder.RegisterType<SpecialOptionService>().As<ISpecialOptionService>();
            #endregion
            builder.RegisterType<TempDemoShowService>().As<ITempDemoShowService>();
            #region 驾意险
            builder.RegisterType<AddJiaYiService>().As<IAddJiaYiService>();
            #endregion
            #endregion

            #region 获取报价
            builder.RegisterType<CheckQuoteValidService>().As<ICheckQuoteValidService>();
            builder.RegisterType<CheckRequestGetPrecisePrice>().As<ICheckRequestGetPrecisePrice>();
            #endregion

            #region 获取核保
            builder.RegisterType<CheckRequestGetSubmitInfo>().As<ICheckRequestGetSubmitInfo>();
            #endregion

            #region 获取车辆信息
            builder.RegisterType<GetMoldNameFromCenter>().As<IGetMoldNameFromCenter>();
            builder.RegisterType<GetNewVehicleInfoService>().As<IGetNewVehicleInfoService>();
            builder.RegisterType<GetFirstVehicleInfoService>().As<IGetFirstVehicleInfoService>();
            builder.RegisterType<GetSecondVehicleInfoService>().As<IGetSecondVehicleInfoService>();
            #endregion

            #region 列表进去获取报价详情
            //正常记录

            //历史
            builder.RegisterType<GetBjdDetailFromHistoryService>().As<IGetBjdDetailFromHistoryService>();
            builder.RegisterType<GetTempInsured>().As<IGetTempInsured>();
            builder.RegisterType<GetModelsFromQuoteHistory>().As<IGetModelsFromQuoteHistory>();
            builder.RegisterType<GetDateNewService>().As<IGetDateNewService>();
            builder.RegisterType<SetOrderNewService>().As<ISetOrderNewService>();
            builder.RegisterType<SetPrecisePriceItemNewService>().As<ISetPrecisePriceItemNewService>();
            builder.RegisterType<SetQuoteReqNewService>().As<ISetQuoteReqNewService>();
            builder.RegisterType<SetPrecisePriceItemNew>().As<ISetPrecisePriceItemNew>();
            builder.RegisterType<SetBaseInfoHistoryService>().As<ISetBaseInfoHistoryService>();
            #endregion

            #region 新车备案
            builder.RegisterType<RecordNewCarService>().As<IRecordNewCarService>();
            #endregion

            #region 摄像头分配
            builder.RegisterType<CameraDistributeService>().As<ICameraDistributeService>();
            builder.RegisterType<FiterAndRepeatDataService>().As<IFiterAndRepeatDataService>();
            builder.RegisterType<FilterMoldNameService>().As<IFilterMoldNameService>();
            #endregion

            #region 调用中心的接口
            builder.RegisterType<GetIntelligentInsurance>().As<IGetIntelligentInsurance>();
            #endregion

            #endregion

            #region commmon
            builder.RegisterType<MessageCenter>().As<IMessageCenter>();
            builder.RegisterType<CacheHelper>().As<ICacheHelper>();
            builder.RegisterType<CarInsuranceCache>().As<ICarInsuranceCache>();
            builder.RegisterType<UserClaimCache>().As<IUserClaimCache>();
            builder.RegisterType<HashOperator>().As<IHashOperator>();
            builder.RegisterType<ValidateService>().As<IValidateService>();
            builder.RegisterType<GetAgentInfoService>().As<IGetAgentInfoService>();
            builder.RegisterType<CheckGetSecCode>().As<ICheckGetSecCode>();
            builder.RegisterType<RedisCacheService>().As<ICacheService>();
            builder.RegisterType<HttpGetRequestValidService>().As<IRequestValidService>();
            builder.RegisterType<AgentPrivilegeService>().As<IAgentPrivilegeService>();
            builder.RegisterType<AuthorityService>().As<IAuthorityService>();

            #endregion

            // Register API controllers using assembly scanning.
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<DefaultCommandBus>().As<ICommandBus>().InstancePerApiRequest();
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerApiRequest();
            //builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerApiRequest();
            //builder.RegisterAssemblyTypes(typeof(CategoryRepository)
            //    .Assembly).Where(t => t.Name.EndsWith("Repository"))
            //.AsImplementedInterfaces().InstancePerApiRequest();
            //var services = Assembly.Load("EFMVC.Domain");
            //builder.RegisterAssemblyTypes(services)
            //.AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerApiRequest();
            //builder.RegisterAssemblyTypes(services)
            //.AsClosedTypesOf(typeof(IValidationHandler<>)).InstancePerApiRequest();
            var container = builder.Build();
            // Set the dependency resolver implementation.
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;
        }
    }
}