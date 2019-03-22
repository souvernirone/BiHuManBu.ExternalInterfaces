using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models.DistributeModels
{
    public class CameraDistributeModel
    {
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }
        [Range(1, 1000000)]
        public int Agent { get; set; }
        //public int agentId { get; set; }
        public int buId { get; set; }
        public int source { get; set; }
        public int isRead { get; set; }

        public int cityCode { get; set; }
        public string licenseNo { get; set; }
        public string businessExpireDate { get; set; }
        public string forceExpireDate { get; set; }
        public int childAgent { get; set; }
        public string carModelKey { get; set; }

        /// <summary>
        /// 是否是老数据
        /// </summary>
        public bool existUserinfo { get; set; }
        /// <summary>
        /// 是否是顶级
        /// </summary>
        public bool isTopAgent { get; set; }
        /// <summary>
        /// 续保状态
        /// </summary>
        public int renewalStatus { get; set; }

        /// <summary>
        /// 原来的录入方式
        /// </summary>
        public int uiRenewalType { get; set; }
        /// <summary>
        /// 新的录入方式
        /// </summary>
        public int reqRenewalType { get; set; }

        /// <summary>
        /// 原来的openid//目前仅方法内部调用
        /// </summary>
        public string uiCustKey { get; set; }

        /// <summary>
        /// 摄像头绑定的代理人20180131启用
        /// </summary>
        public int CameraAgent { get; set; }

    }
    public class AgentIdAndRoleTyoeDto
    {
        public int AgentId { get; set; }
        /// <summary>
        /// manager_role_db.role_type
        /// </summary>
        public int RoleType { get; set; }
    }
    public class carMold
    {
        public int id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// del  add  md
        /// </summary>
        public string status { get; set; }
    }
    public class PushedMessage
    {
        private int _msgType;
        private long _buId;
        private string _title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
            }
        }
        /// <summary>
        /// 消息编号
        /// </summary>
        public int MsgId
        {
            get
            {
                return msgId;
            }

            set
            {
                msgId = value;
            }
        }
        /// <summary>
        /// 信鸽账户
        /// </summary>
        public string Account
        {
            get
            {
                return _account;
            }

            set
            {
                _account = value;
            }
        }
        /// <summary>
        /// bx_userinfo.id
        /// </summary>
        public long BuId
        {
            get
            {
                return _buId;
            }

            set
            {
                _buId = value;
            }
        }
        /// <summary>
        /// 消息类型 8 新进店消息 9 新分配消息
        /// </summary>
        public int MsgType
        {
            get
            {
                return _msgType;
            }

            set
            {
                _msgType = value;
            }
        }

        private string _content;
        private int msgId;
        private string _account;

    }
    public class DuoToNoticeViewModel
    {
        public int AgentId { get; set; }
        public List<CompositeBuIdLicenseNo> Data { get; set; }
        public string BuidsString { get; set; }
    }

    public class CompositeBuIdLicenseNo
    {
        public long BuId { get; set; }
        public string LicenseNo { get; set; }
        public int Days { get; set; }

    }
    public class LeaveDate
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime leave { get; set; }
        /// <summary>
        /// del  add  md
        /// </summary>
        public string status { get; set; }
    }
}
