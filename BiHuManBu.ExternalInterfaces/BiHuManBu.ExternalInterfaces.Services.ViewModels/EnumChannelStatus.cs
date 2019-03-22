using System.ComponentModel;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public enum EnumChannelStatus
    {
        [Description("正常")]
        ZhengChang = 0,
        [Description("无法连接")]
        WuFaLianJie= 1,
        [Description("版本号不一致")]
        BanBenBuYiZhi= 2,
        [Description("超过并发上限")]
        ChaoGuoBingFa= 3,
        [Description("登录失败")]
        DengLuShiBai= 4,
        [Description("执行错误")]
        ZhiXingCuoWu= 97,
        [Description("vpn无法连接")]
        WuFaLianJieVPN = 98,
        [Description("其它配置错误")]
        PeiZhiCuoWu= 99,
        [Description("关机")]
        GuanJi= 100,
        [Description("未知状态")]
        WeiZhiZhuangTai= 101
    }
}
