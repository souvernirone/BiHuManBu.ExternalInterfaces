using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class SpecialOptionService : ISpecialOptionService
    {
        private readonly ISpecialOptionRepository _specialOptionRepository;
        private ILog logError = LogManager.GetLogger("ERROR");
        public SpecialOptionService(ISpecialOptionRepository specialOptionRepository)
        {
            _specialOptionRepository = specialOptionRepository;
        }
        public void AddSpecialOptionList(long buid, string specialOption)
        {
            if (string.IsNullOrWhiteSpace(specialOption))
            {
                return;
            }
            try
            {
                //执行删除操作
                _specialOptionRepository.DelList(buid);
                //执行插入操作
                List<bx_specialoption> specialOptionList = new List<bx_specialoption>();
                bx_specialoption specialoption = new bx_specialoption();
                List<SpecialOption> requestList = specialOption.FromJson<List<SpecialOption>>();
                if (requestList != null)
                {
                    foreach (var item in requestList)
                    {
                        specialOptionList.Add(new bx_specialoption()
                        {
                            BUid = buid,
                            Source = SourceGroupAlgorithm.GetOldSource(item.Source),
                            Content = item.Content,
                            Code = item.Code,
                            Name = item.Name,
                            IsCommerce = item.IsCommerce,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            IsDefault = item.IsDefault,
                            Status = item.Status
                        });
                    }
                    _specialOptionRepository.AddList(specialOptionList);
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
        }
    }
    public class SpecialOption
    {
        public long Source { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        /// <summary>
        ///  2 新增   3 删除
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        ///  1 默认特约  0非默认特约
        /// </summary>
        public int IsDefault { get; set; }
        /// <summary>
        /// 特约的种类，商业险特约、交强险特约、商业险自定义特约、交强险自定义特约
        /// </summary>
        public int IsCommerce { get; set; }

    }
}
