using System;
using BiHuManBu.Redis;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace BiHuManBu.DsitributedMonitor
{
    public  class DmCacheOperator:IDmCacheOperator
    {
        public void AddTrace(DmContainer dc)
        {
            if (dc == null)
            {
                throw new ArgumentException("跟踪不能对象为空");
            }
            //using (var cli = RedisManager.GetClient())
            //{
            //    if (dc.Dc.ChildId == 1)
            //    {
            //        CreateNew(dc, cli);
            //    }
            //    else
            //    {
            //        AppendChildRecord(dc, cli);
            //    }
            //}
        }

        private void CreateNew(DmContainer dc,IRedisClient client)
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            var geneal = "dm" + day;
            //var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
            //var detail = (dc.Dc.RootId + "-" + dc.Dc.Tag ?? string.Empty) + time;
            client.AddItemToSortedSet(geneal, dc.Dc.RootId + "_" + day, DateTime.Now.Ticks);
            client.ExpireEntryAt(geneal, DateTime.Now.AddDays(1).Date);
            client.AddItemToSortedSet(dc.Dc.RootId + "_"+day, dc.ToJson(), DateTime.Now.Ticks);
            client.ExpireEntryAt(geneal, DateTime.Now.AddDays(1).Date);

            //var allItems = client.GetAllItemsFromSortedSet(geneal);
            //foreach (string item in allItems)
            //{
            //    var details = client.GetAllItemsFromSortedSet(item);
            //}
           
        }

        private void AppendChildRecord(DmContainer dc, IRedisClient client)
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            client.AddItemToSortedSet(dc.Dc.RootId  + "_" + day, dc.ToJson(), DateTime.Now.Ticks);
        }

    }
}
