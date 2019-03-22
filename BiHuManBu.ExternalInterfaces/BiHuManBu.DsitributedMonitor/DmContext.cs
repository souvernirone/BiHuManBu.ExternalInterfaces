using System;

namespace BiHuManBu.DsitributedMonitor
{
    public class DmContext
    {
        public string RootId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string Tag { get; set; }
       
    }

    public class Message
    {
        public string ServerIp { get; set; }
        public string ActionName { get; set; }
        public DateTime ExecuteTime { get; set; }
        public string Body { get; set; }
        public BusinessType Type { get; set; }


    }

    public class DmContainer
    {
        public DmContext Dc { get; set; }
        public Message Msg { get; set; }
    }

    public enum BusinessType
    {
        Renewal=1,
        Quote=2
    }
}
