using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class GetBjdCheck : IGetBjdCheck
    {
        private readonly IBaodanxinxiRepository _baodanxinxiRepository;
        private readonly IBaodanXianZhongRepository _baodanXianZhongRepository;

        public GetBjdCheck(IBaodanXianZhongRepository baodanXianZhongRepository, IBaodanxinxiRepository baodanxinxiRepository)
        {
            _baodanXianZhongRepository = baodanXianZhongRepository;
            _baodanxinxiRepository = baodanxinxiRepository;
        }

        public GetBjdCheckMessage BjdCheckMessage(long bxid)
        {
            var checkMessage = new GetBjdCheckMessage() {State = 1};
            try
            {
                var bjdxinxi = _baodanxinxiRepository.Finds(bxid);
                if (bjdxinxi.Baodanxinxi == null)
                {
                    checkMessage.State = 0;
                    checkMessage.Message = string.Format("Bxid为{0}的记录Baodanxinxi为空", bxid);
                    return checkMessage;
                }
                if (bjdxinxi.Baodanxianzhong == null)
                {
                    checkMessage.State = 0;
                    checkMessage.Message = string.Format("Bxid为{0}的记录Baodanxianzhong为空", bxid);
                    return checkMessage;
                }
                checkMessage.BjUnion = bjdxinxi.BjUnion;
                checkMessage.Baodanxinxi = bjdxinxi.Baodanxinxi;
                checkMessage.Baodanxianzhong = bjdxinxi.Baodanxianzhong;
            }
            catch (Exception ex)
            {
                checkMessage.Message = string.Format("程序出险异常，错误信息为：{0}", ex.Message);
                checkMessage.State = -1;
                return checkMessage;
            }
            
            return checkMessage;
        }
    }
}
