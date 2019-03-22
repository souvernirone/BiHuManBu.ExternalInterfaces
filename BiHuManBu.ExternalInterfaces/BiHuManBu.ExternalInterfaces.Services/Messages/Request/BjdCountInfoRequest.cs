
namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class BjdCountInfoRequest : BaseRequest
    {
        private int _isOnlyMine = 1;
        public string OpenId { get; set; }

        /// <summary>
        /// 是否只查属于自己代理的bx_userinfo。1，是；0，否
        /// 如果是1，则不查下级代理
        /// </summary>
        public int IsOnlyMine
        {
            get { return _isOnlyMine; }
            set { _isOnlyMine = value; }
        }
    }
}
