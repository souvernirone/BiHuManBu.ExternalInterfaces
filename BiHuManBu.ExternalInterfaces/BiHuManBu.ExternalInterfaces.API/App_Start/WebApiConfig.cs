﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Tracing;
using WebApiThrottle;
using WebApiThrottle.Repositories;

namespace BiHuManBu.ExternalInterfaces.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
           
            // 取消注释下面的代码行可对具有 IQueryable 或 IQueryable<T> 返回类型的操作启用查询支持。
            // 若要避免处理意外查询或恶意查询，请使用 QueryableAttribute 上的验证设置来验证传入查询。
            // 有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=279712。
            //config.EnableQuerySupport();

            // 若要在应用程序中禁用跟踪，请注释掉或删除以下代码行
            // 有关详细信息，请参阅: http://www.asp.net/web-api
         

            config.Filters.Add(new ThrottlingFilter(
              policy: ThrottlePolicy.FromStore(new PolicyConfigurationProvider()),
              policyRepository: new PolicyCacheRepository(), 
              repository: new RedisCacheRepository(), 
              logger: new TracingThrottleLogger()));
            //config.Filters.Add(new ThrottlingCommonFilter(
            // policy: ThrottlePolicy.FromStore(new PolicyConfigurationProvider()),
            // policyRepository: new PolicyCacheRepository(),
            // repository: new RedisCacheRepository(),
            // logger: new TracingThrottleLogger()));

#if DEBUG
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
#endif 
        }
    }
}